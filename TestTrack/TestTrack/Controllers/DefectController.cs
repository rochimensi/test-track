using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TestTrack.Models;

namespace TestTrack.Controllers
{
    public class DefectController : Controller
    {
        private TestTrackDBContext db = new TestTrackDBContext();

        //
        // GET: /Defect/

        public ActionResult Index()
        {
            var defects = db.Defects.Include(d => d.Result);
            return View(defects.ToList());
        }

        //
        // GET: /Defect/Details/5

        public ActionResult Details(int id = 0)
        {
            Defect defect = db.Defects.Find(id);
            if (defect == null)
            {
                return HttpNotFound();
            }
            return View(defect);
        }

        //
        // GET: /Defect/Create

        public ActionResult Create()
        {
            ViewBag.ResultID = new SelectList(db.Results, "ResultID", "ResultID");
            return View();
        }

        //
        // POST: /Defect/Create

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Defect defect)
        {
            if (ModelState.IsValid)
            {
                db.Defects.Add(defect);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.ResultID = new SelectList(db.Results, "ResultID", "ResultID", defect.ResultID);
            return View(defect);
        }

        //
        // GET: /Defect/Edit/5

        public ActionResult Edit(int id = 0)
        {
            Defect defect = db.Defects.Find(id);
            if (defect == null)
            {
                return HttpNotFound();
            }
            ViewBag.ResultID = new SelectList(db.Results, "ResultID", "ResultID", defect.ResultID);
            return View(defect);
        }

        //
        // POST: /Defect/Edit/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Defect defect)
        {
            if (ModelState.IsValid)
            {
                db.Entry(defect).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.ResultID = new SelectList(db.Results, "ResultID", "ResultID", defect.ResultID);
            return View(defect);
        }

        //
        // GET: /Defect/Delete/5

        public ActionResult Delete(int id = 0)
        {
            Defect defect = db.Defects.Find(id);
            if (defect == null)
            {
                return HttpNotFound();
            }
            return View(defect);
        }

        //
        // POST: /Defect/Delete/5

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Defect defect = db.Defects.Find(id);
            db.Defects.Remove(defect);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}