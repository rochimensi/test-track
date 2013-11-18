using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using TestTrack.Models;

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

        public Team Team { get; set; }

        public IEnumerable<SelectListItem> Teams { get; set; }

        public ICollection<TestCase> TestCases { get; set; }
    }
}