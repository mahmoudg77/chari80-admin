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

    public class BookingReportController : Controller
    {
        private MainEntities db = new MainEntities();

        // GET: BookingReport
        public ActionResult Index(int? id = 0,int? type=0, string status="",int? vehicle_id=0)
        {
            var rptBooking = db.rptBooking.AsQueryable();
            ViewBag.SearchBy = new List<string>();
            if (!UserSession.HasRole("admin"))
            {
                rptBooking = rptBooking.Where(a => a.owner_id == UserSession.User.user_id);
            }
            if (status!="")
            {
                ViewBag.SearchBy.Add("status");
            }
            if (status == "booked")
            {
                rptBooking = rptBooking.Where(a => a.canceled_at == null && a.reached_at == null && a.start_at == null && a.end_at == null);
            }
            if (status == "reached")
            {
                rptBooking = rptBooking.Where(a => a.canceled_at == null && a.reached_at!=null   &&  a.start_at==null && a.end_at==null);
            }                                                               
            if (status == "started")                                        
            {                                                               
                rptBooking = rptBooking.Where(a => a.canceled_at == null && a.reached_at != null &&  a.start_at != null && a.end_at == null);
            }
            if (status == "ended")
            {
                rptBooking = rptBooking.Where(a => a.canceled_at == null && a.reached_at != null &&  a.start_at != null && a.end_at != null);
            }
            if (status == "canceled")
            {
                rptBooking = rptBooking.Where(a => a.canceled_at != null );
            }
            if (vehicle_id>0)
            {
                rptBooking = rptBooking.Where(a => a.vehicle_id == vehicle_id);
                ViewBag.SearchBy.Add("vehicle_id");
            }


            if (id >0)
            {
                rptBooking = rptBooking.Where(a => a.trip_share_details_id==id);
                ViewBag.SearchBy.Add("id");
            }

            if (type > 0)
            {
                rptBooking = rptBooking.Where(a => a.trip_type_id == type);
                ViewBag.SearchBy.Add("type");
            }


            return View(rptBooking.ToList());
        }

        // GET: BookingReport/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            rptBooking rptBooking = db.rptBooking.FirstOrDefault(a=>a.id==id);
            if (rptBooking == null)
            {
                return HttpNotFound();
            }
            return View(rptBooking);
        }

      
        // GET: BookingReport/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            rptBooking rptBooking = db.rptBooking.FirstOrDefault(a=>a.id==id);
            if (rptBooking == null)
            {
                return HttpNotFound();
            }
            return View(rptBooking);
        }

        // POST: BookingReport/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            rptBooking rptBooking = db.rptBooking.Find(id);
            db.rptBooking.Remove(rptBooking);
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
