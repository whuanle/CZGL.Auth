using System;
using System.Collections.Generic;
using System.Text;

namespace CZGL.Auth.Models
{
    public class RoleModel
    {
        public string RoleName { get; set; }
        public List<OneApiModel> Apis { get; set; }
    }
}
