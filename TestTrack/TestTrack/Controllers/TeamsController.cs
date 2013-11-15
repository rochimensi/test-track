using AutoMapper;
using System.Collections.Generic;
using System.Web.Mvc;
using TestTrack.Filters;
using TestTrack.Models;
using TestTrack.ViewModels;

namespace TestTrack.Controllers
{
    [Authorize]
    [ProjectsAvailability]
    public class TeamsController : BaseController
    {
        // GET: /Teams/
        public ActionResult Index()
        {
            UserSettings userSettings = SessionWrapper.UserSettings;
            var teamsVM = Mapper.Map<IList<Team>, IList<TeamVM>>(GetTeamsInProject());

            return View(teamsVM);
        }
        
        [HttpGet]
        public ActionResult Create()
        {
            UserSettings userSettings = SessionWrapper.UserSettings;
            var TeamVM = new TeamVM { ProjectID = userSettings.workingProject };

            return View("Create", TeamVM);
        }

        [HttpPost]
        public ActionResult Create(TeamVM teamVM)
        {
            var team = Mapper.Map<TeamVM, Team>(teamVM);
            db.Teams.Add(team);
            db.SaveChanges();

            return RedirectToAction("Index");

        }

        [HttpGet]
        public ActionResult Edit(int id = 0)
        {
            var team = db.Teams.Find(id);
            if (team == null) return HttpNotFound();
            var teamsVM = Mapper.Map<Team, TeamVM>(team);

            return View(teamsVM);
        }

        [HttpPost]
        public ActionResult Edit(TeamVM teamVM)
        {
            var team = db.Teams.Find(teamVM.TeamID);
            if (team == null) return HttpNotFound();
            db.Entry(team).CurrentValues.SetValues(teamVM);
            db.SaveChanges();

            return RedirectToAction("Index");
        }

        [HttpGet]
        public ActionResult Delete(int id = 0)
        {
            var team = db.Teams.Find(id);
            if (team == null) return HttpNotFound();

            return PartialView(team);
        }

        [HttpPost, ValidateAntiForgeryToken, ActionName("Delete")]
        public ActionResult DeleteConfirm(int id)
        {
            db.Teams.Remove(db.Teams.Find(id));
            db.SaveChanges();

            return RedirectToAction("Index");
        }
    }
}