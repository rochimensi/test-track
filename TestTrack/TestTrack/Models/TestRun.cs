using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace TestTrack.Models
{
    public class TestRun : IAuditable
    {
        [Key]
        public int TestRunID { get; set; }

        [Display(Name = "Test Run Name")]
        [Required]
        [StringLength(100)]
        public string Title { get; set; }

        public bool Closed { get; set; }

        [ForeignKey("TestPlan")]
        [Required]
        public int TestPlanID { get; set; }

        public virtual TestPlan TestPlan { get; set; }
        public virtual ICollection<Result> Results { get; set; }

        public DateTime CreatedOn { get; set; }
        public DateTime? LastModified { get; set; }
    }
}