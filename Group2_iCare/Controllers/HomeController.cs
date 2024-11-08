using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Group2_iCare.Controllers
{ // home controller
    public class HomeController : Controller
    {
        public ActionResult Index() // index
        {
            return View();
        }

        public ActionResult About() // about
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact() //contacts
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}