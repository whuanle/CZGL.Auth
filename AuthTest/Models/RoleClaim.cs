using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AuthTest.Models
{
    public class RoleClaim
    {
        public int Id { get; set; }
        public string RoleId { get; set; }
        public string ApiName { get; set; }
        public string ApiUrl { get; set; }
    }
}
