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
    public class StepsController : Controller
    {
        private TestTrackDBContext db = new TestTrackDBContext();

        // GET: /Steps/

        public ActionResult Index()
        {
            return View(db.Steps.ToList());
        }

        // GET: /Steps/Create

        public ActionResult Create()
        {
            return View("Edit", new Step());
        }

        // GET: /Steps/Edit/5

        public ActionResult Edit(int id = 0)
        {
            Step step = db.Steps.Find(id);
            if (step == null)
            {
                return HttpNotFound();
            }
            return View(step);
        }

        // POST: /Steps/Save

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Save(Step step)
        {
            if (ModelState.IsValid)
            {
                if (step.StepId == 0)
                {
                    db.Steps.Add(step);
                }
                else
                {
                    db.Entry(step).State = EntityState.Modified;
                }
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(step);
        }

        // GET: /Steps/Delete/5

        public ActionResult Delete(int id = 0)
        {
            Step step = db.Steps.Find(id);
            if (step == null)
            {
                return HttpNotFound();
            }
            return View(step);
        }

        // POST: /Steps/Delete/5

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Step step = db.Steps.Find(id);
            db.Steps.Remove(step);
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