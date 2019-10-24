using CZGL.Auth.Interface;
using CZGL.Auth.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Server.Kestrel.Core.Internal.Http;

namespace CZGL.Auth.Services
{
    public class RoleAuthorizationHandler : AuthorizationHandler<AuthorizationRequirement>
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IRolePermission _iRolePermission;
        private readonly IRoleEventsHadner _roleEventsHadner;
        private readonly EncryptionHash hash;
        public RoleAuthorizationHandler(
            IHttpContextAccessor httpContextAccessor,
            IRolePermission rolePermission,
            IRoleEventsHadner roleEventsHadner)
        {
            _httpContextAccessor = httpContextAccessor;
            _iRolePermission = rolePermission;
            _roleEventsHadner = roleEventsHadner;
            hash = new EncryptionHash();
        }
        protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context,
            AuthorizationRequirement requirement)
        {

            string tokenStr = _httpContextAccessor.HttpContext.Request.Headers["Authorization"].ToString();
            string requestUrl = _httpContextAccessor.HttpContext.Request.Path.Value;
            // 3.0 可以用
            //string requestUrl = ((Microsoft.AspNetCore.Routing.RouteEndpoint)context.Resource).RoutePattern.RawText;

            // 请求Token,只能2.x使用
            // (_httpContextAccessor.HttpContext.Request.Headers as  Microsoft.AspNetCore.Server.Kestrel.Core.Internal.Http).HeaderAuthorization;

            // 未携带token时
            if (string.IsNullOrEmpty(tokenStr))
            {
                if (AuthConfig.IsLoginAction)
                {
                    _httpContextAccessor.HttpContext.Response.Redirect(AuthConfig.LoginAction);
                }
                context.Fail();
                await Task.CompletedTask;
                await _roleEventsHadner.NoToken(_httpContextAccessor.HttpContext.Request.Path.Value);
            }

            JwtSecurityToken jst = new JwtSecurityToken();
            // 如果是无效的Token
            if (!hash.IsCanReadToken(tokenStr))
            {
                context.Fail();
                await Task.CompletedTask;
                await _roleEventsHadner.TokenEbnormal(requestUrl, tokenStr);
            }
            try
            {
                // 从 Token 里面解码出 JwtSecurityToken
                jst = hash.GetJwtSecurityToken(tokenStr);
            }
            catch
            {
                context.Fail();
                await Task.CompletedTask;
                await _roleEventsHadner.TokenEbnormal(requestUrl, tokenStr);
            }
            var claims = hash.GetClaims(jst);
            ClaimsIdentity claimsIdentity = new ClaimsIdentity();
            claimsIdentity.AddClaims(claims);
            context.User.AddIdentity(claimsIdentity);
            // 校验 颁发主体
            var aud = claims.FirstOrDefault(x => x.Type == JwtRegisteredClaimNames.Aud).Value;
            if (!(jst.Issuer == AuthConfig.Issuer || aud != AuthConfig.Audience))
            {
                context.Fail();
                await Task.CompletedTask;
                await _roleEventsHadner.TokenIssued(requestUrl, jst.Issuer, aud);
            }

            // 校验过期时间
            long nowTime = DateTimeOffset.Now.ToUnixTimeSeconds();
            // 有效期
            var expiration = Convert.ToInt64(claims.FirstOrDefault(x => x.Type == ClaimTypes.Expiration).Value);
            // 颁发时间
            long issued = expiration + Convert.ToInt64(claims.FirstOrDefault(x => x.Type == JwtRegisteredClaimNames.Iat).Value);

            // 令牌已过期
            if (issued < nowTime)
            {
                context.Fail();
                await Task.CompletedTask;
                await _roleEventsHadner.TokenTime(requestUrl, issued, expiration);
            }

            string userName = claims.FirstOrDefault(x => x.Type == ClaimTypes.Name).Value;
            string roleName = claims.FirstOrDefault(x => x.Type == ClaimTypes.Role).Value;

            // 角色无效
            // 可能后台已经删除了此角色，或者用户被取消此角色
            if (!_iRolePermission.IsUserToRole(userName, roleName))
            {
                context.Fail();
                await Task.CompletedTask;
                await _roleEventsHadner.NoPermissions(requestUrl, roleName);
            }


            // 是否有访问此 API 的权限


            RoleModel apiResource = _iRolePermission.GetRole(roleName);
            if (apiResource == null)
            {
                context.Fail();
                await Task.CompletedTask;
                await _roleEventsHadner.NoPermissions(requestUrl, roleName);
            }
            bool isHas = apiResource.Apis.Any(x => x.Url.ToLower() == requestUrl.ToLower());
            if (!isHas)
            {
                if (AuthConfig.IsDeniedAction == true)
                {
                    //无权限时跳转到某个页面
                    _httpContextAccessor.HttpContext.Response.Redirect(AuthConfig.DeniedAction);
                }
                context.Fail();
                await Task.CompletedTask;
            }


            context.Succeed(requirement);

            await _roleEventsHadner.Authed(requestUrl, userName, roleName);
            await _roleEventsHadner.Custom(requestUrl, claims, requirement);
            context.Succeed(requirement);
            await Task.CompletedTask;
            return;
        }
    }
}
