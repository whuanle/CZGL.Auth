using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using AuthTest.Models;
using AuthTest.Services;
using CZGL.Auth.Interface;
using CZGL.Auth.Models;
using CZGL.Auth.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace AuthTest.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly UserContext _context;
        public AccountController(UserContext context)
        {
            _context = context;
        }

        [HttpPost("Login")]
        public async Task<JsonResult> Login(string username, string password, string rolename)
        {
            // 用户名密码是否正确
            var user = _context.Users.FirstOrDefault(x => x.UserName == username && x.UserPassword == password);

            //一般不使用明文密码
            //   hash.GetByHashString(password); 生成哈希加密的字符串
            if (user == null)
            {
                return new JsonResult(
                    new ResponseModel
                    {
                        Code = 0,
                        Message = "登陆失败!"
                    });
            }
            // 经验用户选择登陆的角色是否有效
            Role role = _context.Roles.FirstOrDefault(x => x.RoleName.ToLower() == rolename.ToLower());
            UserClaim userClaim = _context.UserClaims.FirstOrDefault(x => x.RoleId == role.RoleId);
            if (role == null && userClaim == null)
            {
                return new JsonResult(
                    new ResponseModel
                    {
                        Code = 0,
                        Message = "你不属于此角色!"
                    });
            }
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



            // 方法一，颁发 Token
            ResponseToken token = hash.BuildToken(userClaims);


            //方法二，拆分多步，颁发 token，方便调试
            //var identity = hash.GetIdentity(userClaims);
            //var jwt = hash.BuildJwtToken(userClaims);
            //var token = hash.BuildJwtResponseToken(jwt);

            return new JsonResult(token);
        }

        [HttpPost("Refresh")]
        public async Task<JsonResult> Refresh()
        {
            var mana = new UserMana(_context);
            await mana.UpdateUser();
            return new JsonResult(new
            {
                UserInfo = TestUser.Users,
                RoleInfo = TestUser.Roles
            });
        }

    }
}