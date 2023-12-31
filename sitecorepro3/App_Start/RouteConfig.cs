﻿using System.Web.Mvc;
using System.Web.Routing;

namespace sitecorepro3.Project.sitecorepro3
{
    public class RouteConfig
    {
        //Test
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
