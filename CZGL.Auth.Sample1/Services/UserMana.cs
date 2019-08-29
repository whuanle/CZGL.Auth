using CZGL.Auth.Models;
using CZGL.Auth.Sample1.Models;
using CZGL.Auth.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CZGL.Auth.Sample1.Services
{
    /// <summary>
    /// 更新内存中的用户、角色信息
    /// </summary>
    public class UserMana
    {
        private readonly UserContext _context;
        public UserMana(UserContext context)
        {
            _context = context;
        }

        /// <summary>
        /// 更新一下内存中的用户名及对应的角色
        /// </summary>
        /// <returns></returns>
        public async Task UpdateUser()
        {
            await Task.Run(()=>
            {
                lock (PermissionInfo.objLock)
                {
                    // 从数据库中获取四个表的信息
                    List<User> users = _context.Users.ToList();
                    List<UserClaim> userClaims = _context.UserClaims.ToList();
                    List<Role> roles = _context.Roles.ToList();
                    List<RoleClaim> roleClaims = _context.RoleClaims.ToList();



                    List<UserInfo> userInfos = new List<UserInfo>();
                    Dictionary<string, List<ApiPermission>> Roles = new Dictionary<string, List<ApiPermission>>();

                    // 刷新内存中的用户信息
                    foreach (var item in users)
                    {
                        List<UserClaim> thatclaims = userClaims.Where(x => x.UserId == item.Id).ToList();
                        foreach (var item1 in thatclaims)
                        {
                            userInfos.Add(new UserInfo
                            {
                                UserName = item.UserName,
                                Role = roles.FirstOrDefault(x => x.RoleId == item1.RoleId).RoleName
                            });
                        }
                    }

                    // 刷新内存中角色信息
                    foreach (var item in roles)
                    {
                        List<RoleClaim> thatroleclaims = roleClaims.Where(x => x.RoleId == item.RoleId).ToList();
                        List<ApiPermission> apis = new List<ApiPermission>();
                        foreach (var item1 in thatroleclaims)
                        {
                            apis.Add(new ApiPermission
                            {
                                Name = item1.ApiName,
                                Url = item1.ApiUrl
                            });
                        }
                        Roles.Add(item.RoleName.ToLower(), apis);
                    }

                    // 刷新
                    PermissionInfo.Users = userInfos;
                    PermissionInfo.Roles = Roles;
                }
            });
        }

    }
}
