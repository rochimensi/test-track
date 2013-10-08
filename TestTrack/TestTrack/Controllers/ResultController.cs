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
    public class ResultController : Controller
    {
        private TestTrackDBContext db = new TestTrackDBContext();

        //
        // GET: /Result/

        public ActionResult Index()
        {
            var results = db.Results.Include(r => r.TestCase).Include(r => r.TestRun);
            return View(results.ToList());
        }

        //
        // GET: /Result/Details/5

        public ActionResult Details(int id = 0)
        {
            Result result = db.Results.Find(id);
            if (result == null)
            {
                return HttpNotFound();
            }
            return View(result);
        }

        //
        // GET: /Result/Create

        public ActionResult Create()
        {
            ViewBag.TestCaseID = new SelectList(db.TestCases, "TestCaseID", "Title");
            ViewBag.TestRunID = new SelectList(db.TestRuns, "TestRunID", "Title");
            return View();
        }

        //
        // POST: /Result/Create

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Result result)
        {
            if (ModelState.IsValid)
            {
                db.Results.Add(result);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.TestCaseID = new SelectList(db.TestCases, "TestCaseID", "Title", result.TestCaseID);
            ViewBag.TestRunID = new SelectList(db.TestRuns, "TestRunID", "Title", result.TestRunID);
            return View(result);
        }

        //
        // GET: /Result/Edit/5

        public ActionResult Edit(int id = 0)
        {
            Result result = db.Results.Find(id);
            if (result == null)
            {
                return HttpNotFound();
            }
            ViewBag.TestCaseID = new SelectList(db.TestCases, "TestCaseID", "Title", result.TestCaseID);
            ViewBag.TestRunID = new SelectList(db.TestRuns, "TestRunID", "Title", result.TestRunID);
            return View(result);
        }

        //
        // POST: /Result/Edit/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Result result)
        {
            if (ModelState.IsValid)
            {
                db.Entry(result).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.TestCaseID = new SelectList(db.TestCases, "TestCaseID", "Title", result.TestCaseID);
            ViewBag.TestRunID = new SelectList(db.TestRuns, "TestRunID", "Title", result.TestRunID);
            return View(result);
        }

        //
        // GET: /Result/Delete/5

        public ActionResult Delete(int id = 0)
        {
            Result result = db.Results.Find(id);
            if (result == null)
            {
                return HttpNotFound();
            }
            return View(result);
        }

        //
        // POST: /Result/Delete/5

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Result result = db.Results.Find(id);
            db.Results.Remove(result);
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