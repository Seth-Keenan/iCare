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
    public class ImportImageController : Controller
    {
        private Group2_iCAREDBEntities db = new Group2_iCAREDBEntities();

        // GET: ImportImage
        public ActionResult Index()
        {
            var files = db.Files.Include(f => f.iCAREUser);

            return View(files.ToList());
        }

        // GET: ImportImage/Details/5
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

        // GET: ImportImage/Create
        public ActionResult Create()
        {
            ViewBag.UploadedByID = new SelectList(db.iCAREUser, "ID", "Name");
            return View();
        }

        // POST: ImportImage/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(HttpPostedFileBase fileUpload, string patientId)
        {
            if (fileUpload != null && fileUpload.ContentLength > 0)
            {
                var file = new Files
                {
                    FileName = Path.GetFileName(fileUpload.FileName),
                    ContentType = fileUpload.ContentType,
                    Data = ConvertToBytes(fileUpload),
                    UploadedByID = patientId // Assuming UploadedByID is the foreign key to the patient record
                };

                db.Files.Add(file);
                db.SaveChanges();
                return RedirectToAction("Index", "DisplayPalette");
            }

            return View();
        }

        private byte[] ConvertToBytes(HttpPostedFileBase file)
        {
            byte[] fileBytes = null;
            using (var binaryReader = new BinaryReader(file.InputStream))
            {
                fileBytes = binaryReader.ReadBytes(file.ContentLength);
            }
            return fileBytes;
        }

        // GET: ImportImage/Edit/5
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

        // POST: ImportImage/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,FileName,ContentType,Data,UploadedByID,UploadDate,Descript")] Files files)
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

        // GET: ImportImage/Delete/5
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

        // POST: ImportImage/Delete/5
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
