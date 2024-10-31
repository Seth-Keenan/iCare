using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Group2_iCare.Models
{
    public class AdminCreateUser
    {
        internal DateTime PasswordExpiryDate;

        public string ID { get; set; }
        public string Name { get; set; }
        public string Role { get; set; }
        public string Profession { get; set; }
        public string AdminEmail { get; set; }
        public string UserName { get; set; }

        public string Password { get; set; }

        public int PasswordExpiryTime { get; set; }

        public DateTime UserAccountExpriyDate { get; set; }
    }
}