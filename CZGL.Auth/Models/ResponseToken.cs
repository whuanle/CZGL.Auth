using System;
using System.Collections.Generic;
using System.Text;

namespace CZGL.Auth.Models
{
    public class ResponseToken
    {
        public bool Status { get; set; }
        public string Access_Token { get; set; }
        public double Expires_In { get; set; }
        public string Token_Type { get; set; }
    }
}
