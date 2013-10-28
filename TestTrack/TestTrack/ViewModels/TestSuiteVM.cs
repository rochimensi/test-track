using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace TestTrack.ViewModels
{
    public class TestSuiteVM
    {
        [Display(Name = "Test Suite Name")]
        public string Title { get; set; }

        [HiddenInput(DisplayValue = false)]
        public int TeamID { get; set; }
          
        public IEnumerable<SelectListItem> Teams { get; set; }
    }
}