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
    public class UserPasswordsController : Controller
    {
        private Group2_iCAREDBEntities db = new Group2_iCAREDBEntities();

        // GET: UserPasswords
        public ActionResult Index()
        {
            var userPassword = db.UserPassword.Include(u => u.iCAREUser); // display all user passwords
            return View(userPassword.ToList()); // return the user passwords
        }

        // GET: UserPasswords/Details/5
        public ActionResult Details(string id)
        {
            if (id == null) // id is null
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            UserPassword userPassword = db.UserPassword.Find(id); // find the user password by id
            if (userPassword == null)
            {
                return HttpNotFound();
            }
            return View(userPassword); // return the user password
        }

        // GET: UserPasswords/Create
        public ActionResult Create()
        {
            ViewBag.ID = new SelectList(db.iCAREUser, "ID", "Name"); // select the user
            return View();
        }

        // POST: UserPasswords/Create

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,UserName,EncryptedPassword,PasswordExpiryTime,UserAccountExpriyDate")] UserPassword userPassword)
        {
            if (ModelState.IsValid)
            {
                db.UserPassword.Add(userPassword); // add the user password
                db.SaveChanges(); // save changes
                return RedirectToAction("Index");
            }

            ViewBag.ID = new SelectList(db.iCAREUser, "ID", "Name", userPassword.ID); // return the user password
            return View(userPassword); // return the user password
        }

        // GET: UserPasswords/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest); // if id is null return error
            }
            UserPassword userPassword = db.UserPassword.Find(id); // find the user password by id
            if (userPassword == null)
            {
                return HttpNotFound();
            }
            ViewBag.ID = new SelectList(db.iCAREUser, "ID", "Name", userPassword.ID); // return the user password
            return View(userPassword);
        }

        // POST: UserPasswords/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,UserName,EncryptedPassword,PasswordExpiryTime,UserAccountExpriyDate")] UserPassword userPassword)
        {
            if (ModelState.IsValid)
            {
                db.Entry(userPassword).State = EntityState.Modified; // edit the user password
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.ID = new SelectList(db.iCAREUser, "ID", "Name", userPassword.ID); // return the user password
            return View(userPassword);
        }

        // GET: UserPasswords/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest); 
            }
            UserPassword userPassword = db.UserPassword.Find(id); // find the user password by id
            if (userPassword == null)
            {
                return HttpNotFound();
            }
            return View(userPassword); // return the user password
        }

        // POST: UserPasswords/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id) // delete the user password
        {
            UserPassword userPassword = db.UserPassword.Find(id); // find the user password
            db.UserPassword.Remove(userPassword); // remove the user password
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
