using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TestTrack.Models;
using TestTrack.ViewModels;
using TestTrack.Helpers;

namespace TestTrack.Controllers
{
    [Authorize]
    public class TestCasesController : BaseController
    {
        private TestTrackDBContext db = new TestTrackDBContext();

        // GET: /TestCases/

        public ActionResult Index()
        {
            var testcases = db.TestCases.Include(t => t.TestSuite);
            return View(testcases.ToList());
        }

        // GET: /TestCases/Create

        [HttpGet]
        public ActionResult Create()
        {
            var testCaseVM = new TestCaseVM
            {
                Types = Common.ToSelectList<TestTrack.Models.Type>(),
                Priorities = Common.ToSelectList<TestTrack.Models.Priority>(),
                Methods = Common.ToSelectList<TestTrack.Models.Method>(),
                TestSuites = new SelectList(db.TestSuites, "TeamID", "Title")
            };

            return View("Create", testCaseVM);
        }

        // POST: /TestCases/Create

        [HttpPost]
        public ActionResult Create(TestCaseVM testCaseVM)
        {
            var testCase = new TestCase
            {
                Title = testCaseVM.Title,
                Description = testCaseVM.Description,
                PreConditions = testCaseVM.PreConditions,
                Tags = testCaseVM.Tags,
                Type = testCaseVM.Type,
                Priority = testCaseVM.Priority,
                Method = testCaseVM.Method,
                TestSuiteID = testCaseVM.TestSuiteID
            };

            db.TestCases.Add(testCase);
            db.SaveChanges();

            return RedirectToAction("Index");
        }

        // GET: /TestCases/Edit/5

        [HttpGet]
        public ActionResult Edit(int id = 0)
        {
            var testCase = db.TestCases.Find(id);
            if (testCase == null) return HttpNotFound();

            var testCaseVM = new TestCaseVM
            {
                Title = testCase.Title,
                Description = testCase.Description,
                PreConditions = testCase.PreConditions,
                Tags = testCase.Tags,
                Type = testCase.Type,
                Types = Common.ToSelectList<TestTrack.Models.Type>(),
                Priority = testCase.Priority,
                Priorities = Common.ToSelectList<TestTrack.Models.Priority>(),
                Method = testCase.Method,
                Methods = Common.ToSelectList<TestTrack.Models.Method>(),
                TestSuiteID = testCase.TestSuiteID,
                TestSuites = new SelectList(db.TestSuites, "TeamID", "Title")
            };
            
            return View(testCaseVM);
        }

        // POST: /TestCases/Edit/5

        [HttpPost]
        public ActionResult Edit(TestCaseVM testCaseVM)
        {
            var testCase = db.TestCases.Find(testCaseVM.TestCaseID);
            if (testCase == null) return HttpNotFound();

            testCase.Title = testCaseVM.Title;
            testCase.Description = testCaseVM.Description;
            testCase.PreConditions = testCaseVM.PreConditions;
            testCase.Tags = testCaseVM.Tags;
            testCase.Type = testCaseVM.Type;
            testCase.Priority = testCaseVM.Priority;
            testCase.Method = testCaseVM.Method;
            testCase.TestSuiteID = testCaseVM.TestSuiteID;

            db.Entry(testCase).State = EntityState.Modified;
            db.SaveChanges();

            return RedirectToAction("Index");
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

        [ChildActionOnly]
        public ActionResult List(int id = 0)
        {
            var vm = new TestCasesListVM();
            vm.Values = from tc in db.TestCases
                        where tc.TestSuiteID == id
                        select tc;

            return PartialView("_List", vm);
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}