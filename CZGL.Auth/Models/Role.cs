using CZGL.Auth.Interface;
using System;
using System.Collections.Generic;
using System.Text;

namespace CZGL.Auth.Models
{
    public class Role : IRole
    {
        public string RoleName { get; set; }
        public List<IApiPermission> Apis { get; set; }
    }
}
