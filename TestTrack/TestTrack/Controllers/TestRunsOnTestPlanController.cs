using System.Linq;
using System.Web.Mvc;
using TestTrack.Models;
using TestTrack.ViewModels;

namespace TestTrack.Controllers
{
    [Authorize]
    public class TestRunsOnTestPlanController : BaseController
    {
        private TestTrackDBContext db = new TestTrackDBContext();

        // GET: /TestRunsOnTestPlan/

        public ActionResult Index(int id = 0)
        {
            TestRunsListVM vm = null;

            var values = from value in db.TestRuns
                         where value.TestPlanID == id
                         orderby value.Title
                         select value;
            // There are test runs for the test plan
            if (values.Count() > 0)
            {
                vm = new TestRunsListVM();
                vm.Values = values;
                vm.TestPlanID = id;
                var testPlan = (from value in db.TestPlans
                                where value.TestPlanID == id
                                select value).ToList().First();
                vm.TestPlan = testPlan.Title;
                vm.TestPlanDescription = testPlan.Description;
                vm.IterationID = testPlan.IterationID;
                vm.Iteration = testPlan.Iteration.Title;
            }

            return View(vm);
        }
    }
}
