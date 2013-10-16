﻿using System.Linq;
using System.Web.Mvc;
using TestTrack.Models;

namespace TestTrack.Controllers
{
    [Authorize]
    public class TestPlanPerIterationController : BaseController
    {
        private TestTrackDBContext db = new TestTrackDBContext();

        // GET: /TestPlanPerIteration/

        public ActionResult Index(int id = 0)
        {
            var iteration = new Iteration();

            if (id == 0)
            {
                if (db.Iterations.Count() > 0)
                {
                    iteration = (from i in db.Iterations
                                 orderby i.DueDate
                                 select i).ToList().First();
                }
            }

            if (id > 0)
            {
                iteration = (from i in db.Iterations
                             where i.IterationID == id
                             orderby i.DueDate
                             select i).ToList().First();
            }
            return View(iteration);
        }
    }
}
