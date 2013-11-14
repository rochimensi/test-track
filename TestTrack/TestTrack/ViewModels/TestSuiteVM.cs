using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace TestTrack.ViewModels
{
    public class TestSuiteVM
    {
        [Display(Name = "Title")]
        [Required]
        [StringLength(100)]
        public string Title { get; set; }

        [Display(Name = "Team")]
        [Required]
        [HiddenInput(DisplayValue = false)]
        public int TeamID { get; set; }
        public IEnumerable<SelectListItem> Teams { get; set; }
    }
}