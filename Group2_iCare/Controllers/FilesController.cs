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
    public class FilesController : Controller
    {
        private Group2_iCAREDBEntities db = new Group2_iCAREDBEntities();

        // GET: Files
        public ActionResult Index()
        {
            var files = db.Files.Include(f => f.iCAREUser);
            return View(files.ToList());
        }

        // GET: Files/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Files files = db.Files.Find(id);
            if (files == null)
            {
                return HttpNotFound();
            }
            return View(files);
        }

        // GET: Files/Create
        public ActionResult Create()
        {
            ViewBag.UploadedByID = new SelectList(db.iCAREUser, "ID", "Name");
            return View();
        }

        // POST: Files/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,FileName,ContentType,Data,UploadedByID,UploadDate")] Files files)
        {
            if (ModelState.IsValid)
            {
                db.Files.Add(files);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.UploadedByID = new SelectList(db.iCAREUser, "ID", "Name", files.UploadedByID);
            return View(files);
        }

        // GET: Files/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Files files = db.Files.Find(id);
            if (files == null)
            {
                return HttpNotFound();
            }
            ViewBag.UploadedByID = new SelectList(db.iCAREUser, "ID", "Name", files.UploadedByID);
            return View(files);
        }

        // POST: Files/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,FileName,ContentType,Data,UploadedByID,UploadDate")] Files files)
        {
            if (ModelState.IsValid)
            {
                db.Entry(files).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.UploadedByID = new SelectList(db.iCAREUser, "ID", "Name", files.UploadedByID);
            return View(files);
        }

        // GET: Files/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Files files = db.Files.Find(id);
            if (files == null)
            {
                return HttpNotFound();
            }
            return View(files);
        }

        // POST: Files/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            Files files = db.Files.Find(id);
            db.Files.Remove(files);
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
