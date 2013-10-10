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
            var teams = db.Teams.Include(t => t.Project).Include(t => t.TestSuite);
            return View(teams.ToList());
        }

        // GET: /Teams/Create

        public ActionResult Create()
        {
            ViewBag.ProjectID = new SelectList(db.Projects, "ProjectID", "Title");
            ViewBag.TeamID = new SelectList(db.TestSuites, "TeamID", "Title");
            return View("Edit", new Team());
        }

        // GET: /Teams/Edit/5

        public ActionResult Edit(int id = 0)
        {
            Team team = db.Teams.Find(id);
            if (team == null)
            {
                return HttpNotFound();
            }
            ViewBag.ProjectID = new SelectList(db.Projects, "ProjectID", "Title", team.ProjectID);
            ViewBag.TeamID = new SelectList(db.TestSuites, "TeamID", "Title", team.TeamID);
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
            ViewBag.ProjectID = new SelectList(db.Projects, "ProjectID", "Title", team.ProjectID);
            ViewBag.TeamID = new SelectList(db.TestSuites, "TeamID", "Title", team.TeamID);
            return View(team);
        }

        // GET: /Teams/Delete/5

        public ActionResult Delete(int id = 0)
        {
            Team team = db.Teams.Find(id);
            if (team == null)
            {
                return HttpNotFound();
            }
            return View(team);
        }

        // POST: /Teams/Delete/5

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