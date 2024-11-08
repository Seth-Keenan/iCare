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
            return View(db.GeoCodes.ToList());
        }

        // GET: GeoCode/Details/5
        public ActionResult Details(string id)
        {
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

        // GET: GeoCode/Create
        public ActionResult Create()
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
            if (ModelState.IsValid)
            {
                db.GeoCodes.Add(geoCodes);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(geoCodes);
        }

        // GET: GeoCode/Edit/5
        public ActionResult Edit(string id)
        {
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

        // POST: GeoCode/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,Description")] GeoCodes geoCodes)
        {
            if (ModelState.IsValid)
            {
                db.Entry(geoCodes).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(geoCodes);
        }

        // GET: GeoCode/Delete/5
        public ActionResult Delete(string id)
        {
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
            GeoCodes geoCodes = db.GeoCodes.Find(id);
            db.GeoCodes.Remove(geoCodes);
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
