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
            var ambientazioniTab = db.AmbientazioniTab.Include(a => a.UtentiTab);
            return View(ambientazioniTab.ToList());
        }

        // GET: AmbientazioniTab/Details/5
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
            //ViewBag.IDUtentiTab = new SelectList(db.UtentiTab, "ID", "Username");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Exclude = "Cancellazione,IDUtentiTab")] AmbientazioniTab ambientazioniTab, HttpPostedFileBase Img)
        {
            if (ModelState.IsValid)
            {
                if (Img != null && Img.ContentLength <= 1000000)
                {
                    string ImgCode = DateTime.Now.Day.ToString() + DateTime.Now.Month.ToString() + DateTime.Now.Year.ToString() + DateTime.Now.Hour.ToString() + DateTime.Now.Minute.ToString() + DateTime.Now.Second.ToString() + DateTime.Now.Millisecond.ToString();
                    ambientazioniTab.Immagine = ImgCode;
                    Img.SaveAs(Server.MapPath("~/Content/Images/DB/" + ambientazioniTab.Immagine));
                }
                else if (Img != null && Img.ContentLength > 1000000)
                {
                    ViewBag.ImgError = "The image must be less than 1Mb";
                    return View(ambientazioniTab);
                }
                int id = db.UtentiTab.Where(x => x.Username == User.Identity.Name).FirstOrDefault().ID;
                ambientazioniTab.IDUtentiTab = id;
                db.AmbientazioniTab.Add(ambientazioniTab);
                db.SaveChanges();
                return RedirectToAction("DMScreen", "Home");
            }
            //ViewBag.IDUtentiTab = new SelectList(db.UtentiTab, "ID", "Username", ambientazioniTab.IDUtentiTab);
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
            ViewBag.IDUtentiTab = new SelectList(db.UtentiTab, "ID", "Username", ambientazioniTab.IDUtentiTab);
            return View(ambientazioniTab);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,Nome,Descrizione,Immagine,Privata,Cancellazione,IDUtentiTab")] AmbientazioniTab ambientazioniTab)
        {
            if (ModelState.IsValid)
            {
                db.Entry(ambientazioniTab).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.IDUtentiTab = new SelectList(db.UtentiTab, "ID", "Username", ambientazioniTab.IDUtentiTab);
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

        public ActionResult PWSettingsListByDM()
        {
            int id = db.UtentiTab.Where(x => x.Username == User.Identity.Name).FirstOrDefault().ID;
            return PartialView("_PWSettingsListByDM", AmbientazioniTab.GetListSettingsByDM(id, db));
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
