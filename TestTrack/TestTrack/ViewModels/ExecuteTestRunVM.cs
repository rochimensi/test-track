using System.Collections.Generic;
namespace TestTrack.ViewModels
{
    public class ExecuteTestRunVM
    {
        public int IterationID { get; set; }

        public string Iteration { get; set; }

        public int TestPlanID { get; set; }

        public string TestPlan { get; set; }

        public int TestRunID { get; set; }

        public int TestSuiteID { get; set; }

        public string TestRun { get; set; }

        public bool Closed { get; set; }

        public IEnumerable<TestTrack.Models.Result> Results { get; set; }
    }
}