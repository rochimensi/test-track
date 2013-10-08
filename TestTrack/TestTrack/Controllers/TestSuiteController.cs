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
    public class TestSuiteController : Controller
    {
        private TestTrackDBContext db = new TestTrackDBContext();

        //
        // GET: /TestSuite/

        public ActionResult Index()
        {
            return View(db.TestSuites.ToList());
        }

        //
        // GET: /TestSuite/Details/5

        public ActionResult Details(int id = 0)
        {
            TestSuite testsuite = db.TestSuites.Find(id);
            if (testsuite == null)
            {
                return HttpNotFound();
            }
            return View(testsuite);
        }

        //
        // GET: /TestSuite/Create

        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /TestSuite/Create

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(TestSuite testsuite)
        {
            if (ModelState.IsValid)
            {
                db.TestSuites.Add(testsuite);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(testsuite);
        }

        //
        // GET: /TestSuite/Edit/5

        public ActionResult Edit(int id = 0)
        {
            TestSuite testsuite = db.TestSuites.Find(id);
            if (testsuite == null)
            {
                return HttpNotFound();
            }
            return View(testsuite);
        }

        //
        // POST: /TestSuite/Edit/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(TestSuite testsuite)
        {
            if (ModelState.IsValid)
            {
                db.Entry(testsuite).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(testsuite);
        }

        //
        // GET: /TestSuite/Delete/5

        public ActionResult Delete(int id = 0)
        {
            TestSuite testsuite = db.TestSuites.Find(id);
            if (testsuite == null)
            {
                return HttpNotFound();
            }
            return View(testsuite);
        }

        //
        // POST: /TestSuite/Delete/5

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