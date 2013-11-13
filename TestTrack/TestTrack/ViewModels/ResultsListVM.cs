using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using TestTrack.Models;

namespace TestTrack.ViewModels
{
    public class ResultsListVM
    {
        public IEnumerable<SelectListItem> States { get; set; }

        [HiddenInput(DisplayValue = false)]
        public State SelectedState { get; set; }

        public string SelectedStateName { get; set; }

        public int TestRunID { get; set; }

        public int ResultID { get; set; }

        public string TestCase { get; set; }

        public int TestCaseID { get; set; }

        [StringLength(100)]
        public string AssignedTo { get; set; }

        [StringLength(1000)]
        public string Comments { get; set; }
    }
}