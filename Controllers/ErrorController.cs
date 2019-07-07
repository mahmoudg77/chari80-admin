using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Chair80CP.Controllers
{
    public class ErrorController : Controller
    {
        // GET: Error
        

        public ActionResult Index(int id)
        {
            ViewBag.Error = id;
            return View();
        }

    }
}