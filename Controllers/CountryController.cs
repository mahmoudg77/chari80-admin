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
    public class CountryController : Controller
    {
        private MainEntities db = new MainEntities();

        // GET: Addreesees
         
        public ActionResult Index()
        {
            return View(db.tbl_countries.ToList());
        }

        // GET: Addreesees/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tbl_countries tbl_countries = db.tbl_countries.Find(id);
            if (tbl_countries == null)
            {
                return HttpNotFound();
            }
            return View(tbl_countries);
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
        public ActionResult Create(tbl_countries type)
        {
            var tbl_countries = new tbl_countries();

            var dic = new Dictionary<string, string>();

            foreach (string lang in BLL.GlobalData.Languages)
            {

                dic.Add(lang, Request.Form["name[" + lang + "]"]);
            }
             
                tbl_countries.name = JsonConvert.SerializeObject(dic);

                db.tbl_countries.Add(tbl_countries);
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

            //return View(tbl_countries);
        }

        // GET: Addreesees/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tbl_countries tbl_countries = db.tbl_countries.Find(id);
            if (tbl_countries == null)
            {
                return HttpNotFound();
            }
            return View(tbl_countries);
        }

        // POST: Addreesees/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Name,id")] tbl_countries type)
        {
            var tbl_countries = new tbl_countries();

            var dic = new Dictionary<string, string>();

            foreach (string lang in BLL.GlobalData.Languages)
            {

            dic.Add(lang, Request.Form["name["+lang+"]"]);
            }
            //dic.Add("ar", Request.Form["name[ar]"]);

            tbl_countries.name = JsonConvert.SerializeObject(dic);
            tbl_countries.id = int.Parse(Request.Form["id"]);

            db.Entry(tbl_countries).State = EntityState.Modified;
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
            return View(tbl_countries);
        }

        // GET: Addreesees/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tbl_countries tbl_countries = db.tbl_countries.Find(id);
            if (tbl_countries == null)
            {
                return HttpNotFound();
            }
            return View(tbl_countries);
        }

        // POST: Addreesees/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            tbl_countries tbl_countries = db.tbl_countries.Find(id);
            db.tbl_countries.Remove(tbl_countries);
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
