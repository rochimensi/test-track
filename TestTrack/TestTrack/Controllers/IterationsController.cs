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
    public class IterationsController : Controller
    {
        private TestTrackDBContext db = new TestTrackDBContext();

        // GET: /Iterations/

        public ActionResult Index()
        {
            var iterations = db.Iterations.Include(i => i.Project);
            return View(iterations.ToList());
        }

        // GET: /Iterations/Create

        public ActionResult Create(int id)
        {
            Iteration iteration = new Iteration();
            iteration.ProjectID = id;
            
            ViewBag.ProjectID = new SelectList(db.Projects, "ProjectID", "Title");
            return View("Edit", iteration);
        }
        
        // GET: /Iterations/Edit/5

        public ActionResult Edit(int id = 0)
        {
            Iteration iteration = db.Iterations.Find(id);
            if (iteration == null)
            {
                return HttpNotFound();
            }
            ViewBag.ProjectID = new SelectList(db.Projects, "ProjectID", "Title", iteration.ProjectID);
            return View(iteration);
        }

        // POST: /Iterations/Save

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Save(Iteration iteration)
        {
            if (ModelState.IsValid)
            {
                if (iteration.IterationID == 0)
                {
                    db.Iterations.Add(iteration);
                }
                else
                {
                    db.Entry(iteration).State = EntityState.Modified;
                }

                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.ProjectID = new SelectList(db.Projects, "ProjectID", "Title", iteration.ProjectID);
            return View(iteration);
        }

        // GET: /Iterations/Delete/5

        public ActionResult Delete(int id = 0)
        {
            Iteration iteration = db.Iterations.Find(id);
            if (iteration == null)
            {
                return HttpNotFound();
            }
            return View(iteration);
        }

        // POST: /Iterations/Delete/5

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Iteration iteration = db.Iterations.Find(id);
            db.Iterations.Remove(iteration);
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