using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Group2_iCare.Models;

namespace Group2_iCare.Controllers
{
    public class DisplayPaletteController : Controller
    {
        private Group2_iCAREDBEntities db = new Group2_iCAREDBEntities();

        // GET: DisplayPalette
        public ActionResult Index()
        { 
            var files = db.Files.AsEnumerable();
            var documentMetadata = db.DocumentMetadata.Include(d => d.iCAREUser).Include(d => d.PatientRecord).Include(d => d.iCAREWorker).Include(d => d.ModificationHistory).AsEnumerable();
            return View((documentMetadata, files));
        }

        // GET: DisplayPalette/Details/5
        public ActionResult Details(string id)
        { // display details of the document
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

        // GET: DisplayPalette/Create
        public ActionResult Create()
        { // create a new document
            ViewBag.ModifiedByID = new SelectList(db.iCAREUser, "ID", "Name");
            ViewBag.PatientID = new SelectList(db.PatientRecord, "ID", "WorkerID");
            ViewBag.WorkerID = new SelectList(db.iCAREWorker, "ID", "Profession");
            ViewBag.DocID = new SelectList(db.ModificationHistory, "DocID", "Description");
            return View();
        }

        // POST: DisplayPalette/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "DocID,DocName,DateOfCreation,PatientID,WorkerID,ModifiedByID,CreationDate,ModifyDate,Descript")] DocumentMetadata documentMetadata)
        {
            if (ModelState.IsValid)
            { // create a new document
                db.DocumentMetadata.Add(documentMetadata);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.ModifiedByID = new SelectList(db.iCAREUser, "ID", "Name", documentMetadata.ModifiedByID);
            ViewBag.PatientID = new SelectList(db.PatientRecord, "ID", "WorkerID", documentMetadata.PatientID);
            ViewBag.WorkerID = new SelectList(db.iCAREWorker, "ID", "Profession", documentMetadata.WorkerID);
            ViewBag.DocID = new SelectList(db.ModificationHistory, "DocID", "Description", documentMetadata.DocID);
            return View(documentMetadata);
        }

        // GET: DisplayPalette/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null) // if id is null return error
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DocumentMetadata documentMetadata = db.DocumentMetadata.Find(id); // find document by id
            if (documentMetadata == null)
            {
                return HttpNotFound();
            }
            // select list of modified by id, patient id, worker id, and doc id
            ViewBag.ModifiedByID = new SelectList(db.iCAREUser, "ID", "Name", documentMetadata.ModifiedByID);
            ViewBag.PatientID = new SelectList(db.PatientRecord, "ID", "WorkerID", documentMetadata.PatientID);
            ViewBag.WorkerID = new SelectList(db.iCAREWorker, "ID", "Profession", documentMetadata.WorkerID);
            ViewBag.DocID = new SelectList(db.ModificationHistory, "DocID", "Description", documentMetadata.DocID);
            return View(documentMetadata);
        }

        // POST: DisplayPalette/Edit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "DocID,DocName,DateOfCreation,PatientID,WorkerID,ModifiedByID,CreationDate,ModifyDate,Descript")] DocumentMetadata documentMetadata)
        { // edit document
            if (ModelState.IsValid)
            { // if model state is valid
                db.Entry(documentMetadata).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            // select list of modified by id, patient id, worker id, and doc id
            ViewBag.ModifiedByID = new SelectList(db.iCAREUser, "ID", "Name", documentMetadata.ModifiedByID);
            ViewBag.PatientID = new SelectList(db.PatientRecord, "ID", "WorkerID", documentMetadata.PatientID);
            ViewBag.WorkerID = new SelectList(db.iCAREWorker, "ID", "Profession", documentMetadata.WorkerID);
            ViewBag.DocID = new SelectList(db.ModificationHistory, "DocID", "Description", documentMetadata.DocID);
            return View(documentMetadata);
        }

        // GET: DisplayPalette/Delete/5
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

        // POST: DisplayPalette/Delete/5
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
        { // dispose
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        public ActionResult ShowUploadedFile(string fileName)
        { // show uploaded file
            if (string.IsNullOrEmpty(fileName)) // if file name is null return error
            {
                return HttpNotFound("File name is invalid.");
            }

            var directoryPath = Server.MapPath("~/Repository/UploadedFiles"); // get directory path
            if (directoryPath == null)
            {
                return HttpNotFound("Directory path is invalid."); //error on invalid path
            }

            var path = Path.Combine(directoryPath, fileName);
            // if file exists
            if (System.IO.File.Exists(path))
            {
                ViewBag.FilePath = Url.Content($"~/Repository/UploadedFiles/{fileName}"); // get file path
                return View();
            }

            return HttpNotFound();
        }
    }
}
