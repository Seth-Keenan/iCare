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
    public class DocumentMetadatasController : Controller
    {
        private Group2_iCAREDBEntities db = new Group2_iCAREDBEntities();

        // GET: DocumentMetadatas
        public ActionResult Index()
        {
            var documentMetadata = db.DocumentMetadata.Include(d => d.ModificationHistory).Include(d => d.iCAREUser).Include(d => d.PatientRecord).Include(d => d.iCAREWorker);
            return View(documentMetadata.ToList());
        }

        // GET: DocumentMetadatas/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DocumentMetadata documentMetadata = db.DocumentMetadata.Find(id);
            if (documentMetadata == null)
            {
                return HttpNotFound();
            }
            return View(documentMetadata);
        }

        // GET: DocumentMetadatas/Create
        public ActionResult Create()
        {
            ViewBag.DocID = new SelectList(db.ModificationHistory, "DocID", "Description");
            ViewBag.ModifiedByID = new SelectList(db.iCAREUser, "ID", "Name");
            ViewBag.PatientID = new SelectList(db.PatientRecord, "ID", "Name");
            ViewBag.WorkerID = new SelectList(db.iCAREWorker, "ID", "Profession");
            return View();
        }

        // POST: DocumentMetadatas/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "DocID,DocName,DateOfCreation,PatientID,WorkerID,ModifiedByID,CreationDate,ModifyDate,Descript")] DocumentMetadata documentMetadata)
        {
            if (ModelState.IsValid)
            {
                db.DocumentMetadata.Add(documentMetadata);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.DocID = new SelectList(db.ModificationHistory, "DocID", "Description", documentMetadata.DocID);
            ViewBag.ModifiedByID = new SelectList(db.iCAREUser, "ID", "Name", documentMetadata.ModifiedByID);
            ViewBag.PatientID = new SelectList(db.PatientRecord, "ID", "Name", documentMetadata.PatientID);
            ViewBag.WorkerID = new SelectList(db.iCAREWorker, "ID", "Profession", documentMetadata.WorkerID);
            return View(documentMetadata);
        }

        // GET: DocumentMetadatas/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DocumentMetadata documentMetadata = db.DocumentMetadata.Find(id);
            if (documentMetadata == null)
            {
                return HttpNotFound();
            }
            ViewBag.DocID = new SelectList(db.ModificationHistory, "DocID", "Description", documentMetadata.DocID);
            ViewBag.ModifiedByID = new SelectList(db.iCAREUser, "ID", "Name", documentMetadata.ModifiedByID);
            ViewBag.PatientID = new SelectList(db.PatientRecord, "ID", "Name", documentMetadata.PatientID);
            ViewBag.WorkerID = new SelectList(db.iCAREWorker, "ID", "Profession", documentMetadata.WorkerID);
            return View(documentMetadata);
        }

        // POST: DocumentMetadatas/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "DocID,DocName,DateOfCreation,PatientID,WorkerID,ModifiedByID,CreationDate,ModifyDate,Descript")] DocumentMetadata documentMetadata)
        {
            if (ModelState.IsValid)
            {
                db.Entry(documentMetadata).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.DocID = new SelectList(db.ModificationHistory, "DocID", "Description", documentMetadata.DocID);
            ViewBag.ModifiedByID = new SelectList(db.iCAREUser, "ID", "Name", documentMetadata.ModifiedByID);
            ViewBag.PatientID = new SelectList(db.PatientRecord, "ID", "Name", documentMetadata.PatientID);
            ViewBag.WorkerID = new SelectList(db.iCAREWorker, "ID", "Profession", documentMetadata.WorkerID);
            return View(documentMetadata);
        }

        // GET: DocumentMetadatas/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DocumentMetadata documentMetadata = db.DocumentMetadata.Find(id);
            if (documentMetadata == null)
            {
                return HttpNotFound();
            }
            return View(documentMetadata);
        }

        // POST: DocumentMetadatas/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            DocumentMetadata documentMetadata = db.DocumentMetadata.Find(id);
            db.DocumentMetadata.Remove(documentMetadata);
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
