using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TestTrack.ViewModels
{
    public class ResultsPerTestCaseVM
    {
        public ICollection<TestTrack.Models.Result> Results { get; set; }
        
        public int TestCaseID { get; set; }

        public string TestCase { get; set; }

        public int blocked { get; set; }
        public int failed { get; set; }
        public int passed { get; set; }
        public int retest { get; set; }
    }
}