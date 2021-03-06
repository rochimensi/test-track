﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TestTrack.Models
{
    public class Team : IAuditable
    {
        [Key]
        public int TeamID { get; set; }

        [Required]
        [StringLength(100)]
        public string Name { get; set; }

        [ForeignKey("Project")]
        [Required]
        public int ProjectID { get; set; }

        public virtual Project Project { get; set; }

        public virtual TestSuite TestSuite { get; set; }

        public virtual ICollection<TestPlan> TestPlans { get; set; }

        public DateTime CreatedOn { get; set; }
        public DateTime? LastModified { get; set; }
    }
}