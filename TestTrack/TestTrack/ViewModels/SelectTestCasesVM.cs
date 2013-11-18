using System.Collections.Generic;
using System.Web.Mvc;
using TestTrack.Models;

namespace TestTrack.ViewModels
{
    public class SelectTestCasesVM
    {
        public int TestRunID { get; set; }

        public IEnumerable<SelectListItem> TestCases { get; set; }

        public List<int> TestCasesInTestRun { get; set; }
 
        public string[] SelectedTestCases { get; set; }
    }
}