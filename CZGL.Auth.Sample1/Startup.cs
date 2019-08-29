using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CZGL.Auth.Interface;
using CZGL.Auth.Sample1.Models;
using CZGL.Auth.Sample1.Services;
using CZGL.Auth.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;


namespace CZGL.Auth.Sample1
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
            services.AddTransient<IRoleEventsHadner, RoleEvents>();
            // 角色授权服务
            services.AddRoleService(
                new AuthBuilder()
                .Security("afsdddddddddddddddddd", "ASPNETCORE", "ASPNETCORE")
                .Jump("/account/login", "/account/error", false, false)
                .Time(TimeSpan.FromMinutes(20))
                .Builder()
                , new AuthorizationRequirement()
                , "Permission");

            //services.AddRoleService(
            //    new AuthBuilder().Security().Jump().Time(TimeSpan.FromMinutes(20)).Builder()
            //    , new AuthorizationRequirement()
            //    , "Permission");

            services.AddDbContext<UserContext>(Options => Options.UseSqlite("filename=user.db"));
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo { Title = "My API", Version = "v1" });
            });


            services.AddControllers();
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

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
