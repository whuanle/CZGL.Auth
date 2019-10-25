using System;
using System.Collections.Generic;
using System.Text;

namespace CZGL.Auth.Models
{
    public class UserModel
    {
        /// <summary>
        /// 用户名
        /// </summary>
        public string UserName { get; set; }
        /// <summary>
        /// 所属角色
        /// </summary>
        public List<string> BeRoles { get; set; }
    }
}
