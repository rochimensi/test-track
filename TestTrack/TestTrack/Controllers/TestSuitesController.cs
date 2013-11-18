using AutoMapper;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using TestTrack.Filters;
using TestTrack.Models;
using TestTrack.ViewModels;

namespace TestTrack.Controllers
{
    [Authorize]
    [ProjectsAvailability]
    public class TestSuitesController : BaseController
    {
        [HttpGet]
        public ActionResult Create()
        {   
            TestSuiteVM testSuiteVM = new TestSuiteVM
            {
                Teams = new SelectList(GetTeamsWithNoTestsuite(), "TeamID", "Name")
            };

            return View("Create", testSuiteVM);
        }

        [HttpPost]
        public ActionResult Create(TestSuiteVM testSuiteVM)
        {
            var testsuite = Mapper.Map<TestSuiteVM, TestSuite>(testSuiteVM);
            db.TestSuites.Add(testsuite);
            db.SaveChanges();

            return RedirectToAction("Index", "TestCasesPerTestSuite", new { id = testsuite.TeamID });
        }

        [HttpGet]
        public ActionResult Edit(int id = 0)
        {
            TestSuite testsuite = db.TestSuites.Find(id);
            if (testsuite == null) return HttpNotFound();
            TestSuiteVM testSuiteVM = Mapper.Map<TestSuite, TestSuiteVM>(testsuite);
            testSuiteVM.Teams = new SelectList(GetTeamsWithNoTestsuite(), "TeamID", "Name");
            
            return View("Edit", testSuiteVM);
        }

        [HttpPost]
        public ActionResult Edit(TestSuiteVM testSuiteVM)
        {
            TestSuite testsuite = db.TestSuites.Find(testSuiteVM.TeamID);
            if (testsuite == null) return HttpNotFound();
            db.Entry(testsuite).CurrentValues.SetValues(testSuiteVM);
            db.SaveChanges();

            return RedirectToAction("Index", "TestCasesPerTestSuite", new { id = testsuite.TeamID });
        }

        public ActionResult Delete(int id = 0)
        {
            TestSuite testsuite = db.TestSuites.Find(id);
            if (testsuite == null) return HttpNotFound();
            return PartialView(testsuite);
        }

        [HttpPost, ValidateAntiForgeryToken, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            TestSuite testsuite = db.TestSuites.Find(id);
            db.TestSuites.Remove(testsuite);
            db.SaveChanges();
            return RedirectToAction("Index", "TestCasesPerTestSuite");
        }

        [ChildActionOnly]
        public ActionResult List()
        {
            UserSettings userSettings = SessionWrapper.UserSettings;
            var testSuites = (from ts in db.TestSuites
                              where ts.Team.ProjectID == userSettings.workingProject
                              orderby ts.Title ascending
                              select ts).ToList();
            var testSuitesVM = Mapper.Map<IList<TestSuite>, IList<TestSuiteVM>>(testSuites);

            return PartialView("_List", testSuitesVM);
        }

        private ICollection<Team> GetTeamsWithNoTestsuite()
        {
            UserSettings userSettings = SessionWrapper.UserSettings;
            var teamsInProject = (from team in db.Teams
                                 where team.ProjectID == userSettings.workingProject
                                 select team).ToList();
            var testSuites = (from ts in db.TestSuites
                              select ts.TeamID).ToList();

            IList<Team> teamsWithNoTestSuite = new List<Team>();
            foreach (var team in teamsInProject)
            {
                teamsWithNoTestSuite.Add(team);
            }
            foreach (var team in teamsInProject)
            {
                if (testSuites.Contains(team.TeamID))
                    teamsWithNoTestSuite.Remove(team);
            }

            return teamsWithNoTestSuite;
        }
    }
}