using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AuthTest.Models
{
    /// <summary>
    /// 预设用户
    /// </summary>
    public static class UserModel
    {
        public static List<UserInfo> Users { get; set; }
        static UserModel()
        {
            Users = new List<UserInfo>() {
            new UserInfo{ Role="SupperAdmin",UserName="supperAdmin",UserPossword="123456"},
            new UserInfo{ Role="Admin",UserName="admin",UserPossword="123456"},
            new UserInfo{ Role="User",UserName="user",UserPossword="123456"}
        };
        }
    }
    public class UserInfo
    {
        public string Role { get; set; }
        public string UserName { get; set; }
        public string UserPossword { get; set; }
    }
}
