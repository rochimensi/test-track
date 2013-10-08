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
    public class TestRunsController : Controller
    {
        private TestTrackDBContext db = new TestTrackDBContext();

        // GET: /TestRuns/

        public ActionResult Index()
        {
            var testruns = db.TestRuns.Include(t => t.TestPlan);
            return View(testruns.ToList());
        }

        // GET: /TestRuns/Create

        public ActionResult Create()
        {
            ViewBag.TestPlanID = new SelectList(db.TestPlans, "TestPlanID", "Title");
            return View("Edit", new TestRun());
        }

        // GET: /TestRuns/Edit/5

        public ActionResult Edit(int id = 0)
        {
            TestRun testrun = db.TestRuns.Find(id);
            if (testrun == null)
            {
                return HttpNotFound();
            }
            ViewBag.TestPlanID = new SelectList(db.TestPlans, "TestPlanID", "Title", testrun.TestPlanID);
            return View(testrun);
        }

        // POST: /TestRuns/Save

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Save(TestRun testrun)
        {
            if (ModelState.IsValid)
            {
                if (testrun.TestRunID == 0)
                {
                    db.TestRuns.Add(testrun);
                }
                else
                {
                    db.Entry(testrun).State = EntityState.Modified;
                }
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.TestPlanID = new SelectList(db.TestPlans, "TestPlanID", "Title", testrun.TestPlanID);
            return View(testrun);
        }

        // GET: /TestRuns/Delete/5

        public ActionResult Delete(int id = 0)
        {
            TestRun testrun = db.TestRuns.Find(id);
            if (testrun == null)
            {
                return HttpNotFound();
            }
            return View(testrun);
        }

        //
        // POST: /TestRuns/Delete/5

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            TestRun testrun = db.TestRuns.Find(id);
            db.TestRuns.Remove(testrun);
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