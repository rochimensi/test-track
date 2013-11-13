using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using TestTrack.Helpers;
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
            vm.Results = GetDistinctResults(testRun.TestRunID);

            return View(vm);
        }

        [ChildActionOnly]
        public ActionResult List(int id = 0)
        {
            ResultsListVM vm = GetResultsListVM(id);
            return PartialView("_List", vm);
        }

        [ChildActionOnly]
        public ActionResult ListDisabled(int id = 0)
        {
            ResultsListVM vm = GetResultsListVM(id);
            return PartialView("_ListDisabled", vm);
        }

        private ResultsListVM GetResultsListVM(int id)
        {
            ResultsListVM vm = new ResultsListVM();
            var result = (from value in db.Results
                          where value.ResultID == id
                          select value).First();
            vm.ResultID = id;
            vm.AssignedTo = result.AssignedTo;
            vm.TestRunID = result.TestRunID;
            vm.States = Common.ToSelectList<TestTrack.Models.State>();
            vm.SelectedState = result.State;
            vm.TestCase = result.TestCase.Title;
            vm.TestCaseID = result.TestCaseID;

            return vm;
        }

        private IEnumerable<Result> GetDistinctResults(int testRunID)
        {
            var sortedResults = (from r in db.Results
                                 where r.TestRunID == testRunID
                                 orderby r.TestCaseID, r.CreatedOn descending
                                 select r).ToList();

            List<Result> distinctResults = new List<Result>();
            if (sortedResults.Count() > 0)
                distinctResults.Add(sortedResults.First());

            for (int i = 1; i < sortedResults.Count(); i++)
            {
                if (sortedResults.ElementAt(i).TestCaseID != sortedResults.ElementAt(i - 1).TestCaseID)
                    distinctResults.Add(sortedResults.ElementAt(i));
            }

            return distinctResults;
        }
    }
}
