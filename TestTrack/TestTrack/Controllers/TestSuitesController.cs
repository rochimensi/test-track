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
    public class TestSuitesController : Controller
    {
        private TestTrackDBContext db = new TestTrackDBContext();

        // GET: /TestSuites/

        public ActionResult Index()
        {
            return View(db.TestSuites.ToList());
        }

        // GET: /TestSuites/Create

        public ActionResult Create()
        {
            return View("Edit", new TestSuite());
        }

        // GET: /TestSuites/Edit/5

        public ActionResult Edit(int id = 0)
        {
            TestSuite testsuite = db.TestSuites.Find(id);
            if (testsuite == null)
            {
                return HttpNotFound();
            }
            return View(testsuite);
        }

        // POST: /TestSuites/Save

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Save(TestSuite testsuite)
        {
            if (ModelState.IsValid)
            {
                if (testsuite.TestSuiteID == 0)
                {
                    db.TestSuites.Add(testsuite);
                }
                else
                {
                    db.Entry(testsuite).State = EntityState.Modified;
                }
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(testsuite);
        }

        // GET: /TestSuites/Delete/5

        public ActionResult Delete(int id = 0)
        {
            TestSuite testsuite = db.TestSuites.Find(id);
            if (testsuite == null)
            {
                return HttpNotFound();
            }
            return View(testsuite);
        }

        // POST: /TestSuites/Delete/5

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            TestSuite testsuite = db.TestSuites.Find(id);
            db.TestSuites.Remove(testsuite);
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