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
    public class SessionsController : Controller
    {
       
        private MainEntities db = new MainEntities();

        // GET: Sessions
        public ActionResult Index()
        {
            var sec_sessions = db.sec_sessions.Include(s => s.sec_users).Where(a=>a.end_time==null);
            return View(sec_sessions.ToList());
        }

        // GET: Sessions/Details/5
        public ActionResult Details(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            sec_sessions sec_sessions = db.sec_sessions.Find(id);
            if (sec_sessions == null)
            {
                return HttpNotFound();
            }
            return View(sec_sessions);
        }

        // GET: Sessions/Create
        public ActionResult Create()
        {
            ViewBag.user_id = new SelectList(db.sec_users, "id", "pwd");
            return View();
        }

        // POST: Sessions/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id,user_id,start_time,end_time,ip,agent,browser,country,city,country_code,isp,lat,lon,timezone,paltform,device_id")] sec_sessions sec_sessions)
        {
            if (ModelState.IsValid)
            {
                sec_sessions.id = Guid.NewGuid();
                db.sec_sessions.Add(sec_sessions);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.user_id = new SelectList(db.sec_users, "id", "pwd", sec_sessions.user_id);
            return View(sec_sessions);
        }

        // GET: Sessions/Edit/5
        public ActionResult Edit(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            sec_sessions sec_sessions = db.sec_sessions.Find(id);
            if (sec_sessions == null)
            {
                return HttpNotFound();
            }
            ViewBag.user_id = new SelectList(db.sec_users, "id", "pwd", sec_sessions.user_id);
            return View(sec_sessions);
        }

        // POST: Sessions/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id,user_id,start_time,end_time,ip,agent,browser,country,city,country_code,isp,lat,lon,timezone,paltform,device_id")] sec_sessions sec_sessions)
        {
            if (ModelState.IsValid)
            {
                db.Entry(sec_sessions).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.user_id = new SelectList(db.sec_users, "id", "pwd", sec_sessions.user_id);
            return View(sec_sessions);
        }

        // GET: Sessions/Delete/5
        public ActionResult Delete(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            sec_sessions sec_sessions = db.sec_sessions.Find(id);
            if (sec_sessions == null)
            {
                return HttpNotFound();
            }
            return View(sec_sessions);
        }

        // POST: Sessions/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(Guid id)
        {
            sec_sessions sec_sessions = db.sec_sessions.Find(id);
            sec_sessions.end_time = DateTime.Now;
            db.Entry(sec_sessions).State = EntityState.Modified;
            //db.sec_sessions.Remove(sec_sessions);
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
