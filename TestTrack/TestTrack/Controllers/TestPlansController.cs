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
    public class TestPlansController : Controller
    {
        private TestTrackDBContext db = new TestTrackDBContext();

        // GET: /TestPlans/

        public ActionResult Index()
        {
            var testplans = db.TestPlans.Include(t => t.Iteration);
            return View(testplans.ToList());
        }

        // GET: /TestPlans/Create

        public ActionResult Create()
        {
            ViewBag.IterationID = new SelectList(db.Iterations, "IterationID", "Title");
            return View("Edit" , new TestPlan());
        }

        // GET: /TestPlans/Edit/5

        public ActionResult Edit(int id = 0)
        {
            TestPlan testplan = db.TestPlans.Find(id);
            if (testplan == null)
            {
                return HttpNotFound();
            }
            ViewBag.IterationID = new SelectList(db.Iterations, "IterationID", "Title", testplan.IterationID);
            return View(testplan);
        }

        // POST: /TestPlans/Save

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Save(TestPlan testplan)
        {
            if (ModelState.IsValid)
            {
                if (testplan.TestPlanID == 0)
                {
                    db.TestPlans.Add(testplan);
                }
                else
                {
                    db.Entry(testplan).State = EntityState.Modified;
                }
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.IterationID = new SelectList(db.Iterations, "IterationID", "Title", testplan.IterationID);
            return View(testplan);
        }

        // GET: /TestPlans/Delete/5

        public ActionResult Delete(int id = 0)
        {
            TestPlan testplan = db.TestPlans.Find(id);
            if (testplan == null)
            {
                return HttpNotFound();
            }
            return View(testplan);
        }

        // POST: /TestPlans/Delete/5

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            TestPlan testplan = db.TestPlans.Find(id);
            db.TestPlans.Remove(testplan);
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