using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Web;
using Chair80CP.Models;
using Chair80CP.Libs;
using FirebaseAdmin;
using Google.Apis.Auth.OAuth2;
using FirebaseAdmin.Auth;
using System.Threading.Tasks;

namespace Chair80CP
{

    public class UserSession
    {
        public static sec_sessions User {
            get
            {



                if (HttpContext.Current.Request.Cookies["X-CHAIR80-SESSIONID"] != null && HttpContext.Current.Session["SESSION_ID"]==null)
                {
                    HttpContext.Current.Session.Add("SESSION_ID", HttpContext.Current.Request.Cookies["X-CHAIR80-SESSIONID"].Value);
                }

                if (HttpContext.Current.Session["SESSION_ID"] == null) return null;

                Guid sessionid;
                Guid.TryParse(HttpContext.Current.Session["SESSION_ID"].ToString(),out sessionid);
                if (sessionid == null) return null;
                using (var db=new MainEntities())
                {
                    return db.sec_sessions.Include("sec_users").Include("sec_users.tbl_accounts").Where(a=>a.id== sessionid && a.end_time==null).FirstOrDefault();
                }
            }
        }

        public static bool Allow(string controller_name, string action_name)
        {
            if (User == null) return false;
            using (var db=new MainEntities())
            {

            
                List<int> myGroups = db.sec_users_roles.Where(a => a.user_id == User.user_id).Select(a => a.role_id).Distinct().ToList();

                List<sec_access_right> pers = db.sec_access_right.
                                                    Where(
                                                        a => a.method_name == action_name
                                                        && a.model_name == controller_name
                                                        && myGroups.Contains(a.role_id)
                                                        ).ToList();

                return pers.Count > 0 ? true : false;
            }
        }
        public static async Task<sec_sessions> Login(string uName,string pass,string  AccessToken, bool rem = false)
        {

            string uid = "";
            string email = "";

            if (AccessToken != null)
            {

            try {
                if (FirebaseApp.DefaultInstance != null)
                    FirebaseApp.DefaultInstance.Delete();
                
                    FirebaseApp.Create(new AppOptions()
                    {
                        Credential = GoogleCredential.FromFile(HttpContext.Current.Server.MapPath("~/App_Data/chari80-test-3ff0f2956b83.json")),

                    });
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message+" Line : 412");
            }
            FirebaseToken decodedToken;
            try
            {

                  decodedToken = await FirebaseAuth.DefaultInstance.VerifyIdTokenAsync(AccessToken);
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message + " Line : 424");
            }
                
            try
            {

                if(decodedToken.Claims.Keys.Contains("email")) email=decodedToken.Claims.FirstOrDefault(a => a.Key == "email").Value.ToString();

            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message + " Line : 445");
            }
           }
                 


            using (var ctx=new MainEntities())
            {
                tbl_accounts u;
                if (!string.IsNullOrEmpty(uid))
                {
                      u = ctx.tbl_accounts.Include("sec_users").Where(a => a.sec_users.firebase_uid==uid).FirstOrDefault();

                }
                else
                {
                      u = ctx.tbl_accounts.Include("sec_users").Where(a => a.email == uName && a.sec_users.pwd==pass).FirstOrDefault();

                }

                if (u == null) return null;
                var roles =u.sec_users.sec_users_roles.Select(a => a.sec_roles.role_key.ToLower()).ToList();
                if (!roles.Contains("owner") && !roles.Contains("admin")) return null;

                //u.sec_users.pwd = pass;

                //ctx.SaveChanges();
                var session = GetNewSession(u.sec_users,1);

                HttpContext.Current.Response.Cookies.Clear();

                if (session != null && rem)
                {
                    var cookie = new HttpCookie("X-CHAIR80-SESSIONID", session.id.ToString());
                    cookie.Expires=DateTime.Now.AddDays(5);
                   
                    HttpContext.Current.Response.Cookies.Add(cookie);
                }
                return session;
            }
        }
        public static bool Logout()
        {
            sec_sessions session = User;
            if (session == null)
                return true;

            session.end_time = DateTime.Now;
            using (var db=new MainEntities())
            {

                db.Entry(session).State = System.Data.Entity.EntityState.Modified;
                if (db.SaveChanges() == 0)
                {
                    return false;
                }
            }

            HttpContext.Current.Response.Cookies.Clear();
            HttpContext.Current.Session.Clear();


            return true;
        }

        public static sec_sessions GetNewSession(sec_users usr, int platform = 1)
        {
            using (var ctx = new MainEntities())
            {
                IPResult s = new IPResult();

                string ip = "";
                string agent = "";
                IPResult iploc = new IPResult();
                var request = HttpContext.Current.Request.ServerVariables;
                try
                {
                    ip = request.Get("REMOTE_ADDR");
                    agent = request.Get("HTTP_USER_AGENT");

                    iploc = General.GetResponse("http://ip-api.com/json/" + ip);
                }
                catch (Exception ex)
                {
                    // return APIResult<sec_sessions>.Error(ResponseCode.BackendServerRequest, ex.Message + "get location ip:" + ip + " agent:" + agent);
                }
                try
                {
                    var userSessions = ctx.sec_sessions.Where(a => a.user_id == usr.id && a.end_time == null && a.paltform == platform && a.end_time==null).FirstOrDefault();
                    if (userSessions != null)
                    {
                        HttpContext.Current.Session.Clear();
                        HttpContext.Current.Session.Add("SESSION_ID", userSessions.id);
                        return userSessions;
                    }

                    sec_sessions ses = new sec_sessions();
                    ses.user_id = usr.id;
                    ses.ip = request.Get("REMOTE_ADDR");
                    //IPiploc = new IPResult();// General.GetResponse("http://ip-api.com/json/" + ses.Entity.ip);

                    ses.isp = iploc.isp;
                    ses.lat = iploc.lat;
                    ses.lon = iploc.lon;
                    ses.timezone = iploc.timezone;
                    ses.city = iploc.city;
                    ses.country = iploc.country;
                    ses.country_code = iploc.countryCode;
                    ses.agent = request.Get("HTTP_USER_AGENT");
                    ses.paltform = platform;
                    ses.browser = General.getAgent(ses.agent).name;
                    ses.id = Guid.NewGuid();
                    ses.start_time = DateTime.Now;

                    ctx.sec_sessions.Add(ses);

                    ctx.SaveChanges();
                    HttpContext.Current.Session.Clear();
                    HttpContext.Current.Session.Add("SESSION_ID", ses.id);
                    return ses;
                }
                catch (Exception ex)
                {

                    return null;
                }
            }


            
        }

        public static bool HasRole(string role)
        {
            using (var ctx=new MainEntities())
            {
                return ctx.sec_users_roles.Where(a => a.sec_roles.role_key == role && a.user_id == User.user_id).Count() > 0;
                
            }
        }
          

    }
}