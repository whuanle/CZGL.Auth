using CZGL.Auth.Interface;
using System;
using System.Collections.Generic;
using System.Text;

namespace CZGL.Auth.Models
{
    public class RoleModel : IRoleModel
    {
        public string RoleName { get; set; }
        public List<ApiPermission> Apis { get; set; }
    }
}
