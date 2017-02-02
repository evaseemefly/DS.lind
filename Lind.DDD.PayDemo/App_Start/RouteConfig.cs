using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace Lind.DDD.PayDemo
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Order", action = "Index", id = UrlParameter.Optional }
            );

            routes.MapRoute(
              name: "weixin",
              url: "{controller}/{action}.aspx",
              defaults: new { controller = "Order", action = "Weixin", id = UrlParameter.Optional }
          );

        }
    }
}