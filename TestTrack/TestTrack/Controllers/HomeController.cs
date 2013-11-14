using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TestTrack.Models;

namespace TestTrack.Controllers
{
    [Authorize]
    public class HomeController : BaseController
    {
        private TestTrackDBContext db = new TestTrackDBContext();

        public ActionResult Index()
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
