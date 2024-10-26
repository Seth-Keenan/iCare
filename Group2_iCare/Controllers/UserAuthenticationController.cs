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

        // GET: UserAuthentication
        public ActionResult LoginForm()
        {
            return View();
        }

        public ActionResult Login(UserAuthentication userAuth)
        {
            return View(userAuth);
        }

    }
}