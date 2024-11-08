using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Group2_iCare.Models;

namespace Group2_iCare.Controllers
{
    public class iCAREUsersController : Controller
    {
        private Group2_iCAREDBEntities db = new Group2_iCAREDBEntities();

        // GET: iCAREUsers
        public ActionResult Index()
        { 
            var iCAREUser = db.iCAREUser.Include(i => i.UserPassword); // display all users
            return View(iCAREUser.ToList()); // return the users
        }

        // GET: iCAREUsers/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            iCAREUser iCAREUser = db.iCAREUser.Find(id); // find the user by id
            if (iCAREUser == null)
            {
                return HttpNotFound();
            }
            return View(iCAREUser); // return the user
        }

        // GET: iCAREUsers/Create
        public ActionResult Create()
        {
            ViewBag.ID = new SelectList(db.UserPassword, "ID", "UserName");
            return View();
        }

        // POST: iCAREUsers/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,Name")] iCAREUser iCAREUser)
        {
            if (ModelState.IsValid)
            {
                db.iCAREUser.Add(iCAREUser); // add the user
                db.SaveChanges(); // save changes
                return RedirectToAction("Index");
            }

            ViewBag.ID = new SelectList(db.UserPassword, "ID", "UserName", iCAREUser.ID); // return the user
            return View(iCAREUser);
        }

        // GET: iCAREUsers/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null) // if id is null return error
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            iCAREUser iCAREUser = db.iCAREUser.Find(id); // find the user by id
            if (iCAREUser == null)
            {
                return HttpNotFound();
            }
            ViewBag.ID = new SelectList(db.UserPassword, "ID", "UserName", iCAREUser.ID); // return the user
            return View(iCAREUser);
        }

        // POST: iCAREUsers/Edit/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,Name")] iCAREUser iCAREUser)
        { // edit the user
            if (ModelState.IsValid)
            { // if model state is valid
                db.Entry(iCAREUser).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.ID = new SelectList(db.UserPassword, "ID", "UserName", iCAREUser.ID); // return the user
            return View(iCAREUser);
        }

        // GET: iCAREUsers/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null) // if id is null return error
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            iCAREUser iCAREUser = db.iCAREUser.Find(id); // find the user by id
            if (iCAREUser == null) // if user is null
            {
                return HttpNotFound();
            }
            return View(iCAREUser); // return the user
        }

        // POST: iCAREUsers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            var userPasswords = db.UserPassword.Where(up => up.ID == id).ToList(); // find the user passwords
            foreach (var userPassword in userPasswords)
            {
                db.UserPassword.Remove(userPassword); // remove the user password
            }
            iCAREUser iCAREUser = db.iCAREUser.Find(id); // find the user
            db.iCAREUser.Remove(iCAREUser); // remove the user
            db.SaveChanges();
            return RedirectToAction("Index"); // return to index
        }

        protected override void Dispose(bool disposing)
        { // dispose
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
