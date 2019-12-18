using Chair80CP.Requests;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Chair80CP.Controllers
{
    public class LoginController : Controller
    {

        Models.MainEntities db = new Models.MainEntities();
        // GET: Login
        public ActionResult Index(string next = "/Home")
        {
            ViewBag.next = next;

            // Since the dinosaur-facts repo no longer works, populate your own one with sample data in "sample.json"
            //var firebase = new FirebaseClient("https://dinosaur-facts.firebaseio.com/");
            //var dinos = await firebase
            //  .Child("dinosaurs")
            //  .OrderByKey()
            //  .StartAt("pterodactyl")
            //  .LimitToFirst(2)
            //  .OnceAsync<Dinosaur>();

            //foreach (var dino in dinos)
            //{
            //    Console.WriteLine($"{dino.Key} is {dino.Object.Height}m high.");
            //}
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Index(LoginRequest request,string next="/Home")
        {
            List<string> errors=new List<string>();
          
            if (!request.isValid().isSuccess)
            {
                errors.Add(request.isValid().message);
                return View("Index", errors);
            }
           
            var session = UserSession.Login(request.Username, request.Password,request.firebase_token, request.RememberMe);

            if (session.Result == null)
            {
                errors.Add("Invalid login data !");
                return View("Index", errors);
            }

          
           
            if (string.IsNullOrEmpty(next)) next = "/";

            
            return Redirect(next);
        }


        [HttpGet]
        public ActionResult Logout()
        {
           if(UserSession.Logout())
            {
                return RedirectToAction("Index");
            }

            return RedirectToRoute("/");
        }
        public ActionResult ForgetPassword()
        {
            return View();
        }

        [HttpPost]
        public ActionResult ForgetPassword(string email,bool api=false)
        {
            var b = Libs.General.getAgent(HttpContext.Request.ServerVariables.Get("HTTP_USER_AGENT"));


            List<string> errors=new List<string>();
            var acc = db.tbl_accounts.Where(a => a.email.ToLower() == email.ToLower()).FirstOrDefault();

            if (acc == null)
            {
                errors.Add("This email not exists !");
                return View(errors);
            }

            acc.sec_users.reset_pwd_token = Guid.NewGuid().ToString();

            db.Entry(acc).State = System.Data.Entity.EntityState.Modified;

            db.SaveChanges();

            System.Net.WebClient webClient = new System.Net.WebClient();

            webClient.Encoding = System.Text.Encoding.UTF8;
            
            var html = webClient.DownloadString(Libs.General.ApplicationUrl() + "/MailBody/RestPassword?id=" + acc.id + "&os=" + b.platform + "&browser="+ b.name );

            Libs.General.SendMail(email, "Reset password", html, "Chair80", "", "", "", true);
            
            return View();
        
        }
        public ActionResult ResetPassword(Guid key)
        {
            List<string> errors = new List<string>();
            if (key == null)
            {
                errors.Add("Invalid Reset Password Key !");
                ViewBag.errors = errors;

                return View();
            }

            var acc = db.sec_users.Where(a => a.reset_pwd_token == key.ToString()).FirstOrDefault();

            if (acc == null)
            {
                errors.Add("This key not found or was expired !");
                ViewBag.errors = errors;
                return View();
            }

            return View();
            //acc.reset_pwd_token = null;
            //db.Entry(acc).State = System.Data.Entity.EntityState.Modified;
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ResetPassword(ResetPasswordRequest request, Guid key)
        {
            ViewBag.errors = new List<string>();
            if (key == null)
            {
                ViewBag.errors.Add("Invalid Reset Password Key !");
                return View();
            }

            if (!request.isValid().isSuccess) {
                ViewBag.errors.Add(request.isValid().message);
                return View();
            }
            var acc = db.sec_users.Where(a => a.reset_pwd_token == key.ToString()).FirstOrDefault();

            if (acc == null)
            {
                ViewBag.errors.Add("This key not exists or was expired !");
                return View();
            }

            //if (acc.pwd != request.OldPassword)
            //{
            //    ViewBag.errors.Add("Incorrect Old Password !");
            //    return View();
            //}

            acc.pwd = request.NewPassword;

            db.Entry(acc).State = System.Data.Entity.EntityState.Modified;

            if(db.SaveChanges()>0)
                return RedirectToAction("Index");

            ViewBag.errors.Add("An expected error happen while change password, Please try again !");
            return View();
            //acc.reset_pwd_token = null;
            //db.Entry(acc).State = System.Data.Entity.EntityState.Modified;
        }
    }
}