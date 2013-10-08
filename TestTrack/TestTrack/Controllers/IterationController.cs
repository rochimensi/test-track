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
    public class IterationController : Controller
    {
        private TestTrackDBContext db = new TestTrackDBContext();

        //
        // GET: /Iteration/

        public ActionResult Index()
        {
            var iterations = db.Iterations.Include(i => i.Project);
            return View(iterations.ToList());
        }

        //
        // GET: /Iteration/Details/5

        public ActionResult Details(int id = 0)
        {
            Iteration iteration = db.Iterations.Find(id);
            if (iteration == null)
            {
                return HttpNotFound();
            }
            return View(iteration);
        }

        //
        // GET: /Iteration/Create

        public ActionResult Create()
        {
            ViewBag.ProjectID = new SelectList(db.Projects, "ProjectID", "Title");
            return View();
        }

        //
        // POST: /Iteration/Create

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Iteration iteration)
        {
            if (ModelState.IsValid)
            {
                db.Iterations.Add(iteration);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.ProjectID = new SelectList(db.Projects, "ProjectID", "Title", iteration.ProjectID);
            return View(iteration);
        }

        //
        // GET: /Iteration/Edit/5

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

        //
        // POST: /Iteration/Edit/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Iteration iteration)
        {
            if (ModelState.IsValid)
            {
                db.Entry(iteration).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.ProjectID = new SelectList(db.Projects, "ProjectID", "Title", iteration.ProjectID);
            return View(iteration);
        }

        //
        // GET: /Iteration/Delete/5

        public ActionResult Delete(int id = 0)
        {
            Iteration iteration = db.Iterations.Find(id);
            if (iteration == null)
            {
                return HttpNotFound();
            }
            return View(iteration);
        }

        //
        // POST: /Iteration/Delete/5

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