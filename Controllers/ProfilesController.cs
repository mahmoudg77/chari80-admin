using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Chair80CP.Models;

namespace Chair80CP.Controllers
{
    [LoginFilter]
    [RoutePrefix("Profiles")]
    public class ProfilesController : Controller
    {
        private MainEntities db = new MainEntities();


        // GET: Profiles
        [RoleFilter(new string[] { "admin" })]
        public ActionResult Index()
        {
            return View(db.vwProfile.ToList());
        }

        public ActionResult about(int? id=0)
        {
            return View(db.vwProfile.FirstOrDefault(a => a.id == id));
        }

        public ActionResult docs(int? id=0)
        {
            return View(db.vwProfile.FirstOrDefault(a => a.id == id));
        }

        [RoleFilter(new string[] { "admin" })]
        public ActionResult ByRoles(int[] roles)
        {
            var acc = db.vwProfile.AsQueryable();
            
            if(roles.Contains(3))
                acc = acc.Where(a => a.is_driver==1);

            if (roles.Contains(2))
                acc = acc.Where(a => a.is_owner == 1);

            return View("~/Views/Profiles/Index.cshtml", acc.ToList());
        }

        [RoleFilter(new string[] { "admin" })]
        [Route("GetField/{ID}/{fieldname}")]
        public ActionResult GetField(int ID,string fieldname="")
        {
            ViewBag.Profile = db.vwProfile.FirstOrDefault(a => a.id == ID);

            switch (fieldname)
            {
                case "Vehicles":
                    var tbl_vehicles = db.vwVehicles.Where(a => a.owner_id == ID);
                    ViewBag.query = Request.Url.Query+"&owner="+ID;

                    return View("~/Views/Vehicles/Index.cshtml", tbl_vehicles.ToList());
                case "Drive-Vehicles":
                    var list = db.tbl_drivers_vehicles_rel.Include(s => s.tbl_vehicles).Include(t => t.tbl_accounts).Where(a => a.driver_id == ID);
                    ViewBag.AllowAdd = true;
                    ViewBag.Driver = ID;
                    ViewBag.query = Request.Url.Query + "&driver=" + ID;
                    return View("~/Views/Vehicles/DriverVehicles.cshtml", list);
                default:
                    return RedirectToAction("Details", "Profiles", new { id = ID });
            }
        }
        [RoleFilter(new string[] { "owner" })]
        [Route("GetField/My/Vehicles")]
        public ActionResult MyVehicles()
        {
            var tbl_vehicles = db.vwVehicles.Where(a => a.owner_id == UserSession.User.user_id);
            return View("~/Views/Vehicles/Index.cshtml", tbl_vehicles.ToList());
        }
        [RoleFilter(new string[] { "admin" })]
        [Route("{ID}/Vehicles")]
        public ActionResult ByOwner(int ID)
        {
            var tbl_vehicles = db.vwVehicles.Where(a => a.owner_id == ID);
            return View("~/Views/Vehicles/Index.cshtml", tbl_vehicles.ToList());
        }
        [RoleFilter(new string[] { "admin" })]
        [Route("{ID}/Driver-Vehicles")]
        public ActionResult ByDriver(int ID)
        {
            var list = db.tbl_drivers_vehicles_rel.Include(s=>s.tbl_vehicles).Include(t => t.tbl_accounts).Where(a => a.driver_id==ID);
            return View("~/Views/Vehicles/DriverVehicles.cshtml", list.ToList());
        }

        [RoleFilter(new string[] { "owner" })]
        public ActionResult Search(string mobile)
        {
            List<vwProfile> profiles = db.vwProfile.Where(a => a.mobile == mobile && a.is_driver == 1 && a.is_owner == 0 ).ToList();
            return View(profiles);
        }
        // GET: Profiles/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            vwProfile vwProfile = db.vwProfile.FirstOrDefault(a=>a.id==id);
            if (vwProfile == null)
            {
                return HttpNotFound();
            }
            return View(vwProfile);
        }

        // GET: Profiles/Create
        //public ActionResult Create()
        //{
        //    return View();
        //}

       

        // GET: Profiles/Delete/5
        //public ActionResult Delete(int? id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    vwProfile vwProfile = db.vwProfile.Find(id);
        //    if (vwProfile == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(vwProfile);
        //}

        // POST: Profiles/Delete/5
        //[HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        //public ActionResult DeleteConfirmed(int id)
        //{
        //    vwProfile vwProfile = db.vwProfile.Find(id);
        //    db.vwProfile.Remove(vwProfile);
        //    db.SaveChanges();
        //    return RedirectToAction("Index");
        //}

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        [RoleFilter(new string[] { "admin" })]
        [HttpGet]
        public ActionResult EditRoles(int id)
        {
            ViewBag.roles = db.sec_roles.ToList();
            ViewBag.selected_roles = db.sec_users_roles.Where(a => a.user_id == id).Select(a => a.role_id).ToList();

            var acc = db.vwProfile.FirstOrDefault(a => a.id == id);
            return View(acc);
        }

        [RoleFilter(new string[] { "admin" })]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditRoles(FormCollection request)
        {
            int id = int.Parse(request.Get("id"));

            
            var oldRoles = db.sec_users_roles.Where(a => a.user_id ==id );

            foreach(var role in oldRoles)
            {
                db.Entry(role).State = EntityState.Deleted;
              
            }
            db.SaveChanges();
            foreach(var roleid in request.Get("roles[]").Split(','))
            {
                int role_id = int.Parse(roleid.ToString());
                db.sec_users_roles.Add(new sec_users_roles() { role_id = role_id, user_id = id });
            }

            db.SaveChanges();
            if (Request.IsAjaxRequest())
            {
                return Json(new { type = "success", mesaage = "Save success" });
            }
            else
            {
                return RedirectToAction("Index");
            }
        }
    }
}
