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
    
    public partial class Files
    {
        public string ID { get; set; }
        public string FileName { get; set; }
        public string ContentType { get; set; }
        public byte[] Data { get; set; }
        public string UploadedByID { get; set; }
        public Nullable<System.DateTime> UploadDate { get; set; }
        public string Descript { get; set; }
        public string FilePath { get; set; }
    
        public virtual iCAREUser iCAREUser { get; set; }
    }
}
