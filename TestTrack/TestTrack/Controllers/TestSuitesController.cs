using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
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
        private TestTrackDBContext db = new TestTrackDBContext();

        // GET: /TestSuites/

        public ActionResult Index()
        {
            var testsuites = db.TestSuites.Include(t => t.Team);
            return View(testsuites.ToList());
        }

        // GET: /TestSuites/Create
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
            var testsuite = new TestSuite
            {
                Title = testSuiteVM.Title,
                TeamID = testSuiteVM.TeamID
            };

            db.TestSuites.Add(testsuite);
            db.SaveChanges();

            return RedirectToAction("Index", "TestCasesPerTestSuite", new { id = testsuite.TeamID });
        }

        // GET: /TestSuites/Edit/5
        [HttpGet]
        public ActionResult Edit(int id = 0)
        {
            TestSuite testsuite = db.TestSuites.Find(id);
            if (testsuite == null) return HttpNotFound();

            TestSuiteVM testSuiteVM = new TestSuiteVM
            {
                Title = testsuite.Title,
                TeamID = testsuite.TeamID,
                Teams = new SelectList(GetTeamsWithNoTestsuite(), "TeamID", "Name")
            };

            return View("Edit", testSuiteVM);
        }

        // POST: /TestSuites/Edit/5
        [HttpPost]
        public ActionResult Edit(TestSuiteVM testSuiteVM)
        {
            TestSuite testsuite = db.TestSuites.Find(testSuiteVM.TeamID);
            if (testsuite == null) return HttpNotFound();

            testsuite.Title = testSuiteVM.Title;
            testsuite.TeamID = testSuiteVM.TeamID;

            db.Entry(testsuite).State = EntityState.Modified;
            db.SaveChanges();

            return RedirectToAction("Index", "TestCasesPerTestSuite", new { id = testsuite.TeamID });
        }

        // GET: /TestSuites/Delete/5

        public ActionResult Delete(int id = 0)
        {
            TestSuite testsuite = db.TestSuites.Find(id);
            if (testsuite == null) return HttpNotFound();
            return PartialView(testsuite);
        }

        // POST: /TestSuites/Delete/5

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
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
            var vm = new TestSuitesListVM();
            var testSuites = (from ts in db.TestSuites
                              where ts.Team.ProjectID == userSettings.workingProject
                              orderby ts.Title ascending
                              select ts).ToList();
            vm.Values = new SelectList(testSuites, "TeamID", "Title");

            return PartialView("_List", vm);
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

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}