using System;
using System.ComponentModel.DataAnnotations;

namespace Group2_iCare.Models
{
    public class Documents
    {
        //db connection that includes methods for document id, document name, modified by id, patient id, worker id, description
        public string DocID { get; set; }


        public string DocName { get; set; }


        public string ModifiedByID { get; set; }


        public string PatientID { get; set; }

        public string WorkerID { get; set; } 

        public string Descript { get; set; }
    }
}
