﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace TestTrack.Models
{
    public class Project : IAuditable
    {
        [Key]
        public int ProjectID { get; set; }

        [Required]
        [StringLength(100)]
        public string Title { get; set; }

        [Required]
        [StringLength(1000)]
        public string Description { get; set; }

        public DateTime CreatedOn { get; set; }
        public DateTime? LastModified { get; set; }

        public virtual ICollection<Team> Teams { get; set; }
        public virtual ICollection<Iteration> Iterations { get; set; }
    }
}