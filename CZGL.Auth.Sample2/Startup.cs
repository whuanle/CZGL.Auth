using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using CZGL.Auth.Services;
using CZGL.Auth.Sample2.Models;
using Microsoft.EntityFrameworkCore;
using CZGL.Auth.Sample2.Services;
using CZGL.Auth.Interface;

namespace CZGL.Auth.Sample2
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
            var authOptions = new AuthBuilder()
                .Security("aaaafsfsfdrhdhrejtrjrt", "ASPNETCORE", "ASPNETCORE")
                .Jump("accoun/login", "account/error", false, false)
                .Time(TimeSpan.FromMinutes(20))
                .InfoScheme(new Auth.Models.AuthenticateScheme
                {
                    TokenEbnormal = "Login authentication failed!",
                    TokenIssued = "Login authentication failed!",
                    NoPermissions = "Login authentication failed!"
                }).Build();
            services.AddRoleService(authOptions);
            services.AddSingleton<IRoleEventsHadner, RoleEvents>();
            services.AddDbContext<UserContext>(Options => Options.UseSqlite("filename=user.db"));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();
            app.UseMiddleware<RoleMiddleware>();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
