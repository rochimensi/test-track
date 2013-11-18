using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web.Mvc;
using TestTrack.Models;
using TestTrack.ViewModels;
using TestTrack.Helpers;
using TestTrack.Filters;
using AutoMapper;

namespace TestTrack.Controllers
{
    [Authorize]
    [ProjectsAvailability]
    public class TestCasesController : BaseController
    {
        public ActionResult Index(int id = 0)
        {
            var testCase = db.TestCases.Find(id);
            if (testCase == null) return HttpNotFound();
            var testCaseVM = Mapper.Map<TestCase, TestCaseVM>(testCase);
            testCaseVM.labels = new string[testCase.Steps.Count()];
            
            for (var i = 0; i < testCase.Steps.Count(); i++)
            {
                testCaseVM.labels[i] = (i + 1).ToString();
            }

            return View(testCaseVM);
        }

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

        [HttpPost]
        public ActionResult Create(TestCaseVM testCaseVM)
        {
            var testCase = Mapper.Map<TestCaseVM, TestCase>(testCaseVM);
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

        [HttpGet]
        public ActionResult Edit(int id = 0)
        {
            var testCase = db.TestCases.Find(id);
            if (testCase == null) return HttpNotFound();
            var testCaseVM = Mapper.Map<TestCase, TestCaseVM>(testCase);
            testCaseVM.labels = new string[testCase.Steps.Count()];
            testCaseVM.Types = Common.ToSelectList<TestTrack.Models.Type>();
            testCaseVM.Priorities = Common.ToSelectList<TestTrack.Models.Priority>();
            testCaseVM.Methods = Common.ToSelectList<TestTrack.Models.Method>();
            
            for (var i = 0; i < testCase.Steps.Count(); i++)
            {
                testCaseVM.labels[i] = (i + 1).ToString();
            }

            return View(testCaseVM);
        }

        [HttpPost]
        public ActionResult Edit(TestCaseVM testCaseVM)
        {
            var testCase = db.TestCases.Find(testCaseVM.TestCaseID);
            if (testCase == null) return HttpNotFound();
            db.Entry(testCase).CurrentValues.SetValues(testCaseVM);
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

            return RedirectToAction("Index", new { id = testCase.TestCaseID });
        }

        public ActionResult Delete(int id = 0)
        {
            TestCase testcase = db.TestCases.Find(id);
            if (testcase == null) { return HttpNotFound(); }
            return PartialView(testcase);
        }

        [HttpPost, ValidateAntiForgeryToken, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            TestCase testcase = db.TestCases.Find(id);
            db.TestCases.Remove(testcase);
            db.SaveChanges();
            return RedirectToAction("Index", "TestCasesPerTestSuite");
        }

        [ChildActionOnly]
        public ActionResult List(int id = 0)
        {
            var testCases = (from tc in db.TestCases
                             where tc.TestSuiteID == id
                             select tc).ToList();
            var testCasesVM = Mapper.Map<IList<TestCase>, IList<TestCaseVM>>(testCases);

            return PartialView("_List", testCasesVM);
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
    }
}