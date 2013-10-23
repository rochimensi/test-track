using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TestTrack.Models;
using TestTrack.ViewModels;

namespace TestTrack.Controllers
{
    [Authorize]
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

        public ActionResult Create(int id)
        {
            TestSuite testSuite = new TestSuite();
            testSuite.TeamID = id;

            ViewBag.TeamID = new SelectList(db.Teams, "TeamID", "Title");
            return View("Edit", testSuite);
        }

        // GET: /TestSuites/Edit/5

        public ActionResult Edit(int id = 0)
        {
            TestSuite testsuite = db.TestSuites.Find(id);
            if (testsuite == null)
            {
                return HttpNotFound();
            }
            ViewBag.TeamID = new SelectList(db.Teams, "TeamID", "Title", testsuite.TeamID);
            return View(testsuite);
        }

        // POST: /TestSuites/Save

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Save(TestSuite testsuite)
        {
            if (ModelState.IsValid)
            {
//                if (testsuite.TeamID == 0)
                if (db.TestSuites.AsQueryable().Count(t => t.TeamID == testsuite.TeamID) == 0)
                {
                    db.TestSuites.Add(testsuite);
                }
                else
                {
                    db.Entry(testsuite).State = EntityState.Modified;
                }

                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.TeamID = new SelectList(db.Teams, "TeamID", "Title", testsuite.TeamID);
            return View(testsuite);
        }

        // GET: /TestSuites/Delete/5

        public ActionResult Delete(int id = 0)
        {
            TestSuite testsuite = db.TestSuites.Find(id);
            if (testsuite == null)
            {
                return HttpNotFound();
            }
            return View(testsuite);
        }

        // POST: /TestSuites/Delete/5

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            TestSuite testsuite = db.TestSuites.Find(id);
            db.TestSuites.Remove(testsuite);
            db.SaveChanges();
            return RedirectToAction("Index");
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

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}