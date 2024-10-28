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
            var documentMetadata = db.DocumentMetadata.Include(d => d.ModificationHistory);
            return View(documentMetadata.ToList());
        }

        // GET: DisplayPalette/Details/5
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

        // GET: DisplayPalette/Create
        public ActionResult Create()
        {
            ViewBag.DocID = new SelectList(db.ModificationHistory, "DocID", "Description");
            return View();
        }

        // POST: DisplayPalette/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "DocID,DocName,DateOfCreation,Descript")] DocumentMetadata documentMetadata, HttpPostedFileBase file)
        {
            if (ModelState.IsValid)
            {
                if (file != null && file.ContentLength > 0)
                {
                    var fileName = Path.GetFileName(file.FileName);
                    var path = Path.Combine(Server.MapPath("~/App_Data/Uploads"), fileName);

                    // Ensure the directory exists and if not will create one to store the file
                    var directory = Path.GetDirectoryName(path);
                    if (!Directory.Exists(directory))
                    {
                        Directory.CreateDirectory(directory);
                    }

                    file.SaveAs(path);
                }

                db.DocumentMetadata.Add(documentMetadata);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.DocID = new SelectList(db.ModificationHistory, "DocID", "Description", documentMetadata.DocID);
            return View(documentMetadata);
        }

        // GET: DisplayPalette/Edit/5
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

        // POST: DisplayPalette/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
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
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
