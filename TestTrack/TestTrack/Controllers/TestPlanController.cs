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
    public class TestPlanController : Controller
    {
        private TestTrackDBContext db = new TestTrackDBContext();

        //
        // GET: /TestPlan/

        public ActionResult Index()
        {
            var testplans = db.TestPlans.Include(t => t.Iteration);
            return View(testplans.ToList());
        }

        //
        // GET: /TestPlan/Details/5

        public ActionResult Details(int id = 0)
        {
            TestPlan testplan = db.TestPlans.Find(id);
            if (testplan == null)
            {
                return HttpNotFound();
            }
            return View(testplan);
        }

        //
        // GET: /TestPlan/Create

        public ActionResult Create()
        {
            ViewBag.IterationID = new SelectList(db.Iterations, "IterationID", "Title");
            return View();
        }

        //
        // POST: /TestPlan/Create

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(TestPlan testplan)
        {
            if (ModelState.IsValid)
            {
                db.TestPlans.Add(testplan);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.IterationID = new SelectList(db.Iterations, "IterationID", "Title", testplan.IterationID);
            return View(testplan);
        }

        //
        // GET: /TestPlan/Edit/5

        public ActionResult Edit(int id = 0)
        {
            TestPlan testplan = db.TestPlans.Find(id);
            if (testplan == null)
            {
                return HttpNotFound();
            }
            ViewBag.IterationID = new SelectList(db.Iterations, "IterationID", "Title", testplan.IterationID);
            return View(testplan);
        }

        //
        // POST: /TestPlan/Edit/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(TestPlan testplan)
        {
            if (ModelState.IsValid)
            {
                db.Entry(testplan).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.IterationID = new SelectList(db.Iterations, "IterationID", "Title", testplan.IterationID);
            return View(testplan);
        }

        //
        // GET: /TestPlan/Delete/5

        public ActionResult Delete(int id = 0)
        {
            TestPlan testplan = db.TestPlans.Find(id);
            if (testplan == null)
            {
                return HttpNotFound();
            }
            return View(testplan);
        }

        //
        // POST: /TestPlan/Delete/5

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            TestPlan testplan = db.TestPlans.Find(id);
            db.TestPlans.Remove(testplan);
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