﻿using CZGL.Auth.Services;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;

namespace CZGL.Auth.Models
{
    public static class AuthConfig
    {
        public static AuthConfigModel model  = new AuthConfigModel();
        public static AuthConfigModel Auth { get { return model; } }


        /// <summary>
        /// 用于加密的密钥对象
        /// </summary>
        public static SigningCredentials SigningCredentials=> new SigningCredentials(new SymmetricSecurityKey(Encoding.UTF8.GetBytes(model.SecurityKey)), SecurityAlgorithms.HmacSha256);


        /// <summary>
        /// 初始化
        /// </summary>
        /// <param name="authModel">配置类</param>
        public static void Init(AuthConfigModel authModel)
        {
            model.SecurityKey = authModel.SecurityKey;
            model.Issuer = authModel.Issuer.ToLower();
            model.Audience = authModel.Audience.ToLower();

            model.TimeSpan = authModel.TimeSpan;

            model.LoginAction = authModel.LoginAction;

            model.DeniedAction = authModel.DeniedAction;
            model.IsLoginAction = authModel.IsLoginAction;
            model.IsDeniedAction = authModel.IsDeniedAction;
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
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(model.SecurityKey)),
                ValidateIssuer = true,
                ValidIssuer = model.Issuer,
                ValidateAudience = true,
                ValidAudience = model.Audience,
                ValidateLifetime = true,
                ClockSkew = TimeSpan.Zero,
                RequireExpirationTime = true
            };
            return tokenValida;
        }
    }
}
