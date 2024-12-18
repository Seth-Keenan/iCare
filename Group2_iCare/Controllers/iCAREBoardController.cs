﻿using Group2_iCare.Models;
using System.Linq;
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

        // POST: From iCAREBoardForm
        [HttpPost] // Needed for taking info in from user, not posting to the DB
        [ValidateAntiForgeryToken]
        public ActionResult iCAREBoardResult(string ID)
        {
            GeoCodes gc = db.GeoCodes.Find(ID);

            if (gc == null)
            {
                ModelState.AddModelError("GeoCode", "Invalid GeoCode selected");
                return RedirectToAction("iCAREBoardForm");
            }

            if (string.IsNullOrEmpty(gc.Description))
            {
                ModelState.AddModelError("Descripton", "Please choose a GeoCode");
            }

            var patientRecords = db.PatientRecord.Where(pr => pr.GeoCodeID == ID).ToList();

            ViewBag.SelectedGeoCode = gc.Description;

            return View(patientRecords);
        }
    }
}