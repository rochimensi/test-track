using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using TestTrack.Models;

namespace TestTrack.ViewModels
{
    public class DefectVM
    {
        public int DefectID { get; set; }

        public int ResultID { get; set; }

        public int TestCaseID { get; set; }

        public int TestRunID { get; set; }

        [Display(Name = "Title")]
        [Required]
        [StringLength(200)]
        public string DefectTitle { get; set; }

        [StringLength(1000)]
        public string Comments { get; set; }

        public string Labels { get; set; }

        [HiddenInput(DisplayValue = false)]
        public Severity Severity { get; set; }

        public IEnumerable<SelectListItem> Severities { get; set; }
    }
}