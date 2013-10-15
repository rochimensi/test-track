using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace TestTrack.Models
{
    public class Project : IAuditable
    {
        [Key]
        public int ProjectID { get; set; }

        [Display(Name = "Project name")]
        [Required]
        public string Title { get; set; }

        [Display(Name = "About")]
        [Required]
        [StringLength(200)]
        public string Description { get; set; }

        public DateTime CreatedOn { get; set; }
        public DateTime? LastModified { get; set; }

        public virtual ICollection<Team> Teams { get; set; }
        public virtual ICollection<Iteration> Iterations { get; set; }
    }
}