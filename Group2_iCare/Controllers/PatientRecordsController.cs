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
            PatientRecord patientRecord = db.PatientRecord.Find(id); // find the patient record by id
            if (patientRecord == null) // if patient record is null return error
            {
                return HttpNotFound();
            }
            return View(patientRecord);
        }

        // GET: PatientRecords/Create
        public ActionResult Create()
        {
            var user = Session["User"] as iCAREUser; // get the user session
            var patientRecord = new PatientRecord // create a new patient record
            {
                WorkerID = null
            };

            ViewBag.GeoCodeID = new SelectList(db.GeoCodes, "ID", "Description"); // select the geo code
            ViewBag.WorkerID = user.ID; // get the worker id
            return View(patientRecord); // return the patient record
        }

        // POST: PatientRecords/Create
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

            ViewBag.GeoCodeID = new SelectList(db.GeoCodes, "ID", "Description", patientRecord.GeoCodeID); // select the geo codes
            ViewBag.WorkerID = new SelectList(db.iCAREWorker, "ID", "Profession", patientRecord.WorkerID); // select the worker ids
            return View(patientRecord);
        }


        // GET: PatientRecords/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PatientRecord patientRecord = db.PatientRecord.Find(id); // find the patient record by id
            if (patientRecord == null) // if patient record is null return error
            {
                return HttpNotFound();
            }
            ViewBag.GeoCodeID = new SelectList(db.GeoCodes, "ID", "Description", patientRecord.GeoCodeID); // select the geo codes
            ViewBag.WorkerID = new SelectList(db.iCAREWorker, "ID", "Profession", patientRecord.WorkerID); // select the worker ids
            ViewBag.PatientID = id; // get the patient id
            return View(patientRecord); // return the patient record
        }

        // POST: PatientRecords/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,Name,Address,DateOfBirth,Height,Weight,BloodGroup,BedID,TreatmentArea,GeoCodeID,WorkerID")] PatientRecord patientRecord)
        {
            ViewBag.PatientRecordID = patientRecord.ID;

            if (ModelState.IsValid)
            {
                db.Entry(patientRecord).State = EntityState.Modified; // edit the patient record
                db.SaveChanges(); // save changes
                return RedirectToAction("Index", "ImportImage", new { PatientRecordID = patientRecord.ID }); // return to index
            }

            ViewBag.GeoCodeID = new SelectList(db.GeoCodes, "ID", "Description", patientRecord.GeoCodeID); // select the geo codes
            ViewBag.WorkerID = new SelectList(db.iCAREWorker, "ID", "Profession", patientRecord.WorkerID); // select the worker ids
            return View(patientRecord);
        }

        // GET: PatientRecords/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest); 
            }
            PatientRecord patientRecord = db.PatientRecord.Find(id); // find the patient record by id
            if (patientRecord == null) // if patient record is null return error
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
            PatientRecord patientRecord = db.PatientRecord.Find(id); // select to delete
            db.PatientRecord.Remove(patientRecord); // delete the patient record
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
