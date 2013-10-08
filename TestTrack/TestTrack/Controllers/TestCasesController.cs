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
    public class TestCasesController : Controller
    {
        private TestTrackDBContext db = new TestTrackDBContext();

        // GET: /TestCases/

        public ActionResult Index()
        {
            var testcases = db.TestCases.Include(t => t.TestSuite);
            return View(testcases.ToList());
        }

        // GET: /TestCases/Create

        public ActionResult Create()
        {
            ViewBag.TestSuiteID = new SelectList(db.TestSuites, "TestSuiteID", "Title");
            return View("Edit", new TestCase());
        }

        // GET: /TestCases/Edit/5

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

        // POST: /TestCases/Save

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Save(TestCase testcase)
        {
            if (ModelState.IsValid)
            {
                if (testcase.TestCaseID == 0)
                {
                    db.TestCases.Add(testcase);
                }
                else
                {
                    db.Entry(testcase).State = EntityState.Modified;
                }

                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.TestSuiteID = new SelectList(db.TestSuites, "TestSuiteID", "Title", testcase.TestSuiteID);
            return View(testcase);
        }

        // GET: /TestCases/Delete/5

        public ActionResult Delete(int id = 0)
        {
            TestCase testcase = db.TestCases.Find(id);
            if (testcase == null)
            {
                return HttpNotFound();
            }
            return View(testcase);
        }

        // POST: /TestCases/Delete/5

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