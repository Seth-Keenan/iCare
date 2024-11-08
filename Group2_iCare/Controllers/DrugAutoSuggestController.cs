using Group2_iCare.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Group2_iCare.Controllers
{
    public class DrugController : Controller
    { // drug controller for drug suggestions
        private readonly DrugDictionary drugDictionary;
        // GET: Drug
        public DrugController()
        { // initialize drug dictionary
            var csvFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Models", "DrugInformation", "drugs.csv");
            drugDictionary = new DrugDictionary(csvFilePath);
        }

        [HttpGet]
        public JsonResult GetDrugSuggestions(string query) // get drug suggestions
        {
            var suggestions = drugDictionary.GetDrugSuggestions(query);
            return Json(suggestions, JsonRequestBehavior.AllowGet); // return suggestions
        }
    }
}