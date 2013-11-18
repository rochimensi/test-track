using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using TestTrack.Models;

namespace TestTrack.ViewModels
{
    public class ResultVM
    {
        public IEnumerable<SelectListItem> States { get; set; }

        [HiddenInput(DisplayValue = false)]
        public State State { get; set; }

        public string SelectedStateName { get; set; }

        public int TestRunID { get; set; }

        public TestRun TestRun { get; set; }

        public int ResultID { get; set; }

        public TestCase TestCase { get; set; }

        public int TestCaseID { get; set; }

        [StringLength(100)]
        public string AssignedTo { get; set; }

        [StringLength(1000)]
        public string Comments { get; set; }

        public string Title { get; set; }

        public string Labels { get; set; }

        [HiddenInput(DisplayValue = false)]
        public Severity Severity { get; set; }

        public IEnumerable<SelectListItem> Severities { get; set; }

        public ICollection<Defect> Defects { get; set; }

        public DateTime CreatedOn { get; set; }
    }
}