using Group2_iCare.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Group2_iCare.Controllers
{ // manage accounts controller
    public class ManageAccountsController : Controller
    {
        private Group2_iCAREDBEntities db = new Group2_iCAREDBEntities();

        // GET: ManageAccounts
        public ActionResult AdminDashboard()
        {
            var user = Session["User"] as iCAREUser; // get user session

            if (user == null) // if user is not logged in 
            {
                return RedirectToAction("LoginForm", "UserAuthentication");
            }

            return View(user);
        }
    }
}