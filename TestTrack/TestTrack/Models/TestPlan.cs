using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace TestTrack.Models
{
    public class TestPlan : IAuditable
    {
        [Key]
        public int TestPlanID { get; set; }

        [Required]
        [StringLength(100)]
        public string Title { get; set; }

        [Required]
        [StringLength(1000)]
        public string Description { get; set; }

        [ForeignKey("Iteration")]
        [Required]
        public int IterationID { get; set; }
        public virtual Iteration Iteration { get; set; }

        [ForeignKey("Team")]
        [Required]
        public int TeamID { get; set; }
        public virtual Team Team { get; set; }

        public virtual ICollection<TestRun> TestRuns { get; set; }

        public DateTime CreatedOn { get; set; }
        public DateTime? LastModified { get; set; }
    }
}