using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace TestTrack.ViewModels
{
    public class TestPlanVM
    {
        public int TestPlanID { get; set; }

        [Display(Name = "Test Plan Name")]
        public string Title { get; set; }

        [Display(Name = "About")]
        public string Description { get; set; }

        [HiddenInput(DisplayValue = false)]
        public int IterationID { get; set; }

        [HiddenInput(DisplayValue = false)]
        public int TeamID { get; set; }
          
        public IEnumerable<SelectListItem> Teams { get; set; }

        public IEnumerable<SelectListItem> Iterations { get; set; }
    }
}