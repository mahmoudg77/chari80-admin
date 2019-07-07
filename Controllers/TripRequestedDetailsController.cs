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

    public class TripRequestedDetailsController : Controller
    {
        private MainEntities db = new MainEntities();

        // GET: TripRequestedDetails
        public ActionResult Index(int? id,int? type = 0, DateTime? from = null, DateTime? to = null)
        {
            if (!UserSession.HasRole("admin"))
            {
                ViewBag.Error = 403;
                return View("~/Views/Error/Index.cshtml");
            }
            var trip_request_details = db.trip_request_details.Include(t => t.trip_request);
            if (id > 0)
            {
                trip_request_details = trip_request_details.Where(a => a.trip_request_id == id);
            }
          
            if (type > 0)
            {
                trip_request_details = trip_request_details.Where(a => a.trip_request.trip_type_id == type);
            }
          
            if (from != null)
            {
                trip_request_details = trip_request_details.Where(a => a.start_at_date >= from);
            }
            if (to != null)
            {
                trip_request_details = trip_request_details.Where(a => a.start_at_date <= to);
            }
            return View(trip_request_details.ToList());
        }

        // GET: TripRequestedDetails/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            trip_request_details trip_request_details = db.trip_request_details.Find(id);
            if (trip_request_details == null)
            {
                return HttpNotFound();
            }
            return View(trip_request_details);
        }

      
        // GET: TripRequestedDetails/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            trip_request_details trip_request_details = db.trip_request_details.Find(id);
            if (trip_request_details == null)
            {
                return HttpNotFound();
            }
            return View(trip_request_details);
        }

        // POST: TripRequestedDetails/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            trip_request_details trip_request_details = db.trip_request_details.Find(id);
            db.trip_request_details.Remove(trip_request_details);
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
