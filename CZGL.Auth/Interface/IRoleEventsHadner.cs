using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace CZGL.Auth.Interface
{
    /// <summary>
    /// 授权时事件接口，必须使用异步且返回Task
    /// </summary>
    public interface IRoleEventsHadner
    {

        /// <summary>
        /// 客户端请求时没有携带 Token
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        Task NoToken(string url);

        /// <summary>
        /// 客户端携带的 Token 不是有效的 Jwt 令牌，将不能被解析
        /// </summary>
        /// <returns></returns>
        Task TokenEbnormal(string url, string token);

        /// <summary>
        /// 令牌解码后，issuer 或 audience不正确
        /// </summary>
        /// <param name="url"></param>
        /// <param name="issuer">订阅者</param>
        /// <param name="audience">颁发者</param>
        /// <returns></returns>
        Task TokenIssued(string url, string issuer, string audience);

        /// <summary>
        /// 令牌已经过期
        /// </summary>
        /// <param name="url"></param>
        /// <param name="IssuedTime">令牌颁发的时间(Unix)</param>
        /// <param name="expiration">有效时间</param>
        /// <returns></returns>
        Task TokenTime(string url, long IssuedTime, long expiration);

        /// <summary>
        /// 用户所属的角色中，均无访问API的权限，即无访问此API的权限
        /// </summary>
        /// <param name="url"></param>
        /// <param name="userName">用户名</param>
        /// <returns></returns>
        Task NoPermissions(string url, string userName);

        /// <summary>
        /// 授权成功
        /// </summary>
        /// <param name="url"></param>
        /// <param name="userName">用户名</param>
        /// <param name="roleName">授权角色</param>
        /// <returns></returns>
        Task Authed(string url, string userName, string roleName);

        /// <summary>
        /// 自定义授权
        /// </summary>
        /// <typeparam name="TAuthorizationRequirement"></typeparam>
        /// <param name="url"></param>
        /// <param name="claims"></param>
        /// <param name="requirement"></param>
        /// <returns></returns>
        Task Custom<TAuthorizationRequirement>(string url, IEnumerable<Claim> claims, TAuthorizationRequirement requirement)
            where TAuthorizationRequirement : IAuthorizationRequirement;
    }

}
