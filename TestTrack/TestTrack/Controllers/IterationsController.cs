using AutoMapper;
using DotNet.Highcharts;
using DotNet.Highcharts.Enums;
using DotNet.Highcharts.Helpers;
using DotNet.Highcharts.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using TestTrack.Filters;
using TestTrack.Models;
using TestTrack.ViewModels;

namespace TestTrack.Controllers
{
    [Authorize]
    [ProjectsAvailability]
    public class IterationsController : BaseController
    {
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
            var iteration = Mapper.Map<IterationVM, Iteration>(iterationVM);
            db.Iterations.Add(iteration);
            db.SaveChanges();

            return RedirectToAction("Index", "TestPlanPerIteration");
        }

        [HttpGet]
        public ActionResult Edit(int id = 0)
        {
            Iteration iteration = db.Iterations.Find(id);
            if (iteration == null) return HttpNotFound();
            var iterationVM = Mapper.Map<Iteration, IterationVM>(iteration);

            return View(iterationVM);
        }

        [HttpPost]
        public ActionResult Edit(IterationVM iterationVM)
        {
            var iteration = db.Iterations.Find(iterationVM.IterationID);
            if (iteration == null) return HttpNotFound();
            db.Entry(iteration).CurrentValues.SetValues(iterationVM);
            db.SaveChanges();

            return RedirectToAction("Index", "TestPlanPerIteration", new { id = iteration.IterationID });
        }

        public ActionResult Delete(int id = 0)
        {
            Iteration iteration = db.Iterations.Find(id);
            if (iteration == null) return HttpNotFound();

            return PartialView(iteration);
        }

        [HttpPost, ValidateAntiForgeryToken, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            db.Iterations.Remove(db.Iterations.Find(id));
            db.SaveChanges();

            return RedirectToAction("Index", "TestPlanPerIteration");
        }

        [ChildActionOnly]
        public ActionResult List()
        {
            UserSettings userSettings = SessionWrapper.UserSettings;
            var iterations = (from i in db.Iterations
                              where i.ProjectID == userSettings.workingProject
                              orderby i.DueDate descending
                              select i).ToList();

            var iterationsVM = Mapper.Map<IList<Iteration>, IList<IterationVM>>(iterations);

            return PartialView("_List", iterationsVM);
        }

        [ChildActionOnly]
        public ActionResult DrawLineChart(int id = 0)
        {
            var iteration = db.Iterations.Find(id);
            if (iteration == null) return HttpNotFound();

            var remainingEffort = GetRemainingEffortPerDay(iteration);

            Highcharts chart = new Highcharts("chart")
                .InitChart(new Chart
                {
                    DefaultSeriesType = ChartTypes.Spline,
                    Width = 1000,
                    Style = "margin: '0 auto'"
                })
                .SetTitle(new Title { Text = "Burndown chart" })
                .SetSubtitle(new Subtitle { Text = iteration.Title + " - Untested Test Cases" })
                .SetXAxis(new XAxis
                {
                    Title = new XAxisTitle { Text = "Progress" },
                    Labels = new XAxisLabels { Enabled = false },
                    MaxPadding = 0.05,
                    TickLength = 0
                })
                .SetYAxis(new YAxis
                {
                    Title = new YAxisTitle { Text = "Untested Test Cases" },
                    Labels = new YAxisLabels { Formatter = "function() { return this.value + ' test cases'; }" },
                    LineWidth = 2
                })
                .SetLegend(new Legend
                {
                    Align = HorizontalAligns.Left,
                    VerticalAlign = VerticalAligns.Top,
                    Y = 20,
                    Floating = true,
                    BorderWidth = 0
                })
                .SetTooltip(new Tooltip { Formatter = "function() { return ''+ this.y +' test cases'; }" })
                .SetPlotOptions(new PlotOptions { Spline = new PlotOptionsSpline { Marker = new PlotOptionsSplineMarker { Enabled = true } } })
                .SetSeries(new Series[] {
                    new Series
                    {
                        Name = "Remaining Effort",
                        Data = new Data(remainingEffort)
                    },
                    new Series
                    {
                        Name = "Ideal Burndown",
                        Data = new Data(new object[,]
                            {
                                { iteration.StartDate, GetTotalTestCases(iteration) }, { iteration.DueDate, 0 }
                            })
                    }
                });

            return PartialView("_LineChart", chart);
        }

        private int GetTotalTestCases(Iteration iteration)
        {
            int count = 0;
            foreach (var testPlan in iteration.TestPlans)
            {
                foreach (var testRun in testPlan.TestRuns)
                {
                    count += GetDistinctResults(testRun.TestRunID).Count();
                }
            }

            return count;
        }

        private object[,] GetRemainingEffortPerDay(Iteration iteration)
        {
            // Initialize list with dates
            List<string> dates = new List<string>();
            List<DateTime> xAxisData = new List<DateTime>();
            DateTime d = iteration.StartDate;
            while (d.ToShortDateString().CompareTo(DateTime.Now.ToShortDateString()) <= 0)
            {
                dates.Add(d.ToShortDateString());
                xAxisData.Add(d);
                d = d.AddDays(1);
            }

            // Initialize array with untested counts as 0 for each date
            int[] untested = new int[dates.Count];
            for (int date = 0; date < untested.Length; date++)
            {
                untested[date] = GetTotalTestCases(iteration);
            }

            for (int i = 0; i < dates.Count(); i++)
            {
                int count = 0;
                foreach (var testPlan in iteration.TestPlans)
                    foreach (var testRun in testPlan.TestRuns)
                    {
                        ICollection<Result> distinctResults = GetDistinctResults(testRun.TestRunID);
                        foreach (var result in distinctResults)
                        {
                            if (result.State.Equals(State.Untested) && result.CreatedOn.ToShortDateString().CompareTo(dates.ElementAt(i)) <= 0)
                                count++;
                        }
                    }
                untested[i] = count;
            }

            // Build 2D object array to return
            var data = new object[dates.Count, 2];

            for (int i = 0; i < dates.Count(); i++)
            {
                data[i, 0] = xAxisData.ElementAt(i);
                data[i, 1] = untested[i];
            }

            return data;
        }
    }
}