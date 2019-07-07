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
    public class GenderController : Controller
    {
        private MainEntities db = new MainEntities();

        // GET: Addreesees
         
        public ActionResult Index()
        {
            return View(db.tbl_genders.ToList());
        }

        // GET: Addreesees/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tbl_genders tbl_genders = db.tbl_genders.Find(id);
            if (tbl_genders == null)
            {
                return HttpNotFound();
            }
            return View(tbl_genders);
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
        public ActionResult Create(tbl_genders type)
        {
            var tbl_genders = new tbl_genders();

            var dic = new Dictionary<string, string>();

            foreach (string lang in BLL.GlobalData.Languages)
            {

                dic.Add(lang, Request.Form["name[" + lang + "]"]);
            }
             
                tbl_genders.name = JsonConvert.SerializeObject(dic);

                db.tbl_genders.Add(tbl_genders);
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

            //return View(tbl_genders);
        }

        // GET: Addreesees/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tbl_genders tbl_genders = db.tbl_genders.Find(id);
            if (tbl_genders == null)
            {
                return HttpNotFound();
            }
            return View(tbl_genders);
        }

        // POST: Addreesees/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Name,id")] tbl_genders type)
        {
            var tbl_genders = new tbl_genders();

            var dic = new Dictionary<string, string>();

            foreach (string lang in BLL.GlobalData.Languages)
            {

            dic.Add(lang, Request.Form["name["+lang+"]"]);
            }
            //dic.Add("ar", Request.Form["name[ar]"]);

            tbl_genders.name = JsonConvert.SerializeObject(dic);
            tbl_genders.id = int.Parse(Request.Form["id"]);

            db.Entry(tbl_genders).State = EntityState.Modified;
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
            return View(tbl_genders);
        }

        // GET: Addreesees/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tbl_genders tbl_genders = db.tbl_genders.Find(id);
            if (tbl_genders == null)
            {
                return HttpNotFound();
            }
            return View(tbl_genders);
        }

        // POST: Addreesees/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            tbl_genders tbl_genders = db.tbl_genders.Find(id);
            db.tbl_genders.Remove(tbl_genders);
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
