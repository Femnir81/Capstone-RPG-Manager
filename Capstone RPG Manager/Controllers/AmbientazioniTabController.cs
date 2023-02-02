using System;
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
    public class AmbientazioniTabController : Controller
    {
        private ModelDBContext db = new ModelDBContext();

        public ActionResult Index()
        {
            var ambientazioniTab = db.AmbientazioniTab.Include(a => a.CampagneTab);
            return View(ambientazioniTab.ToList());
        }

        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AmbientazioniTab ambientazioniTab = db.AmbientazioniTab.Find(id);
            if (ambientazioniTab == null)
            {
                return HttpNotFound();
            }
            return View(ambientazioniTab);
        }

        public ActionResult Create()
        {
            ViewBag.IDCampagneTab = new SelectList(db.CampagneTab, "ID", "Nome");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,Nome,Descrizione,Immagine,Privata,Cancellazione,IDCampagneTab")] AmbientazioniTab ambientazioniTab)
        {
            if (ModelState.IsValid)
            {
                db.AmbientazioniTab.Add(ambientazioniTab);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(ambientazioniTab);
        }

        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AmbientazioniTab ambientazioniTab = db.AmbientazioniTab.Find(id);
            if (ambientazioniTab == null)
            {
                return HttpNotFound();
            }
            return View(ambientazioniTab);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,Nome,Descrizione,Immagine,Privata,Cancellazione,IDCampagneTab")] AmbientazioniTab ambientazioniTab)
        {
            if (ModelState.IsValid)
            {
                db.Entry(ambientazioniTab).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(ambientazioniTab);
        }

        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AmbientazioniTab ambientazioniTab = db.AmbientazioniTab.Find(id);
            if (ambientazioniTab == null)
            {
                return HttpNotFound();
            }
            return View(ambientazioniTab);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            AmbientazioniTab ambientazioniTab = db.AmbientazioniTab.Find(id);
            db.AmbientazioniTab.Remove(ambientazioniTab);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        public ActionResult PWListSettingsByDM()
        {
            
            return PartialView("_PWListCampaignsByDM");
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
