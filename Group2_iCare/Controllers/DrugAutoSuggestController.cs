using Group2_iCare.Models;
using System;
using System.IO;
using System.Web.Mvc;

namespace Group2_iCare.Controllers
{
    public class DrugController : Controller
    {
        private readonly DrugDictionary drugDictionary;

        public DrugController()
        {
            var csvFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Models", "DrugInformation", "drugs.csv");
            drugDictionary = new DrugDictionary(csvFilePath);
        }

        [HttpGet]
        public JsonResult GetDrugSuggestions(string query)
        {
            var suggestions = drugDictionary.GetDrugSuggestions(query);
            return Json(suggestions, JsonRequestBehavior.AllowGet);
        }
    }
}