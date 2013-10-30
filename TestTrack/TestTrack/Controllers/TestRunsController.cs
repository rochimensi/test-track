using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TestTrack.Models;
using TestTrack.ViewModels;

namespace TestTrack.Controllers
{
    [Authorize]
    public class TestRunsController : BaseController
    {
        private TestTrackDBContext db = new TestTrackDBContext();

        [HttpGet]
        public ActionResult Create(int id = 0)
        {
            var testRunVM = new TestRunVM
            {
                TestPlanID = id
            };

            return View("Create", testRunVM);
        }

        [HttpPost]
        public ActionResult Create(TestRunVM testRunVM)
        {
            var testRun = new TestRun
            {
                Title = testRunVM.Title,
                Closed = testRunVM.Closed,
                TestPlanID = testRunVM.TestPlanID,
                TestPlan = db.TestPlans.Find(testRunVM.TestPlanID)
            };

            db.TestRuns.Add(testRun);
            db.SaveChanges();

            return RedirectToAction("Index", "TestRunsOnTestPlan", new { id = testRunVM.TestPlanID });
        }

        [HttpGet]
        public ActionResult Edit(int id)
        {
            var testRun = db.TestRuns.Find(id);

            if (testRun == null) return HttpNotFound();

            var testRunVM = new TestRunVM
            {
                TestRunID = id,
                Title = testRun.Title,
                Closed = testRun.Closed,
                TestPlanID = testRun.TestPlanID,
            };

            return View(testRunVM);
        }

        [HttpPost]
        public ActionResult Edit(TestRunVM testRunVM)
        {
            var testRun = db.TestRuns.Find(testRunVM.TestRunID);

            if (testRun == null) return HttpNotFound();

            testRun.Title = testRunVM.Title;
            testRun.Closed = testRunVM.Closed;
            testRun.TestPlanID = testRunVM.TestPlanID;
            testRun.TestPlan = db.TestPlans.Find(testRunVM.TestPlanID);

            db.Entry(testRun).State = EntityState.Modified;
            db.SaveChanges();

            return RedirectToAction("Index", "ExecuteTestRun", new { id = testRun.TestRunID });
        }

        // GET: /TestRuns/Delete/5

        public ActionResult Delete(int id = 0)
        {
            TestRun testrun = db.TestRuns.Find(id);
            if (testrun == null)
            {
                return HttpNotFound();
            }
            return PartialView(testrun);
        }

        // POST: /TestRuns/Delete/5

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            TestRun testrun = db.TestRuns.Find(id);
            int testPlanID = testrun.TestPlanID;
            db.TestRuns.Remove(testrun);
            db.SaveChanges();
            return RedirectToAction("Index", "TestRunsOnTestPlan", new { id = testPlanID });
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}