using System;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using Group2_iCare.Models;

namespace Group2_iCare.Controllers
{
    public class DocumentsController : Controller
    { //new db connection
        private Group2_iCAREDBEntities db = new Group2_iCAREDBEntities();

        public ActionResult Index()
        { // display all doc
            var documentMetadata = db.DocumentMetadata
                .Include(d => d.iCAREUser)
                .Include(d => d.PatientRecord)
                .Include(d => d.iCAREWorker)
                .ToList();

            return View(documentMetadata);
        }


        public ActionResult Create()
        { // create a new doc
            var documentMetadata = new DocumentMetadata();
            var user = Session["User"] as iCAREUser;
            ViewBag.userID = user.ID;
            documentMetadata.WorkerID = user.ID;
            PopulateDropdowns();
            return View(documentMetadata);
        }

        // GET: Documents/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null) // id is null
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            // find the document
            DocumentMetadata documentMetadata = db.DocumentMetadata
                .Include(d => d.iCAREUser)
                .Include(d => d.PatientRecord)
                .Include(d => d.iCAREWorker)
                .FirstOrDefault(d => d.DocID == id);

            if (documentMetadata == null)
            {
                return HttpNotFound();
            }

            return View(documentMetadata); // return the document
        }

        // POST: Documents/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        { // find the document and delete it
            DocumentMetadata documentMetadata = db.DocumentMetadata.Find(id);
            if (documentMetadata != null)
            {
                db.DocumentMetadata.Remove(documentMetadata);
                db.SaveChanges();
            }

            return RedirectToAction("Index"); // return to index
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "DocID,DocName,PatientID,WorkerID,ModifiedByID,Descript")] DocumentMetadata documentMetadata)
        {
            // create a new doc
            if (ModelState.IsValid)
            {
                try
                { // add the doc to the db
                    db.DocumentMetadata.Add(documentMetadata);
                    db.SaveChanges(); // save changes
                    return RedirectToAction("Index");
                }
                catch (System.Data.Entity.Validation.DbEntityValidationException ex) // catch block for error
                {
                    foreach (var validationErrors in ex.EntityValidationErrors) // foreach error 
                    {
                        foreach (var validationError in validationErrors.ValidationErrors)
                        {
                            ModelState.AddModelError(validationError.PropertyName, validationError.ErrorMessage);
                            System.Diagnostics.Debug.WriteLine($"Property: {validationError.PropertyName} Error: {validationError.ErrorMessage}");
                        }
                    }
                }
            }

            PopulateDropdowns(); // populate dropdowns
            return View(documentMetadata);
        }

        public ActionResult Edit(string id)
        {
            
            var user = Session["User"] as iCAREUser;
            ViewBag.MID = user.ID; // get the user id
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest); // if id is null return error
            }

            DocumentMetadata documentMetadata = db.DocumentMetadata // find the document
                .Include(d => d.iCAREUser)
                .Include(d => d.PatientRecord)
                .Include(d => d.iCAREWorker)
                .FirstOrDefault(d => d.DocID == id);

            if (documentMetadata == null)
            {
                return HttpNotFound();
            }

            PopulateDropdowns();
            return View(documentMetadata); // return the document
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "DocID,DocName,PatientID,WorkerID,ModifiedByID,Descript")] DocumentMetadata documentMetadata)
        {
            if (ModelState.IsValid) // if model state is valid
            {
                try
                {
                    db.Entry(documentMetadata).State = EntityState.Modified; // edit the document
                    db.SaveChanges(); // save changes
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

            var documentMetadata = db.DocumentMetadata // find the document
                .Include(d => d.iCAREUser)
                .Include(d => d.PatientRecord)
                .Include(d => d.iCAREWorker)
                .FirstOrDefault(d => d.DocID == id);

            if (documentMetadata == null)
            {
                return HttpNotFound();
            }

            return View(documentMetadata); // return the document
        }

        private void PopulateDropdowns() // populate dropdowns
        {
            ViewBag.ModifiedByID = new SelectList(db.iCAREUser, "ID", "Name");
            ViewBag.PatientID = new SelectList(db.PatientRecord, "ID", "Name");
            ViewBag.WorkerID = new SelectList(db.iCAREWorker, "ID", "Profession");
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
