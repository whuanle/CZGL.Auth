using CZGL.Auth.Interface;
using CZGL.Auth.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;

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
        /// <typeparam name="TAuthorizationRequirement">自定义验证的标识</typeparam>
        /// <param name="services">服务上下文</param>
        /// <param name="authModel">验证授权配置</param>
        /// <param name="name">定义策略名称</param>
        /// <returns></returns>
        public static IServiceCollection AddRoleService(
            this IServiceCollection services,
            AuthModel authModel,
            AuthorizationRequirement requirement,
            string name
            )
        {


            AuthConfig.Init(authModel);


            // 定义如何生成用户的 Token
            var tokenValidationParameters = AuthConfig.GetTokenValidationParameters();


            // 导入角色身份认证策略
            services.AddAuthorization(options =>
                {
                    options.AddPolicy(name,
                policy => policy.Requirements.Add(requirement));

                    // ↓ 身份认证类型
                }).AddAuthentication(options =>
                {
                    options.DefaultSignInScheme = JwtBearerDefaults.AuthenticationScheme;
                    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
                    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                })
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = tokenValidationParameters;
                options.SaveToken = true;
            });
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddSingleton<IRolePermission,RolePermission>();
            services.AddSingleton<IAuthorizationPolicyProvider,AuthorizationPolicyProvider>();
            return services;
        }
    }
}
