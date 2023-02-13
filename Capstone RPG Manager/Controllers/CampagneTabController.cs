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
    public class CampagneTabController : Controller
    {
        private ModelDBContext db = new ModelDBContext();

        public ActionResult Index()
        {
            var campagneTab = db.CampagneTab.Include(c => c.AmbientazioniTab);
            return View(campagneTab.ToList());
        }

        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CampagneTab campagneTab = db.CampagneTab.Find(id);
            if (campagneTab.Cancellazione == true)
            {
                return HttpNotFound();
            }
            TempData["IdCampaignForSession"] = id;
            return View(campagneTab);
        }

        public ActionResult Create()
        {
            int id = db.UtentiTab.Where(x => x.Username == User.Identity.Name).FirstOrDefault().ID;
            ViewBag.IDAmbientazioniTab = new SelectList(AmbientazioniTab.GetListSettingsByDM(id, db), "ID", "Nome");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Exclude = "Cancellazione")] CampagneTab campagneTab)
        {
            if (ModelState.IsValid)
            {
                db.CampagneTab.Add(campagneTab);
                db.SaveChanges();
                return RedirectToAction("DMScreen", "Home");
            }
            int id = db.UtentiTab.Where(x => x.Username == User.Identity.Name).FirstOrDefault().ID;
            ViewBag.IDAmbientazioniTab = new SelectList(AmbientazioniTab.GetListSettingsByDM(id, db), "ID", "Nome", campagneTab.IDAmbientazioniTab);
            return View(campagneTab);
        }

        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CampagneTab campagneTab = db.CampagneTab.Find(id);
            if (campagneTab.Cancellazione == true)
            {
                return HttpNotFound();
            }
            int idDM = db.UtentiTab.Where(x => x.Username == User.Identity.Name).FirstOrDefault().ID;
            ViewBag.IDAmbientazioniTab = new SelectList(AmbientazioniTab.GetListSettingsByDM(idDM, db), "ID", "Nome", campagneTab.IDAmbientazioniTab);
            return View(campagneTab);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Exclude = "Cancellazione")] CampagneTab campagneTab)
        {
            if (ModelState.IsValid)
            {
                db.Entry(campagneTab).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("DMScreen", "Home");
            }
            int idDM = db.UtentiTab.Where(x => x.Username == User.Identity.Name).FirstOrDefault().ID;
            ViewBag.IDAmbientazioniTab = new SelectList(AmbientazioniTab.GetListSettingsByDM(idDM, db), "ID", "Nome", campagneTab.IDAmbientazioniTab);
            return View(campagneTab);
        }

        public ActionResult Delete(int? id)
        {
            CampagneTab campagneTab = db.CampagneTab.Find(id);
            campagneTab.Cancellazione = true;
            db.Entry(campagneTab).State = EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("DMScreen", "Home");
        }

        public ActionResult PWCampaignsListByDM()
        {
            int id = db.UtentiTab.Where(x => x.Username == User.Identity.Name).FirstOrDefault().ID;
            return PartialView("_PWCampaignsListByDM", CampagneTab.GetListCampaignsByDM(id, db));
        }

        public ActionResult PWCampaignsListBySetting(int id)
        {
            return PartialView("_PWCampaignsListBySetting", CampagneTab.GetListCampaignsBySetting(id, db));
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
