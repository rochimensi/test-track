using System.Collections.Generic;
using System.Web.Mvc;

namespace TestTrack.ViewModels
{
    public class TestCasesListVM
    {
        public IEnumerable<TestTrack.Models.TestCase> Values { get; set; }
    }
}