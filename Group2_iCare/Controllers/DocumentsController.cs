using System;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using Group2_iCare.Models;

namespace Group2_iCare.Controllers
{
    public class DocumentsController : Controller
    {
        private Group2_iCAREDBEntities db = new Group2_iCAREDBEntities();

        public ActionResult Index()
        {
            var documentMetadata = db.DocumentMetadata
                .Include(d => d.iCAREUser)
                .Include(d => d.PatientRecord)
                .Include(d => d.iCAREWorker)
                .ToList();

            return View(documentMetadata);
        }


        public ActionResult Create()
        {
            PopulateDropdowns();
            return View();
        }

        // GET: Documents/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            DocumentMetadata documentMetadata = db.DocumentMetadata
                .Include(d => d.iCAREUser)
                .Include(d => d.PatientRecord)
                .Include(d => d.iCAREWorker)
                .FirstOrDefault(d => d.DocID == id);

            if (documentMetadata == null)
            {
                return HttpNotFound();
            }

            return View(documentMetadata);
        }

        // POST: Documents/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            DocumentMetadata documentMetadata = db.DocumentMetadata.Find(id);
            if (documentMetadata != null)
            {
                db.DocumentMetadata.Remove(documentMetadata);
                db.SaveChanges();
            }

            return RedirectToAction("Index");
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "DocID,DocName,PatientID,WorkerID,ModifiedByID,Descript")] DocumentMetadata documentMetadata)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    db.DocumentMetadata.Add(documentMetadata);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                catch (System.Data.Entity.Validation.DbEntityValidationException ex)
                {
                    foreach (var validationErrors in ex.EntityValidationErrors)
                    {
                        foreach (var validationError in validationErrors.ValidationErrors)
                        {
                            ModelState.AddModelError(validationError.PropertyName, validationError.ErrorMessage);
                            System.Diagnostics.Debug.WriteLine($"Property: {validationError.PropertyName} Error: {validationError.ErrorMessage}");
                        }
                    }
                }
            }

            PopulateDropdowns();
            return View(documentMetadata);
        }

        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            DocumentMetadata documentMetadata = db.DocumentMetadata
                .Include(d => d.iCAREUser)
                .Include(d => d.PatientRecord)
                .Include(d => d.iCAREWorker)
                .FirstOrDefault(d => d.DocID == id);

            if (documentMetadata == null)
            {
                return HttpNotFound();
            }

            PopulateDropdowns();
            return View(documentMetadata);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "DocID,DocName,PatientID,WorkerID,ModifiedByID,Descript")] DocumentMetadata documentMetadata)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    db.Entry(documentMetadata).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                catch (System.Data.Entity.Validation.DbEntityValidationException ex)
                {
                    foreach (var validationErrors in ex.EntityValidationErrors)
                    {
                        foreach (var validationError in validationErrors.ValidationErrors)
                        {
                            ModelState.AddModelError(validationError.PropertyName, validationError.ErrorMessage);
                            System.Diagnostics.Debug.WriteLine($"Property: {validationError.PropertyName} Error: {validationError.ErrorMessage}");
                        }
                    }
                }
            }

            PopulateDropdowns();
            return View(documentMetadata);
        }

        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var documentMetadata = db.DocumentMetadata
                .Include(d => d.iCAREUser)
                .Include(d => d.PatientRecord)
                .Include(d => d.iCAREWorker)
                .FirstOrDefault(d => d.DocID == id);

            if (documentMetadata == null)
            {
                return HttpNotFound();
            }

            return View(documentMetadata);
        }

        private void PopulateDropdowns()
        {
            ViewBag.ModifiedByID = new SelectList(db.iCAREUser, "ID", "Name");
            ViewBag.PatientID = new SelectList(db.PatientRecord, "ID", "Name");
            ViewBag.WorkerID = new SelectList(db.iCAREWorker, "ID", "Profession");
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
