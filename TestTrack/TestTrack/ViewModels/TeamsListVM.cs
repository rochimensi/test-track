using System.Collections.Generic;
using System.Web.Mvc;
using TestTrack.Models;

namespace TestTrack.ViewModels
{
    public class TeamsListVM
    {
        public IEnumerable<Team> Teams { get; set; }
    }
}