using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Group2_iCare.Models
{
    public class AdminCreateUser
    {
        internal DateTime PasswordExpiryDate;
        // Admin create user model that includes db connection for id, name, role, profession, admin email, username, password, password expiry time, and user account expiry date
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