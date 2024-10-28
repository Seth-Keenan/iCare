using Group2_iCare.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace Group2_iCare.Controllers
{
    public class iCAREBoardController : Controller
    {
        private Group2_iCAREDBEntities db = new Group2_iCAREDBEntities();


        // GET: iCAREBoardForm
        public ActionResult iCAREBoardForm()
        {
            // Fetch the list of geocodes
            var geoCodes = db.GeoCodes.Select(gc => new SelectListItem
            {
                Value = gc.ID.ToString(), 
                Text = gc.Description 
            }).ToList();

            // Pass the geocode list to the view using ViewBag
            ViewBag.GeoCodeList = geoCodes;

            return View();
        }

        // GET: iCAREBoardResults
        public ActionResult iCAREBoardResult(GeoCodes code)
        {
            return View(db.PatientRecord.ToList());
        }

        // GET: PatientRecords/Details
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PatientRecord patientRecord = db.PatientRecord.Find(id);
            if (patientRecord == null)
            {
                return HttpNotFound();
            }
            return View(patientRecord);
        }
    }
}