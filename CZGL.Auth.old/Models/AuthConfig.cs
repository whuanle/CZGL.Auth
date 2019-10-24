using CZGL.Auth.Interface;
using CZGL.Auth.Services;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;

namespace CZGL.Auth.Models
{
    public static class AuthConfig
    {

        /// <summary>
        /// 加密 Token 的密钥
        /// </summary>
        public static string SecurityKey { get; private set; }

        /// <summary>
        /// 默认用户
        /// </summary>
        public static string DefautRole { get; private set; }

        /// <summary>
        /// 订阅人
        /// </summary>
        public static string Audience { get; private set; }

        /// <summary>
        /// 发行人
        /// </summary>
        public static string Issuer { get; private set; }

        /// <summary>
        /// 过期时间
        /// </summary>
        public static TimeSpan TimeSpan { get; private set; }

        /// <summary>
        /// 权限不足时是否跳转到失败页面
        /// </summary>
        public static bool IsDeniedAction { get; private set; }

        /// <summary>
        /// 验证失败时跳转到此API
        /// </summary>
        public static string DeniedAction { get; private set; }

        /// <summary>
        /// 未携带验证信息是否跳转到登录页面
        /// </summary>
        public static bool IsLoginAction { get; private set; }

        /// <summary>
        /// 未携带任何身份信息时时跳转到登陆API
        /// </summary>
        public static string LoginAction { get; private set; }

        /// <summary>
        /// 用于加密的密钥对象
        /// </summary>
        public static SigningCredentials SigningCredentials=> new SigningCredentials(new SymmetricSecurityKey(Encoding.UTF8.GetBytes(SecurityKey)), SecurityAlgorithms.HmacSha256);


        /// <summary>
        /// 初始化
        /// </summary>
        /// <param name="authModel">配置类</param>
        public static void Init(AuthModel authModel)
        {
            AuthConfig.SecurityKey = authModel.SecurityKey;
            AuthConfig.Issuer = authModel.Issuer.ToLower();
            AuthConfig.Audience = authModel.Audience.ToLower();

            AuthConfig.TimeSpan = authModel.TimeSpan;

            AuthConfig.LoginAction = authModel.LoginAction;

            AuthConfig.DeniedAction = authModel.DeniedAction;
            AuthConfig.IsLoginAction = authModel.IsLoginAction;
            AuthConfig.IsDeniedAction = authModel.IsDeniedAction;
        }


        /// <summary>
        /// 获取用户 Token 配置
        /// </summary>
        /// <returns></returns>
        public static TokenValidationParameters GetTokenValidationParameters()
        {
            var tokenValida = new TokenValidationParameters
            {
                // 定义 Token 内容
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(AuthConfig.SecurityKey)),
                ValidateIssuer = true,
                ValidIssuer = AuthConfig.Issuer,
                ValidateAudience = true,
                ValidAudience = AuthConfig.Audience,
                ValidateLifetime = true,
                ClockSkew = TimeSpan.Zero,
                RequireExpirationTime = true
            };
            return tokenValida;
        }
    }
}
