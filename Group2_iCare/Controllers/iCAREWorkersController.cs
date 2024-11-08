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
    public class iCAREWorkersController : Controller
    { // iCARE workers controller
        private Group2_iCAREDBEntities db = new Group2_iCAREDBEntities();

        // GET: iCAREWorkers
        public ActionResult Index()
        {
            var iCAREWorker = db.iCAREWorker.Include(i => i.iCAREUser);
            return View(iCAREWorker.ToList());
        }

        // GET: iCAREWorkers/Details/5
        public ActionResult Details(string id)
        {
            if (id == null) // id is null
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            iCAREWorker iCAREWorker = db.iCAREWorker.Find(id); // find the iCAREWorker by id
            if (iCAREWorker == null)
            {
                return HttpNotFound();
            }
            return View(iCAREWorker); // return the iCAREWorker
        }

        // GET: iCAREWorkers/Create
        public ActionResult Create()
        {
            ViewBag.ID = new SelectList(db.iCAREUser, "ID", "Name");
            return View();
        }

        // POST: iCAREWorkers/Create

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,Profession")] iCAREWorker iCAREWorker)
        {
            if (ModelState.IsValid) // if model state is valid
            {
                db.iCAREWorker.Add(iCAREWorker); // add the iCAREWorker
                db.SaveChanges(); // save changes
                return RedirectToAction("Index"); // return to index
            }

            ViewBag.ID = new SelectList(db.iCAREUser, "ID", "Name", iCAREWorker.ID);
            return View(iCAREWorker); // return the iCAREWorker
        }

        // GET: iCAREWorkers/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null) // if id is null return error
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            iCAREWorker iCAREWorker = db.iCAREWorker.Find(id); // find the iCAREWorker by id
            if (iCAREWorker == null)
            {
                return HttpNotFound();
            }
            ViewBag.ID = new SelectList(db.iCAREUser, "ID", "Name", iCAREWorker.ID); // return the iCAREWorker
            return View(iCAREWorker);
        }

        // POST: iCAREWorkers/Edit/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,Profession")] iCAREWorker iCAREWorker) // edit the iCAREWorker
        {
            if (ModelState.IsValid) // if model state is valid
            {
                db.Entry(iCAREWorker).State = EntityState.Modified; // edit the iCAREWorker
                db.SaveChanges(); // save changes
                return RedirectToAction("Index");
            }
            ViewBag.ID = new SelectList(db.iCAREUser, "ID", "Name", iCAREWorker.ID); // return the iCAREWorker
            return View(iCAREWorker); // return the iCAREWorker
        }

        // GET: iCAREWorkers/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null) // if id is null return error
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            iCAREWorker iCAREWorker = db.iCAREWorker.Find(id); // find the iCAREWorker by id
            if (iCAREWorker == null) // if iCAREWorker is null
            {
                return HttpNotFound(); 
            }
            return View(iCAREWorker); // return the iCAREWorker
        }

        // POST: iCAREWorkers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            iCAREWorker iCAREWorker = db.iCAREWorker.Find(id); // find the iCAREWorker by id
            db.iCAREWorker.Remove(iCAREWorker); // remove the iCAREWorker
            db.SaveChanges(); // save changes
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
