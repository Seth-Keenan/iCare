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
        private Group2_ICAREDBEntities db = new Group2_ICAREDBEntities();

        // GET: UserPasswords
        public ActionResult Index()
        {
            var userPasswords = db.UserPasswords.Include(u => u.iCAREUser);
            return View(userPasswords.ToList());
        }

        // GET: UserPasswords/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            UserPassword userPassword = db.UserPasswords.Find(id);
            if (userPassword == null)
            {
                return HttpNotFound();
            }
            return View(userPassword);
        }

        // GET: UserPasswords/Create
        public ActionResult Create()
        {
            ViewBag.ID = new SelectList(db.iCAREUsers, "ID", "Name");
            return View();
        }

        // POST: UserPasswords/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,UserName,EncryptedPassword,PasswordExpiryTime,UserAccountExpriyDate")] UserPassword userPassword)
        {
            if (ModelState.IsValid)
            {
                db.UserPasswords.Add(userPassword);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.ID = new SelectList(db.iCAREUsers, "ID", "Name", userPassword.ID);
            return View(userPassword);
        }

        // GET: UserPasswords/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            UserPassword userPassword = db.UserPasswords.Find(id);
            if (userPassword == null)
            {
                return HttpNotFound();
            }
            ViewBag.ID = new SelectList(db.iCAREUsers, "ID", "Name", userPassword.ID);
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
                db.Entry(userPassword).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.ID = new SelectList(db.iCAREUsers, "ID", "Name", userPassword.ID);
            return View(userPassword);
        }

        // GET: UserPasswords/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            UserPassword userPassword = db.UserPasswords.Find(id);
            if (userPassword == null)
            {
                return HttpNotFound();
            }
            return View(userPassword);
        }

        // POST: UserPasswords/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            UserPassword userPassword = db.UserPasswords.Find(id);
            db.UserPasswords.Remove(userPassword);
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
