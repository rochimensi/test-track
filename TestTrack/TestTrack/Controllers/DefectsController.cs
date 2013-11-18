using AutoMapper;
using System.Data;
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
        [HttpGet]
        public ActionResult Edit(int id = 0)
        {
            Defect defect = db.Defects.Find(id);
            if (defect == null) { return HttpNotFound(); }
            var defectVM = Mapper.Map<Defect, DefectVM>(defect);
            defectVM.Severities = Common.ToSelectList<TestTrack.Models.Severity>();

            return View(defectVM);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(DefectVM vm)
        {
            var defect = db.Defects.Find(vm.DefectID);
            if (defect == null) return HttpNotFound();
            db.Entry(defect).CurrentValues.SetValues(vm);
            var result = db.Results.Find(vm.ResultID);
            result.Comments = vm.Comments;
            db.Entry(result).State = EntityState.Modified;
            db.SaveChanges();

            return RedirectToAction("Index", "Results", new { id = defect.Result.TestRunID, tcId = defect.Result.TestCaseID });
        }
    }
}