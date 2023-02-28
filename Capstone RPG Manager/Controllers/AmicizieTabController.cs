using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Mvc;
using Capstone_RPG_Manager.Models;

namespace Capstone_RPG_Manager.Controllers
{
    public class AmicizieTabController : Controller
    {
        private ModelDBContext db = new ModelDBContext();

        public ActionResult Index()
        {
            var amicizieTab = db.AmicizieTab.Include(a => a.UtentiTab).Include(a => a.UtentiTab1);
            return View(amicizieTab.ToList());
        }

        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AmicizieTab amicizieTab = db.AmicizieTab.Find(id);
            if (amicizieTab == null)
            {
                return HttpNotFound();
            }
            return View(amicizieTab);
        }

        public ActionResult Create()
        {
            ViewBag.IDUtentiTabA = new SelectList(db.UtentiTab, "ID", "Username");
            ViewBag.IDUtentiTabB = new SelectList(db.UtentiTab, "ID", "Username");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,Messaggio,Cancellazione,IDUtentiTabA,IDUtentiTabB")] AmicizieTab amicizieTab)
        {
            if (ModelState.IsValid)
            {
                db.AmicizieTab.Add(amicizieTab);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.IDUtentiTabA = new SelectList(db.UtentiTab, "ID", "Username", amicizieTab.IDUtentiTabA);
            ViewBag.IDUtentiTabB = new SelectList(db.UtentiTab, "ID", "Username", amicizieTab.IDUtentiTabB);
            return View(amicizieTab);
        }

        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AmicizieTab amicizieTab = db.AmicizieTab.Find(id);
            if (amicizieTab == null)
            {
                return HttpNotFound();
            }
            ViewBag.IDUtentiTabA = new SelectList(db.UtentiTab, "ID", "Username", amicizieTab.IDUtentiTabA);
            ViewBag.IDUtentiTabB = new SelectList(db.UtentiTab, "ID", "Username", amicizieTab.IDUtentiTabB);
            return View(amicizieTab);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,Messaggio,Cancellazione,IDUtentiTabA,IDUtentiTabB")] AmicizieTab amicizieTab)
        {
            if (ModelState.IsValid)
            {
                db.Entry(amicizieTab).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.IDUtentiTabA = new SelectList(db.UtentiTab, "ID", "Username", amicizieTab.IDUtentiTabA);
            ViewBag.IDUtentiTabB = new SelectList(db.UtentiTab, "ID", "Username", amicizieTab.IDUtentiTabB);
            return View(amicizieTab);
        }

        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AmicizieTab amicizieTab = db.AmicizieTab.Find(id);
            if (amicizieTab == null)
            {
                return HttpNotFound();
            }
            return View(amicizieTab);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            AmicizieTab amicizieTab = db.AmicizieTab.Find(id);
            db.AmicizieTab.Remove(amicizieTab);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        public JsonResult InvitationMessage(int idUser, int idSetting)
        {
            UtentiTab DM = db.UtentiTab.Where(x => x.Username == User.Identity.Name).FirstOrDefault();
            AmbientazioniTab Setting = db.AmbientazioniTab.Find(idSetting);
            string urlHost = Request.Url.Host.ToString();
            string urlInvitation = "https://www.localhost:44397/PermessiDMTab/InvitationAccepted?idDM=" + DM.ID + "&idUser=" + idUser + "&idSetting=" + idSetting;
            urlInvitation = "<a href=\"" + urlInvitation + "\" class=\"evidence\">Clicca qui per accettare la richiesta</a>";
            string RequestMessage = "L'utente " + DM.Username + " ti ha invitato ad unirsi alla sua ambientazione " + Setting.Nome + ". Clicca sul seguente link per accettare la richiesta <br/>" + MvcHtmlString.Create(urlInvitation);
            AmicizieTab InvitationMessage = new AmicizieTab();
            InvitationMessage.IDUtentiTabA = DM.ID;
            InvitationMessage.IDUtentiTabB = idUser;
            InvitationMessage.Messaggio = RequestMessage;
            InvitationMessage.OraMessaggio = DateTime.Now;
            db.AmicizieTab.Add(InvitationMessage);
            db.SaveChanges();
            return Json("Invito Ambientazione", JsonRequestBehavior.AllowGet);
        }

        public ActionResult PWProfileMessages()
        {
            int idUser = db.UtentiTab.Where(x => x.Username == User.Identity.Name).FirstOrDefault().ID;
            List<AmicizieTab> Messaggi = db.AmicizieTab.Where(x => x.IDUtentiTabB == idUser && x.Cancellazione == false).OrderBy(x => x.OraMessaggio).ToList();            
            return PartialView("_PWProfileMessages", Messaggi);
        }

        public JsonResult DeleteMessage(int id)
        {
            AmicizieTab messaggio = db.AmicizieTab.Find(id);
            messaggio.Cancellazione = true;
            db.Entry(messaggio).State = EntityState.Modified;
            db.SaveChanges();
            return Json("Delete", JsonRequestBehavior.AllowGet);
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
