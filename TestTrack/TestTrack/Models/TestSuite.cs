using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace TestTrack.Models
{
    public class TestSuite : IAuditable
    {
        [Key]
        public int TeamID { get; set; }
        public virtual Team Team { get; set; }

        [Required]
        [StringLength(100)]
        public string Title { get; set; }

        public virtual ICollection<TestCase> TestCases { get; set; }

        public DateTime CreatedOn { get; set; }
        public DateTime? LastModified { get; set; }
    }
}