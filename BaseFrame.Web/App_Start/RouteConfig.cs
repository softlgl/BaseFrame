using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace BaseFrame.Web
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            //404页面配置
            routes.MapRoute(
                name: "Error404",
                url: "404",
                defaults: new { controller = "Error", action = "Page404", }
            );
            //500页面配置
            routes.MapRoute(
                name: "Error500",
                url: "500",
                defaults: new { controller = "Error", action = "Page500", }
            );

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
