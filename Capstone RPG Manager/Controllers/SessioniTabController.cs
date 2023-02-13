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
    public class SessioniTabController : Controller
    {
        private ModelDBContext db = new ModelDBContext();

        public ActionResult Index()
        {
            var sessioniTab = db.SessioniTab.Include(s => s.CampagneTab);
            return View(sessioniTab.ToList());
        }

        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SessioniTab sessioniTab = db.SessioniTab.Find(id);
            if (sessioniTab.Cancellazione == true)
            {
                return HttpNotFound();
            }
            return View(sessioniTab);
        }

        public ActionResult Create()
        {           
            //ViewBag.IDCampagna = new SelectList(db.CampagneTab, "ID", "Nome");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Exclude = "Cancellazione,IDCampagna")] SessioniTab sessioniTab)
        {
            if (ModelState.IsValid)
            {
                int IdCampaignForSession = Convert.ToInt32(TempData["IdCampaignForSession"]);
                sessioniTab.IDCampagna = IdCampaignForSession;
                db.SessioniTab.Add(sessioniTab);
                db.SaveChanges();
                return RedirectToAction("Details", "CampagneTab", new { id = IdCampaignForSession});
            }

            //ViewBag.IDCampagna = new SelectList(db.CampagneTab, "ID", "Nome", sessioniTab.IDCampagna);
            return View(sessioniTab);
        }

        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SessioniTab sessioniTab = db.SessioniTab.Find(id);
            if (sessioniTab.Cancellazione == true)
            {
                return HttpNotFound();
            }
            //ViewBag.IDCampagna = new SelectList(db.CampagneTab, "ID", "Nome", sessioniTab.IDCampagna);
            return View(sessioniTab);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Exclude = "Cancellazione")] SessioniTab sessioniTab)
        {
            if (ModelState.IsValid)
            {
                SessioniTab SessioniInDB = db.SessioniTab.Find(sessioniTab.ID);
                SessioniInDB.Titolo= sessioniTab.Titolo;
                SessioniInDB.Descrizione= sessioniTab.Descrizione;
                SessioniInDB.Data = sessioniTab.Data;
                db.Entry(SessioniInDB).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Details", "CampagneTab", new { id = SessioniInDB.IDCampagna});
            }
            //ViewBag.IDCampagna = new SelectList(db.CampagneTab, "ID", "Nome", sessioniTab.IDCampagna);
            return View(sessioniTab);
        }

        public ActionResult Delete(int id)
        {
            SessioniTab sessioniTab = db.SessioniTab.Find(id);
            SessioniTab.Delete(db, sessioniTab);
            int idCampagna = db.CampagneTab.Where(x => x.ID == sessioniTab.IDCampagna).FirstOrDefault().ID;
            return RedirectToAction("Details", "RegioniTab", new { id = idCampagna });
        }

        public ActionResult PWSessionsListByCampaign(int id)
        {
            int idDM = db.UtentiTab.Where(x => x.Username == User.Identity.Name).FirstOrDefault().ID;
            return PartialView("_PWSessionsListByCampaign", SessioniTab.GetListSessionsByCampaign(id, db, idDM));
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
