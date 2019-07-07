using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Reflection;
using System.Web;
using System.Web.Mvc;

namespace Chair80CP.Libs
{
    public class SettingFilter : FilterAttribute, IAuthorizationFilter
    {
        public string setting { get; set; }
        public SettingFilter(string _setting)
        {
            setting = _setting;
        }
        public void OnAuthorization(AuthorizationContext filterContext)
        {

            if (Libs.Settings.AppSetting.Where(a => a.setting_key == this.setting).FirstOrDefault().setting_value != "1")
            {
                if (filterContext.HttpContext.Request.IsAjaxRequest())
                {
                    filterContext.HttpContext.Response.Clear();
                    filterContext.HttpContext.Response.Write("<div class='alert alert-danger'>This option not applicable now!</div>");
                    filterContext.HttpContext.Response.End();
                    return;
                }
                filterContext.Result = new HttpUnauthorizedResult();
                filterContext.HttpContext.Response.Redirect("/Login/?next=" + filterContext.HttpContext.Request.RawUrl);
            }
        }

       
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
            filters.Add(new LoginFilter());
        }


    }

   
}
