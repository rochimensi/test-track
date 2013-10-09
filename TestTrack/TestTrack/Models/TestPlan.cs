﻿using System;
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

        [Display(Name = "Test Plan Name")]
        [Required]
        [StringLength(100)]
        public string Title { get; set; }

        [Required]
        [StringLength(500)]
        public string Description { get; set; }

        [ForeignKey("Iteration")]
        [Required]
        public int IterationID { get; set; }

        public virtual Iteration Iteration { get; set; }
        public virtual ICollection<TestRun> TestRuns { get; set; }

        public DateTime CreatedOn { get; set; }
        public DateTime? LastModified { get; set; }
    }
}