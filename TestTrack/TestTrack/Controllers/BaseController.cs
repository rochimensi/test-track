using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using TestTrack.Helpers;
using TestTrack.Infrastructure.EF;
using TestTrack.Models;

namespace TestTrack.Controllers
{
    public class BaseController : Controller
    {
        //Property Injection for testTrackDbContext see WindsorInstaller
        //TODO: next iteration try to beetr use constructor injector
        public IDbContext db { get; set; }

        public SessionWrapper SessionWrapper { get; set; }

        public BaseController()
        {
            SessionWrapper = new SessionWrapper();
        }

        //TODO: Queries should be moved to Repository
        protected IList<Iteration> GetIterationsInProject()
        {
            UserSettings userSettings = SessionWrapper.UserSettings;
            return (from iteration in db.Iterations
                    where iteration.ProjectID == userSettings.workingProject
                    select iteration).ToList();
        }

        //TODO: Queries should be moved to Repository
        protected IList<Team> GetTeamsInProject()
        {
            UserSettings userSettings = SessionWrapper.UserSettings;
            return (from team in db.Teams
                    where team.ProjectID == userSettings.workingProject
                    select team).ToList();
        }

        //TODO: Queries should be moved to Repository
        protected ICollection<Result> GetDistinctResults(int testRunID)
        {
            var sortedResults = (from r in db.Results
                                 where r.TestRunID == testRunID
                                 orderby r.TestCaseID, r.CreatedOn descending
                                 select r).ToList();

            List<Result> distinctResults = new List<Result>();
            if (sortedResults.Count() > 0)
                distinctResults.Add(sortedResults.First());

            for (int i = 1; i < sortedResults.Count(); i++)
            {
                if (sortedResults.ElementAt(i).TestCaseID != sortedResults.ElementAt(i - 1).TestCaseID)
                    distinctResults.Add(sortedResults.ElementAt(i));
            }

            return distinctResults;
        }
    }
}