using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace MyWorkshop
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                name: "Thoughts/Archive",
                url: "Thoughts/Archive/{year}/{month}/{page}",
                defaults: new { controller = "Thoughts", action = "Archive", page = UrlParameter.Optional },
                    namespaces: new[] { "MyWorkshop.Controllers" }
            );

            routes.MapRoute(
                name: "Thoughts/View",
                url: "Thoughts/View/{id}/{urlSlug}",
                defaults: new { controller = "Thoughts", action = "View", id = UrlParameter.Optional, urlSlug = UrlParameter.Optional },
                 namespaces: new[] { "MyWorkshop.Controllers" }
            );

            routes.MapRoute(
                name: "Thoughts/Edit",
                url: "Thoughts/Edit/{id}",
                defaults: new {controller="Thoughts", action = "Edit" },
                namespaces: new[] { "MyWorkshop.Controllers" }
            );

            routes.MapRoute(
                name: "Thoughts",
                url: "Thoughts/{action}/{parameter}/{page}",
                defaults: new { controller = "Thoughts", action = "List", parameter = UrlParameter.Optional, page = UrlParameter.Optional },
                namespaces: new[] { "MyWorkshop.Controllers" }
            );



            routes.MapRoute(
                name: "Photos/Archive",
                url: "Photos/Archive/{year}/{month}",
                defaults: new { controller = "Photos", action = "Archive" },
                namespaces: new[] { "MyWorkshop.Controllers" }
            );

            routes.MapRoute(
                name: "Photos",
                url: "Photos/Album/{urlSlug}",
                defaults: new { controller = "Photos", action = "Album", urlSlug = UrlParameter.Optional },
                namespaces: new[] { "MyWorkshop.Controllers" }
            );

            routes.MapRoute(
                name: "Photos/EditAlbum",
                url: "Photos/EditAlbum/{id}",
                defaults: new { controller = "Photos", action = "EditAlbum" },
                namespaces: new[] { "MyWorkshop.Controllers" }
            );

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional },
                namespaces: new[] { "MyWorkshop.Controllers" }
            );
        }
    }
}

