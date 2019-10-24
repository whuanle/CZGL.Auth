using System;
using System.Collections.Generic;
using System.Text;
using CZGL.Auth.Models;
using System.Linq;

namespace CZGL.Auth.Services
{
    public abstract class ManaRole
    {
        private static Dictionary<string, List<OneApiModel>> roleModel = new Dictionary<string, List<OneApiModel>>();
        private static Dictionary<string,List<string>> userModel = new Dictionary<string, List<string>>();
        public List<RoleModel> Roles
        { 
            get 
            {
                List<RoleModel> roles = new List<RoleModel>();
                foreach (var item in roleModel)
                {
                    roles.Add(new RoleModel() { RoleName = item.Key.ToLower(), Apis = item.Value });
                }
                return roles;
            } 
        }
        public List<UserModel> User
        {
            get
            {
                List<UserModel> users = new List<UserModel>();
                foreach (var item in userModel)
                {
                    users.Add(new UserModel() { UserName = item.Key.ToLower(), BeRole = item.Value });
                }
                return users;
            }
        }
        // 锁
        private static object objLock = new object();

        /// <summary>
        /// 检查是用户是否属于此角色
        /// </summary>
        /// <param name="userName">用户名</param>
        /// <param name="roleName">角色名</param>
        /// <returns></returns>
        public bool IsUserToRole(string userName, string roleName)
        {
            if (!userModel.ContainsKey(userName.ToLower())) return false;
            List<string> rolesList = userModel[userName.ToLower()];
            return rolesList.Any(x=>x == roleName.ToLower());
        }

        // 检查是否存在此角色
        public bool IsHasRole(string roleName)
        {
            return roleModel.Any(x => x.Key.ToLower() == roleName.ToLower());
        }

    }
}
