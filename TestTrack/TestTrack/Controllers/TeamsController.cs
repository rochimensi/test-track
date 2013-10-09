using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TestTrack.Models;

namespace TestTrack.Controllers
{
    [Authorize]
    public class TeamsController : Controller
    {
        private TestTrackDBContext db = new TestTrackDBContext();

        // GET: /Teams/

        public ActionResult Index()
        {
            var teams = db.Teams.Include(t => t.TestSuite).Include(t => t.Project);
            return View(teams.ToList());
        }

        // GET: /Team/Create

        public ActionResult Create()
        {
            ViewBag.TestSuiteID = new SelectList(db.TestSuites, "TestSuiteID", "Title");
            ViewBag.ProjectID = new SelectList(db.Projects, "ProjectID", "Title");
            return View("Edit", new Team());
        }

        // GET: /Team/Edit/5

        public ActionResult Edit(int id = 0)
        {
            Team team = db.Teams.Find(id);
            if (team == null)
            {
                return HttpNotFound();
            }
            ViewBag.TestSuiteID = new SelectList(db.TestSuites, "TestSuiteID", "Title", team.TestSuiteID);
            ViewBag.ProjectID = new SelectList(db.Projects, "ProjectID", "Title", team.ProjectID);
            return View(team);
        }

        // POST: /Teams/Save

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Save(Team team)
        {
            if (ModelState.IsValid)
            {
                if (team.TeamID == 0)
                {
                    db.Teams.Add(team);
                }
                else
                {
                    db.Entry(team).State = EntityState.Modified;
                }
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.TestSuiteID = new SelectList(db.TestSuites, "TestSuiteID", "Title", team.TestSuiteID);
            ViewBag.ProjectID = new SelectList(db.Projects, "ProjectID", "Title", team.ProjectID);
            return View(team);
        }

        // GET: /Team/Delete/5

        public ActionResult Delete(int id = 0)
        {
            Team team = db.Teams.Find(id);
            if (team == null)
            {
                return HttpNotFound();
            }
            return View(team);
        }

        // POST: /Team/Delete/5

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Team team = db.Teams.Find(id);
            db.Teams.Remove(team);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}