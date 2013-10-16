using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace TestTrack.ViewModels
{
    public class IterationVM
    {
        public int IterationID { get; set; }

        [Display(Name = "Iteration Name")]
        public string Title { get; set; }

        [Display(Name = "End Date")]
        [DataType(DataType.Date)]
        public DateTime DueDate { get; set; }

        [HiddenInput(DisplayValue = false)]
        public int ProjectID { get; set; }
          
        public IEnumerable<SelectListItem> Projects { get; set; }
    }
}