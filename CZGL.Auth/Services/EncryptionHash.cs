using CZGL.Auth.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace CZGL.Auth.Services
{
    /// <summary>
    /// 用于加密生成
    /// </summary>
    public class EncryptionHash
    {

        /// <summary>
        /// 获取字符串的哈希值
        /// </summary>
        /// <param name="source">源字符串</param>
        /// <param name="clearChar">需要去除的特殊字符</param>
        /// <returns></returns>
        public static string GetByHashString(string source, char[] clearChar = null)
        {

            if (clearChar != null)
            {
                foreach (var item in clearChar)
                {
                    source = source.Replace(item.ToString(), String.Empty);
                }
            }

            string hash = GetByHashString(source);
            return hash;
        }

        /// <summary>
        /// 将字符串生成密钥
        /// </summary>
        /// <param name="key">字符串</param>
        /// <param name="encryptionType">加密方式</param>
        /// <returns></returns>
        public SigningCredentials GetTokenSecurityKey(string key, string encryptionType = SecurityAlgorithms.HmacSha256)
        {
            var securityKey = new SigningCredentials(
                new SymmetricSecurityKey(
                    Encoding.UTF8.GetBytes(key)), encryptionType);
            return securityKey;
        }


        /// <summary>
        /// 获取字符串的哈希值
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public string GetByHashString(string source)
        {
            MD5 md5Hash = MD5.Create();
            byte[] data = md5Hash.ComputeHash(Encoding.UTF8.GetBytes(source));
            StringBuilder sBuilder = new StringBuilder();
            for (int i = 0; i < data.Length; i++)
            {
                sBuilder.Append(data[i].ToString("x2"));
            }
            return sBuilder.ToString();
        }

        /// <summary>
        /// 生成 Web Token (JWT) 令牌
        /// </summary>
        /// <typeparam name="TRequirement">实现 IAuthorizationRequirement 接口</typeparam>
        /// <param name="claims">用户身份信息</param>
        /// <param name="requirement">认证类型</param>
        /// <param name="timeSpan">Token 有效时间</param>
        /// <param name="signingCredentials">用于加密Token的密钥</param>
        /// <param name="issuer">颁发者</param>
        /// <param name="audience">接收者</param>
        /// <returns></returns>
        public JwtSecurityToken BuildJwtToken(Claim[] claims)
        {
            var now = DateTime.UtcNow;
            var jwt = new JwtSecurityToken(
                issuer: AuthConfig.Issuer,
                audience: AuthConfig.Audience,
                claims: claims,
                notBefore: now,
                expires: now.Add(AuthConfig.TimeSpan),
                signingCredentials: AuthConfig.SigningCredentials
            );

            return jwt;
        }
        /// <summary>
        /// 生成 Token 信息
        /// </summary>
        /// <param name="jwt">JWT 令牌</param>
        /// <param name="timeSpan">Token过期时间</param>
        /// <returns>匿名类型</returns>
        public dynamic BuildJwtTokenDynamic(JwtSecurityToken jwt)
        {
            var encodedJwt = new JwtSecurityTokenHandler().WriteToken(jwt);
            var response = new
            {
                status = true,
                access_token = encodedJwt,
                expires_in = AuthConfig.TimeSpan.TotalMilliseconds,
                token_type = "Bearer"
            };
            return response;
        }
        /// <summary>
        /// 生成 Token 信息
        /// </summary>
        /// <param name="jwt">JWT 令牌</param>
        /// <param name="timeSpan">Token过期时间</param>
        /// <returns>CZGL.Auth.Models。ResponseToken</returns>
        public ResponseToken BuildJwtResponseToken(JwtSecurityToken jwt)
        {
            var encodedJwt = new JwtSecurityTokenHandler().WriteToken(jwt);
            var response = new ResponseToken
            {
                Status = true,
                Access_Token = encodedJwt,
                Expires_In = AuthConfig.TimeSpan.TotalMilliseconds,
                Token_Type = "Bearer"
            };
            return response;
        }
        /// <summary>
        /// 直接生成 Token
        /// </summary>
        /// <param name="jwt">JWT 令牌</param>
        /// <param name="timeSpan">Token过期时间</param>
        /// <returns>Token字符串</returns>
        public string BuildJwtToken(JwtSecurityToken jwt)
        {
            var encodedJwt = new JwtSecurityTokenHandler().WriteToken(jwt);
            return encodedJwt;
        }

        /// <summary>
        /// 生成身份信息
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="roleName"></param>
        /// <returns></returns>
        public Claim[] GetClaims(string userName, string roleName)
        {
            // 配置用户标识
            var userClaims = new Claim[]
            {
                new Claim(ClaimTypes.Name,userName),
                new Claim(ClaimTypes.Role,roleName),
                new Claim(ClaimTypes.Expiration,DateTime.Now.AddMinutes(AuthConfig.TimeSpan.TotalMinutes).ToString()),
            };
            return userClaims;
        }

        /// <summary>
        /// 生成用户标识
        /// </summary>
        public ClaimsIdentity GetIdentity(Claim[] claims)
        {
            var identity = new ClaimsIdentity(JwtBearerDefaults.AuthenticationScheme);
            identity.AddClaims(claims);
            return identity;
        }
    }
}
