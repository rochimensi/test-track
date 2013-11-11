using DotNet.Highcharts;
using DotNet.Highcharts.Enums;
using DotNet.Highcharts.Helpers;
using DotNet.Highcharts.Options;
using System;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;
using TestTrack.Models;
using TestTrack.ViewModels;

namespace TestTrack.Controllers
{
    [Authorize]
    public class IterationsController : BaseController
    {
        private TestTrackDBContext db = new TestTrackDBContext();

        [HttpGet]
        public ActionResult Create(int id = 0)
        {
            var iterationVM = new IterationVM
            {
                StartDate = DateTime.Now,
                DueDate = DateTime.Now,
                ProjectID = id
            };

            return View("Create", iterationVM);
        }

        [HttpPost]
        public ActionResult Create(IterationVM iterationVM)
        {
            Iteration iteration = new Iteration()
            {
                Title = iterationVM.Title,
                StartDate = iterationVM.StartDate,
                DueDate = iterationVM.DueDate,
                ProjectID = iterationVM.ProjectID
            };

            db.Iterations.Add(iteration);
            db.SaveChanges();

            return RedirectToAction("Index", "TestPlanPerIteration");
        }

        [HttpGet]
        public ActionResult Edit(int id = 0)
        {
            Iteration iteration = db.Iterations.Find(id);

            if (iteration == null) return HttpNotFound();

            var iterationVM = new IterationVM
            {
                IterationID = iteration.IterationID,
                Title = iteration.Title,
                StartDate = iteration.StartDate,
                DueDate = iteration.DueDate,
                ProjectID = iteration.ProjectID
            };

            return View(iterationVM);
        }

        [HttpPost]
        public ActionResult Edit(IterationVM iterationVM)
        {
            var iteration = db.Iterations.Find(iterationVM.IterationID);
            if (iteration == null) return HttpNotFound();

            iteration.IterationID = iterationVM.IterationID;
            iteration.Title = iterationVM.Title;
            iteration.DueDate = iterationVM.DueDate;
            iteration.ProjectID = iterationVM.ProjectID;

            db.Entry(iteration).State = EntityState.Modified;
            db.SaveChanges();

            return RedirectToAction("Index", "TestPlanPerIteration");
        }

        public ActionResult Delete(int id = 0)
        {
            Iteration iteration = db.Iterations.Find(id);
            if (iteration == null)
            {
                return HttpNotFound();
            }
            return PartialView(iteration);
        }

        [HttpPost, ValidateAntiForgeryToken, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            Iteration iteration = db.Iterations.Find(id);
            db.Iterations.Remove(iteration);
            db.SaveChanges();
            return RedirectToAction("Index", "TestPlanPerIteration");
        }

        [ChildActionOnly]
        public ActionResult List()
        {
            UserSettings userSettings = SessionWrapper.UserSettings;
            var vm = new IterationsListVM();
            var iterations = (from i in db.Iterations
                              where i.ProjectID == userSettings.workingProject
                              orderby i.DueDate descending
                              select i).ToList();
            vm.Values = new SelectList(iterations, "IterationID", "Title");

            return PartialView("_List", vm);
        }

        [ChildActionOnly]
        public ActionResult DrawLineChart(int id = 0)
        {
            var iteration = db.Iterations.Find(id);
            if (iteration == null) return HttpNotFound();

            int maxYAxis = GetTotalTestCases(iteration);

            Highcharts chart = new Highcharts("chart")
                .SetTitle(new Title { Text = "Burndown chart" })
                .SetSubtitle(new Subtitle { Text = iteration.Title + " - Untested Test Cases" })
                .SetXAxis(new XAxis
                {
                    Type = AxisTypes.Datetime,
                    TickInterval = 24 * 3600 * 1000, // one day
                    TickWidth = 0,
                    GridLineWidth = 1,
                    Labels = new XAxisLabels
                    {
                        Align = HorizontalAligns.Left,
                        X = 3,
                        Y = -3
                    },
                    Categories = GetIterationDates(iteration)
                })
                .SetYAxis(new YAxis
                {
                    Title = new YAxisTitle { Text = "" },
                    Labels = new YAxisLabels
                    {
                        Align = HorizontalAligns.Left,
                        X = 3,
                        Y = 16,
                        Formatter = "function() { return Highcharts.numberFormat(this.value, 0); }",
                    },
                    Min = 0,
                    Max = maxYAxis + maxYAxis * 0.2
                })
                .SetLegend(new Legend
                {
                    Align = HorizontalAligns.Left,
                    VerticalAlign = VerticalAligns.Top,
                    Y = 20,
                    Floating = true,
                    BorderWidth = 0
                })
                .SetTooltip(new Tooltip
                {
                    Shared = true,
                    Crosshairs = new Crosshairs(true)
                })
                .SetPlotOptions(new PlotOptions
                {
                    Series = new PlotOptionsSeries
                    {
                        Cursor = Cursors.Pointer,
                        Point = new PlotOptionsSeriesPoint
                        {
                            Events = new PlotOptionsSeriesPointEvents
                            {
                                Click = @"function() { alert(Highcharts.dateFormat('%A, %b %e, %Y', this.x) +': '+ this.y +' visits'); }"
                            }
                        },
                        Marker = new PlotOptionsSeriesMarker { LineWidth = 1 }
                    }
                })
                .SetSeries(new[]
                    {
                        new Series { Name = "Remaining effort", Data = new Data(GetRemainingEffort(iteration)) },
                        new Series { Name = "Ideal burndown", Data = new Data(new object[] { maxYAxis, 0 }) }
                    });

            return PartialView("_LineChart", chart);
        }

        private string[] GetIterationDates(Iteration iteration)
        {
            int duration = (iteration.DueDate - iteration.StartDate).Days;

            string[] iterationDates = new string[duration];
            iterationDates[0] = iteration.StartDate.ToShortDateString();

            for (int i = 1; i < duration - 1; i++)
            {
                iterationDates[i] = iteration.StartDate.AddDays(i).ToShortDateString();
            }

            iterationDates[duration - 1] = iteration.DueDate.ToShortDateString();
            
            return iterationDates;
        }

        private int GetTotalTestCases(Iteration iteration)
        {
            int count = 0;

            foreach (var testPlan in iteration.TestPlans)
            {
                foreach (var testRun in testPlan.TestRuns)
                {
                    count += testRun.Results.Count();
                }
            }

            return count;
        }

        private object[] GetRemainingEffort(Iteration iteration)
        {
            return new object[1];
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}