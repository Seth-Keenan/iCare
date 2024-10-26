﻿using System;
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
        [HttpPost] // Needed for taking info in from user, not posting to the DB
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
                    // RedirectToAction for the user based on their role

                    iCAREAdmin admin = db.iCAREAdmin.Find(userPassword.ID); // if null, user is not admin
                    if (admin != null) // Check if the user is the Admin
                    {
                        return RedirectToAction("AdminDashboard", "ManageAccounts");
                    }
                    //else
                    //{
                    //    return RedirectToAction("WorkerDashboard", "Worker");
                    //}
                }
                else
                {
                    ModelState.AddModelError("", "Invalid username or password.");
                }
            }

            return View(user);
        }
    }
}