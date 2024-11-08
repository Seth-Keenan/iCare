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

        public ActionResult Index()
        {
            var documentMetadata = db.DocumentMetadata.Include(d => d.ModificationHistory);
            return View(documentMetadata.ToList());
        }

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

        public ActionResult Create()
        {
            ViewBag.DocID = new SelectList(db.ModificationHistory, "DocID", "Description");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "DocID,DocName,DateOfCreation,Descript")] DocumentMetadata documentMetadata, HttpPostedFileBase file)
        {
            if (ModelState.IsValid)
            {
                db.DocumentMetadata.Add(documentMetadata);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.DocID = new SelectList(db.ModificationHistory, "DocID", "Description", documentMetadata.DocID);
            return View(documentMetadata);
        }

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
            return View(documentMetadata);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "DocID,DocName,DateOfCreation")] DocumentMetadata documentMetadata)
        {
            if (ModelState.IsValid)
            {
                db.Entry(documentMetadata).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.DocID = new SelectList(db.ModificationHistory, "DocID", "Description", documentMetadata.DocID);
            return View(documentMetadata);
        }

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
