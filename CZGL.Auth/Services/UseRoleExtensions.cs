using Microsoft.AspNetCore.Builder;
using System;
using System.Collections.Generic;
using System.Text;

namespace CZGL.Auth.Services
{
    public static class UseRoleExtensions
    {
        public static IApplicationBuilder UseRole(this IApplicationBuilder builder)
        {
            return builder;
        }
    }
}
