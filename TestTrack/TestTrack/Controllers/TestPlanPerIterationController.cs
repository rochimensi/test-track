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
                iteration = (from i in db.Iterations
                             where i.IterationID == id && i.ProjectID == userSettings.workingProject
                             orderby i.DueDate descending 
                             select i).ToList().First();
            }
            return View(iteration);
        }
    }
}
