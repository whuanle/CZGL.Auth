using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CZGL.Auth.Interface;
using CZGL.Auth.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace MyAuth
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
                .InfoScheme(new CZGL.Auth.Models.AuthenticateScheme
                {
                    TokenEbnormal = "Login authentication failed!1",
                    TokenIssued = "Login authentication failed!2",
                    NoPermissions = "Login authentication failed!3"
                }).Build();
            services.AddSingleton<IRoleEventsHadner, RoleEvents>();
            services.AddRoleService(authOptions);
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
