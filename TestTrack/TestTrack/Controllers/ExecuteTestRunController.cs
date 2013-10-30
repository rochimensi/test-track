using System;
using System.Data;
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
            vm.Results = testRun.Results;

            return View(vm);
        }

        [ChildActionOnly]
        public ActionResult List(int id = 0)
        {
            ResultsListVM vm = new ResultsListVM();
            var result = (from value in db.Results
                            where value.ResultID == id
                            select value).First();
            vm.ResultID = id;
            vm.States = Common.ToSelectList<TestTrack.Models.State>();
            vm.SelectedState = result.State;
            vm.TestCase = result.TestCase.Title;
            vm.TestCaseID = result.TestCaseID;

            return PartialView("_List", vm);
        }

        [ChildActionOnly]
        public ActionResult ListDisabled(int id = 0)
        {
            ResultsListVM vm = new ResultsListVM();
            var result = (from value in db.Results
                          where value.ResultID == id
                          select value).First();
            vm.ResultID = id;
            vm.States = Common.ToSelectList<TestTrack.Models.State>();
            vm.SelectedState = result.State;
            vm.TestCase = result.TestCase.Title;
            vm.TestCaseID = result.TestCaseID;

            return PartialView("_ListDisabled", vm);
        }

        [HttpPost]
        public ActionResult SetResult(ResultsListVM vm)
        {
            var result = db.Results.Find(vm.ResultID);
            if (result == null) return HttpNotFound();
            
            result.State = vm.SelectedState;
            db.Entry(result).State = EntityState.Modified;
            db.SaveChanges();

            return Redirect(Request.UrlReferrer.ToString());
        }
    }
}
