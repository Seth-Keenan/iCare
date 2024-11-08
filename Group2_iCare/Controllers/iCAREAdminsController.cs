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
    public class iCAREAdminsController : Controller
    {
        private Group2_iCAREDBEntities db = new Group2_iCAREDBEntities();

        // GET: iCAREAdmins
        public ActionResult Index()
        { // display all iCAREAdmin
            var iCAREAdmin = db.iCAREAdmin.Include(i => i.iCAREUser);
            return View(iCAREAdmin.ToList());
        }

        // GET: iCAREAdmins/Details/5
        public ActionResult Details(string id)
        {
            if (id == null) // id is null return error
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            iCAREAdmin iCAREAdmin = db.iCAREAdmin.Find(id); // find the iCAREAdmin by id if null return error
            if (iCAREAdmin == null)
            {
                return HttpNotFound();
            }
            return View(iCAREAdmin);
        }

        // GET: iCAREAdmins/Create
        public ActionResult Create()
        { // create a new iCAREAdmin
            ViewBag.ID = new SelectList(db.iCAREUser, "ID", "Name");
            return View();
        }

        // POST: iCAREAdmins/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,AdminEmail,DateHired,DateFinished")] iCAREAdmin iCAREAdmin)
        {
            if (ModelState.IsValid) // if model state is valid
            {
                db.iCAREAdmin.Add(iCAREAdmin); // add the iCAREAdmin
                db.SaveChanges(); // save changes
                return RedirectToAction("Index");
            }

            ViewBag.ID = new SelectList(db.iCAREUser, "ID", "Name", iCAREAdmin.ID); // return the iCAREAdmin
            return View(iCAREAdmin);
        }

        // GET: iCAREAdmins/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            iCAREAdmin iCAREAdmin = db.iCAREAdmin.Find(id); // find the iCAREAdmin by id
            if (iCAREAdmin == null)
            {
                return HttpNotFound();
            }
            ViewBag.ID = new SelectList(db.iCAREUser, "ID", "Name", iCAREAdmin.ID); // return the iCAREAdmin
            return View(iCAREAdmin);
        }

        // POST: iCAREAdmins/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,AdminEmail,DateHired,DateFinished")] iCAREAdmin iCAREAdmin)
        { 
            if (ModelState.IsValid) // if model state is valid
            {
                db.Entry(iCAREAdmin).State = EntityState.Modified; // edit the iCAREAdmin
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.ID = new SelectList(db.iCAREUser, "ID", "Name", iCAREAdmin.ID); // select list of iCAREUser
            return View(iCAREAdmin); // return the iCAREAdmin
        }

        // GET: iCAREAdmins/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null) // if null return error
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            iCAREAdmin iCAREAdmin = db.iCAREAdmin.Find(id); // find the iCAREAdmin by id
            if (iCAREAdmin == null)
            {
                return HttpNotFound();
            }
            return View(iCAREAdmin); // return the iCAREAdmin
        }

        // POST: iCAREAdmins/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            iCAREAdmin iCAREAdmin = db.iCAREAdmin.Find(id); // find the iCAREAdmin by id
            db.iCAREAdmin.Remove(iCAREAdmin); // remove and save
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        { //dispose
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
