using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using TestTrack.Models;

namespace TestTrack.ViewModels
{
    public class TestCaseVM
    {
        public int TestCaseID { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        [Display(Name = "Pre conditions")]
        public string PreConditions { get; set; }

        [HiddenInput(DisplayValue = false)]
        public Type Type { get; set; }

        public IEnumerable<SelectListItem> Types { get; set; }

        [HiddenInput(DisplayValue = false)]
        public Priority Priority { get; set; }

        public IEnumerable<SelectListItem> Priorities { get; set; }

        [HiddenInput(DisplayValue = false)]
        public Method Method { get; set; }

        public IEnumerable<SelectListItem> Methods { get; set; }

        public string Tags { get; set; }

        [Display(Name = "Test Suite")]
        [HiddenInput(DisplayValue = false)]
        public int TestSuiteID { get; set; }

        public string TestSuite { get; set; }

        public IEnumerable<TestTrack.Models.Step> Steps { get; set; }

        public string[] labels { get; set; }
        public string[] action { get; set; }
        public string[] result { get; set; }
        public string[] stepsID { get; set; } 
    }
}