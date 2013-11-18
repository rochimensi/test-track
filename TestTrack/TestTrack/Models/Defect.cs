using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace TestTrack.Models
{
    public enum Severity
    {
        Critical, High, Medium, Low
    }

    public class Defect : IAuditable
    {
        [Key]
        public int DefectID { get; set; }

        [Required]
        [StringLength(200)]
        public string Title { get; set; }

        [Required]
        [StringLength(1000)]
        public string Comments { get; set; }

        [Required]
        public Severity Severity { get; set; }

        public string Labels { get; set; }

        [ForeignKey("Result")]
        [Required]
        public int ResultID { get; set; }

        public virtual Result Result { get; set; }

        public DateTime CreatedOn { get; set; }
        public DateTime? LastModified { get; set; }
    }
}