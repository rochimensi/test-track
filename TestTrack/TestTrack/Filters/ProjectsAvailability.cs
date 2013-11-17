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

            int projectsCount = db.Projects.Count();
            bool isProjectAccesibleAction = filterContext.RouteData.Values["controller"].ToString() == "Projects" && (filterContext.RouteData.Values["action"].ToString() == "Index" || filterContext.RouteData.Values["action"].ToString() == "Create");
            
            if (projectsCount == 0 && !isProjectAccesibleAction)
            {
                filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary { { "controller", "Projects" }, { "action", "Index" } });
            }
            else if (projectsCount == 0 && isProjectAccesibleAction)
            {
                filterContext.Controller.ViewBag.ThereIsProjects = false;
            }
            else
            {
                filterContext.Controller.ViewBag.ThereIsProjects = true;
            }
        }
    }
}