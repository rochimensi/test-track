using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace TestTrack.Models
{
    public class Step : IAuditable
    {
        [Key]
        public virtual int StepId { get; set; }

        [ForeignKey("TestCase")]
        [Required]
        public int TestCaseId { get; set; }

        public virtual TestCase TestCase { get; set; }

        [Required]
        [StringLength(1000)]
        public virtual string Action { get; set; }

        [Display(Name = "Expected result")]
        [Required]
        [StringLength(1000)]
        public virtual string Result { get; set; }

        public DateTime CreatedOn { get; set; }
        public DateTime? LastModified { get; set; }
    }
}