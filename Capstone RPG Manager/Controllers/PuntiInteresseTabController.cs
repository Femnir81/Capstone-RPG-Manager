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
    public class PuntiInteresseTabController : Controller
    {
        private ModelDBContext db = new ModelDBContext();

        public ActionResult Index()
        {
            var puntiInteresseTab = db.PuntiInteresseTab.Include(p => p.LuoghiTab);
            return View(puntiInteresseTab.ToList());
        }

        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PuntiInteresseTab puntiInteresseTab = db.PuntiInteresseTab.Find(id);
            if (puntiInteresseTab.Cancellazione == true)
            {
                return HttpNotFound();
            }
            return View(puntiInteresseTab);
        }

        public ActionResult Create()
        {
            //ViewBag.IDLuoghiTab = new SelectList(db.LuoghiTab, "ID", "Nome");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Exclude = "Cancellazione, IDLuoghiTab")] PuntiInteresseTab puntiInteresseTab, HttpPostedFileBase Img)
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
                    puntiInteresseTab.Immagine = ImgCode;
                    Img.SaveAs(Server.MapPath("~/Content/Images/DB/" + puntiInteresseTab.Immagine));
                }
                else if (Img != null && Img.ContentLength > 1000000)
                {
                    ViewBag.ImgError = "The image must be less than 1Mb";
                    return View(puntiInteresseTab);
                }
                int IDAreaForPointOfInterest = Convert.ToInt32(TempData["IDAreaForPointOfInterest"]);
                puntiInteresseTab.IDLuoghiTab = IDAreaForPointOfInterest;
                db.PuntiInteresseTab.Add(puntiInteresseTab);
                db.SaveChanges();
                return RedirectToAction("Details", "LuoghiTab", new { id = IDAreaForPointOfInterest });
            }
            //ViewBag.IDLuoghiTab = new SelectList(db.LuoghiTab, "ID", "Nome", puntiInteresseTab.IDLuoghiTab);
            return View(puntiInteresseTab);
        }

        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PuntiInteresseTab puntiInteresseTab = db.PuntiInteresseTab.Find(id);
            if (puntiInteresseTab.Cancellazione == true)
            {
                return HttpNotFound();
            }
            //ViewBag.IDLuoghiTab = new SelectList(db.LuoghiTab, "ID", "Nome", puntiInteresseTab.IDLuoghiTab);
            return View(puntiInteresseTab);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Exclude = "Cancellazione,IDLuoghiTab")] PuntiInteresseTab puntiInteresseTab, HttpPostedFileBase Img)
        {
            if (ModelState.IsValid)
            {
                PuntiInteresseTab puntiInteresseInDB = db.PuntiInteresseTab.Find(puntiInteresseTab.ID);
                if (Img != null && Img.ContentLength <= 1000000)
                {
                    if (puntiInteresseInDB.Immagine != null)
                    {
                        System.IO.File.Delete(Server.MapPath("~/Content/Images/DB/" + puntiInteresseInDB.Immagine));
                    }
                    int lastIndex = Img.FileName.LastIndexOf('.');
                    //var name = Img.FileName.Substring(0, lastIndex);
                    string ext = Img.FileName.Substring(lastIndex + 1);
                    Random random = new Random();
                    string ImgCode = DateTime.Now.Day.ToString() + DateTime.Now.Month.ToString() + DateTime.Now.Year.ToString() + DateTime.Now.Hour.ToString() + DateTime.Now.Minute.ToString() + DateTime.Now.Second.ToString() + DateTime.Now.Millisecond.ToString() + random.Next(1, 100001) + "." + ext;
                    puntiInteresseInDB.Immagine = ImgCode;
                    Img.SaveAs(Server.MapPath("~/Content/Images/DB/" + puntiInteresseInDB.Immagine));
                }
                else if (Img != null && Img.ContentLength > 1000000)
                {
                    ViewBag.ImgError = "The image must be less than 1Mb";
                    return View(puntiInteresseTab);
                }
                puntiInteresseInDB.Nome = puntiInteresseTab.Nome;
                puntiInteresseInDB.Descrizione = puntiInteresseTab.Descrizione;
                puntiInteresseInDB.Privata = puntiInteresseTab.Privata;
                db.Entry(puntiInteresseInDB).State = EntityState.Modified;
                db.SaveChanges();
                int idLuogo = db.LuoghiTab.Where(x => x.ID == puntiInteresseInDB.IDLuoghiTab).FirstOrDefault().ID;
                return RedirectToAction("Details", "LuoghiTab", new { id = idLuogo });
            }
            //ViewBag.IDLuoghiTab = new SelectList(db.LuoghiTab, "ID", "Nome", puntiInteresseTab.IDLuoghiTab);
            return View(puntiInteresseTab);
        }

        public ActionResult Delete(int id)
        {
            PuntiInteresseTab puntiInteresseTab = db.PuntiInteresseTab.Find(id);
            PuntiInteresseTab.Delete(db, puntiInteresseTab);
            int idLuogo = db.LuoghiTab.Where(x => x.ID == puntiInteresseTab.IDLuoghiTab).FirstOrDefault().ID;
            return RedirectToAction("Details", "RegioniTab", new { id = idLuogo });
        }

        public ActionResult PWPointsOfInterestListByArea(int id)
        {
            int idDM = db.UtentiTab.Where(x => x.Username == User.Identity.Name).FirstOrDefault().ID;
            return PartialView("_PWPointsOfInterestListByArea", PuntiInteresseTab.GetListPointsOfInterestByArea(id, db, idDM));
        }

        public ActionResult PrivateOnPW(int id)
        {
            PuntiInteresseTab puntiInteresseTab = db.PuntiInteresseTab.Find(id);
            PuntiInteresseTab.GetPrivateOnPW(db, puntiInteresseTab);
            LuoghiTab luoghiTab = db.LuoghiTab.Where(x => x.ID == puntiInteresseTab.IDLuoghiTab).FirstOrDefault();
            return RedirectToAction("Details", "LuoghiTab", luoghiTab);
        }

        public ActionResult PrivateOnDetails(int id)
        {
            PuntiInteresseTab puntiInteresseTab = db.PuntiInteresseTab.Find(id);
            PuntiInteresseTab.GetPrivateOnPW(db, puntiInteresseTab);
            return RedirectToAction("Details", puntiInteresseTab);
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
