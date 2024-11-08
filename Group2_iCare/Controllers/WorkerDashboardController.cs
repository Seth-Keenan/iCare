using Group2_iCare.Models;
using System.Web.Mvc;

namespace Group2_iCare.Controllers
{
    public class WorkerDashboardController : Controller
    {
        // GET: WorkerDashboard
        public ActionResult Index()
        {
            var user = Session["User"] as iCAREUser;

            if (user == null)
            {
                return RedirectToAction("LoginForm", "UserAuthentication");
            }

            return View(user);
        }
    }
}