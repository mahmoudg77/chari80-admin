using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Chair80CP.Controllers
{
    [LoginFilter]
    public class HomeController : Controller
    {
        Models.MainEntities db = new Models.MainEntities();
        // GET: Home
        public ActionResult Index()
        {
            var counters = db.spWebCounters();
            return View(counters);
        }
    }
}