using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TestTrack.Models;
using TestTrack.ViewModels;

namespace TestTrack.Controllers
{
    [Authorize]
    public class ResultsController : BaseController
    {
        private TestTrackDBContext db = new TestTrackDBContext();

        // GET: /Results/

        public ActionResult Index()
        {
            var results = db.Results.Include(r => r.TestCase).Include(r => r.TestRun);
            return View(results.ToList());
        }

        // GET: /Results/Create

        [HttpGet]
        public ActionResult AssignTestCases(int id = 0)
        {
            var testRun = db.TestRuns.Find(id);

            if (testRun == null) return HttpNotFound();

            ResultVM vm = new ResultVM();
            vm.TestRunID = testRun.TestRunID;
            var testSuite = (from tc in db.TestCases
                             where tc.TestSuiteID == testRun.TestPlan.TeamID
                             orderby tc.Title
                             select tc).ToList();
            vm.TestCases = new SelectList(testSuite, "TestCaseID", "Title");

            vm.TestCasesInTestRun = new List<int>();
            foreach (var item in testRun.Results)
            {
                vm.TestCasesInTestRun.Add(item.TestCaseID);
            }

            return View(vm);
        }

        [HttpPost]
        public ActionResult AssignTestCases(ResultVM vm)
        {
            List<int> uncheckedTestCases = new List<int>(from value in db.Results
                                                         where value.TestRunID == vm.TestRunID
                                                         select value.TestCaseID).ToList();

            // If there is at least 1 test case checked
            if (vm.SelectedTestCases != null)
            {
                foreach (var item in vm.SelectedTestCases)
                {
                    if (uncheckedTestCases.Contains(Convert.ToInt32(item)))
                    {
                        uncheckedTestCases.Remove(Convert.ToInt32(item));
                    }
                    else
                    {
                        var result = new Result
                        {
                            TestRunID = vm.TestRunID,
                            TestCaseID = Convert.ToInt32(item),
                            State = State.Untested,
                            TestCase = db.TestCases.Find(Convert.ToInt32(item)),
                            TestRun = db.TestRuns.Find(vm.TestRunID)
                        };

                        db.Results.Add(result);
                        db.SaveChanges();
                    }
                }
            }

            RemoveResults(uncheckedTestCases, vm.TestRunID);

            return RedirectToAction("Index", "ExecuteTestRun", new { id = vm.TestRunID });
        }

        private void RemoveResults(List<int> uncheckedTestCases, int testRunID)
        {
            foreach (var item in uncheckedTestCases)
            {
                var result = (from value in db.Results
                              where value.TestRunID == testRunID && value.TestCaseID == item
                              select value).First();
                db.Results.Remove(result);
                db.SaveChanges();
            }
        }



        // GET: /Results/Delete/5

        public ActionResult Delete(int id = 0)
        {
            Result result = db.Results.Find(id);
            if (result == null)
            {
                return HttpNotFound();
            }
            return View(result);
        }

        // POST: /Results/Delete/5

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Result result = db.Results.Find(id);
            db.Results.Remove(result);
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