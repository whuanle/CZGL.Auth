using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using CZGL.Auth.Services;
using Swashbuckle.AspNetCore.Swagger;
using CZGL.Auth.Interface;
using AuthTest.Services;
using AuthTest.Models;
using Microsoft.EntityFrameworkCore;

namespace AuthTest
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

            services.AddTransient<IRolePermission, RolePermission>();
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
                c.SwaggerDoc("v1", new Info { Title = "My API", Version = "v1" });
            });

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();


            app.UseAuthentication();
            app.UseAuthorization();

            app.UseMvc();
            // 添加下面的内容
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
            });
        }
    }
}
