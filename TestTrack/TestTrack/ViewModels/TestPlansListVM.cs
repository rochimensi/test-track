using System.Collections.Generic;
using System.Web.Mvc;

namespace TestTrack.ViewModels
{
    public class TestPlansListVM
    {
        public IEnumerable<SelectListItem> Values { get; set; }
    }
}