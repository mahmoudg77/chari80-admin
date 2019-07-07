using Chair80CP.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Mvc;

namespace Chair80CP
{
    public class LoginFilter : FilterAttribute, IAuthorizationFilter
    {
        public void OnAuthorization(AuthorizationContext filterContext)
        {
            
 
            if (UserSession.User == null)
            {
                if (filterContext.HttpContext.Request.IsAjaxRequest())
                {
                    filterContext.HttpContext.Response.Clear();
                    filterContext.HttpContext.Response.Write("<div class='alert alert-danger'>You session has been expired, You must <a href='/login/?next="+ filterContext.HttpContext.Request.UrlReferrer + "'>login</a> again !</div>");
                    filterContext.HttpContext.Response.End();
                    return;
                }
                filterContext.Result = new HttpUnauthorizedResult();
                filterContext.HttpContext.Response.Redirect("/Login/?next=" + filterContext.HttpContext.Request.RawUrl );
            }
            
        }
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
            filters.Add(new LoginFilter());
        }
    }

    public class AuthFilter : FilterAttribute, IAuthorizationFilter
    {
        public void OnAuthorization(AuthorizationContext filterContext)
        {
 
            if (UserSession.User == null)
            {
                if (filterContext.HttpContext.Request.IsAjaxRequest())
                {
                    filterContext.HttpContext.Response.Clear();
                    filterContext.HttpContext.Response.Write("<div class='alert alert-danger'>You session has been expired, You must <a href='/login/?next=" + filterContext.HttpContext.Request.UrlReferrer + "'>login</a> again !</div>");
                    filterContext.HttpContext.Response.End();
                    return;
                }
                filterContext.Result = new HttpUnauthorizedResult();
                filterContext.HttpContext.Response.Redirect("/LoginData/?next=" + filterContext.HttpContext.Request.RawUrl );
                filterContext.HttpContext.Response.End();
                return;
            }




            string actionName = (string)filterContext.RouteData.Values["action"];
            if (filterContext.HttpContext.Request.HttpMethod == "POST")
            {
                actionName += "-post";
            }
            string controllername = (string)filterContext.RouteData.Values["controller"];

            if (!UserSession.Allow(controllername,actionName))
            {
                filterContext.Result = new HttpUnauthorizedResult();
                if (filterContext.HttpContext.Request.IsAjaxRequest())
                {
                    filterContext.HttpContext.Response.ContentType = "application/json";
                    filterContext.HttpContext.Response.Write("{\"type\":\"error\",\"message\":\"You are not authorized to do this action !!\"}");
                    //filterContext.HttpContext.Response.End();
                }
                else
                {
                     
                    filterContext.HttpContext.Response.Redirect("/NotAuthorized");


                }

                return;
            }
        }
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
            filters.Add(new AuthFilter());
        }
    }

    public class JsonResult
    {
        public string type;
        public string message;
        public dynamic data;
    }

    public class Functions
    {
        public static List<string> NameSpaceClasses()
        {

            List<string> items = new List<string>();
            Assembly asm = Assembly.GetExecutingAssembly();
            var lst = asm.GetTypes()
                 .Where(type => typeof(Controller).IsAssignableFrom(type)) ;

            foreach (Type item in lst)
            {
                items.Add( item.Name.Replace("Controller",""));
            }

            return items;

        }
        public static List<string> ClassMethods(string ControllerName)
        {
            Assembly asm = Assembly.GetExecutingAssembly();
            List<string> items = new List<string>();

            var lst = asm.GetTypes()
                   .Where(type => typeof(Controller).IsAssignableFrom(type) && type.Name == ControllerName) //filter controllers
                   .SelectMany(type => type.GetMethods())
                   .Where(method => method.IsPublic && !method.IsDefined(typeof(NonActionAttribute)) && (method.ReturnType.Name ==  "ActionResult" || method.ReturnType.Name ==  typeof(System.Threading.Tasks.Task<ActionResult>).Name) );//
            foreach (MethodInfo item in lst)
            {
                string HttpMethod = "";
                if(item.GetCustomAttributes(typeof(HttpPostAttribute), false).Length > 0)
                {
                    HttpMethod = "-post";
                }

                items.Add(item.Name.Replace("Confirmed","") + HttpMethod);
            }

            return items;
        }

        
    }
}
