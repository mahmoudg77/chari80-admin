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
    [RoleFilter(new string[] { "admin","owner" })]

    public class TripSharedController : Controller
    {
        private MainEntities db = new MainEntities();

        // GET: SharedTrips
        public ActionResult Index(int? type = 0,DateTime? from=null, DateTime? to=null,int? driver=0)
        {
            var trip_share = db.trip_share.Include(t => t.tbl_accounts).Include(t => t.tbl_vehicles).Include(t => t.trip_types).AsQueryable();
            if (!UserSession.HasRole("admin"))
            {
                var car_ids = db.tbl_drivers_vehicles_rel.Where(a => a.tbl_vehicles.owner_id == UserSession.User.user_id).Select(a => a.vehicle_id).ToList();
                trip_share = trip_share.Where(a=> car_ids.Contains(a.vehicle_id));
            }
            if (type > 0)
            {
                trip_share = trip_share.Where(a => a.trip_type_id== type);
            }
            if (driver > 0)
            {
                trip_share = trip_share.Where(a => a.driver_id == driver);
            }

            if (from !=null)
            {
                trip_share = trip_share.Where(a => a.start_at_date>=from);
            }

            if (to!=null)
            {
                trip_share = trip_share.Where(a => a.end_at_date<=to);
            }

           

            return View(trip_share.ToList());
        }

       
        
        // GET: SharedTrips/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            trip_share trip_share = db.trip_share.Find(id);
            if (trip_share == null)
            {
                return HttpNotFound();
            }
            return View(trip_share);
        }

       
        // GET: SharedTrips/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            trip_share trip_share = db.trip_share.Find(id);
            if (trip_share == null)
            {
                return HttpNotFound();
            }
            return View(trip_share);
        }

        // POST: SharedTrips/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            trip_share trip_share = db.trip_share.Find(id);
            db.trip_share.Remove(trip_share);
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
    }
}
