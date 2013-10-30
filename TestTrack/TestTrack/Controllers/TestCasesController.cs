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

        public ActionResult Index(int id = 0)
        {
            var testCase = db.TestCases.Find(id);
            if (testCase == null) return HttpNotFound();

            var testCaseVM = new TestCaseVM
            {
                TestCaseID = id,
                Title = testCase.Title,
                Description = testCase.Description,
                PreConditions = testCase.PreConditions,
                Tags = testCase.Tags,
                Type = testCase.Type,
                Priority = testCase.Priority,
                Method = testCase.Method,
                TestSuiteID = testCase.TestSuiteID,
                TestSuite = db.TestSuites.Find(testCase.TestSuiteID).Title,
                Steps = testCase.Steps
            };

            return View(testCaseVM);
        }

        // GET: /TestCases/Create

        [HttpGet]
        public ActionResult Create(int id = 0)
        {
            var testCaseVM = new TestCaseVM
            {
                Types = Common.ToSelectList<TestTrack.Models.Type>(),
                Priorities = Common.ToSelectList<TestTrack.Models.Priority>(),
                Methods = Common.ToSelectList<TestTrack.Models.Method>(),
                TestSuiteID = id
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
                TestSuiteID = testCaseVM.TestSuiteID,
            };

            db.TestCases.Add(testCase);
            db.SaveChanges();

            for (int i = 0; i < testCaseVM.action.Length; i++)
            {
                var step = new Step
                {
                    Action = testCaseVM.action[i],
                    Result = testCaseVM.result[i],
                    TestCaseId = testCase.TestCaseID
                };

                db.Steps.Add(step);
                db.SaveChanges();
            }

            return RedirectToAction("Index", "TestCasesPerTestSuite");
        }

        // GET: /TestCases/Edit/5

        [HttpGet]
        public ActionResult Edit(int id = 0)
        {
            var testCase = db.TestCases.Find(id);
            if (testCase == null) return HttpNotFound();

            var testCaseVM = new TestCaseVM
            {
                TestCaseID = id,
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
                Steps = testCase.Steps
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

            DeleteRemovedSteps(testCase, testCaseVM.stepsID);

            // For each id on stepsID >> id = 0: new step, id already on Steps: update step.
            for (int i = 0; i < testCaseVM.stepsID.Length; i++)
            {
                if (testCaseVM.stepsID[i] == "0")
                {
                    var step = new Step
                    {
                        Action = testCaseVM.action[i],
                        Result = testCaseVM.result[i],
                        TestCaseId = testCase.TestCaseID
                    };

                    db.Steps.Add(step);
                    db.SaveChanges();
                }
                else if (TestCaseContainsStep(testCase, Convert.ToInt32(testCaseVM.stepsID[i])))
                {
                    var step = db.Steps.Find(Convert.ToInt32(testCaseVM.stepsID[i]));
                    step.Action = testCaseVM.action[i];
                    step.Result = testCaseVM.result[i];
                    step.TestCaseId = testCase.TestCaseID;
                    db.Entry(step).State = EntityState.Modified;
                    db.SaveChanges();
                }
            }

            db.Entry(testCase).State = EntityState.Modified;
            db.SaveChanges();

            return RedirectToAction("Index", new { id = testCase.TestCaseID});
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

        private bool TestCaseContainsStep(TestCase tc, int id)
        {
            foreach (var step in tc.Steps)
            {
                if (step.StepId == id) return true;
            }
            return false;
        }

        private void DeleteRemovedSteps(TestCase tc, string[] stepsID)
        {
            List<int> stepToRemove = new List<int>();
            foreach (var step in tc.Steps)
            {
                if (!stepsID.Contains(step.StepId.ToString()))
                {
                    stepToRemove.Add(step.StepId);
                }
            }

            foreach (var item in stepToRemove)
            {
                Step step = db.Steps.Find(item);
                db.Steps.Remove(step);
                db.SaveChanges();
            }
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}