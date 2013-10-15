using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TestTrack.Helpers;

namespace TestTrack.Controllers
{
    public class BaseController : Controller
    {
        public SessionWrapper SessionWrapper { get; set; }

        public BaseController()
        {
            SessionWrapper = new SessionWrapper();
        }
    }
}