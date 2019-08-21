using CZGL.Auth.Interface;
using CZGL.Auth.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;

namespace CZGL.Auth.Services
{
    public static class RolePermission
    {

        private static Dictionary<string, List<IApiPermission>> Roles { get; set; } = new Dictionary<string, List<IApiPermission>>();
        private static List<Role> UserRole
        {
            get
            {
                List<Role> roles = new List<Role>();
                foreach (var item in Roles)
                {
                    roles.Add(new Role() { RoleName = item.Key.ToLower(), Apis = item.Value });
                }
                return roles;
            }
        }

        public static List<Role> GetRoles()
        {
            return UserRole;
        }

        public static IEnumerable<Role> GetRole()
        {
            foreach (var item in Roles)
            {
                Role role = new Role()
                {
                    RoleName = item.Key.ToLower(),
                    Apis = item.Value
                };
                yield return role;
            }
        }

        public static Role GetRole(string roleName)
        {
            List<IApiPermission> roles;

            var re = Roles.TryGetValue(roleName.ToLower(),out roles);
            if (re == false) return null;

            Role role = new Role();
            role.RoleName = roleName;
            role.Apis = roles;

            return role;
        }

        public static bool AddRole(Role role)
        {
            if (Roles.ContainsKey(role.RoleName.ToLower()))
                return false;
            Roles.Add(role.RoleName.ToLower(), role.Apis);
            return true;
        }

        public static bool RemoveRole(string roleName)
        {
            bool result = Roles.Remove(roleName);
            return result;
        }

        public static void Clear()
        {
            Roles.Clear();
        }

    }
}
