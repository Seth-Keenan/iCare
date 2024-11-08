using Group2_iCare.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Group2_iCare.Controllers
{
    public class DisplayMyBoardController : Controller
    {
        private Group2_iCAREDBEntities db = new Group2_iCAREDBEntities();

        // GET: DisplayMyBoard
        public ActionResult DisplayMyBoard()
        {
            var user = Session["User"] as iCAREUser;
            ViewBag.Username = user.Name;

            if (user == null)
            {
                return RedirectToAction("LoginForm", "UserAuthentication");
            }

            List<PatientRecord> patientRecords = null;
            if(user.Role == "DOCTOR")
            {
                patientRecords = db.PatientRecord.Where(pr => pr.WorkerID == user.ID).ToList();
            } else
            {
                patientRecords = db.PatientRecord
                    .ToList()
                    .Where(u => u.NID_Array != null && JArray.Parse(u.NID_Array).Any(id => id.ToString() == user.ID.ToString()))
                    .ToList();
            }

            return View(patientRecords);
        }
    }
}