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

    public class TripSharedDetailsController : Controller
    {
        private MainEntities db = new MainEntities();

        // GET: trip_share_details
        public ActionResult Index(int? id,int? type=0,DateTime? from=null,DateTime? to=null, bool? successed =null)
        {

            var trip_share_details = db.trip_share_details.Include(t => t.trip_share).AsQueryable();
            if (id > 0)
            {
                trip_share_details = trip_share_details.Where(a => a.trip_share_id == id);
            }
            if (type > 0)
            {
                trip_share_details = trip_share_details.Where(a => a.trip_share.trip_type_id == type);
            }
            if (!UserSession.HasRole("admin"))
            {
                trip_share_details = trip_share_details.Where(a=>a.trip_share.driver_id==UserSession.User.user_id);
            }
            if (from!=null)
            {
                trip_share_details = trip_share_details.Where(a=>a.start_at_date>=from);
            }
            if (to!=null)
            {
                trip_share_details = trip_share_details.Where(a=>a.start_at_date<=to);
            }

            if (successed == true)
            {
                trip_share_details = trip_share_details.Where(a => a.trip_book.Where(b=>b.canceled_at==null).Count() > 0 && a.trip_book.Where(b => b.end_at == null && b.canceled_at==null).Count() == 0);
            }
            if (successed == false)
            {
                trip_share_details = trip_share_details.Where(a => a.trip_book.Where(b => b.canceled_at == null).Count() > 0 && a.trip_book.Where(b => b.end_at == null && b.canceled_at == null).Count() > 0);
            }

            return View(trip_share_details.ToList());
        }

      

        
        // GET: trip_share_details/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            trip_share_details trip_share_details = db.trip_share_details.Find(id);
            if (trip_share_details == null)
            {
                return HttpNotFound();
            }
            return View(trip_share_details);
        }

       
        // GET: trip_share_details/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            trip_share_details trip_share_details = db.trip_share_details.Find(id);
            if (trip_share_details == null)
            {
                return HttpNotFound();
            }
            return View(trip_share_details);
        }

        // POST: trip_share_details/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            trip_share_details trip_share_details = db.trip_share_details.Find(id);
            db.trip_share_details.Remove(trip_share_details);
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
