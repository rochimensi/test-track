using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace TestTrack.ViewModels
{
    public class TeamVM
    {
        public int TeamID { get; set; }

        [Display(Name = "Team Name")]
        public string Title { get; set; }

        [HiddenInput(DisplayValue = false)]
        public int ProjectID { get; set; }
          
        public IEnumerable<SelectListItem> Projects { get; set; }
    }
}