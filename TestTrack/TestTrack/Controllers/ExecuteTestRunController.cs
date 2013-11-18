using AutoMapper;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using TestTrack.Filters;
using TestTrack.Helpers;
using TestTrack.Models;
using TestTrack.ViewModels;

namespace TestTrack.Controllers
{
    [Authorize]
    [ProjectsAvailability]
    public class ExecuteTestRunController : BaseController
    {
        public ActionResult Index(int id = 0)
        {
            var testRun = (from value in db.TestRuns
                           where value.TestRunID == id
                           select value).First();
            var testRunVM = Mapper.Map<TestRun, TestRunVM>(testRun);
            testRunVM.Results = GetDistinctResults(testRun.TestRunID);
            
            return View(testRunVM);
        }

        [ChildActionOnly]
        public ActionResult List(int id = 0)
        {
            ResultVM vm = GetResultVM(id);
            return PartialView("_List", vm);
        }

        [ChildActionOnly]
        public ActionResult ListDisabled(int id = 0)
        {
            ResultVM vm = GetResultVM(id);
            return PartialView("_ListDisabled", vm);
        }

        private ResultVM GetResultVM(int id)
        {
            
            var result = db.Results.Find(id);
            var resultVM = Mapper.Map<Result, ResultVM>(result);
            resultVM.States = Common.ToSelectList<TestTrack.Models.State>();
            
            return resultVM;
        }

        private ICollection<Result> GetDistinctResults(int testRunID)
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
