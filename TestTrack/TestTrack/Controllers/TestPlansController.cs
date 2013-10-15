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
    public class TestPlansController : Controller
    {
        private TestTrackDBContext db = new TestTrackDBContext();

        // GET: /TestPlans/

        public ActionResult Index()
        {
            var testplans = db.TestPlans.Include(t => t.Iteration).Include(t => t.Team);
            return View(testplans.ToList());
        }

        [HttpGet]
        public ActionResult Create()
        {
            var testPlanVM = new TestPlanVM
            {
                Iterations = new SelectList(db.Iterations.ToList(), "IterationID", "Title"),
                Teams = new SelectList(db.Teams.ToList(), "TeamID", "Title")
            };

            return View("Create" , testPlanVM);
        }

        [HttpPost]
        public ActionResult Create(TestPlanVM testPlanVM)
        {
            var testPlan = new TestPlan
            {
                Title = testPlanVM.Title,
                Description = testPlanVM.Description,
                IterationID = testPlanVM.IterationID,
                Iteration = db.Iterations.Find(testPlanVM.IterationID),
                TeamID = testPlanVM.TeamID,
                Team = db.Teams.Find(testPlanVM.TeamID)
            };

            db.TestPlans.Add(testPlan);
            db.SaveChanges();

            return RedirectToAction("Index");
        }

        [HttpGet]
        public ActionResult Edit(int id = 0)
        {
            var testPlan = db.TestPlans.Find(id);

            if (testPlan == null) return HttpNotFound();

            var testPlanVM = new TestPlanVM
            {
                Title = testPlan.Title,
                Description = testPlan.Description,
                IterationID = testPlan.IterationID,
                Iterations = new SelectList(db.Iterations.ToList(), "IterationID", "Title", testPlan.IterationID),
                TeamID = testPlan.TeamID,
                Teams = new SelectList(db.Teams.ToList(), "TeamID", "Title", testPlan.TeamID)
            };

            return View(testPlanVM);
        }

        [HttpPost]
        public ActionResult Edit(TestPlanVM testPlanVM)
        {
            var testPlan = db.TestPlans.Find(testPlanVM.TestPlanID);

            if (testPlan == null) return HttpNotFound();

            testPlan.Title = testPlanVM.Title;
            testPlan.Description = testPlanVM.Description;
            testPlan.IterationID = testPlanVM.IterationID;
            testPlan.Iteration = db.Iterations.Find(testPlanVM.IterationID);
            testPlan.TeamID = testPlanVM.TeamID;
            testPlan.Team = db.Teams.Find(testPlanVM.TeamID);

            db.Entry(testPlan).State = EntityState.Modified;
            db.SaveChanges();

            return RedirectToAction("Index");
        }

        [HttpGet]
        public ActionResult Delete(int id = 0)
        {
            TestPlan testplan = db.TestPlans.Find(id);
            if (testplan == null)
            {
                return HttpNotFound();
            }
            return View(testplan);
        }

        [HttpPost, ValidateAntiForgeryToken, ActionName("Delete")]
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