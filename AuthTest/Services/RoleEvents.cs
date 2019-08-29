using CZGL.Auth.Interface;
using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace AuthTest.Services
{
    public class RoleEvents : IRoleEventsHadner
    {
        public async Task Authed(string url, string userName, string roleName)
        {
            await Task.Run(() =>
            {

                // 要编写的代码


            });
        }

        public async Task Custom<TAuthorizationRequirement>(string url, IEnumerable<Claim> claims, TAuthorizationRequirement requirement) where TAuthorizationRequirement : IAuthorizationRequirement
        {
            await Task.Run(() =>
            {

                // 要编写的代码
                Console.WriteLine("success");

            });
        }

        public async Task NoPermissions(string url, string userName)
        {
            await Task.Run(() =>
            {

                // 要编写的代码


            });
        }

        public async Task NoToken(string url)
        {
            await Task.Run(() =>
            {

                // 要编写的代码


            });
        }

        public async Task TokenEbnormal(string url, string token)
        {
            await Task.Run(() =>
            {

                // 要编写的代码


            });
        }

        public async Task TokenIssued(string url, string issuer, string audience)
        {
            await Task.Run(() =>
            {

                // 要编写的代码


            });
        }

        public async Task TokenTime(string url, long IssuedTime, long expiration)
        {
            await Task.Run(()=> 
            {

                // 要编写的代码


            });
        }
    }
}
