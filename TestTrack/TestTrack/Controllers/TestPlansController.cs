using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;
using TestTrack.Models;
using TestTrack.ViewModels;

namespace TestTrack.Controllers
{
    [Authorize]
    public class TestPlansController : BaseController
    {
        private TestTrackDBContext db = new TestTrackDBContext();

        [HttpGet]
        public ActionResult Create()
        {
            var testPlanVM = new TestPlanVM
            {
                Iterations = new SelectList(db.Iterations.ToList(), "IterationID", "Title"),
                Teams = new SelectList(db.Teams.ToList(), "TeamID", "Title")
            };

            return View("Create", testPlanVM);
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

            return RedirectToAction("Index", "TestPlanPerIteration", new { id = testPlanVM.IterationID });
        }

        [HttpGet]
        public ActionResult Edit(int id = 0)
        {
            var testPlan = db.TestPlans.Find(id);

            if (testPlan == null) return HttpNotFound();

            var testPlanVM = new TestPlanVM
            {
                Title = testPlan.Title,
                TestPlanID = id,
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

            return RedirectToAction("Index", "TestRunsOnTestPlan", new { id = testPlanVM.TestPlanID });
        }

        [HttpGet]
        public ActionResult Delete(int id = 0)
        {
            TestPlan testplan = db.TestPlans.Find(id);
            if (testplan == null)
            {
                return HttpNotFound();
            }
            return PartialView(testplan);
        }

        [HttpPost, ValidateAntiForgeryToken, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            TestPlan testplan = db.TestPlans.Find(id);
            int iterationID = testplan.IterationID;
            db.TestPlans.Remove(testplan);
            db.SaveChanges();
            return RedirectToAction("Index", "TestPlanPerIteration", new { id = iterationID });
        }

        [ChildActionOnly]
        public ActionResult List(int id = 0)
        {
            var vm = new TestPlansListVM();
            vm.Values = from value in db.TestPlans
                        where value.IterationID == id
                        orderby value.Title
                        select value;

            return PartialView("_List", vm);
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}