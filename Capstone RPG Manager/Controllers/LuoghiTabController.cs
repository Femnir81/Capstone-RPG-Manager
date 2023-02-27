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
    public class LuoghiTabController : Controller
    {
        private ModelDBContext db = new ModelDBContext();

        public ActionResult Index()
        {
            var luoghiTab = db.LuoghiTab.Include(l => l.RegioniTab);
            return View(luoghiTab.ToList());
        }

        public ActionResult Details(int? id)
        {
            UtentiTab LoggedUser = db.UtentiTab.Where(x => x.Username == User.Identity.Name).FirstOrDefault();
            ViewBag.LoggedUser = LoggedUser.DM;
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            LuoghiTab luoghiTab = db.LuoghiTab.Find(id);
            if (luoghiTab.Cancellazione == true)
            {
                return HttpNotFound();
            }
            TempData["IDAreaForPointOfInterest"] = id;
            return View(luoghiTab);
        }

        public ActionResult Create()
        {
            //ViewBag.IDRegioniTab = new SelectList(db.RegioniTab, "ID", "Nome");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Exclude = "Cancellazione")] LuoghiTab luoghiTab, HttpPostedFileBase Img)
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
                    luoghiTab.Immagine = ImgCode;
                    Img.SaveAs(Server.MapPath("~/Content/Images/DB/" + luoghiTab.Immagine));
                }
                else if (Img != null && Img.ContentLength > 1000000)
                {
                    ViewBag.ImgError = "The image must be less than 1Mb";
                    return View(luoghiTab);
                }
                int IDRegionForArea = Convert.ToInt32(TempData["IDRegionForArea"]);
                luoghiTab.IDRegioniTab = IDRegionForArea;
                db.LuoghiTab.Add(luoghiTab);
                db.SaveChanges();
                return RedirectToAction("Details", "RegioniTab", new { id = IDRegionForArea });
            }
            //ViewBag.IDRegioniTab = new SelectList(db.RegioniTab, "ID", "Nome", luoghiTab.IDRegioniTab);
            return View(luoghiTab);
        }

        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            LuoghiTab luoghiTab = db.LuoghiTab.Find(id);
            if (luoghiTab.Cancellazione == true)
            {
                return HttpNotFound();
            }
            //ViewBag.IDRegioniTab = new SelectList(db.RegioniTab, "ID", "Nome", luoghiTab.IDRegioniTab);
            return View(luoghiTab);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Exclude = "Cancellazione,IDRegioniTab")] LuoghiTab luoghiTab, HttpPostedFileBase Img)
        {
            if (ModelState.IsValid)
            {
                LuoghiTab LuogoInDB = db.LuoghiTab.Find(luoghiTab.ID);
                if (Img != null && Img.ContentLength <= 1000000)
                {
                    if (LuogoInDB.Immagine != null)
                    {
                        System.IO.File.Delete(Server.MapPath("~/Content/Images/DB/" + LuogoInDB.Immagine));
                    }
                    int lastIndex = Img.FileName.LastIndexOf('.');
                    //var name = Img.FileName.Substring(0, lastIndex);
                    string ext = Img.FileName.Substring(lastIndex + 1);
                    Random random = new Random();
                    string ImgCode = DateTime.Now.Day.ToString() + DateTime.Now.Month.ToString() + DateTime.Now.Year.ToString() + DateTime.Now.Hour.ToString() + DateTime.Now.Minute.ToString() + DateTime.Now.Second.ToString() + DateTime.Now.Millisecond.ToString() + random.Next(1, 100001) + "." + ext;
                    LuogoInDB.Immagine = ImgCode;
                    Img.SaveAs(Server.MapPath("~/Content/Images/DB/" + LuogoInDB.Immagine));                   
                }
                else if (Img != null && Img.ContentLength > 1000000)
                {
                    ViewBag.ImgError = "The image must be less than 1Mb";
                    return View(luoghiTab);
                }
                LuogoInDB.Nome = luoghiTab.Nome;
                LuogoInDB.Descrizione = luoghiTab.Descrizione;
                LuogoInDB.Citta = luoghiTab.Citta;
                LuogoInDB.Privata = luoghiTab.Privata;
                db.Entry(LuogoInDB).State = EntityState.Modified;
                db.SaveChanges();
                int idRegione = db.RegioniTab.Where(x => x.ID == LuogoInDB.IDRegioniTab).FirstOrDefault().ID;
                return RedirectToAction("Details", "RegioniTab", new { id = idRegione });
            }
            //ViewBag.IDRegioniTab = new SelectList(db.RegioniTab, "ID", "Nome", luoghiTab.IDRegioniTab);
            return View(luoghiTab);
        }

        public ActionResult Delete(int id)
        {
            LuoghiTab luoghiTab = db.LuoghiTab.Find(id);
            LuoghiTab.Delete(id, db, luoghiTab);
            int idRegione = db.RegioniTab.Where(x => x.ID == luoghiTab.IDRegioniTab).FirstOrDefault().ID;
            return RedirectToAction("Details", "RegioniTab", new { id = idRegione });
        }

        public ActionResult PWAreasListByRegion(int id)
        {
            UtentiTab LoggedUser = db.UtentiTab.Where(x => x.Username == User.Identity.Name).FirstOrDefault();
            ViewBag.LoggedUser = LoggedUser.DM;
            if (LoggedUser.DM)
            {
                return PartialView("_PWAreasListByRegion", LuoghiTab.GetListAreasByRegion(id, db, LoggedUser.ID));
            }
            else
            {
                return PartialView("_PWAreasListByRegion", LuoghiTab.GetListAreasAllowedByRegion(id, LoggedUser.ID, db));
            }            
        }

        public ActionResult PWCitiesListByRegion(int id)
        {
            UtentiTab LoggedUser = db.UtentiTab.Where(x => x.Username == User.Identity.Name).FirstOrDefault();
            ViewBag.LoggedUser = LoggedUser.DM;
            if (LoggedUser.DM)
            {
                return PartialView("_PWCitiesListByRegion", LuoghiTab.GetListCitiesByRegion(id, db, LoggedUser.ID));
            }
            else
            {
                return PartialView("_PWCitiesListByRegion", LuoghiTab.GetListCitiesAllowedByRegion(id, LoggedUser.ID, db));
            }
        }

        public ActionResult PrivateOnPW(int id)
        {
            LuoghiTab luoghiTab = db.LuoghiTab.Find(id);
            LuoghiTab.GetPrivateOnPW(id, db, luoghiTab);
            RegioniTab regioniTab = db.RegioniTab.Where(x => x.ID == luoghiTab.IDRegioniTab).FirstOrDefault();
            return RedirectToAction("Details", "RegioniTab", regioniTab);
        }

        public ActionResult PrivateOnDetails(int id)
        {
            LuoghiTab luoghiTab = db.LuoghiTab.Find(id);
            LuoghiTab.GetPrivateOnPW(id, db, luoghiTab);
            return RedirectToAction("Details", luoghiTab);
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
