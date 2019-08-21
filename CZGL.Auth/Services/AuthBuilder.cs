using CZGL.Auth.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace CZGL.Auth.Services
{
    public class AuthBuilder
    {
        public AuthBuilder Security(string securityKey = "MIICdwIBADANBgkqhkiG9w0BAQEFAASCAmEwggJdAgEAAoGBAKEfbzk6z6ACak8teTzcWR9Ota9N", string defaultRoleName = "user", string issuer = "ASPNETCORE", string audience = "ASPNETCORE")
        {

            AuthConfig.SecurityKey = securityKey;
            AuthConfig.DefautRole = defaultRoleName;
            AuthConfig.Issuer = issuer;
            AuthConfig.Audience = audience;
            return this;
        }

        public AuthBuilder Time(TimeSpan timeSpan)
        {
            AuthConfig.TimeSpan = timeSpan;
            return this;
        }

        public AuthBuilder Jump(string loginApi = "account/login",
            string logoutApi = "account/logout",
            string deniedApi = "account/error",
            bool isLogin = false,
            bool isDenied = false
            )
        {
            AuthConfig.LoginAction = loginApi;
            AuthConfig.Logout = logoutApi;
            AuthConfig.DeniedAction = deniedApi;
            AuthConfig.IsLoginAction = isLogin;
            AuthConfig.IsDeniedAction = isDenied;
            return this;
        }
        public AuthBuilder DefaultRole(string roleName)
        {
            AuthConfig.SetDefault(roleName);
            return this;
        }
        public void End()
        {

        }
    }
}
