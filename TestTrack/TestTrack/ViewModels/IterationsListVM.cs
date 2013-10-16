using System.Collections.Generic;
using System.Web.Mvc;

namespace TestTrack.ViewModels
{
    public class IterationsListVM
    {
        public IEnumerable<SelectListItem> Values { get; set; }
    }
}