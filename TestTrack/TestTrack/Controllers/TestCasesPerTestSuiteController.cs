﻿using System.Linq;
using System.Web.Mvc;
using TestTrack.Models;

namespace TestTrack.Controllers
{
    [Authorize]
    public class TestCasesPerTestSuiteController : BaseController
    {
        private TestTrackDBContext db = new TestTrackDBContext();

        public ActionResult Index(int id = 0)
        {
            UserSettings userSettings = SessionWrapper.UserSettings;
            TestSuite testSuite = null;

            // No Test Suite selected by the user
            if (id == 0)
            {
                var testSuites = from ts in db.TestSuites
                                           where ts.Team.ProjectID == userSettings.workingProject
                                           select ts;

                // If there are Test Suites created for the project, the first from the list ordered by title is selected as default
                testSuite = testSuites.Count() > 0 ? testSuites.First() : null;
            }

            // A Test Suite was selected by the user
            if (id > 0)
            {
                var testSuites = from ts in db.TestSuites
                                           where ts.TeamID == id && ts.Team.ProjectID == userSettings.workingProject
                                           select ts;

                if (testSuites.Count() > 0)
                {
                    testSuite = testSuites.First();
                }
                // When user is at /TestCasesPerTestSuite/Index/{id} and there is no such TestCase for the new selected project, redirects to /TestCasesPerTestSuite
                else
                {
                    return RedirectToAction("Index", new { id = (int?)null });
                }
            }

            return View(testSuite);
        }
    }
}