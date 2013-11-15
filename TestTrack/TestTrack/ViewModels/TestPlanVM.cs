using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace TestTrack.ViewModels
{
    public class TestPlanVM
    {
        public int TestPlanID { get; set; }

        [Display(Name = "Title")]
        [Required]
        [StringLength(100)]
        public string Title { get; set; }

        [Display(Name = "Description")]
        [Required]
        [StringLength(1000)]
        public string Description { get; set; }

        [HiddenInput(DisplayValue = false)]
        public int IterationID { get; set; }
        public IEnumerable<SelectListItem> Iterations { get; set; }

        [Display(Name = "Team")]
        [Required]
        [HiddenInput(DisplayValue = false)]
        public int TeamID { get; set; }

        public IEnumerable<SelectListItem> Teams { get; set; }

        public DateTime CreatedOn { get; set; }
    }
}