using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Chair80CP.BLL;
using Chair80CP.Models;

namespace Chair80CP.Controllers
{
    [LoginFilter]
    [RoutePrefix("Vehicles")]
    public class VehiclesController : Controller
    {
        private MainEntities db = new MainEntities();
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [RoleFilter(new string[] { "admin" })]
        // GET: Vehicles
        public ActionResult Index()
        {
            var tbl_vehicles = db.vwVehicles.Where(a=>a.is_delete!=true);
            return View(tbl_vehicles.ToList());
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [RoleFilter(new string[] { "owner" })]
        public ActionResult MyOwn()
        {
            var tbl_vehicles = db.vwVehicles.Where(a=>a.owner_id== UserSession.User.user_id && a.is_delete != true);
            ViewBag.AllowAdd = true;
            return View("Index",tbl_vehicles.ToList());
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [RoleFilter(new string[] { "driver" })]
        public ActionResult MyDrive()
        {
            var ids = db.tbl_drivers_vehicles_rel.Where(b => b.status == 1 && b.driver_id== UserSession.User.user_id).Select(i => i.vehicle_id).ToList();
            var tbl_vehicles = db.vwVehicles.Where(a => ids.Contains(a.id) && a.is_delete != true);
            ViewBag.AllowAdd = false;
           // var tbl_vehicles = db.tbl_vehicles.Include(t => t.tbl_accounts).Include(c=>c.tbl_drivers_vehicles_rel).Where(a => a.owner_id == UserSession.User.user_id);
            return View("Index", tbl_vehicles.ToList());
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        // GET: Vehicles/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            vwVehicles tbl_vehicles = db.vwVehicles.FirstOrDefault(a=>a.id==id);
            if (tbl_vehicles == null)
            {
                return HttpNotFound();
            }
            return View(tbl_vehicles);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tbl_vehicles tbl_vehicles = db.tbl_vehicles.Find(id);
            if (tbl_vehicles == null)
            {
                return HttpNotFound();
            }
            ViewBag.Images = db.tbl_images.Where(a => a.model_name == "tbl_vehicles" && a.model_id == id).ToList();
            
            return View(tbl_vehicles);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [RoleFilter(new string[] { "owner" })]
        public ActionResult AssginVehcileToDriver()
        {
            ViewBag.Vehciles = db.tbl_vehicles.Where(a=>a.is_delete!=true && a.owner_id==UserSession.User.user_id).ToList();
            ViewBag.Driver = Request.QueryString.Get("driver");
            ViewBag.Vehicle = Request.QueryString.Get("vehicle");
            return View();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="form"></param>
        /// <returns></returns>
        [RoleFilter(new string[] { "owner" })]
        [HttpPost]
        public ActionResult AssginVehcileToDriver(FormCollection form)
        {
            var vehcile_id = int.Parse(form.Get("vehicle_id")==null?"0": form.Get("vehicle_id"));
            var driver_id = int.Parse(form.Get("account_id")==null?"0": form.Get("account_id"));

            tbl_drivers_vehicles_rel rel = new tbl_drivers_vehicles_rel()
            {
                vehicle_id = vehcile_id,
                driver_id = driver_id,
                created_at = DateTime.Now,
                created_by = UserSession.User.user_id,
                status = 0,
               
            };

            db.tbl_drivers_vehicles_rel.Add(rel);

            if (db.SaveChanges() <= 0)
            {

                return Json(new { type = "error", message = "" });
            }
            tbl_accounts driver = db.tbl_accounts.Include(t => t.sec_users).FirstOrDefault(a => a.id == driver_id);

            return Json(new { type = "success", message = "Your request sent to " + driver.first_name +" " + driver.last_name +", Please wait for his action" });
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        // GET: Vehicles/Create
        [RoleFilter(new string[] { "owner" })]
        public ActionResult Create()
        {
            if (!string.IsNullOrEmpty(Request.QueryString.Get("owner")) && UserSession.HasRole("admin"))
            {
                ViewBag.owner_id = Request.QueryString.Get("owner");
            }
            else
            {
                ViewBag.owner_id = UserSession.User.user_id;
            }
            return View();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="tbl_vehicles"></param>
        /// <returns></returns>
        // POST: Vehicles/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [RoleFilter(new string[] { "owner" })]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "model,color,capacity,owner_id,license_no")] tbl_vehicles tbl_vehicles)
        {
            if (tbl_vehicles.owner_id == null) tbl_vehicles.owner_id = UserSession.User.user_id;

            if (ModelState.IsValid)
            {
                tbl_vehicles.created_at = DateTime.Now;
                tbl_vehicles.created_by = UserSession.User.user_id;
               
                db.tbl_vehicles.Add(tbl_vehicles);
                db.SaveChanges();

                var httpRequest = Request;
                if (httpRequest.Files.Count > 0)
                    if (!Images.SaveImagesFromRequest(httpRequest, "en", "tbl_vehicles", tbl_vehicles.id, "main"))
                    {
                        //return Json(new { type = "success", message = "Upload images success" });
                        return Json(new { type = "error", message = "Error while upload images" });
                    }


                if (Request.IsAjaxRequest())
                {
                    return Json(new { type = "success", message = "" });
                }


               
                return RedirectToAction("Create");
            }
            List<string> errors = new List<string>();
            foreach(var i in ModelState.Values)
            {
                if (i.Errors.Count > 0) errors.AddRange(i.Errors.Select(a => a.ErrorMessage));
            }
            if (Request.IsAjaxRequest())
            {
                return Json(new {type="error",message=errors[0],data=errors});
            }
            return View(tbl_vehicles);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        // GET: Vehicles/Edit/5
        [RoleFilter(new string[] { "admin" })]
        public ActionResult owner(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tbl_vehicles tbl_vehicles = db.tbl_vehicles.Find(id);
            if (tbl_vehicles == null)
            {
                return HttpNotFound();
            }
            ViewBag.owner_id = new SelectList(db.tbl_accounts, "id", "first_name", tbl_vehicles.owner_id);
            return View(tbl_vehicles);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="tbl_vehicles"></param>
        /// <returns></returns>
        // POST: Vehicles/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [RoleFilter(new string[] { "owner" })]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id,model,color,capacity,owner_id,license_no")] tbl_vehicles tbl_vehicles)
        {
            if (ModelState.IsValid)
            {
                var old = db.tbl_vehicles.Find(tbl_vehicles.id);
                old.model = tbl_vehicles.model;
                old.color = tbl_vehicles.color;
                old.license_no = tbl_vehicles.license_no;
                old.owner_id = tbl_vehicles.owner_id;
                old.capacity = tbl_vehicles.capacity;
                db.Entry(old).State = EntityState.Modified;
                db.SaveChanges();

                var httpRequest = Request;
                if(httpRequest.Files.Count>0)
                if (!Images.SaveImagesFromRequest(httpRequest, "en", "tbl_vehicles", old.id, "main"))
                {
                    //return Json(new { type = "success", message = "Upload images success" });
                    return Json(new { type = "error", message = "Error while upload images" });
                }


                if (Request.IsAjaxRequest())
                {
                    return Json(new { type = "success", message = "" });
                }
                return RedirectToAction("Edit",tbl_vehicles.id);
            }
            List<string> errors = new List<string>();

            foreach (var i in ModelState.Values)
            {
                if (i.Errors.Count > 0) errors.AddRange(i.Errors.Select(a => a.ErrorMessage));
            }
            if (Request.IsAjaxRequest())
            {
                return Json(new { type = "error", message = errors[0], data = errors });
            }

            return View(tbl_vehicles);
        }

        // GET: Vehicles/Delete/5
        [RoleFilter(new string[] { "owner" })]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tbl_vehicles tbl_vehicles = db.tbl_vehicles.Find(id);
            if (tbl_vehicles == null)
            {
                return HttpNotFound();
            }
            return View(tbl_vehicles);
        }

        // POST: Vehicles/Delete/5
        [RoleFilter(new string[] { "owner" })]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            tbl_vehicles tbl_vehicles = db.tbl_vehicles.Find(id);
            db.tbl_vehicles.Remove(tbl_vehicles);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        [RoleFilter(new string[] { "admin" })]
        [Route("GetField/{ID}/{fieldname}")]
        public ActionResult GetField(int ID, string fieldname = "")
        {
            ViewBag.Profile = db.vwProfile.FirstOrDefault(a => a.id == ID);

            switch (fieldname)
            {

                case "Vehicles-Drive":
                    var list = db.tbl_drivers_vehicles_rel.Include(s => s.tbl_vehicles).Include(t => t.tbl_accounts).Where(a => a.vehicle_id == ID);
                    ViewBag.AllowAdd = true;
                    ViewBag.Vehicle = ID;
                    ViewBag.query = Request.Url.Query + "&vehicle=" + ID;
                    return View("~/Views/Vehicles/DriverVehicles.cshtml", list);
                default:
                    return RedirectToAction("Details", "Vehicles", new { id = ID });
            }
        }

        [RoleFilter(new string[] { "owner" })]
        public ActionResult Search(string number)
        {
            List<tbl_vehicles> vehicles = db.tbl_vehicles.Where(a => a.license_no ==number && a.is_delete !=true).ToList();
            return View(vehicles);
        }
    }
}
