using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TestTrack.Filters;
using TestTrack.Models;

namespace TestTrack.Controllers
{
    [Authorize]
    [ProjectsAvailability]
    public class HomeController : BaseController
    {
        private TestTrackDBContext db = new TestTrackDBContext();

        public ActionResult Index()
        {
            return View(db.Projects.Count());
        }

        public ActionResult Help()
        {
            return View(db.Projects.Count());
        }

        public ActionResult About()
        {
            return View(db.Projects.Count());
        }

        public ActionResult Contact()
        {
            return View(db.Projects.Count());
        }
    }
}
