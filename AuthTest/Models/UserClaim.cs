﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AuthTest.Models
{
    public class UserClaim
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string RoleId { get; set; }
    }
}
