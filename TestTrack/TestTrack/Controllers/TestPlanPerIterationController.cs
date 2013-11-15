using AutoMapper;
using System.Linq;
using System.Web.Mvc;
using TestTrack.Filters;
using TestTrack.Models;
using TestTrack.ViewModels;

namespace TestTrack.Controllers
{
    [Authorize]
    [ProjectsAvailability]
    public class TestPlanPerIterationController : BaseController
    {
        // GET: /TestPlanPerIteration/
        public ActionResult Index(int id = 0)
        {
            UserSettings userSettings = SessionWrapper.UserSettings;

            var iterationVM = new IterationVM
            {
                ProjectID = userSettings.workingProject,
                Project = db.Projects.Find(userSettings.workingProject)
            };
                        
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
                    Iteration iteration = iterationForProject.First();
                    iterationVM = Mapper.Map<Iteration, IterationVM>(iteration);
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
                    Iteration iteration = iterationForProject.First();
                    iterationVM = Mapper.Map<Iteration, IterationVM>(iteration);
                }

                // When user is at /TestPlanPerIteration/Index/{id} and there is no such IterationID for the new selected project, redirects to /TestPlanPerIteration
                else
                {
                    return RedirectToAction("Index", new { id = (int?)null });
                }
            }
            return View(iterationVM);
        }
    }
}
