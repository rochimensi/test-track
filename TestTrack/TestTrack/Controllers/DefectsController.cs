using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TestTrack.Filters;
using TestTrack.Helpers;
using TestTrack.Models;
using TestTrack.ViewModels;

namespace TestTrack.Controllers
{
    [Authorize]
    [ProjectsAvailability]
    public class DefectsController : BaseController
    {
        // GET: /Defects/Edit/5
        [HttpGet]
        public ActionResult Edit(int id = 0)
        {
            Defect defect = db.Defects.Find(id);
            if (defect == null)
            {
                return HttpNotFound();
            }
            DefectVM vm = new DefectVM
            {
                DefectID = defect.DefectID,
                DefectTitle = defect.Title,
                TestCaseID = defect.Result.TestCaseID,
                TestRunID = defect.Result.TestRunID,
                ResultID = defect.ResultID,
                Comments = defect.Description,
                Severity = defect.Severity,
                Labels = defect.Labels,
                Severities = Common.ToSelectList<TestTrack.Models.Severity>()                
            };
            return View(vm);
        }
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(DefectVM vm)
        {
            var defect = db.Defects.Find(vm.DefectID);
            if (defect == null) return HttpNotFound();

            var result = db.Results.Find(vm.ResultID);

            defect.Title = vm.DefectTitle;
            defect.Description = vm.Comments;
            result.Comments = vm.Comments;
            defect.Labels = vm.Labels;
            defect.Severity = vm.Severity;

            db.Entry(defect).State = EntityState.Modified;
            db.Entry(result).State = EntityState.Modified;
            db.SaveChanges();

            return RedirectToAction("Index", "Results", new { id = vm.TestRunID, tcId = vm.TestCaseID });
        }
    }
}