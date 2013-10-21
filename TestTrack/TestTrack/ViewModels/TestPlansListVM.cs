using System.Collections.Generic;
using System.Web.Mvc;

namespace TestTrack.ViewModels
{
    public class TestPlansListVM
    {
        public IEnumerable<TestTrack.Models.TestPlan> Values { get; set; }
    }
}