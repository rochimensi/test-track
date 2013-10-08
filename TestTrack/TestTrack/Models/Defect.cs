using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

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
        [StringLength(100)]
        public string Title { get; set; }

        [Required]
        [StringLength(500)]
        public string Description { get; set; }

        [Required]
        public Severity Severity { get; set; }

        [Required]
        public int ResultID { get; set; } // Foreign Key
        
        public virtual Result Result { get; set; }

        public DateTime CreatedOn { get; set; }
        public DateTime? LastModified { get; set; }
    }
}