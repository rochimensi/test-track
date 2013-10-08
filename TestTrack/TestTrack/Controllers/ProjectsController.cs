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
    public class ProjectsController : Controller
    {
        private TestTrackDBContext db = new TestTrackDBContext();

        // GET: /Projects/

        public ActionResult Index()
        {
            return View(db.Projects.ToList());
        }

        // GET: /Projects/Create

        public ActionResult Create()
        {
            return View("Edit", new Project());
        }

        // GET: /Projects/Edit/5

        public ActionResult Edit(int id = 0)
        {
            Project project = db.Projects.Find(id);
            if (project == null)
            {
                return HttpNotFound();
            }
            return View(project);
        }

        // POST: /Projects/Save/

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Save(Project project)
        {
            if (ModelState.IsValid)
            {
                if (project.ProjectID == 0)
                {
                    db.Projects.Add(project);
                }
                else
                {
                    db.Entry(project).State = EntityState.Modified;
                }

                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(project);
        }

        // GET: /Projects/Delete/5

        public ActionResult Delete(int id = 0)
        {
            Project project = db.Projects.Find(id);
            if (project == null)
            {
                return HttpNotFound();
            }
            return View(project);
        }

        // POST: /Projects/Delete/5

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Project project = db.Projects.Find(id);
            db.Projects.Remove(project);
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