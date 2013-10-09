using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace TestTrack.Models
{
    public class Team : IAuditable
    {
        [Key]
        public int TeamID { get; set; }

        [Display(Name = "Team Name")]
        [Required]
        [StringLength(100)]
        public string Title { get; set; }

        [ForeignKey("Project")]
        [Required]
        public int ProjectID { get; set; }

        public virtual Project Project { get; set; }

        public virtual TestSuite TestSuite { get; set; }

        public DateTime CreatedOn { get; set; }
        public DateTime? LastModified { get; set; }
    }
}