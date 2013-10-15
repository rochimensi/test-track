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
    [Authorize]
    public class DefectsController : BaseController
    {
        private TestTrackDBContext db = new TestTrackDBContext();

        // GET: /Defects/

        public ActionResult Index()
        {
            var defects = db.Defects.Include(d => d.Result);
            return View(defects.ToList());
        }

        // GET: /Defects/Create

        public ActionResult Create()
        {
            ViewBag.ResultID = new SelectList(db.Results, "ResultID", "ResultID");
            return View("Edit", new Defect());
        }
        
        // GET: /Defects/Edit/5

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
        
        // POST: /Defects/Save

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Save(Defect defect)
        {
            if (ModelState.IsValid)
            {
                if (defect.DefectID == 0)
                {
                    db.Defects.Add(defect);
                }
                else
                {
                    db.Entry(defect).State = EntityState.Modified;
                }
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.ResultID = new SelectList(db.Results, "ResultID", "ResultID", defect.ResultID);
            return View(defect);
        }

        // GET: /Defects/Delete/5

        public ActionResult Delete(int id = 0)
        {
            Defect defect = db.Defects.Find(id);
            if (defect == null)
            {
                return HttpNotFound();
            }
            return View(defect);
        }

        // POST: /Defects/Delete/5

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