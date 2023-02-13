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
    public class RegioniTabController : Controller
    {
        private ModelDBContext db = new ModelDBContext();

        public ActionResult Index()
        {
            var regioniTab = db.RegioniTab.Include(r => r.AmbientazioniTab);
            return View(regioniTab.ToList());
        }

        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            RegioniTab regioniTab = db.RegioniTab.Find(id);
            if (regioniTab.Cancellazione == true)
            {
                return HttpNotFound();
            }
            TempData["IDRegionForArea"] = id;
            return View(regioniTab);
        }

        public ActionResult Create()
        {
            //ViewBag.IDAmbientazioniTab = new SelectList(db.AmbientazioniTab, "ID", "Nome");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Exclude = "Cancellazione")] RegioniTab regioniTab, HttpPostedFileBase Img)
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
                    regioniTab.Immagine = ImgCode;
                    Img.SaveAs(Server.MapPath("~/Content/Images/DB/" + regioniTab.Immagine));
                }
                else if (Img != null && Img.ContentLength > 1000000)
                {
                    ViewBag.ImgError = "The image must be less than 1Mb";
                    return View(regioniTab);
                }
                int IDSettingForRegion = Convert.ToInt32(TempData["IDSettingForRegion"]);
                regioniTab.IDAmbientazioniTab = IDSettingForRegion;
                db.RegioniTab.Add(regioniTab);
                db.SaveChanges();
                return RedirectToAction("Details", "AmbientazioniTab", new { id = IDSettingForRegion});
            }
            //ViewBag.IDAmbientazioniTab = new SelectList(db.AmbientazioniTab, "ID", "Nome", regioniTab.IDAmbientazioniTab);
            return View(regioniTab);
        }

        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            RegioniTab regioniTab = db.RegioniTab.Find(id);
            if (regioniTab.Cancellazione == true)
            {
                return HttpNotFound();
            }
            //ViewBag.IDAmbientazioniTab = new SelectList(db.AmbientazioniTab, "ID", "Nome", regioniTab.IDAmbientazioniTab);
            return View(regioniTab);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Exclude = "Cancellazione,IDAmbientazioniTab")] RegioniTab regioniTab, HttpPostedFileBase Img)
        {
            if (ModelState.IsValid)
            {
                RegioniTab RegioneInDB = db.RegioniTab.Find(regioniTab.ID);
                if (Img != null && Img.ContentLength <= 1000000)
                {
                    if (RegioneInDB.Immagine != null)
                    {
                        System.IO.File.Delete(Server.MapPath("~/Content/Images/DB/" + RegioneInDB.Immagine));
                    }
                    int lastIndex = Img.FileName.LastIndexOf('.');
                    //var name = Img.FileName.Substring(0, lastIndex);
                    string ext = Img.FileName.Substring(lastIndex + 1);
                    Random random = new Random();
                    string ImgCode = DateTime.Now.Day.ToString() + DateTime.Now.Month.ToString() + DateTime.Now.Year.ToString() + DateTime.Now.Hour.ToString() + DateTime.Now.Minute.ToString() + DateTime.Now.Second.ToString() + DateTime.Now.Millisecond.ToString() + random.Next(1, 100001) + "." + ext;
                    RegioneInDB.Immagine = ImgCode;
                    Img.SaveAs(Server.MapPath("~/Content/Images/DB/" + RegioneInDB.Immagine));
                }
                else if (Img != null && Img.ContentLength > 1000000)
                {
                    ViewBag.ImgError = "The image must be less than 1Mb";
                    return View(regioniTab);
                }
                RegioneInDB.Nome = regioniTab.Nome;
                RegioneInDB.Descrizione = regioniTab.Descrizione;
                RegioneInDB.Privata = regioniTab.Privata;
                db.Entry(RegioneInDB).State = EntityState.Modified;
                db.SaveChanges();
                int idAmbientazione = db.AmbientazioniTab.Where(x => x.ID == RegioneInDB.IDAmbientazioniTab).FirstOrDefault().ID;
                return RedirectToAction("Details", "AmbientazioniTab", new { id = idAmbientazione });
            }
            //ViewBag.IDAmbientazioniTab = new SelectList(db.AmbientazioniTab, "ID", "Nome", regioniTab.IDAmbientazioniTab);
            return View(regioniTab);
        }

        public ActionResult Delete(int id)
        {
            RegioniTab regioniTab = db.RegioniTab.Find(id);
            RegioniTab.Delete(id, db, regioniTab);
            int idAmbientazione = db.AmbientazioniTab.Where(x => x.ID == regioniTab.IDAmbientazioniTab).FirstOrDefault().ID;
            return RedirectToAction("Details", "AmbientazioniTab", new { id = idAmbientazione });
        }

        public ActionResult PWRegionsListBySetting(int id)
        {
            int idDM = db.UtentiTab.Where(x => x.Username == User.Identity.Name).FirstOrDefault().ID;
            return PartialView("_PWRegionsListBySetting", RegioniTab.GetListRegionsBySetting(id, db, idDM));
        }

        public ActionResult PrivateOnPW(int id)
        {
            RegioniTab regioniTab = db.RegioniTab.Find(id);
            RegioniTab.GetPrivateOnPW(id, db, regioniTab);
            AmbientazioniTab ambientazioniTab = db.AmbientazioniTab.Where(x => x.ID == regioniTab.IDAmbientazioniTab).FirstOrDefault();
            return RedirectToAction("Details", "AmbientazioniTab", ambientazioniTab);
        }

        public ActionResult PrivateOnDetails(int id)
        {
            RegioniTab regioniTab = db.RegioniTab.Find(id);
            RegioniTab.GetPrivateOnPW(id, db, regioniTab);
            return RedirectToAction("Details", regioniTab);
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
