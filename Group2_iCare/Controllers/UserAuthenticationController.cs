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
                // Check if the user exists in the UserPassword table by username
                UserPassword userPassword = db.UserPassword.FirstOrDefault(up => up.UserName == user.UserName);
                if (userPassword == null)
                {
                    ModelState.AddModelError("", "Account does not exist with username: " + user.UserName);
                    return View(user);
                }

                // Verify the password
                if (userPassword.EncryptedPassword == user.Password && userPassword.UserName == user.UserName)
                {
                    // Check if the user is an Admin
                    bool isAdmin = db.iCAREAdmin.Any(a => a.ID == userPassword.ID);
                    //bool isWorker = db.iCAREWorker.Any(w => w.ID == userPassword.ID);

                    // Set role in session and redirect based on role
                    if (isAdmin)
                    {
                        Session["User"] = db.iCAREUser.Find(userPassword.ID); // Store worker in Session
                        TempData["LoginMessage"] = "Logged in Successfully";
                        return RedirectToAction("AdminDashboard", "ManageAccounts");
                    }
                    else if (worker != null) // Check if the user is a worker
                    {
                        Session["User"] = db.iCAREUser.Find(userPassword.ID); // Store worker in Session
                        TempData["LoginMessage"] = "Logged in Successfully";
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

