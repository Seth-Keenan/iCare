using System;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Runtime.InteropServices;
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
                        Role = user.Role,
                        UserName = user.UserPassword?.UserName ?? "N/A",
                        Password = user.UserPassword?.EncryptedPassword ?? "N/A",
                        AdminEmail = user.iCAREAdmin?.AdminEmail ?? "N/A",
                        Profession = user.iCAREWorker?.Profession ?? "N/A"
                    };
                    return viewModel;
                }).ToList();

            var admins = users.Where(u => u.Role == "Admin").AsEnumerable();
            var workers = users.Where(u => u.Role == "Worker").AsEnumerable();
            var others = users.Where(u => u.Role != "Admin" && u.Role != "Worker");


            return View((admins, workers, others));
        }



        public ActionResult Create()
        {
            var roles = db.UserRole.Select(r => new { r.RoleName }).ToList();
            ViewBag.RoleList = new SelectList(roles, "RoleName", "RoleName");
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]

        public ActionResult Create(AdminCreateUser model)
        {
            if (ModelState.IsValid)
            {
                var newUser = new iCAREUser
                {
                    ID = model.ID,
                    Name = model.Name,
                    Role = model.Role
                };
                db.iCAREUser.Add(newUser);

                var userPassword = new UserPassword
                {
                    ID = model.ID,
                    UserName = model.UserName,
                    EncryptedPassword = model.Password,
                    PasswordExpiryTime = 365,
                    UserAccountExpriyDate = DateTime.Now.AddYears(1)
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

            string role = user.Role;
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
                Role = user.Role,
                AdminEmail = admin?.AdminEmail,
                Profession = worker?.Profession
            };

            var roles = db.UserRole.Select(r => new { r.RoleName }).ToList();
            ViewBag.RoleList = new SelectList(roles, "RoleName", "RoleName");

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
                    user.Role = model.Role;
                }

                var userPassword = db.UserPassword.Find(model.ID);
                var admin = db.iCAREAdmin.Find(model.ID);
                var worker = db.iCAREWorker.Find(model.ID);

                if (userPassword != null)
                {
                    userPassword.UserName = model.UserName;
                    userPassword.EncryptedPassword = model.Password;
                    userPassword.UserAccountExpriyDate = model.PasswordExpiryDate;
                }

                if(model.Role == "Admin")
                {
                    if(worker != null)
                    {
                        db.iCAREWorker.Remove(worker);
                    }
                    if(admin == null)
                    {
                        admin = new iCAREAdmin
                        {
                            ID = model.ID,
                            AdminEmail = model.AdminEmail
                        };
                        db.iCAREAdmin.Add(admin);
                    }
                    else
                    {
                        admin.AdminEmail = model.AdminEmail;
                    }
                }
                else
                {
                    if (admin != null)
                    {
                        db.iCAREAdmin.Remove(admin);
                    }

                    if (worker == null)
                    {
                        worker = new iCAREWorker
                        {
                            ID = model.ID,
                            Profession = model.Profession
                        };
                        db.iCAREWorker.Add(worker);
                    }
                    else
                    {
                        worker.Profession = model.Profession;
                    }
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
                Role = user.Role,
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
