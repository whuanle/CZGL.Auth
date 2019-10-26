using CZGL.Auth.Models;
using CZGL.Auth.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyAuth
{
    public class RoleService : ManaRole
    {
        /// <summary>
        /// 用于加载角色禾API
        /// </summary>
        public void UpdateRole()
        {
            List<RoleModel> roles = new List<RoleModel>
            {
                new RoleModel
                {
                    RoleName="A",
                    Apis=new List<OneApiModel>
                    {
                        new OneApiModel
                        {
                            ApiName="A",
                            ApiUrl="/A"
                        },
                        new OneApiModel
                        {
                            ApiName="AB",
                            ApiUrl="/AB"
                        },
                        new OneApiModel
                        {
                            ApiName="AC",
                            ApiUrl="/AC"
                        },
                        new OneApiModel
                        {
                            ApiName="ABC",
                            ApiUrl="/ABC"
                        }
                    }
                },
                new RoleModel
                {
                    RoleName="B",
                    Apis=new List<OneApiModel>
                    {
                        new OneApiModel
                        {
                            ApiName="B",
                            ApiUrl="/B"
                        },
                        new OneApiModel
                        {
                            ApiName="AB",
                            ApiUrl="/AB"
                        },
                        new OneApiModel
                        {
                            ApiName="BC",
                            ApiUrl="/BC"
                        },
                        new OneApiModel
                        {
                            ApiName="ABC",
                            ApiUrl="/ABC"
                        }
                    }
                },
                new RoleModel
                {
                    RoleName="A",
                    Apis=new List<OneApiModel>
                    {
                        new OneApiModel
                        {
                            ApiName="A",
                            ApiUrl="/A"
                        },
                        new OneApiModel
                        {
                            ApiName="AB",
                            ApiUrl="/AB"
                        },
                        new OneApiModel
                        {
                            ApiName="AC",
                            ApiUrl="/AC"
                        },
                        new OneApiModel
                        {
                            ApiName="ABC",
                            ApiUrl="/ABC"
                        }
                    }
                }
            };
            foreach (var item in roles)
            {
                AddRole(item);
            }

        }

        public void UpdateUser()
        {
            AddUser(new UserModel { UserName = "aa", BeRoles = new List<string> { "A" } });
            AddUser(new UserModel { UserName = "bb", BeRoles = new List<string> { "B" } });
            AddUser(new UserModel { UserName = "cc", BeRoles = new List<string> { "C" } });
        }
    }
}
