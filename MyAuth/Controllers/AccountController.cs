using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CZGL.Auth.Models;
using CZGL.Auth.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace MyAuth.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        [HttpPost("/Login")]
        public async Task<JsonResult> Login([FromQuery]string username, string password, string rolename)
        {
            // 用户名密码是否正确
            if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password) || string.IsNullOrWhiteSpace(rolename))
            {
                return new JsonResult(new 
                {
                    Code = 0,
                    Message = "尼玛，上传什么垃圾信息",
                });
            }

            if(!((username=="aa"||username=="bb"||username=="cc")&&password=="123456"))
            {
                return new JsonResult(new
                {
                    Code = 0,
                    Message = "账号或密码错误",
                });
            }

            // 你自己定义的角色/用户信息服务
            RoleService roleService = new RoleService();

            // 检验用户是否属于此角色
            var role = roleService.IsUserToRole(username,rolename);

            // CZGL.Auth 中一个用于加密解密的类
            EncryptionHash hash = new EncryptionHash();

            // 设置用户标识
            var userClaims = hash.BuildClaims(username, rolename);

            //// 自定义构建配置用户标识
            /// 自定义的话，至少包含如下标识
            //var userClaims = new Claim[]
            //{
            //new Claim(ClaimTypes.Name, userName),
            //    new Claim(ClaimTypes.Role, roleName),
            //    new Claim(JwtRegisteredClaimNames.Aud, Audience),
            //    new Claim(ClaimTypes.Expiration, TimeSpan.TotalSeconds.ToString()),
            //    new Claim(JwtRegisteredClaimNames.Iat, new DateTimeOffset(DateTime.Now).ToUnixTimeSeconds().ToString())
            //};
            /*
            iss (issuer)：签发人
            exp (expiration time)：过期时间
            sub (subject)：主题
            aud (audience)：受众
            nbf (Not Before)：生效时间
            iat (Issued At)：签发时间
            jti (JWT ID)：编号
            */



            // 方法一，直接颁发 Token
            ResponseToken token = hash.BuildToken(userClaims);


            //方法二，拆分多步，颁发 token，方便调试
            //var identity = hash.GetIdentity(userClaims);
            //var jwt = hash.BuildJwtToken(userClaims);
            //var token = hash.BuildJwtResponseToken(jwt);

            return new JsonResult(token);
        }
    }
}