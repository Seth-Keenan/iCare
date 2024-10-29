using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Group2_iCare.Models;
using System.Diagnostics;

namespace Group2_iCare.Controllers
{
    public class UserAuthenticationController : Controller
    {
        private Group2_iCAREDBEntities db = new Group2_iCAREDBEntities();

        // GET: User/Login
        public ActionResult LoginForm()
        {
            return View();
        }

        // POST: User/Login
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LoginForm(UserAuthentication user)
        {
            if (ModelState.IsValid)
            {
                UserPassword userPassword = db.UserPassword.FirstOrDefault(up => up.UserName == user.UserName);
                if (userPassword == null)
                {
                    ModelState.AddModelError("", "Account does not exist with username: " + user.UserName);
                    return View(user);
                }

                if (userPassword.EncryptedPassword == user.Password && userPassword.UserName == user.UserName)
                {
                    // Check for Admin role
                    iCAREAdmin admin = db.iCAREAdmin.Find(userPassword.ID);
                    iCAREWorker worker = db.iCAREWorker.Find(userPassword.ID);

                    if (admin != null) 
                    {
                        Session["User"] = db.iCAREUser.Find(userPassword.ID);
                        Session["UserRole"] = "Admin"; 
                        return RedirectToAction("AdminDashboard", "ManageAccounts");
                    }
                    else if (worker != null)
                    {
                        Session["User"] = db.iCAREUser.Find(userPassword.ID);
                        Session["UserRole"] = "Worker"; 
                        return RedirectToAction("Index", "WorkerDashboard");
                    }
                }
                else
                {
                    ModelState.AddModelError("", "Invalid username or password.");
                }
            }

            return View(user);
        }

        // Logout method
        public ActionResult Logout()
        {
            Session.Clear(); // Clear all session data
            return RedirectToAction("Index", "Home"); // Redirect to the home page or login page
        }
    }
}
