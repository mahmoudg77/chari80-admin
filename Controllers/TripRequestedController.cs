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
    [RoleFilter(new string[] { "admin" })]

    public class TripRequestedController : Controller
    {
        private MainEntities db = new MainEntities();

        // GET: TripRequested
        public ActionResult Index(int? type = 0, DateTime? from = null, DateTime? to = null,int? rider=0)
        {
            if (!UserSession.HasRole("admin"))
            {
                ViewBag.Error = 403;
                return View("~/Views/Error/Index.cshtml");
            }

            var trip_request = db.trip_request.Include(t => t.trip_types).Include(t=>t.tbl_accounts);
            if (type > 0)
            {
                trip_request = trip_request.Where(a => a.trip_type_id == type);
            }
            if (rider > 0)
            {
                trip_request = trip_request.Where(a => a.rider_id == rider);
            }

            if (from != null)
            {
                trip_request = trip_request.Where(a => a.start_at_date >= from);
            }

            if (to != null)
            {
                trip_request = trip_request.Where(a => a.end_at_date <= to);
            }

            return View(trip_request.ToList());
        }

        // GET: TripRequested/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            trip_request trip_request = db.trip_request.Find(id);
            if (trip_request == null)
            {
                return HttpNotFound();
            }
            return View(trip_request);
        }

       
        // GET: TripRequested/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            trip_request trip_request = db.trip_request.Find(id);
            if (trip_request == null)
            {
                return HttpNotFound();
            }
            return View(trip_request);
        }

        // POST: TripRequested/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            trip_request trip_request = db.trip_request.Find(id);
            db.trip_request.Remove(trip_request);
            db.SaveChanges();
            if (Request.IsAjaxRequest())
            {
                return Json(new { type = "success", message="Deleted Success" });
            }
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
