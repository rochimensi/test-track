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
                // If there are Iterations created, the first from the list ordered by dueDate is selected as default
                if (db.Iterations.Count() > 0)
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
