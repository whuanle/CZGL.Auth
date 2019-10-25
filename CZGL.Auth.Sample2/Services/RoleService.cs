using CZGL.Auth.Sample2.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CZGL.Auth.Services;
using CZGL.Auth.Models;

namespace CZGL.Auth.Sample2.Services
{
    // 更新内存中的角色和用户信息
    public class RoleService : ManaRole
    {
        private readonly UserContext _context = new UserContext();

        public async Task UpdateRoleAndUser()
        {
            await Task.Run(() =>
            {
                // 从数据库中获取四个表的信息
                List<User> users = _context.Users.ToList();
                List<UserClaim> userClaims = _context.UserClaims.ToList();
                List<Role> roles = _context.Roles.ToList();
                List<RoleClaim> roleClaims = _context.RoleClaims.ToList();

                // 从数据库获取用户信息及其对应的角色
                foreach (var item in users)
                {
                    List<UserClaim> thatclaims = userClaims.Where(x => x.UserId == item.Id).ToList();
                    foreach (var item1 in thatclaims)
                    {
                        AddUser(new UserModel
                        {
                            UserName = item.UserName,
                            BeRoles = roles.Where(x => x.RoleId == item1.RoleId).Select(x => x.RoleName).ToList()
                        });
                    }
                }

                // 从数据库获取角色信息及其对应的API地址
                foreach (var item in roles)
                {
                    List<RoleClaim> thatroleclaims = roleClaims.Where(x => x.RoleId == item.RoleId).ToList();

                    foreach (var item1 in thatroleclaims)
                    {
                        AddRole(new RoleModel
                        {
                            RoleName = item.RoleName,
                            Apis = roleClaims.Where(x => x.RoleId == item1.RoleId)
                            .Select(x =>
                            new OneApiModel
                            {
                                ApiName = x.ApiName,
                                ApiUrl = x.ApiUrl
                            }).ToList()
                        });
                    }

                }

            });
        }
    }
}
