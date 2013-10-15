using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TestTrack.Models;

namespace TestTrack.ViewModels
{
    public class ProjectsDropdownVM
    {
        public int SelectedValue { get; set; }
        public IEnumerable<SelectListItem> Values { get; set; }
    }
}