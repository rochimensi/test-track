using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TestTrack.Models
{
    public enum Type
    {
        Functional, Sanity, Regression
    }

    public enum Method
    {
        Automatable, Automated, Manual
    }

    public enum Priority
    {
        High, Medium, Low
    }

    public class TestCase : IAuditable
    {
        [Key]
        public int TestCaseID { get; set; }

        [Required]
        [StringLength(200)]
        public string Title { get; set; }

        [StringLength(1000)]
        public string Description { get; set; }

        [StringLength(1000)]
        public string PreConditions { get; set; }

        [Required]
        public Type Type { get; set; }

        [Required]
        public Priority Priority { get; set; }

        [Required]
        public Method Method { get; set; }

        [StringLength(1000)]
        public string Tags { get; set; } // Will have the format as follows "sanity,log in,etc". Tags are part of the string, separated by commas

        [ForeignKey("TestSuite")]
        [Required]
        public int TestSuiteID { get; set; }
        public virtual TestSuite TestSuite { get; set; }

        public virtual ICollection<Step> Steps { get; set; }
        public virtual ICollection<Result> Results { get; set; }

        public DateTime CreatedOn { get; set; }
        public DateTime? LastModified { get; set; }
    }
}