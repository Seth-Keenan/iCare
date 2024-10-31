using System;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using Group2_iCare.Models;

namespace Group2_iCare.Controllers
{
    public class AdminCreateUsersController : Controller
    {
        private Group2_iCAREDBEntities db = new Group2_iCAREDBEntities();


        public ActionResult Index()
        {
            var users = db.iCAREUser
                .Include("UserPassword")
                .Include("iCAREAdmin")
                .Include("iCAREWorker")
                .ToList()
                .Select(user =>
                {
                    var viewModel = new AdminCreateUser
                    {
                        ID = user.ID,
                        Name = user.Name,
                        Role = "N/A",
                        UserName = "N/A",
                        Password = "N/A",
                        AdminEmail = "N/A",
                        Profession = "N/A"
                    };

                    if (user.UserPassword != null)
                    {
                        viewModel.UserName = user.UserPassword.UserName;
                        viewModel.Password = user.UserPassword.EncryptedPassword;
                    }

                    if (user.iCAREAdmin != null)
                    {
                        viewModel.Role = "Admin";
                        viewModel.AdminEmail = user.iCAREAdmin.AdminEmail;
                    }
                    else if (user.iCAREWorker != null)
                    {
                        viewModel.Role = "Worker";
                        viewModel.Profession = user.iCAREWorker.Profession;
                    }

                    return viewModel;
                }).ToList();

            return View(users);
        }

        // GET: AdminCreateUsers/Create
        public ActionResult Create()
        {
            return View();
        }


        // POST: AdminCreateUsers/Create
        [HttpPost]
        [ValidateAntiForgeryToken]

        public ActionResult Create(AdminCreateUser model)
        {
            if (ModelState.IsValid)
            {
                var newUser = new iCAREUser
                {
                    ID = model.ID,
                    Name = model.Name
                };
                db.iCAREUser.Add(newUser);

                var userPassword = new UserPassword
                {
                    ID = model.ID,
                    UserName = model.UserName,
                    EncryptedPassword = model.Password,
                    PasswordExpiryTime = model.PasswordExpiryTime,
                    UserAccountExpriyDate = model.UserAccountExpriyDate
                };
                db.UserPassword.Add(userPassword);

                if (model.Role == "Admin")
                {
                    var admin = new iCAREAdmin
                    {
                        ID = model.ID,
                        AdminEmail = model.AdminEmail,
                        DateHired = DateTime.Now
                    };
                    db.iCAREAdmin.Add(admin);
                }
                else if (model.Role == "Worker")
                {
                    var worker = new iCAREWorker
                    {
                        ID = model.ID,
                        Profession = model.Profession
                    };
                    db.iCAREWorker.Add(worker);
                }

                db.SaveChanges();

                return RedirectToAction("Index");
            }

            return View(model);
        }



        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var user = db.iCAREUser.Find(id);
            if (user == null)
            {
                return HttpNotFound();
            }

            var userPassword = db.UserPassword.Find(id);
            var admin = db.iCAREAdmin.Find(id);
            var worker = db.iCAREWorker.Find(id);

            string role = admin != null ? "Admin" : (worker != null ? "Worker" : "None");
            string profession = worker?.Profession;

            var viewModel = new AdminCreateUser
            {
                ID = user.ID,
                Name = user.Name,
                UserName = userPassword?.UserName,
                Password = userPassword?.EncryptedPassword,
                PasswordExpiryDate = userPassword?.UserAccountExpriyDate ?? DateTime.Now,
                Role = role,
                AdminEmail = admin?.AdminEmail,
                Profession = profession
            };

            return View(viewModel);
        }

        // GET: AdminCreateUsers/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var user = db.iCAREUser.Find(id);
            if (user == null)
            {
                return HttpNotFound();
            }

            var userPassword = db.UserPassword.Find(id);
            var admin = db.iCAREAdmin.Find(id);
            var worker = db.iCAREWorker.Find(id);

            var viewModel = new AdminCreateUser
            {
                ID = user.ID,
                Name = user.Name,
                UserName = userPassword?.UserName,
                Password = userPassword?.EncryptedPassword,
                PasswordExpiryDate = userPassword?.UserAccountExpriyDate ?? DateTime.Now,
                Role = admin != null ? "Admin" : "Worker",
                AdminEmail = admin?.AdminEmail,
                Profession = worker?.Profession
            };

            return View(viewModel);
        }

        // POST: AdminCreateUsers/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(AdminCreateUser model)
        {
            if (ModelState.IsValid)
            {

                var user = db.iCAREUser.Find(model.ID);
                if (user != null)
                {
                    user.Name = model.Name;
                }

                var userPassword = db.UserPassword.Find(model.ID);
                if (userPassword != null)
                {
                    userPassword.UserName = model.UserName;
                    userPassword.EncryptedPassword = model.Password;
                    userPassword.UserAccountExpriyDate = model.PasswordExpiryDate;
                }

                db.SaveChanges();

                return RedirectToAction("Index");
            }

            return View(model);
        }

        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var user = db.iCAREUser.Find(id);
            if (user == null)
            {
                return HttpNotFound();
            }

            var userPassword = db.UserPassword.Find(id);
            var admin = db.iCAREAdmin.Find(id);
            var worker = db.iCAREWorker.Find(id);

            var viewModel = new AdminCreateUser
            {
                ID = user.ID,
                Name = user.Name,
                UserName = userPassword?.UserName,
                Password = userPassword?.EncryptedPassword,
                PasswordExpiryDate = userPassword?.UserAccountExpriyDate ?? DateTime.Now,
                Role = admin != null ? "Admin" : "Worker",
                AdminEmail = admin?.AdminEmail,
                Profession = worker?.Profession
            };

            return View(viewModel);
        }

        // POST: AdminCreateUsers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]

        public ActionResult DeleteConfirmed(string id)
        {
            var user = db.iCAREUser.Find(id);
            var userPassword = db.UserPassword.Find(id);
            var admin = db.iCAREAdmin.Find(id);
            var worker = db.iCAREWorker.Find(id);

            if (admin != null) db.iCAREAdmin.Remove(admin);
            if (worker != null) db.iCAREWorker.Remove(worker);
            if (userPassword != null) db.UserPassword.Remove(userPassword);
            if (user != null) db.iCAREUser.Remove(user);

            db.SaveChanges();

            return RedirectToAction("Index");
        }


        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
