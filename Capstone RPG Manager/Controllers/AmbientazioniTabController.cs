using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Web.UI.WebControls;
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
            if (ambientazioniTab.Cancellazione == true)
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
                    int lastIndex = Img.FileName.LastIndexOf('.');
                    //var name = Img.FileName.Substring(0, lastIndex);
                    string ext = Img.FileName.Substring(lastIndex + 1);
                    Random random = new Random();
                    string ImgCode = DateTime.Now.Day.ToString() + DateTime.Now.Month.ToString() + DateTime.Now.Year.ToString() + DateTime.Now.Hour.ToString() + DateTime.Now.Minute.ToString() + DateTime.Now.Second.ToString() + DateTime.Now.Millisecond.ToString() + random.Next(1, 100001) + "." + ext;
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
            if (ambientazioniTab.Cancellazione == true)
            {
                return HttpNotFound();
            }
            //ViewBag.IDUtentiTab = new SelectList(db.UtentiTab, "ID", "Username", ambientazioniTab.IDUtentiTab);
            return View(ambientazioniTab);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Exclude = "Cancellazione,IDUtentiTab")] AmbientazioniTab ambientazioniTab, HttpPostedFileBase Img)
        {
            if (ModelState.IsValid)
            {
                AmbientazioniTab AmbientazioneInDB = db.AmbientazioniTab.Find(ambientazioniTab.ID);
                if (Img != null && Img.ContentLength <= 1000000)
                {
                    System.IO.File.Delete(Server.MapPath("~/Content/Images/DB/" + AmbientazioneInDB.Immagine));
                    int lastIndex = Img.FileName.LastIndexOf('.');
                    //var name = Img.FileName.Substring(0, lastIndex);
                    string ext = Img.FileName.Substring(lastIndex + 1);
                    Random random = new Random();
                    string ImgCode = DateTime.Now.Day.ToString() + DateTime.Now.Month.ToString() + DateTime.Now.Year.ToString() + DateTime.Now.Hour.ToString() + DateTime.Now.Minute.ToString() + DateTime.Now.Second.ToString() + DateTime.Now.Millisecond.ToString() + random.Next(1, 100001) + "." + ext;
                    AmbientazioneInDB.Immagine = ImgCode;
                    Img.SaveAs(Server.MapPath("~/Content/Images/DB/" + AmbientazioneInDB.Immagine));
                }
                else if (Img != null && Img.ContentLength > 1000000)
                {
                    ViewBag.ImgError = "The image must be less than 1Mb";
                    return View(ambientazioniTab);
                }
                AmbientazioneInDB.Nome = ambientazioniTab.Nome;
                AmbientazioneInDB.Descrizione = ambientazioniTab.Descrizione;
                AmbientazioneInDB.Privata = ambientazioniTab.Privata;
                int id = db.UtentiTab.Where(x => x.Username == User.Identity.Name).FirstOrDefault().ID;
                AmbientazioneInDB.ID = id;
                db.Entry(AmbientazioneInDB).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("DMScreen", "Home");
            }
            //ViewBag.IDUtentiTab = new SelectList(db.UtentiTab, "ID", "Username", ambientazioniTab.IDUtentiTab);
            return View(ambientazioniTab);
        }

        public ActionResult Delete(int? id)
        {
            AmbientazioniTab ambientazioniTab = db.AmbientazioniTab.Find(id);
            ambientazioniTab.Cancellazione = true;
            db.Entry(ambientazioniTab).State = EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("DMScreen", "Home");
        }

        public ActionResult PWSettingsListByDM()
        {
            int id = db.UtentiTab.Where(x => x.Username == User.Identity.Name).FirstOrDefault().ID;
            return PartialView("_PWSettingsListByDM", AmbientazioniTab.GetListSettingsByDM(id, db));
        }

        public ActionResult PrivateOnPW(int id)
        {
            AmbientazioniTab ambientazioniTab = db.AmbientazioniTab.Find(id);
            if (ambientazioniTab.Privata)
            {
                ambientazioniTab.Privata = false;               
            }
            else
            {
                ambientazioniTab.Privata = true;
            }
            db.Entry(ambientazioniTab).State = EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("DMScreen", "Home");
        }

        public ActionResult PrivateOnDetails(int id)
        {
            AmbientazioniTab ambientazioniTab = db.AmbientazioniTab.Find(id);
            if (ambientazioniTab.Privata)
            {
                ambientazioniTab.Privata = false;
            }
            else
            {
                ambientazioniTab.Privata = true;
            }
            db.Entry(ambientazioniTab).State = EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("Details", ambientazioniTab);
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
