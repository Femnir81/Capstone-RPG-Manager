using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
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
            return RedirectToAction("UserProfile", utentiTab);
        }

        //public ActionResult SearchFriends()
        //{

        //}

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
