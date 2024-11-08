using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CsvHelper;

namespace Group2_iCare.Models
{
    public class Drug
    {
        public String DrugID { get; set; }
        public String DrugName { get; set; }

        public String DrugDescription { get; set; }
    }
}