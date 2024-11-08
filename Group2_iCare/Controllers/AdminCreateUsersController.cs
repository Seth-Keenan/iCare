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
        { //used to display the list of users in the database
            var users = db.iCAREUser
                .Include("UserPassword")
                .Include("iCAREAdmin")
                .Include("iCAREWorker")
                .ToList()
                .Select(user =>
                { //used to display the list of users in the database
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
            //used to display the list of users in the database
            var admins = users.Where(u => u.Role == "ADMIN").AsEnumerable();
            var workers = users.Where(u => u.Role == "WORKER").AsEnumerable();
            var others = users.Where(u => u.Role != "ADMIN" && u.Role != "WORKER");

            //return to view
            return View((admins, workers, others));
        }


        // GET: AdminCreateUsers/Create
        public ActionResult Create()
        {
            var roles = db.UserRole.Select(r => new { r.RoleName }).ToList();
            ViewBag.RoleList = new SelectList(roles, "RoleName", "RoleName");
            return View();
        }

        // POST: AdminCreateUsers/Create
        [HttpPost]
        [ValidateAntiForgeryToken]

        //used to create a new user
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

                if (model.Role == "ADMIN")
                {
                    var admin = new iCAREAdmin
                    {
                        ID = model.ID,
                        AdminEmail = model.AdminEmail,
                        DateHired = DateTime.Now
                    };
                    db.iCAREAdmin.Add(admin);
                }
                else if (model.Role == "WORKER")
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


        // GET: AdminCreateUsers/Details
        public ActionResult Details(string id)
        {
            if (id == null) // if the id is null return error
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            //find the user by id
            var user = db.iCAREUser.Find(id);
            //if the user is null return error
            if (user == null)
            {
                return HttpNotFound();
            }
            //find the user password, admin and worker by id
            var userPassword = db.UserPassword.Find(id);
            var admin = db.iCAREAdmin.Find(id);
            var worker = db.iCAREWorker.Find(id);
            //get the profession and role of the user
            string role = user.Role;
            string profession = worker?.Profession;
            // create a new view model to display the user details
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
            if (id == null) // if the id is null return error
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var user = db.iCAREUser.Find(id); // find the user by id
            if (user == null)
            {
                return HttpNotFound();
            }
            // find the user password, admin and worker by id
            var userPassword = db.UserPassword.Find(id);
            var admin = db.iCAREAdmin.Find(id);
            var worker = db.iCAREWorker.Find(id);
            // create a new view model to display the user details
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
            // get the role list from the database
            var roles = db.UserRole.Select(r => new { r.RoleName }).ToList();
            ViewBag.RoleList = new SelectList(roles, "RoleName", "RoleName");

            return View(viewModel);
        }

        // POST: AdminCreateUsers/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(AdminCreateUser model)
        {
            if (ModelState.IsValid) // if the model state is valid
            {

                var user = db.iCAREUser.Find(model.ID); // find the user by id
                if (user != null) // if user is not null set
                {
                    user.Name = model.Name;
                    user.Role = model.Role;
                }
                // find the user password, admin and worker by id
                var userPassword = db.UserPassword.Find(model.ID);
                var admin = db.iCAREAdmin.Find(model.ID);
                var worker = db.iCAREWorker.Find(model.ID);
                // if the user password is not null set 
                if (userPassword != null)
                {
                    userPassword.UserName = model.UserName;
                    userPassword.EncryptedPassword = model.Password;
                    userPassword.UserAccountExpriyDate = model.PasswordExpiryDate;
                }
                // if the role is admin
                if (model.Role == "ADMIN")
                {
                    if(worker != null) // if worker isnt null remove
                    {
                        db.iCAREWorker.Remove(worker);
                    }
                    if(admin == null) // if admin null create new admin
                    {
                        admin = new iCAREAdmin // create new
                        {
                            ID = model.ID,
                            AdminEmail = model.AdminEmail
                        };
                        db.iCAREAdmin.Add(admin);
                    }
                    else
                    {
                        admin.AdminEmail = model.AdminEmail; // set the email
                    }
                }
                else if (model.Role == "WORKER") // if role is worker
                {
                    if (admin != null) // if admin isnt null remove
                    {
                        db.iCAREAdmin.Remove(admin);
                    }

                    if (worker == null) // if worker is null create worker
                    {
                        worker = new iCAREWorker
                        {
                            ID = model.ID,
                            Profession = model.Profession
                        };
                        db.iCAREWorker.Add(worker);
                    }
                    else
                    { // set the profession
                        worker.Profession = model.Profession;
                    }
                }


                db.SaveChanges(); // save changes

                return RedirectToAction("Index"); // return to index
            }

            return View(model); 
        }

        // GET: AdminCreateUsers/Delete
        public ActionResult Delete(string id)
        {
            if (id == null) // if id is null return error
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var user = db.iCAREUser.Find(id); // find the user by id
            if (user == null)
            {
                return HttpNotFound(); // if user is null return error
            }
            // find the user password, admin and worker by id
            var userPassword = db.UserPassword.Find(id);
            var admin = db.iCAREAdmin.Find(id);
            var worker = db.iCAREWorker.Find(id);

            var viewModel = new AdminCreateUser // create a new view model to display the user details
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

        public ActionResult DeleteConfirmed(string id) // delete the user
        { // find the user by id, user password, admin and worker
            var user = db.iCAREUser.Find(id); 
            var userPassword = db.UserPassword.Find(id);
            var admin = db.iCAREAdmin.Find(id);
            var worker = db.iCAREWorker.Find(id);
            // if not null remove
            if (admin != null) db.iCAREAdmin.Remove(admin);
            if (worker != null) db.iCAREWorker.Remove(worker);
            if (userPassword != null) db.UserPassword.Remove(userPassword);
            if (user != null) db.iCAREUser.Remove(user);

            db.SaveChanges(); // save changes

            return RedirectToAction("Index");
        }

        // Dispose
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
