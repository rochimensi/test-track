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
    public class TeamController : Controller
    {
        private TestTrackDBContext db = new TestTrackDBContext();

        //
        // GET: /Team/

        public ActionResult Index()
        {
            var teams = db.Teams.Include(t => t.TestSuite).Include(t => t.Project);
            return View(teams.ToList());
        }

        //
        // GET: /Team/Details/5

        public ActionResult Details(int id = 0)
        {
            Team team = db.Teams.Find(id);
            if (team == null)
            {
                return HttpNotFound();
            }
            return View(team);
        }

        //
        // GET: /Team/Create

        public ActionResult Create()
        {
            ViewBag.TestSuiteID = new SelectList(db.TestSuites, "TestSuiteID", "Title");
            ViewBag.ProjectID = new SelectList(db.Projects, "ProjectID", "Title");
            return View();
        }

        //
        // POST: /Team/Create

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Team team)
        {
            if (ModelState.IsValid)
            {
                db.Teams.Add(team);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.TestSuiteID = new SelectList(db.TestSuites, "TestSuiteID", "Title", team.TestSuiteID);
            ViewBag.ProjectID = new SelectList(db.Projects, "ProjectID", "Title", team.ProjectID);
            return View(team);
        }

        //
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

        //
        // POST: /Team/Edit/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Team team)
        {
            if (ModelState.IsValid)
            {
                db.Entry(team).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.TestSuiteID = new SelectList(db.TestSuites, "TestSuiteID", "Title", team.TestSuiteID);
            ViewBag.ProjectID = new SelectList(db.Projects, "ProjectID", "Title", team.ProjectID);
            return View(team);
        }

        //
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

        //
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