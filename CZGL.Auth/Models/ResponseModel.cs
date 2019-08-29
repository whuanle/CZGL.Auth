using System;
using System.Collections.Generic;
using System.Text;

namespace CZGL.Auth.Models
{
    public class ResponseModel
    {
        public int Code { get; set; }
        public string Message { get; set; }
        public dynamic Data { get; set; }
    }
}
