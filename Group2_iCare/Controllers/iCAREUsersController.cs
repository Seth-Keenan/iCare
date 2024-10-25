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
        private Group2_iCAREDBEntities1 db = new Group2_iCAREDBEntities1();

        // GET: iCAREUsers
        public ActionResult Index()
        {
            var iCAREUser = db.iCAREUser.Include(i => i.UserPassword);
            return View(iCAREUser.ToList());
        }

        // GET: iCAREUsers/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            iCAREUser iCAREUser = db.iCAREUser.Find(id);
            if (iCAREUser == null)
            {
                return HttpNotFound();
            }
            return View(iCAREUser);
        }

        // GET: iCAREUsers/Create
        public ActionResult Create()
        {
            ViewBag.ID = new SelectList(db.UserPassword, "ID", "UserName");
            return View();
        }

        // POST: iCAREUsers/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,Name")] iCAREUser iCAREUser)
        {
            if (ModelState.IsValid)
            {
                db.iCAREUser.Add(iCAREUser);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.ID = new SelectList(db.UserPassword, "ID", "UserName", iCAREUser.ID);
            return View(iCAREUser);
        }

        // GET: iCAREUsers/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            iCAREUser iCAREUser = db.iCAREUser.Find(id);
            if (iCAREUser == null)
            {
                return HttpNotFound();
            }
            ViewBag.ID = new SelectList(db.UserPassword, "ID", "UserName", iCAREUser.ID);
            return View(iCAREUser);
        }

        // POST: iCAREUsers/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,Name")] iCAREUser iCAREUser)
        {
            if (ModelState.IsValid)
            {
                db.Entry(iCAREUser).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.ID = new SelectList(db.UserPassword, "ID", "UserName", iCAREUser.ID);
            return View(iCAREUser);
        }

        // GET: iCAREUsers/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            iCAREUser iCAREUser = db.iCAREUser.Find(id);
            if (iCAREUser == null)
            {
                return HttpNotFound();
            }
            return View(iCAREUser);
        }

        // POST: iCAREUsers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            iCAREUser iCAREUser = db.iCAREUser.Find(id);
            db.iCAREUser.Remove(iCAREUser);
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
