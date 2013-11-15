using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using TestTrack.Models;

namespace TestTrack.ViewModels
{
    public class TeamVM
    {
        public int TeamID { get; set; }

        [Display(Name = "Name")]
        [Required]
        [StringLength(100)]
        public string Name { get; set; }

        [Display(Name = "Project")]
        [HiddenInput(DisplayValue = false)]
        public int ProjectID { get; set; }

        public Project Project { get; set; }

        public ICollection<TestPlan> TestPlans { get; set; }
    }
}