using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CZGL.Auth.Sample1.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace CZGL.Auth.Sample1
{
    public class Program
    {
        public static void Main(string[] args)
        {           
            // AddRole();
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });



        // ÃÌº”≤‚ ‘ ˝æ›
        public static void AddRole()
        {
            UserContext context = new UserContext();

            var a = Guid.NewGuid().ToString();
            var b = Guid.NewGuid().ToString();
            var c = Guid.NewGuid().ToString();
            List<Role> roles = new List<Role>()
            {
                new Role
                {
                    Id =1,
                    RoleId =a,
                    RoleName ="A"
                },
                new Role
                {
                    Id =2,
                    RoleId =b,
                    RoleName ="B"
                },
                new Role
                {
                    Id =3,
                    RoleId =c,
                    RoleName ="C"
                }
            };
            context.Roles.AddRange(roles);

            context.SaveChanges();

            List<RoleClaim> rolea = new List<RoleClaim>()
            {
                new RoleClaim
                {
                    RoleId=a,ApiName="A",ApiUrl="/api/Test/A"
                },
                 new RoleClaim
                {
                    RoleId=a,ApiName="AB",ApiUrl="/api/Test/AB"
                },
                 new RoleClaim
                {
                    RoleId=a,ApiName="AC",ApiUrl="/api/Test/AC"
                },
                 new RoleClaim
                {
                    RoleId=a,ApiName="ABC",ApiUrl="/api/Test/ABC"
                }

            };
            List<RoleClaim> roleb = new List<RoleClaim>()
            {
                new RoleClaim
                {
                    RoleId=b,ApiName="AB",ApiUrl="/api/Test/AB"
                },
                 new RoleClaim
                {
                    RoleId=b,ApiName="B",ApiUrl="/api/Test/B"
                },
                 new RoleClaim
                {
                    RoleId=b,ApiName="BC",ApiUrl="/api/Test/BC"
                },
                 new RoleClaim
                {
                    RoleId=b,ApiName="ABC",ApiUrl="/api/Test/ABC"
                }

            };
            List<RoleClaim> rolec = new List<RoleClaim>()
            {
                new RoleClaim
                {
                    RoleId=c,ApiName="AC",ApiUrl="/api/Test/AC"
                },
                 new RoleClaim
                {
                    RoleId=c,ApiName="BC",ApiUrl="/api/Test/BC"
                },
                 new RoleClaim
                {
                    RoleId=c,ApiName="C",ApiUrl="/api/Test/C"
                },
                 new RoleClaim
                {
                    RoleId=c,ApiName="ABC",ApiUrl="/api/Test/ABC"
                }

            };

            context.RoleClaims.AddRange(rolea);
            context.RoleClaims.AddRange(roleb);
            context.RoleClaims.AddRange(rolec);

            context.SaveChanges();


            List<User> users = new List<User>()
            {
                new User
                {
                    Id=1,
                    UserName="aa",
                    UserPassword="aa"
                },
                new User
                {
                    Id=2,
                    UserName="bb",
                    UserPassword="b"
                },
                new User
                {
                    Id=3,
                    UserName="cc",
                    UserPassword="cc"
                }
            };
            context.Users.AddRange(users);
            context.SaveChanges();

            List<UserClaim> userclaims = new List<UserClaim>
            {
                new UserClaim
                {
                    Id=1,
                    UserId=1,
                    RoleId=a
                },
                new UserClaim
                {
                    Id=2,
                    UserId=2,
                    RoleId=b
                },
                new UserClaim
                {
                    Id=3,
                    UserId=3,
                    RoleId=c
                }
            };
            context.UserClaims.AddRange(userclaims);
            context.SaveChanges();

        }
    }
}
