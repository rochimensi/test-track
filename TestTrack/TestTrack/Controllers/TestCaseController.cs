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
    public class TestCaseController : Controller
    {
        private TestTrackDBContext db = new TestTrackDBContext();

        //
        // GET: /TestCase/

        public ActionResult Index()
        {
            var testcases = db.TestCases.Include(t => t.TestSuite);
            return View(testcases.ToList());
        }

        //
        // GET: /TestCase/Details/5

        public ActionResult Details(int id = 0)
        {
            TestCase testcase = db.TestCases.Find(id);
            if (testcase == null)
            {
                return HttpNotFound();
            }
            return View(testcase);
        }

        //
        // GET: /TestCase/Create

        public ActionResult Create()
        {
            ViewBag.TestSuiteID = new SelectList(db.TestSuites, "TestSuiteID", "Title");
            return View();
        }

        //
        // POST: /TestCase/Create

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(TestCase testcase)
        {
            if (ModelState.IsValid)
            {
                db.TestCases.Add(testcase);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.TestSuiteID = new SelectList(db.TestSuites, "TestSuiteID", "Title", testcase.TestSuiteID);
            return View(testcase);
        }

        //
        // GET: /TestCase/Edit/5

        public ActionResult Edit(int id = 0)
        {
            TestCase testcase = db.TestCases.Find(id);
            if (testcase == null)
            {
                return HttpNotFound();
            }
            ViewBag.TestSuiteID = new SelectList(db.TestSuites, "TestSuiteID", "Title", testcase.TestSuiteID);
            return View(testcase);
        }

        //
        // POST: /TestCase/Edit/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(TestCase testcase)
        {
            if (ModelState.IsValid)
            {
                db.Entry(testcase).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.TestSuiteID = new SelectList(db.TestSuites, "TestSuiteID", "Title", testcase.TestSuiteID);
            return View(testcase);
        }

        //
        // GET: /TestCase/Delete/5

        public ActionResult Delete(int id = 0)
        {
            TestCase testcase = db.TestCases.Find(id);
            if (testcase == null)
            {
                return HttpNotFound();
            }
            return View(testcase);
        }

        //
        // POST: /TestCase/Delete/5

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            TestCase testcase = db.TestCases.Find(id);
            db.TestCases.Remove(testcase);
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