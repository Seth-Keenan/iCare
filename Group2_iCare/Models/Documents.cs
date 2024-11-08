using System;
using System.ComponentModel.DataAnnotations;

namespace Group2_iCare.Models
{
    public class Documents
    {
        public string DocID { get; set; }


        public string DocName { get; set; }


        public string ModifiedByID { get; set; }


        public string PatientID { get; set; }

        public string WorkerID { get; set; } 

        public string Descript { get; set; }
    }
}
