using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using TestTrack.Models;

namespace TestTrack.ViewModels
{
    public class TestCaseVM
    {
        public int TestCaseID { get; set; }

        [Display(Name = "Title")]
        [Required]
        [StringLength(200)]
        public string Title { get; set; }

        [StringLength(1000)]
        public string Description { get; set; }

        [Display(Name = "Pre conditions")]
        [StringLength(1000)]
        public string PreConditions { get; set; }

        [HiddenInput(DisplayValue = false)]
        [Required]
        public Type Type { get; set; }
        public IEnumerable<SelectListItem> Types { get; set; }

        [HiddenInput(DisplayValue = false)]
        [Required]
        public Priority Priority { get; set; }
        public IEnumerable<SelectListItem> Priorities { get; set; }

        [HiddenInput(DisplayValue = false)]
        [Required]
        public Method Method { get; set; }
        public IEnumerable<SelectListItem> Methods { get; set; }

        [StringLength(1000)]
        public string Tags { get; set; }

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