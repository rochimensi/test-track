using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using TestTrack.Models;

namespace TestTrack.ViewModels
{
    public class TestRunVM
    {
        public int TestRunID { get; set; }

        [Display(Name = "Title")]
        [Required]
        [StringLength(100)]
        public string Title { get; set; }

        public bool Closed { get; set; }
        
        [HiddenInput(DisplayValue = false)]
        public int TestPlanID { get; set; }

        public TestPlan TestPlan { get; set; }

        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime CreatedOn { get; set; }

        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime? LastModified { get; set; }

        public ICollection<Result> Results { get; set; }
    }
}