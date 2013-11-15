using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TestTrack.Models
{
    public class Iteration : IAuditable
    {
        [Key]
        public int IterationID { get; set; }

        [Required]
        [StringLength(100)]
        public string Title { get; set; }

        [Display(Name = "Start Date")]
        [Required]
        public DateTime StartDate { get; set; }

        [Display(Name = "End Date")]
        [Required]
        public DateTime DueDate { get; set; }

        [ForeignKey("Project")]
        [Required]
        public int ProjectID { get; set; }

        public virtual Project Project { get; set; }
        public virtual ICollection<TestPlan> TestPlans { get; set; }

        public DateTime CreatedOn { get; set; }
        public DateTime? LastModified { get; set; }
    }
}