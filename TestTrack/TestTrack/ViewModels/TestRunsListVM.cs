using System.Collections.Generic;
using System.Web.Mvc;

namespace TestTrack.ViewModels
{
    public class TestRunsListVM
    {
        public int IterationID { get; set; }

        public string Iteration { get; set; }

        public int TestPlanID { get; set; }

        public string TestPlan { get; set; }

        public string TestPlanDescription { get; set; }

        public IEnumerable<TestTrack.Models.TestRun> TestRuns { get; set; }
    }
}