using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using TestTrack.Helpers;
using TestTrack.Models;

namespace TestTrack.Controllers
{
    public class BaseController : Controller
    {
        protected TestTrackDBContext db = new TestTrackDBContext();

        public SessionWrapper SessionWrapper { get; set; }

        public BaseController()
        {
            SessionWrapper = new SessionWrapper();
        }
        
        protected IList<Iteration> GetIterationsInProject()
        {
            UserSettings userSettings = SessionWrapper.UserSettings;
            return (from iteration in db.Iterations
                    where iteration.ProjectID == userSettings.workingProject
                    select iteration).ToList();
        }

        protected IList<Team> GetTeamsInProject()
        {
            UserSettings userSettings = SessionWrapper.UserSettings;
            return (from team in db.Teams
                    where team.ProjectID == userSettings.workingProject
                    select team).ToList();
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}