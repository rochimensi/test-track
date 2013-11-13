using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace TestTrack
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );

            routes.MapRoute(
                name: "ResultsCreate",
                url: "Results/Create/{id}/{state}",
                defaults: new { controller = "Results", action = "Create", id = UrlParameter.Optional, state = UrlParameter.Optional }
            );

            routes.MapRoute(
                name: "TestCaseOnTestRun",
                url: "Results/Index/{id}/{tcId}",
                defaults: new { controller = "Results", action = "Index", id = UrlParameter.Optional, tcId = UrlParameter.Optional }
            );
        }
    }
}