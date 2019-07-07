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
using Chair80CP.Libs;

namespace Chair80CP.Controllers
{
    [RoleFilter(new string[]{ "admin"})]
    public class CityController : Controller
    {
        private MainEntities db = new MainEntities();

        // GET: Addreesees
         
        public ActionResult Index()
        {
            return View(db.tbl_cities.ToList());
        }

        // GET: Addreesees/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tbl_cities tbl_cities = db.tbl_cities.Find(id);
            if (tbl_cities == null)
            {
                return HttpNotFound();
            }
            return View(tbl_cities);
        }

         // GET: Addreesees/Create
        public ActionResult Create()
        {

            var itms = db.tbl_countries.ToList();
            itms.ForEach(b => b.name = b.name.local());

            var lst= new SelectList(itms,"id","name");

            ViewBag.country_id = lst;

            return View();
        }

        // POST: Addreesees/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(tbl_cities type)
        {
            var tbl_cities = new tbl_cities();

            var dic = new Dictionary<string, string>();

            foreach (string lang in BLL.GlobalData.Languages)
            {

                dic.Add(lang, Request.Form["name[" + lang + "]"]);
            }
             
                tbl_cities.name = JsonConvert.SerializeObject(dic);
            tbl_cities.country_id = int.Parse(Request.Form["country_id"]);

            db.tbl_cities.Add(tbl_cities);
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

            //return View(tbl_cities);
        }

        // GET: Addreesees/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tbl_cities tbl_cities = db.tbl_cities.Find(id);
            if (tbl_cities == null)
            {
                return HttpNotFound();
            }
            var itms = db.tbl_countries.ToList();
            itms.ForEach(b => b.name = b.name.local());

            var lst = new SelectList(itms, "id", "name",tbl_cities.country_id);

            ViewBag.country_id = lst;
            return View(tbl_cities);
        }

        // POST: Addreesees/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "name,id")] tbl_cities type)
        {
            var tbl_cities = new tbl_cities();

            var dic = new Dictionary<string, string>();

            foreach (string lang in BLL.GlobalData.Languages)
            {

            dic.Add(lang, Request.Form["name["+lang+"]"]);
            }
            //dic.Add("ar", Request.Form["name[ar]"]);

            tbl_cities.name = JsonConvert.SerializeObject(dic);
            tbl_cities.id = int.Parse(Request.Form["id"]);
            tbl_cities.country_id = int.Parse(Request.Form["country_id"]);

            db.Entry(tbl_cities).State = EntityState.Modified;
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
            return View(tbl_cities);
        }

        // GET: Addreesees/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tbl_cities tbl_cities = db.tbl_cities.Find(id);
            if (tbl_cities == null)
            {
                return HttpNotFound();
            }
            return View(tbl_cities);
        }

        // POST: Addreesees/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            tbl_cities tbl_cities = db.tbl_cities.Find(id);
            db.tbl_cities.Remove(tbl_cities);
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
