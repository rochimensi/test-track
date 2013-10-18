using System.Linq;
using System.Web.Mvc;
using TestTrack.Models;

namespace TestTrack.Controllers
{
    [Authorize]
    public class TestPlanPerIterationController : BaseController
    {
        private TestTrackDBContext db = new TestTrackDBContext();

        // GET: /TestPlanPerIteration/

        public ActionResult Index(int id = 0)
        {
            UserSettings userSettings = SessionWrapper.UserSettings;
            Iteration iteration = null;

            // No Iteration selected by the user
            if (id == 0)
            {
                var itForProject = (from i in db.Iterations
                                    where i.ProjectID == userSettings.workingProject
                                    orderby i.DueDate descending
                                    select i);

                // If there are Iterations created for the project, the first from the list ordered by dueDate is selected as default
                if (itForProject.Count() > 0)
                {
                    iteration = (from i in db.Iterations
                                 where i.ProjectID == userSettings.workingProject
                                 orderby i.DueDate descending 
                                 select i).ToList().First();
                }
            }

            // An iteration was selected by the user
            if (id > 0)
            {
                var itForProject = from i in db.Iterations
                                   where i.IterationID == id && i.ProjectID == userSettings.workingProject
                                   orderby i.DueDate descending
                                   select i;

                if (itForProject.Count() > 0)
                {
                    iteration = itForProject.ToList().First();
                }
                else
                {
                    string url = Request.Url.AbsoluteUri;
                    int lastPath = url.LastIndexOf('/');
                    url = url.Substring(0, lastPath);
                    return Redirect(url);
                }
            }
            return View(iteration);
        }
    }
}
