using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace Chair80CP
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            //routes.MapRoute(
            //    name: "ProjectData",
            //    url: "Trip/{id}/field/{fieldname}",
            //    defaults: new {controller= "Projects",action = "GetField", id = 0, fieldname="Info" }
            //    //constraints:new {HttpContext.Current.Session["USER_ID"].ToString() = ""}
            //);
            routes.MapRoute(
               name: "ProfileData",
               url: "Profiles/GetField/{id}/{fieldname}",
               defaults: new { controller = "Profiles", action = "GetField", id = 0, fieldname = UrlParameter.Optional }
                //constraints:new {HttpContext.Current.Session["USER_ID"].ToString() = ""}
           );
            routes.MapRoute(
               name: "VehicleData",
               url: "Vehicles/GetField/{id}/{fieldname}",
               defaults: new { controller = "Vehicles", action = "GetField", id = 0, fieldname = UrlParameter.Optional }
                //constraints:new {HttpContext.Current.Session["USER_ID"].ToString() = ""}
           );
            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );

            Libs.Settings.Load();

        }
    }
}
