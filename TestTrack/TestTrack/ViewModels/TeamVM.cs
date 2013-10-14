using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace TestTrack.Models
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