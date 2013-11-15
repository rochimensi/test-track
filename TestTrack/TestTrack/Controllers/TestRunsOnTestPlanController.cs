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
            TestRunsListVM vm = new TestRunsListVM();
            vm.TestPlanID = id;
            var testPlan = (from value in db.TestPlans
                            where value.TestPlanID == id
                            select value).First();
            vm.TestPlan = testPlan.Title;
            vm.TestPlanDescription = testPlan.Description;
            vm.IterationID = testPlan.IterationID;
            vm.Iteration = testPlan.Iteration.Title;

            vm.TestRuns = from value in db.TestRuns
                         where value.TestPlanID == id && value.Closed == false 
                         orderby value.Title
                         select value;
            vm.ClosedTestRuns = from value in db.TestRuns
                                where value.TestPlanID == id && value.Closed == true
                                orderby value.Title
                                select value;

            return View(vm);
        }
    }
}
