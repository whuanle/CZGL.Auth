using CZGL.Auth.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace CZGL.Auth.Interface
{
    public interface IRole
    {
         string RoleName { get; set; }
         List<IApiPermission> Apis { get; set; }
    }
}
