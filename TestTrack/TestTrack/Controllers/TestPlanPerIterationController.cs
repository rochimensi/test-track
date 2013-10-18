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
                var iterationForProject = (from i in db.Iterations
                                    where i.ProjectID == userSettings.workingProject
                                    orderby i.DueDate descending
                                    select i);

                // If there are Iterations created for the project, the first from the list ordered by dueDate is selected as default
                if (iterationForProject.Count() > 0)
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
                var iterationForProject = from i in db.Iterations
                                   where i.IterationID == id && i.ProjectID == userSettings.workingProject
                                   orderby i.DueDate descending
                                   select i;

                if (iterationForProject.Count() > 0)
                {
                    iteration = iterationForProject.ToList().First();
                }

                // When user is at /TestPlanPerIteration/Index/{id} and there is no such IterationID for the new selected project, redirects to /TestPlanPerIteration
                else
                {
                    return RedirectToAction("Index", new { id = (int?)null});
                }
            }
            return View(iteration);
        }
    }
}
