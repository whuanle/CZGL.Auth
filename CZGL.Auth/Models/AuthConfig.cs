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
        public static string SecurityKey { get; set; } = "MIICdwIBADANBgkqhkiG9w0BAQEFAASCAmEwggJdAgEAAoGBAKEfbzk6z6ACak8teTzcWR9Ota9N+sKz6HwFiGJbbhqZ9V4+wZap3Vha1EPNCCHI0tA8xlaOMmxee/62eZXT0v8wIqG6IhJGemviN7ygv7NytdcXxPSeOwi4NrI6kQKq4VYwezttfHqQO5JLWZWp+CgHegxteNQTBfJWoEhn/vsdAgMBAAECgYBEjsmWwm2CGkT99810lhXd+nHYbAfdPQwZaYlEwL6y1vvO5EkfQJwMcmmLh/yD053QOWvzfIH8FqAQG7jUhdNrURRmtWEkbEvJKfxHQVScfj5bttoJFrULMGaMnjs3lAj5783AZ3OtQ0BrFXwiwH9B/vJCHJmp/bDWkR15WPUogQJBANWII3Bz+NsrGuMpYQ21ER2RZvNlYaLxP4mztwxYHUE2i/LOOyFbhiP8lYJlhDdc/bLchlGHU+Sopd/YhPD9N40CQQDBKusvjfOxjhiFiIRR4ae5A/VyQr+uuU7sp7FYkCDjoXHdZXeE6rrRSWBazDlIuc3wU/+z4NejUCHCCh0wQmXRAkEAiCWHQxoOn996A0DM6up6ATpGRAZuHHBprKjzm2FLNdtLnAK2XOx4ONXBliSYCpy1/abx1WXNrcuCB5mMGgO5uQJAZK6pHQVBIqesslUgmskiMbYVhbOy0zA1KfaR4lZlPiBVCA+uBzKNoy46sbjGlth5ta0ilzA3VSEcJ1Y8Nn41MQJBALS8yInKmXe3Ar5TU5iUJuTgKFyWHcJMUXB/pamKwxiDmkFjNzDT0438b48ffHFWfXhc4wqiQFs1i76IQSGWLg=";

        /// <summary>
        /// 默认用户
        /// </summary>
        public static string DefautRole { get; set; } = "user";

        /// <summary>
        /// 订阅人
        /// </summary>
        public static string Audience { get; set; } = "ASPNETCORE";

        /// <summary>
        /// 发行人
        /// </summary>
        public static string Issuer { get; set; } = "ASPNETCORE";

        /// <summary>
        /// 过期时间
        /// </summary>
        public static TimeSpan TimeSpan { get; set; } = TimeSpan.FromMinutes(20);

        /// <summary>
        /// 权限不足时是否跳转到失败页面
        /// </summary>
        public static bool IsDeniedAction { get; set; } = false;

        /// <summary>
        /// 验证失败时跳转到此API
        /// </summary>
        public static string DeniedAction { get; set; }

        /// <summary>
        /// 未携带验证信息是否跳转到登录页面
        /// </summary>
        public static bool IsLoginAction { get; set; } = false;

        /// <summary>
        /// 未携带任何身份信息时时跳转到登陆API
        /// </summary>
        public static string LoginAction { get; set; } = "Account/Login";

        /// <summary>
        /// 退出登录的地址
        /// </summary>

        public static string Logout { get; set; }
        /// <summary>
        /// 用于加密的密钥对象
        /// </summary>
        public static SigningCredentials SigningCredentials { get; private set; }

        private static AuthorizationRequirement Requirement { get; set; }

        public static void SetDefalutRole(AuthorizationRequirement authorizationRequirement)
        {
            Requirement = authorizationRequirement;
        }

        public static void SetDefault(string roleName)
        {
            var requirement = new AuthorizationRequirement
                (
                roleName,     // 用户权限信息
                expiration: AuthConfig.TimeSpan    //Token过期时间
                );
            Requirement = requirement;
        }

        /// <summary>
        /// 默认用户
        /// </summary>
        /// <param name="signingCredentials"></param>
        /// <returns></returns>
        public static AuthorizationRequirement GetDefaultRole()
        {
            return Requirement;
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
