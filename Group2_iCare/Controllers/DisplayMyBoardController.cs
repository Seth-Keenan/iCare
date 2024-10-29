using Group2_iCare.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

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

            var patientRecords = db.PatientRecord.Where(pr => pr.WorkerID == user.ID).ToList();

            return View(patientRecords);
        }
    }
}