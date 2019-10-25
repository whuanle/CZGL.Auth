using System;
using System.Collections.Generic;
using System.Text;

namespace CZGL.Auth.Models
{
    public class EventsInfo
    {
        public string UserName { get; set; }
        public string RoleName { get; set; }
        public string Url { get; set; }
        public string Issuer { get; set; }
        public IEnumerable<string> Audience { get; set; }
        public string Token { get; set; }
    }
}
