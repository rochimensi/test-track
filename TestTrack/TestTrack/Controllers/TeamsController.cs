using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;
using TestTrack.Models;
using TestTrack.ViewModels;

namespace TestTrack.Controllers
{
    [Authorize]
    public class TeamsController : BaseController
    {
        private TestTrackDBContext db = new TestTrackDBContext();

        // GET: /Teams/
        public ActionResult Index()
        {
            var teams = db.Teams.Include(t => t.Project).Include(t => t.TestSuite);
            return View(teams.ToList());
        }


        [HttpGet]
        public ActionResult Create()
        {
            var TeamVM = new TeamVM
            {
                Projects = new SelectList(db.Projects.ToList(), "ProjectID", "Title")
            };

            return View("Create", TeamVM);
        }

        [HttpPost]
        public ActionResult Create(TeamVM teamVM)
        {
            var team = new Team
            {
                 TeamID = teamVM.TeamID,
                 Title = teamVM.Title,
                 ProjectID = teamVM.ProjectID,
                 Project = db.Projects.Find(teamVM.ProjectID)
            };

            db.Teams.Add(team);
            db.SaveChanges();

            return RedirectToAction("Index");
             
        }

        [HttpGet]
        public ActionResult Edit(int id = 0)
        {
            var team = db.Teams.Find(id);

            if (team == null) return HttpNotFound();

            var teamVm = new TeamVM
            {
                ProjectID = team.ProjectID,
                Title = team.Title,
                TeamID = team.TeamID,
                Projects = new SelectList(db.Projects.ToList(), "ProjectID", "Title", team.ProjectID)
            };

            return View(teamVm);
        }


        [HttpPost]
        public ActionResult Edit(TeamVM teamVM)
        {
            var team = db.Teams.Find(teamVM.TeamID);

            if (team == null) return HttpNotFound();

            team.Title = teamVM.Title;
            team.Project = db.Projects.Find(teamVM.ProjectID);
            team.ProjectID = teamVM.ProjectID;
            team.TeamID = teamVM.TeamID;

            db.Entry(team).State = EntityState.Modified;
            db.SaveChanges();

            return RedirectToAction("Index");
        }
        
        [HttpGet]
        public ActionResult Delete(int id = 0)
        {
            var team = db.Teams.Find(id);

            if (team == null) return HttpNotFound();
             
            return View(team);
        }

        
        [HttpPost, ValidateAntiForgeryToken, ActionName("Delete")]
        public ActionResult DeleteConfirm(int id)
        { 
            db.Teams.Remove( db.Teams.Find(id));
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