using CZGL.Auth.Interface;
using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace CZGL.Auth.Sample2.Services
{
    public class RoleEvents : IRoleEventsHadner
    {
        public async Task Start(object httpContext)
        {
            await Task.CompletedTask;
        }

        public void TokenEbnormal(object eventsInfo)
        {

        }


        public void TokenIssued(object eventsInfo)
        {

        }


        public void NoPermissions(object eventsInfo)
        {

        }

        public void Success(object eventsInfo)
        {

        }


        public async Task End(object httpContext)
        {
            await Task.CompletedTask;
        }
    }
}
