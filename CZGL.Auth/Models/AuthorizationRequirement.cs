using Microsoft.AspNetCore.Authorization;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Text;
using CZGL.Auth.Interface;
using System.Security.Claims;

namespace CZGL.Auth.Models
{
    public class AuthorizationRequirement : IAuthorizationRequirement
    {
        /// <summary>
        /// 默认用户所属角色
        /// </summary>
        public string UserRole { get; set; }

        /// <summary>
        /// 认证授权类型
        /// </summary>
        public string ClaimType { get; private set; }

        /// <summary>
        /// 发行人
        /// </summary>
        public string Issuer { get; private set; }

        /// <summary>
        /// 订阅人
        /// </summary>
        public string Audience { get;  set; }

        /// <summary>
        /// 颁发时间，Unix时间戳
        /// </summary>
        public long IssuedTime { get; set; } = DateTimeOffset.Now.ToUnixTimeSeconds();

        /// <summary>
        /// 过期时间
        /// </summary>
        public TimeSpan Expiration { get;private set; }

        /// <summary>
        /// 签名验证
        /// </summary>
        public SigningCredentials SigningCredentials { get; private set; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="roleName">默认角色名称</param>
        /// <param name="expiration">过期时间</param>
        public AuthorizationRequirement(string roleName, TimeSpan expiration)
        {
            UserRole = roleName;
            ClaimType = ClaimTypes.Role;
            Issuer = AuthConfig.Issuer;
            Audience = AuthConfig.Audience;
            Expiration = expiration;
            SigningCredentials = AuthConfig.SigningCredentials;
        }

        public void SetUserRole(string userRoleName)
        {
            IssuedTime = DateTimeOffset.Now.ToUnixTimeSeconds();
            UserRole = userRoleName;
        }
    }
}
