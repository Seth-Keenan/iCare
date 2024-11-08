using Group2_iCare.Models;
using System.Web.Mvc;

namespace Group2_iCare.Controllers
{
    public class ManageAccountsController : Controller
    {
        private Group2_iCAREDBEntities db = new Group2_iCAREDBEntities();

        // GET: ManageAccounts
        public ActionResult AdminDashboard()
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