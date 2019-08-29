using CZGL.Auth.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace CZGL.Auth.Interface
{


    /// <summary>
    /// 角色管理接口
    /// </summary>
    public interface IRolePermission
    {
        // static object locker;

        /// <summary>
        /// 获取所有角色以及授权 API
        /// </summary>
        /// <returns></returns>
        IEnumerable<RoleModel> GetAllRoles();

        /// <summary>
        /// 获取一个角色及其授权 API
        /// </summary>
        /// <param name="roleName"></param>
        /// <returns></returns>
        RoleModel GetRole(string roleName);

        /// <summary>
        /// 获取某个用户拥有的角色及API权限
        /// </summary>
        /// <param name="userName"></param>
        /// <returns></returns>
        IEnumerable<RoleModel> GetUserRoles(string userName);

        /// <summary>
        /// 获取某个用户拥有的角色
        /// </summary>
        /// <param name="userName"></param>
        /// <returns></returns>
        IEnumerable<string> GetUserRole(string userName);

        /// <summary>
        /// 用户是否属于此角色
        /// </summary>
        /// <param name="userName">用户名</param>
        /// <param name="roleName">角色名称</param>
        /// <returns></returns>
        bool IsUserToRole(string userName,string roleName);

        /// <summary>
        /// 是否存在某个角色
        /// </summary>
        /// <param name="roleName"></param>
        /// <returns></returns>
        bool IsHasRole(string roleName);

        /// <summary>
        /// 添加一个角色
        /// </summary>
        /// <param name="role"></param>
        /// <returns></returns>
        bool AddRole(RoleModel role);

        /// <summary>
        /// 移除一个角色
        /// </summary>
        /// <param name="roleName"></param>
        /// <returns></returns>
        bool RemoveRole(string roleName);

        /// <summary>
        /// 清除所有角色
        /// </summary>
        void Clear();
    }
}
