using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace TestTrack.Models
{
    public class Iteration : IAuditable
    {
        [Key]
        public int IterationID { get; set; }

        [Display(Name = "Iteration Name")]
        [Required]
        [StringLength(50)]
        public string Title { get; set; }

        [Display(Name = "End Date")]
        [Required]
        [DataType(DataType.Date)]
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