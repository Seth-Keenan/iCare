using Group2_iCare.Models;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;

namespace Group2_iCare.Controllers
{
    public class iCAREWorkersController : Controller
    {
        private Group2_iCAREDBEntities db = new Group2_iCAREDBEntities();

        // GET: iCAREWorkers
        public ActionResult Index()
        {
            var iCAREWorker = db.iCAREWorker.Include(i => i.iCAREUser);
            return View(iCAREWorker.ToList());
        }

        // GET: iCAREWorkers/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            iCAREWorker iCAREWorker = db.iCAREWorker.Find(id);
            if (iCAREWorker == null)
            {
                return HttpNotFound();
            }
            return View(iCAREWorker);
        }

        // GET: iCAREWorkers/Create
        public ActionResult Create()
        {
            ViewBag.ID = new SelectList(db.iCAREUser, "ID", "Name");
            return View();
        }

        // POST: iCAREWorkers/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,Profession")] iCAREWorker iCAREWorker)
        {
            if (ModelState.IsValid)
            {
                db.iCAREWorker.Add(iCAREWorker);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.ID = new SelectList(db.iCAREUser, "ID", "Name", iCAREWorker.ID);
            return View(iCAREWorker);
        }

        // GET: iCAREWorkers/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            iCAREWorker iCAREWorker = db.iCAREWorker.Find(id);
            if (iCAREWorker == null)
            {
                return HttpNotFound();
            }
            ViewBag.ID = new SelectList(db.iCAREUser, "ID", "Name", iCAREWorker.ID);
            return View(iCAREWorker);
        }

        // POST: iCAREWorkers/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,Profession")] iCAREWorker iCAREWorker)
        {
            if (ModelState.IsValid)
            {
                db.Entry(iCAREWorker).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.ID = new SelectList(db.iCAREUser, "ID", "Name", iCAREWorker.ID);
            return View(iCAREWorker);
        }

        // GET: iCAREWorkers/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            iCAREWorker iCAREWorker = db.iCAREWorker.Find(id);
            if (iCAREWorker == null)
            {
                return HttpNotFound();
            }
            return View(iCAREWorker);
        }

        // POST: iCAREWorkers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            iCAREWorker iCAREWorker = db.iCAREWorker.Find(id);
            db.iCAREWorker.Remove(iCAREWorker);
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
