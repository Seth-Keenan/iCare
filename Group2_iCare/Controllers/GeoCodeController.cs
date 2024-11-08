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
    public class GeoCodeController : Controller
    {
        private Group2_iCAREDBEntities db = new Group2_iCAREDBEntities();

        // GET: GeoCode
        public ActionResult Index()
        {
            return View(db.GeoCodes.ToList()); // display all geo codes as a list
        }

        // GET: GeoCode/Details/5
        public ActionResult Details(string id)
        {
            if (id == null) // id is null error
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            GeoCodes geoCodes = db.GeoCodes.Find(id); // find the geo code by id
            if (geoCodes == null) // if geo code is null/ not found
            {
                return HttpNotFound();
            }
            return View(geoCodes); // return the geo code if no error
        }

        // GET: GeoCode/Create
        public ActionResult Create() // create a new geo code
        {
            return View();
        }

        // POST: GeoCode/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,Description")] GeoCodes geoCodes)
        {
            if (string.IsNullOrEmpty(geoCodes.ID))
            {
                ModelState.AddModelError("ID", "ID is required");
            }

            if (string.IsNullOrEmpty(geoCodes.Description)) 
            {
                ModelState.AddModelError("Description", "Description is required");
            }

            if (ModelState.IsValid)
            {
                db.GeoCodes.Add(geoCodes); // add the geo code
                db.SaveChanges(); // save changes
                return RedirectToAction("Index"); 
            }

            return View(geoCodes); // return the geo code
        }

        // GET: GeoCode/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null) // is id null return error
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            GeoCodes geoCodes = db.GeoCodes.Find(id); // find the geo code by id
            if (geoCodes == null)
            {
                return HttpNotFound();
            }
            return View(geoCodes); // return the geo code
        }

        // POST: GeoCode/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,Description")] GeoCodes geoCodes)
        {
            if (string.IsNullOrEmpty(geoCodes.Description))
            {
                ModelState.AddModelError("Description", "Description is required");
            }

            if (ModelState.IsValid)
            {
                db.Entry(geoCodes).State = EntityState.Modified; // edit the geo code
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(geoCodes); // return the geo code 
        }

        // GET: GeoCode/Delete/5
        public ActionResult Delete(string id)
        { //similar to above
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            GeoCodes geoCodes = db.GeoCodes.Find(id);
            if (geoCodes == null)
            {
                return HttpNotFound();
            }
            return View(geoCodes);
        }

        // POST: GeoCode/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            GeoCodes geoCodes = db.GeoCodes.Find(id); // find the geo code and delete it
            db.GeoCodes.Remove(geoCodes);
            db.SaveChanges(); // save changes
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
    }
}
