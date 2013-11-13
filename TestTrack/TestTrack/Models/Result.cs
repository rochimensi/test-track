using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

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

        [ForeignKey("TestCase")]
        [Required]
        public int TestCaseID { get; set; }

        [ForeignKey("TestRun")]
        [Required]
        public int TestRunID { get; set; }

        [Required]
        public State State { get; set; }

        [StringLength(1000)]
        public string Comments { get; set; }

        [StringLength(100)]
        public string AssignedTo { get; set; }

        public virtual TestCase TestCase { get; set; }
        public virtual TestRun TestRun { get; set; }

        public virtual ICollection<Defect> Defects { get; set; }

        public DateTime CreatedOn { get; set; }
        public DateTime? LastModified { get; set; }
    }
}