using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace TestTrack.ViewModels
{
    public class ProjectVM
    {
        public int ProjectID { get; set; }

        [Display(Name = "Name")]
        [Required]
        [StringLength(100)]
        public string Title { get; set; }

        [Display(Name = "About")]
        [Required]
        [StringLength(1000)]
        public string Description { get; set; }
    }
}