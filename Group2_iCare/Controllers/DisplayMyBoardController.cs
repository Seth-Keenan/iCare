using Group2_iCare.Models;
using System.Linq;
using System.Web.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;

namespace Group2_iCare.Controllers
{
    public class DisplayMyBoardController : Controller
    {
        private Group2_iCAREDBEntities db = new Group2_iCAREDBEntities(); // db connection

        // GET: DisplayMyBoard
        public ActionResult DisplayMyBoard()
        { // display my board if user is logged in
            var user = Session["User"] as iCAREUser;
            ViewBag.Username = user.Name;

            if (user == null) // if user is not logged in
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

        public ActionResult ReleasePatient(string patientID)
        {
            var user = Session["User"] as iCAREUser;

            if (user == null)
            {
                return RedirectToAction("LoginForm", "UserAuthentication");
            }

            if (user.Role == "DOCTOR")
            {
                var patient = db.PatientRecord.Find(patientID);
                patient.WorkerID = null;

            } else
            {
                var patient = db.PatientRecord.Find(patientID);
                var nurses = JArray.Parse(patient.NID_Array).ToList();
                nurses.Remove(user.ID);
                patient.NID_Array = JsonConvert.SerializeObject(nurses);
            }

            db.SaveChanges();


            return RedirectToAction("DisplayMyBoard", "DisplayMyBoard");

        }

    }
}