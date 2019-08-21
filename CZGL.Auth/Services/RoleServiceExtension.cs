using CZGL.Auth.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authorization.Policy;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using System.Threading.Tasks;

namespace CZGL.Auth.Services
{
    /// <summary>
    /// 角色授权服务
    /// </summary>
    public static class RoleServiceExtension
    {
        /// <summary>
        /// 角色授权服务
        /// </summary>
        public static IServiceCollection AddRoleService(this IServiceCollection services)
        {
            //services.AddHttpContextAccessor();


            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            // 设置用于加密 Token 的密钥
            // 配置角色权限 
            var roleRequirement = AuthConfig.GetDefaultRole();

            // 定义如何生成用户的 Token
            var tokenValidationParameters = AuthConfig.GetTokenValidationParameters();


            // 导入角色身份认证策略
            services.AddAuthorization(options =>
            {
                options.AddPolicy("Permission",
                   policy => policy.Requirements.Add(roleRequirement));


                // ↓ 身份认证类型
            }).AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;

                // ↓ Jwt 认证配置
            })
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = tokenValidationParameters;
                options.SaveToken = true;
                options.Events = new JwtBearerEvents()
                {
                    // 在安全令牌通过验证和ClaimsIdentity通过验证之后调用
                    // 如果用户访问注销页面
                    OnTokenValidated = context =>
                    {
                        if (context.Request.Path.Value.ToString() == AuthConfig.Logout)
                        {
                            var token = ((context as TokenValidatedContext).SecurityToken as JwtSecurityToken).RawData;
                        }
                        return Task.CompletedTask;
                    }
                };
            });
            // 添加 httpcontext 拦截
            services.AddSingleton<IAuthorizationHandler, RoleAuthorizationHandler>();

            services.AddSingleton(roleRequirement);


            return services;
        }
    }
}
