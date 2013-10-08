using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TestTrack.Models
{
    public class Team
    {
        [Key]
        public int TeamID { get; set; }

        [Display(Name = "Team Name")]
        [Required]
        [StringLength(100)]
        public string Title { get; set; }

        [Required]
        public int ProjectID { get; set; } // Foreign Key

        [Required]
        public int TestSuiteID { get; set; } // Foreign Key

        public virtual TestSuite TestSuite { get; set; }
        public virtual Project Project { get; set; }

        public DateTime CreatedOn { get; set; }
        public DateTime? LastModified { get; set; }
    }
}