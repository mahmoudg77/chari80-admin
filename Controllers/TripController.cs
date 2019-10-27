using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Chair80CP.Controllers
{
    public class TripController : Controller
    {
        Models.MainEntities db = new Models.MainEntities();
        // GET: Trip
        public ActionResult Tracking(Guid id)
        {
            Models.vwTripsDetails trip = db.vwTripsDetails.FirstOrDefault(a => a.guid == id);
            ViewBag.Images = db.tbl_images.Where(a => a.model_name == "tbl_trips" && a.model_id == trip.trip_id).ToList();
            return View(trip);
        }
    }
}