using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using System.Web.Services.Description;
using Capstone_RPG_Manager.Models;

namespace Capstone_RPG_Manager.Controllers
{
    public class UtentiTabController : Controller
    {
        private ModelDBContext db = new ModelDBContext();

        public ActionResult Index()
        {
            var utentiTab = db.UtentiTab.Include(u => u.RuoliTab);
            return View(utentiTab.ToList());
        }

        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            UtentiTab utentiTab = db.UtentiTab.Find(id);
            if (utentiTab == null)
            {
                return HttpNotFound();
            }
            return View(utentiTab);
        }

        public ActionResult Create()
        {
            ViewBag.IDRuoliTab = new SelectList(db.RuoliTab, "ID", "Ruolo");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,Username,Password,DM,Email,IDRuoliTab")] UtentiTab utentiTab)
        {
            if (ModelState.IsValid)
            {
                db.UtentiTab.Add(utentiTab);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.IDRuoliTab = new SelectList(db.RuoliTab, "ID", "Ruolo", utentiTab.IDRuoliTab);
            return View(utentiTab);
        }

        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            UtentiTab utentiTab = db.UtentiTab.Find(id);
            if (utentiTab == null)
            {
                return HttpNotFound();
            }
            ViewBag.IDRuoliTab = new SelectList(db.RuoliTab, "ID", "Ruolo", utentiTab.IDRuoliTab);
            return View(utentiTab);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,Username,Password,DM,Email,IDRuoliTab")] UtentiTab utentiTab)
        {
            if (ModelState.IsValid)
            {
                db.Entry(utentiTab).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.IDRuoliTab = new SelectList(db.RuoliTab, "ID", "Ruolo", utentiTab.IDRuoliTab);
            return View(utentiTab);
        }

        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            UtentiTab utentiTab = db.UtentiTab.Find(id);
            if (utentiTab == null)
            {
                return HttpNotFound();
            }
            return View(utentiTab);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            UtentiTab utentiTab = db.UtentiTab.Find(id);
            db.UtentiTab.Remove(utentiTab);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        public ActionResult Login()
        {           
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(UtentiTab utentiTab)
        {
            ModelState.Remove("Email");
            if (ModelState.IsValid == true && UtentiTab.IsLogged(utentiTab, db) == true)
            {
                FormsAuthentication.SetAuthCookie(utentiTab.Username, false);
                return Redirect(FormsAuthentication.DefaultUrl);
            }
            ViewBag.Error = "The Username or Password is incorrect";
            //ViewBag.IDRuoliTab = new SelectList(db.RuoliTab, "ID", "Ruolo", utentiTab.IDRuoliTab);
            return View();
        }

        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            return Redirect(FormsAuthentication.DefaultUrl);
        }

        public ActionResult Register()
        {
            //ViewBag.IDRuoliTab = new SelectList(db.RuoliTab, "ID", "Ruolo");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Register([Bind(Exclude = "IDRuoliTab")] UtentiTab utentiTab, string PasswordRepeat)
        {
            if (ModelState.IsValid == true && UtentiTab.IsRegistered(utentiTab, db) == true)
            {
                if (utentiTab.Password != PasswordRepeat)
                {
                    ViewBag.PasswordErrata = "Passwords do not match";
                    return View(utentiTab);
                }
                utentiTab.IDRuoliTab = 2;
                db.UtentiTab.Add(utentiTab);
                db.SaveChanges();
                return RedirectToAction("Home", "Index");
            }
            //ViewBag.IDRuoliTab = new SelectList(db.RuoliTab, "ID", "Ruolo", utentiTab.IDRuoliTab);
            return View(utentiTab);
        }

        public ActionResult UserProfile()
        {
            int idUser = db.UtentiTab.Where(x => x.Username == User.Identity.Name).FirstOrDefault().ID;
            UtentiTab user = db.UtentiTab.Find(idUser);
            ViewBag.NumSettingsLive = db.AmbientazioniTab.Where(x => x.IDUtentiTab == idUser && x.Cancellazione == false).Count();
            ViewBag.NumSettingsDelete = db.AmbientazioniTab.Where(x => x.IDUtentiTab == idUser && x.Cancellazione == true).Count();
            ViewBag.NumSettingsTot = db.AmbientazioniTab.Where(x => x.IDUtentiTab == idUser).Count();
            ViewBag.NumCampaignsLive = db.CampagneTab.Where(x => x.AmbientazioniTab.IDUtentiTab == idUser && x.Cancellazione == false).Count();
            ViewBag.NumCampaignsDelete = db.CampagneTab.Where(x => x.AmbientazioniTab.IDUtentiTab == idUser && x.Cancellazione == true).Count();
            ViewBag.NumCampaignsTot = db.CampagneTab.Where(x => x.AmbientazioniTab.IDUtentiTab == idUser).Count();
            ViewBag.NumSessionsLive = db.SessioniTab.Where(x => x.CampagneTab.AmbientazioniTab.IDUtentiTab == idUser && x.Cancellazione == false).Count();
            ViewBag.NumSessionsDelete = db.SessioniTab.Where(x => x.CampagneTab.AmbientazioniTab.IDUtentiTab == idUser && x.Cancellazione == true).Count();
            ViewBag.NumSessionsTot = db.SessioniTab.Where(x => x.CampagneTab.AmbientazioniTab.IDUtentiTab == idUser).Count();
            ViewBag.Messages = db.AmicizieTab.Where(x => x.IDUtentiTabB == idUser && x.Cancellazione == false).OrderBy(x => x.OraMessaggio).Count();
            return View(user);
        }

        public ActionResult DMStatus (int id)
        {
            UtentiTab utentiTab = db.UtentiTab.Find(id);
            if (utentiTab.DM)
            {
                utentiTab.DM = false;
            }
            else
            {
                utentiTab.DM = true;
            }
            db.Entry(utentiTab).State = EntityState.Modified;
            db.SaveChanges();
            //return RedirectToAction("UserProfile", utentiTab);
            return RedirectToAction("DMScreen", "Home");
        }

        public ActionResult PWSearchingList()
        {
            int idDM = db.UtentiTab.Where(x => x.Username == User.Identity.Name).FirstOrDefault().ID;
            ViewBag.IDAmbientazioniTab = new SelectList(AmbientazioniTab.GetListSettingsByDM(idDM, db), "ID", "Nome");
            List<UtentiTab> ListaUtenti = (List<UtentiTab>)TempData["ListaUtenti"];
            return PartialView("_PWSearchingList", ListaUtenti);
        }

        [HttpPost]
        public ActionResult PWSearchingList(string TextUser)
        {
            if (string.IsNullOrEmpty(TextUser))
            {
                TempData["ListaUtenti"] = null;
            }
            else 
            {
                List<UtentiTab> ListaUtenti = db.UtentiTab.Where(x => x.Username.Contains(TextUser)).ToList();
                TempData["ListaUtenti"] = ListaUtenti;
            }
            return RedirectToAction("UserProfile");
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
