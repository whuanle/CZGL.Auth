using CZGL.Auth.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace CZGL.Auth.Interface
{
    public interface IRoleModel
    {
         string RoleName { get; set; }
         List<ApiPermission> Apis { get; set; }
    }
}
