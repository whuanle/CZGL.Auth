using CZGL.Auth.Interface;
using CZGL.Auth.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CZGL.Auth.Services
{
    public class RoleAuthorizationHandler : AuthorizationHandler<AuthorizationRequirement>
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        public RoleAuthorizationHandler(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, AuthorizationRequirement requirement)
        {
            if (context.Requirements.Count() == 0)
            {
                if (AuthConfig.IsLoginAction)
                {
                    _httpContextAccessor.HttpContext.Response.Redirect(AuthConfig.LoginAction);
                }
                context.Fail();
                return Task.CompletedTask;
            }

            List<AuthorizationRequirement> requirements = new List<AuthorizationRequirement>();

            foreach (var item in context.Requirements)
            {
                requirements.Add((AuthorizationRequirement)item);
            }

            foreach (var item in requirements)
            {
                // 校验 颁发和接收对象
                if (!(item.Issuer == AuthConfig.Issuer ?
                    item.Audience == AuthConfig.Audience ?
                    true : false : false))
                {
                    context.Fail();
                    return Task.CompletedTask;
                }
                // 校验过期时间
                long nowTime = DateTimeOffset.Now.ToUnixTimeSeconds();
                var a = item.IssuedTime;
                long issued = item.IssuedTime + Convert.ToInt64(item.Expiration.TotalSeconds);
                if (issued < nowTime)
                {
                    context.Fail();
                    return Task.CompletedTask;
                }

                var re = RolePermission.GetRole(item.UserRole);
                if (re == null)
                {
                    context.Fail();
                    return Task.CompletedTask;
                }


                IRole role = re;

                // 是否有访问此 API 的权限

                // 3.0 才能用 ↓
                //var resource = ((Microsoft.AspNetCore.Routing.RouteEndpoint)context.Resource).RoutePattern.RawText;

                var resource = _httpContextAccessor.HttpContext.Request.Path.Value;
                var permissions = role.Apis.ToList();
                var apis = permissions.Any(x => x.Url.ToLower() == resource.ToLower());
                if (!apis)
                {
                    if (AuthConfig.IsDeniedAction == true)
                    {
                        //无权限时跳转到某个页面
                        _httpContextAccessor.HttpContext.Response.Redirect(AuthConfig.DeniedAction);
                    }
                    context.Fail();
                    return Task.CompletedTask;
                }
            }

            context.Succeed(requirement);
            return Task.CompletedTask;
        }
    }
}
