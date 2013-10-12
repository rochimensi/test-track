using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace TestTrack.ViewModels
{
    public class ProjectVM
    {
        public int ProjectID { get; set; }

        [Display(Name = "Project name")]
        public string Title { get; set; }

        [Display(Name = "About")] 
        public string Description { get; set; }
         
    }
}