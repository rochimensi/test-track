using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TestTrack.Models
{
    public enum State
    {
        Blocked, Failed, Passed, Retest, Untested
    }

    public class Result : IAuditable
    {
        [Key]
        public int ResultID { get; set; }

        [Required]
        public int TestCaseID { get; set; } // Foreign Key

        [Required]
        public int TestRunID { get; set; } // Foreign Key

        [Required]
        public State State { get; set; }

        public virtual TestCase TestCase { get; set; }
        public virtual TestRun TestRun { get; set; }

        public virtual ICollection<Defect> Defects { get; set; }

        public DateTime CreatedOn { get; set; }
        public DateTime? LastModified { get; set; }
    }
}