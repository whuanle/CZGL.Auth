using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace CZGL.Auth.Services
{
    public class AuthorizationPolicyProvider : IAuthorizationPolicyProvider
    {
        private readonly AuthorizationRequirement _requirement;
        public AuthorizationPolicyProvider()
        {
            _requirement = new AuthorizationRequirement();
        }
        public async  Task<AuthorizationPolicy> GetDefaultPolicyAsync()
        {
            await Task.CompletedTask;
            return null;
        }

        public async Task<AuthorizationPolicy> GetFallbackPolicyAsync()
        {
            await Task.CompletedTask;
            return null;
        }

        public async Task<AuthorizationPolicy> GetPolicyAsync(string policyName)
        {
            AuthorizationPolicy auth =await Task.FromResult(new AuthorizationPolicyBuilder().AddAuthenticationSchemes(JwtBearerDefaults.AuthenticationScheme)
                .AddRequirements(new AuthorizationRequirement()).Build());

            return auth;
        }
    }
}
