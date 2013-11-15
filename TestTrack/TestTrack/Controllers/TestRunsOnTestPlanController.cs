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
    public class TestRunsOnTestPlanController : BaseController
    {
        // GET: /TestRunsOnTestPlan/
        public ActionResult Index(int id = 0)
        {
            var testPlan = db.TestPlans.Find(id);
            if (testPlan == null) return HttpNotFound();
            var testPlanVM = Mapper.Map<TestPlan, TestPlanVM>(testPlan);

            return View(testPlanVM);
        }

        [ChildActionOnly]
        public ActionResult TestPlanSection(int id = 0)
        {
            var testPlan = db.TestPlans.Find(id);
            if (testPlan == null) return HttpNotFound();

            var testPlanVM = Mapper.Map<TestPlan, TestPlanVM>(testPlan);
            return PartialView("_TestPlan", testPlanVM);
        }

        [ChildActionOnly]
        public ActionResult TestRunsSection(int id = 0)
        {
            var testRuns = (from testRun in db.TestRuns
                            where testRun.TestPlanID == id
                            orderby testRun.Title
                            select testRun).ToList();
            var testRunsVM = Mapper.Map<IList<TestRun>, IList<TestRunVM>>(testRuns);
            return PartialView("_TestRuns", testRunsVM);
        }
    }
}
