//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Group2_iCare.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class UserPassword
    {
        public string ID { get; set; }
        public string UserName { get; set; }
        public string EncryptedPassword { get; set; }
        public Nullable<int> PasswordExpiryTime { get; set; }
        public Nullable<System.DateTime> UserAccountExpriyDate { get; set; }
    
        public virtual iCAREUser iCAREUser { get; set; }
    }
}
