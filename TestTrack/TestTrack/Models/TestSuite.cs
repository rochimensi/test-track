using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace TestTrack.Models
{
    public class TestSuite : IAuditable
    {
        [Key]
        public int TeamID { get; set; }

        [Display(Name = "Test Suite Name")]
        [Required]
        [StringLength(100)]
        public string Title { get; set; }

        public virtual ICollection<TestCase> TestCases { get; set; }

        public virtual Team Team { get; set; }

        public DateTime CreatedOn { get; set; }
        public DateTime? LastModified { get; set; }
    }
}