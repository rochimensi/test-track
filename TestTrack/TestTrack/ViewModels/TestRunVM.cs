using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace TestTrack.ViewModels
{
    public class TestRunVM
    {
        public int TestRunID { get; set; }

        [Display(Name = "Test Run Name")]
        public string Title { get; set; }

        [HiddenInput(DisplayValue = false)]
        public int TestPlanID { get; set; }

        public bool Closed { get; set; }
    }
}