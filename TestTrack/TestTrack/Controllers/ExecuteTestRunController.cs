using System.Linq;
using System.Web.Mvc;
using TestTrack.Models;
using TestTrack.ViewModels;

namespace TestTrack.Controllers
{
    [Authorize]
    public class ExecuteTestRunController : BaseController
    {
        private TestTrackDBContext db = new TestTrackDBContext();

        // GET: /ExecuteTestRun/

        public ActionResult Index(int id = 0)
        {
            ExecuteTestRunVM vm = new ExecuteTestRunVM();

            var testRun = (from value in db.TestRuns
                            where value.TestRunID == id
                            select value).First();
            vm.TestRun = testRun.Title;
            vm.TestRunID = testRun.TestRunID;
            vm.Closed = testRun.Closed;

            var testPlan = testRun.TestPlan;
            vm.TestPlan = testPlan.Title;
            vm.TestPlanID = testPlan.TestPlanID;
            vm.Iteration = testPlan.Iteration.Title;
            vm.IterationID = testPlan.IterationID;
            vm.TestSuiteID = testPlan.TeamID;

            return View(vm);
        }
    }
}
