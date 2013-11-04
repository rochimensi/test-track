using DotNet.Highcharts;
using DotNet.Highcharts.Enums;
using DotNet.Highcharts.Helpers;
using DotNet.Highcharts.Options;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
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

        public ActionResult Index(int id = 0)
        {
            var testCase = db.TestCases.Find(id);
            if(testCase == null) return HttpNotFound();

            int[] states = StatesCount(testCase.Results);

            var resultsPerTestCaseVM = new ResultsPerTestCaseVM
            {
                TestCase = testCase.Title,
                TestCaseID = id,
                Results = testCase.Results
            };

            return View(resultsPerTestCaseVM);
        }

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

            return PartialView(vm);
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

        [ChildActionOnly]
        public ActionResult DrawChart(int id = 0)
        {
            var testRun = db.TestRuns.Find(id);
            if (testRun == null) return HttpNotFound();

            int[] states = StatesCount(testRun.Results);

            Highcharts chart = new Highcharts("chart")
                .InitChart(new Chart { PlotShadow = false, PlotBackgroundColor = null, PlotBorderWidth = null })
                .SetTitle(new Title { Text = "Test Run Current Results" })
                .SetTooltip(new Tooltip { Formatter = "function() { return '<b>'+ this.point.name +'</b>: '+ this.percentage +' % - '+ this.point.y + ' Test Case(s)'; }" })
                .SetPlotOptions(new PlotOptions
                {
                    Pie = new PlotOptionsPie
                    {
                        AllowPointSelect = true,
                        Cursor = Cursors.Pointer,
                        DataLabels = new PlotOptionsPieDataLabels { Enabled = false },
                        ShowInLegend = true
                    }
                })
                .SetSeries(new Series
                {
                    Type = ChartTypes.Pie,
                    Name = "Current status",
                    Data = new Data(new object[]
                            {
                                new object[] { "Blocked", states[0] },
                                new object[] { "Retest", states[3] },
                                new object[] { "Passed", states[2] },
                                new object[] { "Failed", states[1] },
                                new object[] { "Untested", states[4] }
                            })
                });

            return PartialView("_PieChart", chart);
        }

        [ChildActionOnly]
        public ActionResult Percentage(int id = 0)
        {
            var testRun = db.TestRuns.Find(id);
            if (testRun == null) return HttpNotFound();

            int[] states = StatesCount(testRun.Results);
            int percentage = 0; 

            if (testRun.Results.Count() > 0)
            {
                percentage = (states[2] * 100) / (states[0] + states[1] + states[2] + states[3] + states[4]);
            }
    
            return PartialView("_Percentage", percentage);
        }

        [ChildActionOnly]
        public ActionResult ProgressBar(int id = 0)
        {
            var testRun = db.TestRuns.Find(id);
            if (testRun == null) return HttpNotFound();

            int[] states = StatesCount(testRun.Results);
            int sum = (states[0] + states[1] + states[2] + states[3] + states[4]);

            int[] percentages = null; 

            if (testRun.Results.Count() > 0)
            {
                percentages = new int[5] { (states[0] * 100) / sum, (states[1] * 100) / sum, (states[2] * 100) / sum, (states[3] * 100) / sum, (states[4] * 100) / sum };
            }
            return PartialView("_ProgressBar", percentages);
        }

        public int[] StatesCount(ICollection<Result> results)
        {
            int[] statesCount = new int[5]{0,0,0,0,0};

            foreach (var result in results)
            {
                switch (result.State)
                {
                    case State.Blocked:
                        statesCount[0]++;
                        break;
                    case State.Failed:
                        statesCount[1]++;
                        break;
                    case State.Passed:
                        statesCount[2]++;
                        break;
                    case State.Retest:
                        statesCount[3]++;
                        break;
                    case State.Untested:
                        statesCount[4]++;
                        break;
                    default:
                        break;
                }
            }
            return statesCount;
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}