﻿using Group2_iCare.Models;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;

namespace Group2_iCare.Controllers
{
    public class PatientRecordsController : Controller
    {
        private Group2_iCAREDBEntities db = new Group2_iCAREDBEntities();

        // GET: PatientRecords
        public ActionResult Index()
        {
            var patientRecord = db.PatientRecord.Include(p => p.GeoCodes).Include(p => p.iCAREWorker);
            return View(patientRecord.ToList());
        }

        // GET: PatientRecords/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PatientRecord patientRecord = db.PatientRecord.Find(id);
            if (patientRecord == null)
            {
                return HttpNotFound();
            }
            return View(patientRecord);
        }

        // GET: PatientRecords/Create
        public ActionResult Create()
        {
            var user = Session["User"] as iCAREUser;
            var patientRecord = new PatientRecord
            {
                WorkerID = null
            };

            ViewBag.GeoCodeID = new SelectList(db.GeoCodes, "ID", "Description");
            ViewBag.WorkerID = user.ID;
            return View(patientRecord);
        }

        // POST: PatientRecords/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,Name,Address,DateOfBirth,Height,Weight,BloodGroup,BedID,TreatmentArea,GeoCodeID,WorkerID")] PatientRecord patientRecord)
        {
            if (db.PatientRecord.Find(patientRecord.ID) != null)
            {
                ModelState.AddModelError("ID", "ID already exists");
            }

            if (string.IsNullOrEmpty(patientRecord.Name))
            {
                ModelState.AddModelError("Name", "Insert Valid name");
            }

            if (ModelState.IsValid)
            {

                db.PatientRecord.Add(patientRecord);
                db.SaveChanges();
                return RedirectToAction("Index", "ImportImage", new { PatientRecordID = patientRecord.ID });
            }

            ViewBag.GeoCodeID = new SelectList(db.GeoCodes, "ID", "Description", patientRecord.GeoCodeID);
            ViewBag.WorkerID = new SelectList(db.iCAREWorker, "ID", "Profession", patientRecord.WorkerID);
            return View(patientRecord);
        }


        // GET: PatientRecords/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PatientRecord patientRecord = db.PatientRecord.Find(id);
            if (patientRecord == null)
            {
                return HttpNotFound();
            }
            ViewBag.GeoCodeID = new SelectList(db.GeoCodes, "ID", "Description", patientRecord.GeoCodeID);
            ViewBag.WorkerID = new SelectList(db.iCAREWorker, "ID", "Profession", patientRecord.WorkerID);
            ViewBag.PatientID = id;
            return View(patientRecord);
        }

        // POST: PatientRecords/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,Name,Address,DateOfBirth,Height,Weight,BloodGroup,BedID,TreatmentArea,GeoCodeID,WorkerID")] PatientRecord patientRecord)
        {
            ViewBag.PatientRecordID = patientRecord.ID;

            if (ModelState.IsValid)
            {
                db.Entry(patientRecord).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index", "ImportImage", new { PatientRecordID = patientRecord.ID });
            }

            ViewBag.GeoCodeID = new SelectList(db.GeoCodes, "ID", "Description", patientRecord.GeoCodeID);
            ViewBag.WorkerID = new SelectList(db.iCAREWorker, "ID", "Profession", patientRecord.WorkerID);
            return View(patientRecord);
        }

        // GET: PatientRecords/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PatientRecord patientRecord = db.PatientRecord.Find(id);
            if (patientRecord == null)
            {
                return HttpNotFound();
            }
            return View(patientRecord);
        }

        // POST: PatientRecords/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            PatientRecord patientRecord = db.PatientRecord.Find(id);
            db.PatientRecord.Remove(patientRecord);
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
