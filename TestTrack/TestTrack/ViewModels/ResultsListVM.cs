﻿using System.Collections.Generic;
using System.Web.Mvc;
using TestTrack.Models;

namespace TestTrack.ViewModels
{
    public class ResultsListVM
    {
        public IEnumerable<SelectListItem> States { get; set; }

        [HiddenInput(DisplayValue = false)]
        public State SelectedState { get; set; }

        public int ResultID { get; set; }

        public string TestCase { get; set; }

        public int TestCaseID { get; set; }
    }
}