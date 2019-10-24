using System;
using System.Collections.Generic;
using System.Text;

namespace CZGL.Auth.Models
{
    /// <summary>
    /// 每个API的附加信息，用于许可验证授权
    /// </summary>
    public  class ApiPermission
    {
        /// <summary>
        /// API名字
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// API地址
        /// </summary>
        public string Url { get; set; }
    }
}
