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
    public class IterationsController : BaseController
    {
        private TestTrackDBContext db = new TestTrackDBContext();

        // GET: /Iterations/

        public ActionResult Index()
        {
            var iterations = db.Iterations.Include(i => i.Project);
            return View(iterations.ToList());
        }

        [HttpGet]
        public ActionResult Create()
        {
            var iterationVM = new IterationVM
            {
                Projects = new SelectList(db.Projects.ToList(), "ProjectID", "Title")
            };

            return View("Create", iterationVM);
        }

        [HttpPost]
        public ActionResult Create(IterationVM iterationVM)
        {
            Iteration iteration = new Iteration()
            {
                Title = iterationVM.Title,
                DueDate = iterationVM.DueDate,
                ProjectID = iterationVM.ProjectID,
                Project = db.Projects.Find(iterationVM.ProjectID)
            };

            db.Iterations.Add(iteration);
            db.SaveChanges();

            return RedirectToAction("Index");
        }

        [HttpGet]
        public ActionResult Edit(int id = 0)
        {
            Iteration iteration = db.Iterations.Find(id);

            if (iteration == null) return HttpNotFound();

            var iterationVM = new IterationVM
            {
                IterationID = iteration.IterationID,
                Title = iteration.Title,
                DueDate = iteration.DueDate,
                ProjectID = iteration.ProjectID,
                Projects = new SelectList(db.Projects.ToList(), "ProjectID", "Title", iteration.ProjectID)
            };

            return View(iterationVM);
        }

        [HttpPost]
        public ActionResult Edit(IterationVM iterationVM)
        {
            var iteration = db.Iterations.Find(iterationVM.IterationID);

            if (iteration == null) return HttpNotFound();
            
            iteration.IterationID = iterationVM.IterationID;
            iteration.Title = iterationVM.Title;
            iteration.DueDate = iterationVM.DueDate;
            iteration.ProjectID = iterationVM.ProjectID;
            iteration.Project = db.Projects.Find(iterationVM.ProjectID);

            db.Entry(iteration).State = EntityState.Modified;
            db.SaveChanges();

            return RedirectToAction("Index");
        }

        public ActionResult Delete(int id = 0)
        {
            Iteration iteration = db.Iterations.Find(id);
            if (iteration == null)
            {
                return HttpNotFound();
            }
            return View(iteration);
        }

        [HttpPost, ValidateAntiForgeryToken, ActionName("Delete")]
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