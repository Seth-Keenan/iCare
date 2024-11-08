using System;
using System.Collections.Generic;
using System.Data.Entity.SqlServer;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Newtonsoft.Json;
using Group2_iCare.Models;
using Newtonsoft.Json.Linq;

namespace Group2_iCare.Controllers
{

    public class AssignPatientController : Controller
    {
        private Group2_iCAREDBEntities db = new Group2_iCAREDBEntities();

        // GET: AssignPatient
        public ActionResult AssignPatientForm()
        {
            var user = Session["User"] as iCAREUser;
            List<PatientRecord> patients = null;
            if (user.Role == "DOCTOR")
            {
                patients = db.PatientRecord.Where(u => u.WorkerID == null).ToList();
            }
            else if (user.Role == "NURSE")
            {
                patients = db.PatientRecord.Where(u => JArray.Parse(u.NID_Array).Count < 3).ToList();
            }
            return View(patients);
        }

        // POST: AssignPatient
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AssignPatientForm(List<string> selectedPatientIds/*, workerId*/)
        {
            var user = Session["User"] as iCAREUser;

            if (user == null)
            {
                return RedirectToAction("LoginForm", "UserAuthentication");
            }

            if (selectedPatientIds == null || !selectedPatientIds.Any())
            {
                ModelState.AddModelError("", "No patients selected.");
                return RedirectToAction("AssignPatientForm");
            }

            //if (string.IsNullOrEmpty(workerId))
            //{
            //    ModelState.AddModelError("", "Worker ID is required.");
            //    return RedirectToAction("AssignPatientForm");
            //}

            foreach (var patientId in selectedPatientIds)
            {
                var patient = db.PatientRecord.Find(patientId);
                if (patient != null)
                {
                    patient.WorkerID = user.ID;
                }
            }

            db.SaveChanges();
            return RedirectToAction("AssignPatientForm");
        }
    }
}