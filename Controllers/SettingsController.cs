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
    public class SettingsController : Controller
    {
        private MainEntities db = new MainEntities();

        // GET: Settions
        public ActionResult Index()
        {
            return View(db.tbl_setting.ToList());
        }

        // GET: Settions/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tbl_setting tbl_setting = db.tbl_setting.Find(id);
            if (tbl_setting == null)
            {
                return HttpNotFound();
            }
            return View(tbl_setting);
        }

        // GET: Settions/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Settions/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id,setting_key,setting_name,setting_value,setting_type,datasource_url,datasource_json,setting_group,display,sequance")] tbl_setting tbl_setting)
        {
            if (ModelState.IsValid)
            {
                db.tbl_setting.Add(tbl_setting);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(tbl_setting);
        }

        // GET: Settions/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tbl_setting tbl_setting = db.tbl_setting.Find(id);
            if (tbl_setting == null)
            {
                return HttpNotFound();
            }
            return View(tbl_setting);
        }

        // POST: Settions/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id,setting_key,setting_name,setting_value,setting_type,datasource_url,datasource_json,setting_group,display,sequance")] tbl_setting tbl_setting)
        {
            if (ModelState.IsValid)
            {
                db.Entry(tbl_setting).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(tbl_setting);
        }

        // GET: Settions/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tbl_setting tbl_setting = db.tbl_setting.Find(id);
            if (tbl_setting == null)
            {
                return HttpNotFound();
            }
            return View(tbl_setting);
        }

        // POST: Settions/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            tbl_setting tbl_setting = db.tbl_setting.Find(id);
            db.tbl_setting.Remove(tbl_setting);
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
