﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Capstone_RPG_Manager.Models;

namespace Capstone_RPG_Manager.Controllers
{
    public class PermessiDMTabController : Controller
    {
        private ModelDBContext db = new ModelDBContext();

        public ActionResult Index()
        {
            var permessiDMTab = db.PermessiDMTab.Include(p => p.AmbientazioniTab).Include(p => p.UtentiTab).Include(p => p.UtentiTab1);
            return View(permessiDMTab.ToList());
        }

        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PermessiDMTab permessiDMTab = db.PermessiDMTab.Find(id);
            if (permessiDMTab == null)
            {
                return HttpNotFound();
            }
            return View(permessiDMTab);
        }

        public ActionResult Create()
        {
            ViewBag.IDAmbientazioniTab = new SelectList(db.AmbientazioniTab, "ID", "Nome");
            ViewBag.IDUtentiTabA = new SelectList(db.UtentiTab, "ID", "Username");
            ViewBag.IDUtentiTabB = new SelectList(db.UtentiTab, "ID", "Username");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,Permesso,Cancellazione,IDUtentiTabA,IDUtentiTabB,IDAmbientazioniTab")] PermessiDMTab permessiDMTab)
        {
            if (ModelState.IsValid)
            {
                db.PermessiDMTab.Add(permessiDMTab);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.IDAmbientazioniTab = new SelectList(db.AmbientazioniTab, "ID", "Nome", permessiDMTab.IDAmbientazioniTab);
            ViewBag.IDUtentiTabA = new SelectList(db.UtentiTab, "ID", "Username", permessiDMTab.IDUtentiTabA);
            ViewBag.IDUtentiTabB = new SelectList(db.UtentiTab, "ID", "Username", permessiDMTab.IDUtentiTabB);
            return View(permessiDMTab);
        }

        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PermessiDMTab permessiDMTab = db.PermessiDMTab.Find(id);
            if (permessiDMTab == null)
            {
                return HttpNotFound();
            }
            ViewBag.IDAmbientazioniTab = new SelectList(db.AmbientazioniTab, "ID", "Nome", permessiDMTab.IDAmbientazioniTab);
            ViewBag.IDUtentiTabA = new SelectList(db.UtentiTab, "ID", "Username", permessiDMTab.IDUtentiTabA);
            ViewBag.IDUtentiTabB = new SelectList(db.UtentiTab, "ID", "Username", permessiDMTab.IDUtentiTabB);
            return View(permessiDMTab);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,Permesso,Cancellazione,IDUtentiTabA,IDUtentiTabB,IDAmbientazioniTab")] PermessiDMTab permessiDMTab)
        {
            if (ModelState.IsValid)
            {
                db.Entry(permessiDMTab).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.IDAmbientazioniTab = new SelectList(db.AmbientazioniTab, "ID", "Nome", permessiDMTab.IDAmbientazioniTab);
            ViewBag.IDUtentiTabA = new SelectList(db.UtentiTab, "ID", "Username", permessiDMTab.IDUtentiTabA);
            ViewBag.IDUtentiTabB = new SelectList(db.UtentiTab, "ID", "Username", permessiDMTab.IDUtentiTabB);
            return View(permessiDMTab);
        }

        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PermessiDMTab permessiDMTab = db.PermessiDMTab.Find(id);
            if (permessiDMTab == null)
            {
                return HttpNotFound();
            }
            return View(permessiDMTab);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            PermessiDMTab permessiDMTab = db.PermessiDMTab.Find(id);
            db.PermessiDMTab.Remove(permessiDMTab);
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
