using System.Linq;
using System.Web.Mvc;
using System.Web.Routing;
using TestTrack.Models;

namespace TestTrack.Filters
{
    public class ProjectsAvailability : ActionFilterAttribute, IActionFilter
    {
        void IActionFilter.OnActionExecuting(ActionExecutingContext filterContext)
        {
            TestTrackDBContext db = new TestTrackDBContext();

            if (db.Projects.Count() == 0)
            {
                filterContext.Result = new RedirectToRouteResult(
                    new RouteValueDictionary { { "controller", "Projects" }, { "action", "Index" } });
            }
        }
    }
}