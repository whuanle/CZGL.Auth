using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using AuthTest.Models;
using CZGL.Auth.Models;
using CZGL.Auth.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AuthTest.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {

        private readonly AuthorizationRequirement _requirement;
        public AccountController(AuthorizationRequirement requirement)
        {
            _requirement = requirement;
        }
        [HttpPost("Login")]
        public JsonResult Login(string username, string password)
        {
            var user = UserModel.Users.FirstOrDefault(x => x.UserName == username && x.UserPossword == password);
            if (user == null)
                return new JsonResult(
                    new ResponseModel
                    {
                        Code = 0,
                        Message = "登陆失败!"
                    });

            EncryptionHash hash = new EncryptionHash();

            // 将用户标识存储到系统中
            _requirement.SetUserRole(user.Role);


            //// 配置用户标识
            //var userClaims = new Claim[]
            //{
            //    new Claim(ClaimTypes.Name,user.UserName),
            //    new Claim(ClaimTypes.Role,user.Role),
            //    new Claim(ClaimTypes.Expiration,DateTime.Now.AddMinutes(TimeSpan.FromMinutes(20)).ToString()),
            //};

            // 设置用户信息
            var userClaims = hash.GetClaims(username, user.Role);


            // 颁发 token
            var identity = hash.GetIdentity(userClaims);
            var jwt = hash.BuildJwtToken(userClaims);
            var token = hash.BuildJwtResponseToken(jwt);

            return new JsonResult(
                new ResponseModel
                {
                    Code = 200,
                    Message = "登陆成功!请注意保存你的 Token 凭证!",
                    Data = token
                });
        }
    }
}