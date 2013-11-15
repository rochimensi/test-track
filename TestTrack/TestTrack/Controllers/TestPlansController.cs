using AutoMapper;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using TestTrack.Filters;
using TestTrack.Models;
using TestTrack.ViewModels;

namespace TestTrack.Controllers
{
    [Authorize]
    [ProjectsAvailability]
    public class TestPlansController : BaseController
    {
        private TestTrackDBContext db = new TestTrackDBContext();

        [HttpGet]
        public ActionResult Create(int id = 0)
        {
            var iteration = db.Iterations.Find(id);
            if (iteration == null) return HttpNotFound();

            var testPlanVM = new TestPlanVM
            {
                IterationID = iteration.IterationID,
                Teams = new SelectList(GetTeamsInProject(), "TeamID", "Name")
            };

            return View("Create", testPlanVM);
        }

        [HttpPost]
        public ActionResult Create(TestPlanVM testPlanVM)
        {
            var testPlan = Mapper.Map<TestPlanVM, TestPlan>(testPlanVM);
            db.TestPlans.Add(testPlan);
            db.SaveChanges();

            return RedirectToAction("Index", "TestPlanPerIteration", new { id = testPlanVM.IterationID });
        }

        [HttpGet]
        public ActionResult Edit(int id = 0)
        {
            var testPlan = db.TestPlans.Find(id);
            if (testPlan == null) return HttpNotFound();

            var testPlanVM = Mapper.Map<TestPlan, TestPlanVM>(testPlan);
            testPlanVM.Iterations = new SelectList(GetIterationsInProject(), "IterationID", "Title", testPlan.IterationID);
            testPlanVM.Teams = new SelectList(GetTeamsInProject(), "TeamID", "Name", testPlan.TeamID);

            return View(testPlanVM);
        }

        [HttpPost]
        public ActionResult Edit(TestPlanVM testPlanVM)
        {
            var testPlan = db.TestPlans.Find(testPlanVM.TestPlanID);
            if (testPlan == null) return HttpNotFound();
            db.Entry(testPlan).CurrentValues.SetValues(testPlanVM);
            db.SaveChanges();

            return RedirectToAction("Index", "TestRunsOnTestPlan", new { id = testPlanVM.TestPlanID });
        }

        [HttpGet]
        public ActionResult Delete(int id = 0)
        {
            TestPlan testplan = db.TestPlans.Find(id);
            if (testplan == null) return HttpNotFound();

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
            var testPlans = (from value in db.TestPlans
                             where value.IterationID == id
                             orderby value.Title
                             select value).ToList();

            var testPlansVM = Mapper.Map<IList<TestPlan>, IList<TestPlanVM>>(testPlans);
            return PartialView("_List", testPlansVM);
        }

        private IList<Iteration> GetIterationsInProject()
        {
            UserSettings userSettings = SessionWrapper.UserSettings;
            return (from iteration in db.Iterations
                    where iteration.ProjectID == userSettings.workingProject
                    select iteration).ToList();
        }

        private IList<Team> GetTeamsInProject()
        {
            UserSettings userSettings = SessionWrapper.UserSettings;
            return (from team in db.Teams
                    where team.ProjectID == userSettings.workingProject
                    select team).ToList();
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}