using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CZGL.Auth.Services
{
    /// <summary>
    /// 自定义实现验证需要的信息，数据是全局的
    /// </summary>
    public class AuthorizationRequirement: IAuthorizationRequirement
    {
        public int ID { get; set; } = 1;
    }
}
