using System.Collections.Generic;
using System.Web.Mvc;
using TestTrack.Models;

namespace TestTrack.ViewModels
{
    public class ResultVM
    {
        public int ResultID { get; set; }

        public int TestCaseID { get; set; }

        public int TestRunID { get; set; }

        public State State { get; set; }

        public IEnumerable<SelectListItem> TestCases { get; set; }

        public List<int> TestCasesInTestRun { get; set; }
 
        public string[] SelectedTestCases { get; set; }
    }

    
}