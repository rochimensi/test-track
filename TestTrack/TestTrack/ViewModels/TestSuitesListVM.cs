using System.Collections.Generic;
using System.Web.Mvc;

namespace TestTrack.ViewModels
{
    public class TestSuitesListVM
    {
        public IEnumerable<SelectListItem> Values { get; set; }
    }
}