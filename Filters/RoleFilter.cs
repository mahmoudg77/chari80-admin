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
    public class RoleFilter : FilterAttribute, IAuthorizationFilter
    {
        public string[] roles { get; set; }

        public RoleFilter(string[] roles)
        {
            this.roles = roles;
        }
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

            bool hasOne = false;
            foreach (var role in roles)
            {
                if (UserSession.HasRole(role)) hasOne = true;
            }

            if (!hasOne)
            {
                if (filterContext.HttpContext.Request.IsAjaxRequest())
                {
                    filterContext.HttpContext.Response.Clear();
                    filterContext.HttpContext.Response.Write("<div class='alert alert-danger'>Sorry; You haven't rote to access this method !</div>");
                    filterContext.HttpContext.Response.End();
                    return;
                }
                filterContext.Result = new HttpUnauthorizedResult();
                filterContext.HttpContext.Response.Redirect("/Error/index/403");
            }

        }
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
            filters.Add(new LoginFilter());
        }
    }
   

}
