using DotNet.Highcharts;
using DotNet.Highcharts.Enums;
using DotNet.Highcharts.Helpers;
using DotNet.Highcharts.Options;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
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
    public class ResultsController : BaseController
    {
        // GET: /Results/
        public ActionResult Index(int id = 0, int tcId = 0)
        {
            var testRun = db.TestRuns.Find(id);
            if (testRun == null) return HttpNotFound();

            var testCase = db.TestCases.Find(tcId);
            if (testCase == null) return HttpNotFound();

            var results = (from r in db.Results
                           where r.TestCaseID == tcId
                           orderby r.CreatedOn descending
                           select r).ToList();
            int[] states = StatesCount(results);

            var resultsPerTestCaseVM = new ResultsPerTestCaseVM
            {
                TestCase = testCase.Title,
                TestCaseID = tcId,
                TestRun = testRun.Title,
                TestRunID = testRun.TestRunID,
                Results = results,
                blocked = states[0],
                failed = states[1],
                passed = states[2],
                retest = states[3]
            };

            return View(resultsPerTestCaseVM);
        }

        [HttpGet]
        public ActionResult Create(int id = 0, string state = "")
        {
            int testRunID = db.Results.Find(id).TestRunID;
            int testCaseID = db.Results.Find(id).TestCaseID;
            ResultsListVM vm = new ResultsListVM
            {
                TestCaseID = testCaseID,
                TestCase = db.TestCases.Find(testCaseID).Title,
                SelectedStateName = state,
                Severities = Common.ToSelectList<TestTrack.Models.Severity>(),
                TestRunID = testRunID
            };
            return PartialView(vm);
        }

        [HttpPost]
        public ActionResult Create(ResultsListVM vm)
        {
            var result = new Result
            {
                TestRunID = vm.TestRunID,
                TestCaseID = vm.TestCaseID,
                TestCase = db.TestCases.Find(vm.TestCaseID),
                TestRun = db.TestRuns.Find(vm.TestRunID),
                Comments = vm.Comments,
                CreatedOn = DateTime.Now,
                State = (State)Enum.Parse(typeof(State), vm.SelectedStateName)
            };
            db.Results.Add(result);
            db.SaveChanges();

            if (vm.DefectTitle != null)
            {
                var defect = new Defect
                {
                    ResultID = result.ResultID,
                    Title = vm.DefectTitle,
                    Severity = vm.Severity,
                    Description = vm.Comments,
                    Labels = vm.Labels,
                    CreatedOn = DateTime.Now
                };
                db.Defects.Add(defect);
                db.SaveChanges();
            }
            return RedirectToAction("Index", "ExecuteTestRun", new { id = vm.TestRunID });
        }

        [HttpGet]
        public ActionResult SetAssignee(int id = 0)
        {
            ResultsListVM vm = new ResultsListVM
            {
                TestCaseID = db.Results.Find(id).TestCaseID,
                ResultID = id,
                TestRunID = db.Results.Find(id).TestRunID
            };

            return PartialView(vm);
        }

        [HttpPost]
        public ActionResult SetAssignee(ResultsListVM vm)
        {
            var result = db.Results.Find(vm.ResultID);
            if (result == null) return HttpNotFound();

            result.AssignedTo = vm.AssignedTo;

            db.Entry(result).State = EntityState.Modified;
            db.SaveChanges();

            return RedirectToAction("Index", "ExecuteTestRun", new { id = vm.TestRunID });
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
                foreach (var testCaseID in vm.SelectedTestCases)
                {
                    if (uncheckedTestCases.Contains(Convert.ToInt32(testCaseID)))
                    {
                        int id = Convert.ToInt32(testCaseID);
                        uncheckedTestCases.RemoveAll(item => item == id);
                    }
                    else
                    {
                        var result = new Result
                        {
                            TestRunID = vm.TestRunID,
                            TestCaseID = Convert.ToInt32(testCaseID),
                            State = State.Untested,
                            TestCase = db.TestCases.Find(Convert.ToInt32(testCaseID)),
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

        [HttpGet]
        public ActionResult Delete(int id = 0)
        {
            Result result = db.Results.Find(id);
            if (result == null)
            {
                return HttpNotFound();
            }
            return PartialView(result);
        }

        [HttpPost, ValidateAntiForgeryToken, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            Result result = db.Results.Find(id);
            if (result.Defects.Count() > 0)
            {
                var defect = result.Defects.First();
                db.Defects.Remove(defect);
                db.SaveChanges();
            }
            int testCaseID = result.TestCaseID;
            int testRunID = result.TestRunID;
            db.Results.Remove(result);
            db.SaveChanges();
            return RedirectToAction("Index", "Results", new { id = testRunID, tcId = testCaseID });
        }

        [ChildActionOnly]
        public ActionResult DrawChart(int id = 0)
        {
            var testRun = db.TestRuns.Find(id);
            if (testRun == null) return HttpNotFound();

            int[] states = StatesCount(GetDistinctResults(testRun.TestRunID));

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

            int[] states = StatesCount(GetDistinctResults(testRun.TestRunID));
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

            int[] states = StatesCount(GetDistinctResults(testRun.TestRunID));
            int sum = (states[0] + states[1] + states[2] + states[3] + states[4]);

            int[] percentages = null;

            if (testRun.Results.Count() > 0)
            {
                percentages = new int[5] { (states[0] * 100) / sum, (states[1] * 100) / sum, (states[2] * 100) / sum, (states[3] * 100) / sum, (states[4] * 100) / sum };
            }
            return PartialView("_ProgressBar", percentages);
        }

        [ChildActionOnly]
        public ActionResult DrawBarChart(int id = 0)
        {
            var testCase = db.TestCases.Find(id);
            if (testCase == null) return HttpNotFound();

            int[] states = StatesCount(testCase.Results);

            Highcharts chart = new Highcharts("chart")
                .InitChart(new Chart { DefaultSeriesType = ChartTypes.Bar })
                .SetTitle(new Title { Text = "Historic Test Case Results by State" })
                .SetSubtitle(new Subtitle { Text = "Blocked - Failed - Passed - Retest" })
                .SetXAxis(new XAxis
                {
                    Title = new XAxisTitle { Text = string.Empty }
                })
                .SetYAxis(new YAxis
                {
                    Min = 0,
                    Title = new YAxisTitle
                    {
                        Text = "Number of results",
                        Align = AxisTitleAligns.High
                    },
                    Max = states.ToList().Max() * 1.2
                })
                .SetTooltip(new Tooltip { Formatter = "function() { return ''+ this.series.name +': '+ this.y +' results'; }" })
                .SetPlotOptions(new PlotOptions
                {
                    Bar = new PlotOptionsBar
                    {
                        DataLabels = new PlotOptionsBarDataLabels { Enabled = true }
                    }
                })
                .SetLegend(new Legend
                {
                    Layout = Layouts.Vertical,
                    Align = HorizontalAligns.Right,
                    VerticalAlign = VerticalAligns.Top,
                    X = -70,
                    Y = 50,
                    Floating = true,
                    BorderWidth = 1,
                    BackgroundColor = new BackColorOrGradient(ColorTranslator.FromHtml("#FFFFFF")),
                    Shadow = true
                })
                .SetCredits(new Credits { Enabled = false })
                .SetSeries(new[]
                    {
                        new Series { Name = "Blocked", Data = new Data(new object[] { states[0] }) },
                        new Series { Name = "Retest", Data = new Data(new object[] { states[3] }) },
                        new Series { Name = "Passed", Data = new Data(new object[] { states[2] }) },
                        new Series { Name = "Failed", Data = new Data(new object[] { states[1] }) }
                    });

            return PartialView("_BarChart", chart);
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

        public int[] StatesCount(ICollection<Result> results)
        {
            int[] statesCount = new int[5] { 0, 0, 0, 0, 0 };

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
    }
}