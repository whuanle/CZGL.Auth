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

    /// <summary>
    /// 将数据库的用户名和角色信息存储到内存中
    /// </summary>
    public static class PermissionInfo
    {
        // 锁
        public static object objLock = new object();
        // 用户名和所属角色
        public static List<UserInfo> Users { get; set; } = new List<UserInfo>();
        // 角色及其权限
        public static Dictionary<string, List<ApiPermission>> Roles { get; set; } = new Dictionary<string, List<ApiPermission>>();
    }
    public class UserInfo
    {
        public string Role { get; set; }
        public string UserName { get; set; }
    }
    /// <summary>
    /// 角色校验的类 
    /// </summary>
    public class RolePermission : IRolePermission
    {
        public RolePermission()
        {

        }

        private List<RoleModel> UserRole
        {
            get
            {
                List<RoleModel> roles = new List<RoleModel>();
                foreach (var item in PermissionInfo.Roles)
                {
                    roles.Add(new RoleModel() { RoleName = item.Key.ToLower(), Apis = item.Value });
                }
                return roles;
            }
        }

        public IEnumerable<RoleModel> GetAllRoles()
        {
            return UserRole;
        }

        public IEnumerable<RoleModel> GetUserRoles(string userName)
        {
            List<RoleModel> result = new List<RoleModel>();
            foreach (var item in PermissionInfo.Users)
            {
                if (item.UserName.ToLower() == userName.ToLower())
                    result.Add(GetRole(item.Role));
            }

            return result;
        }

        public IEnumerable<string> GetUserRole(string userName)
        {
            List<string> result = new List<string>();
            foreach (var item in PermissionInfo.Users)
            {
                if (item.UserName.ToLower() == userName.ToLower())
                    result.Add(item.Role);
            }

            return result;
        }
        public bool IsUserToRole(string userName, string roleName)
        {
            return PermissionInfo.Users.Any(x =>
                x.UserName.ToLower() == userName.ToLower()
                 &&
                x.Role.ToLower() == roleName.ToLower());
        }

        public bool IsHasRole(string roleName)
        {
            return PermissionInfo.Roles.Any(x => x.Key.ToLower() == roleName.ToLower());
        }
        public IEnumerable<RoleModel> GetRole()
        {
            foreach (var item in PermissionInfo.Roles)
            {
                RoleModel role = new RoleModel()
                {
                    RoleName = item.Key.ToLower(),
                    Apis = item.Value
                };
                yield return role;
            }
        }

        public RoleModel GetRole(string roleName)
        {

            var result = PermissionInfo.Roles.GetValueOrDefault(roleName.ToLower());
            if (result.Count==0) return null;

            RoleModel role = new RoleModel
            {
                RoleName = roleName,
                Apis = result
            };

            return role;
        }

        public bool AddRole(RoleModel role)
        {
            if (PermissionInfo.Roles.ContainsKey(role.RoleName.ToLower()))
                return false;
            PermissionInfo.Roles.Add(role.RoleName.ToLower(), role.Apis);
            return true;
        }

        public bool RemoveRole(string roleName)
        {
            bool result = PermissionInfo.Roles.Remove(roleName);
            return result;
        }

        public void Clear()
        {
            PermissionInfo.Roles.Clear();
        }

    }
}
