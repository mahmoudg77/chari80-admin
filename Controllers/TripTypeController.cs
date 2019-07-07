using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Chair80CP.Models;
using Newtonsoft.Json;

namespace Chair80CP.Controllers
{
    [RoleFilter(new string[] { "admin" })]
    public class TripTypeController :Controller
    {
        private MainEntities db = new MainEntities();

        // GET: Addreesees
         
        public ActionResult Index()
        {
            return View(db.trip_types.ToList());
        }

        // GET: Addreesees/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            trip_types trip_types = db.trip_types.Find(id);
            if (trip_types == null)
            {
                return HttpNotFound();
            }
            return View(trip_types);
        }

         // GET: Addreesees/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Addreesees/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(trip_types type)
        {
            var trip_types = new trip_types();

            var dic = new Dictionary<string, string>();

            foreach (string lang in BLL.GlobalData.Languages)
            {

                dic.Add(lang, Request.Form["name[" + lang + "]"]);
            }
             
                trip_types.name = JsonConvert.SerializeObject(dic);

                db.trip_types.Add(trip_types);
                db.SaveChanges();
                if (Request.IsAjaxRequest())
                {
                    var res = new { type = "success", message = "Save Success" };
                    return Json(res);
                }
                return RedirectToAction("Index");
             
            //if (Request.IsAjaxRequest())
            //{
            //    var res = new { type = "error", message = "Error while save !" };
            //    return Json(res);
            //}

            //return View(trip_types);
        }

        // GET: Addreesees/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            trip_types trip_types = db.trip_types.Find(id);
            if (trip_types == null)
            {
                return HttpNotFound();
            }
            return View(trip_types);
        }

        // POST: Addreesees/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Name,id")] trip_types type)
        {
            var trip_types = new trip_types();

            var dic = new Dictionary<string, string>();

            foreach (string lang in BLL.GlobalData.Languages)
            {

            dic.Add(lang, Request.Form["name["+lang+"]"]);
            }
            //dic.Add("ar", Request.Form["name[ar]"]);

            trip_types.name = JsonConvert.SerializeObject(dic);
            trip_types.id = int.Parse(Request.Form["id"]);

            db.Entry(trip_types).State = EntityState.Modified;
                db.SaveChanges();
                if (Request.IsAjaxRequest())
                {
                    var res = new { type = "success", message = "Save Success" };
                    return Json(res);
                }
                return RedirectToAction("Index");
             
            if (Request.IsAjaxRequest())
            {
                var res = new { type = "error", message = "Error while save !" };
                return Json(res);
            }
            return View(trip_types);
        }

        // GET: Addreesees/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            trip_types trip_types = db.trip_types.Find(id);
            if (trip_types == null)
            {
                return HttpNotFound();
            }
            return View(trip_types);
        }

        // POST: Addreesees/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            trip_types trip_types = db.trip_types.Find(id);
            db.trip_types.Remove(trip_types);
            db.SaveChanges();
            if (Request.IsAjaxRequest())
            {
                var res = new { type = "success", message = "Deleted Success" };
                return Json(res);
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
