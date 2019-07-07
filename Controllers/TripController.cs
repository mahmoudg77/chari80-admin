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
            return View(trip);
        }
    }
}