using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using CZGL.Auth.Services;
using CZGL.Auth.Models;
using CZGL.Auth.Interface;

namespace AuthTest
{
    public class Program
    {
        public static void SetRole()
        {
            new AuthBuilder()
               .Security()
               .Jump()
               .Time(TimeSpan.FromMinutes(20))
               .DefaultRole("user")
               .End();

            
            var usera = new Role()
            {
                RoleName = "supperadmin",
                Apis = new List<IApiPermission>
                {
                new ApiPermission{Name="A",Url="/api/Test/A" },
                new ApiPermission{Name="AB",Url="/api/Test/AB" },
                new ApiPermission{Name="AC",Url="/api/Test/AC" },
                new ApiPermission{Name="ABC",Url="/api/Test/ABC" }
                }
            };
            var userb = new Role()
            {
                RoleName = "admin",
                Apis = new List<IApiPermission>
                {
                new ApiPermission{Name="B",Url="/api/Test/B" },
                new ApiPermission{Name="AB",Url="/api/Test/AB" },
                new ApiPermission{Name="BC",Url="/api/Test/BC" },
                new ApiPermission{Name="ABC",Url="/api/Test/ABC" },
                }
            };
            var userc = new Role()
            {
                RoleName = "admin",
                Apis = new List<IApiPermission>
                {
                new ApiPermission{Name="C",Url="/api/Test/C" },
                new ApiPermission{Name="AC",Url="/api/Test/AC" },
                new ApiPermission{Name="BC",Url="/api/Test/BC" },
                new ApiPermission{Name="ABC",Url="/api/Test/ABC" },
                }
            };
            RolePermission.AddRole(usera);
            RolePermission.AddRole(userb);
            RolePermission.AddRole(userc);
        }

        public static void Main(string[] args)
        {
            SetRole();
            CreateWebHostBuilder(args).Build().Run();
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>();
    }
}
