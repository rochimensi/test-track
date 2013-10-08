using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace TestTrack.Models
{
    public class TestSuite : IAuditable
    {
        [Key]
        public int TestSuiteID { get; set; }

        [Display(Name = "Test Suite Name")]
        [Required]
        [StringLength(100)]
        public string Title { get; set; }

        public virtual ICollection<TestCase> TestCases { get; set; }

        public DateTime CreatedOn { get; set; }
        public DateTime? LastModified { get; set; }
    }
}