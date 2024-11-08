﻿using Group2_iCare.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Group2_iCare.Controllers
{
    public class WorkerDashboardController : Controller
    {
        // GET: WorkerDashboard
        public ActionResult Index()
        {
            var user = Session["User"] as iCAREUser; // get user session

            if (user == null)
            {
                return RedirectToAction("LoginForm", "UserAuthentication"); // if user is not logged in
            }

            return View(user);
        }
    }
}