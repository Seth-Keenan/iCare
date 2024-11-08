using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Group2_iCare.Models;

namespace Group2_iCare.Controllers
{
    public class AssignPatientController : Controller
    {
        private Group2_iCAREDBEntities db = new Group2_iCAREDBEntities();

        // GET: AssignPatient
        public ActionResult AssignPatientForm()
        { // assignment
            var user = Session["User"] as iCAREUser;
            var patients = db.PatientRecord.Where(u => u.WorkerID != user.ID && u.WorkerID == null).ToList();
            return View(patients);
        }

        // POST: AssignPatient
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AssignPatientForm(List<string> selectedPatientIds/*, workerId*/)
        { // assignment 
            var user = Session["User"] as iCAREUser;

            if (user == null) // if user is not logged in
            {
                return RedirectToAction("LoginForm", "UserAuthentication");
            }
            // if no patients are selected
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

            foreach (var patientId in selectedPatientIds) // assign patients to worker
            {
                var patient = db.PatientRecord.Find(patientId);
                if (patient != null)
                {
                    patient.WorkerID = user.ID;
                }
            }

            db.SaveChanges(); // save changes
            return RedirectToAction("AssignPatientForm");
        }
    }
}