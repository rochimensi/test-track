using System.Collections.Generic;
using System.Web.Mvc;
using TestTrack.Models;

namespace TestTrack.ViewModels
{
    public class ProjectsListVM
    {
        public int SelectedProject { get; set; }
        public IEnumerable<Project> Values { get; set; }
    }
}