namespace Capstone_RPG_Manager.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity;
    using System.Data.Entity.Spatial;
    using System.Data.SqlTypes;
    using System.Linq;
    using System.Runtime.InteropServices.ComTypes;
    using System.Web;
    using System.Web.Mvc;

    [Table("AmbientazioniTab")]
    public partial class AmbientazioniTab
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public AmbientazioniTab()
        {
            RegioniTab = new HashSet<RegioniTab>();
            CampagneTab = new HashSet<CampagneTab>();
        }

        public int ID { get; set; }

        [Required]
        [Display(Name = "Setting")]
        public string Nome { get; set; }

        [Display(Name = "Description")]
        public string Descrizione { get; set; }

        [Display(Name = "Image")]
        public string Immagine { get; set; }

        [Display(Name = "Private")]
        public bool Privata { get; set; }

        public bool Cancellazione { get; set; }

        public int IDUtentiTab { get; set; }

        public virtual UtentiTab UtentiTab { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<RegioniTab> RegioniTab { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CampagneTab> CampagneTab { get; set; }

        public static List<AmbientazioniTab> GetListSettingsByDM(int id, ModelDBContext db)
        {
            List<AmbientazioniTab> ListaAmbientazioni = db.AmbientazioniTab.Where(x => x.IDUtentiTab == id && x.UtentiTab.DM == true && x.Cancellazione == false).ToList();
            return ListaAmbientazioni;
        }

        public static void Delete(int id, ModelDBContext db, AmbientazioniTab ambientazioniTab)
        {
            ambientazioniTab.Cancellazione = true;
            if (ambientazioniTab.Immagine != null)
            {
                System.IO.File.Delete(HttpContext.Current.Server.MapPath("~/Content/Images/DB/" + ambientazioniTab.Immagine));
                ambientazioniTab.Immagine = null;
            }
            List<RegioniTab> ListaRegioni = db.RegioniTab.Where(x => x.IDAmbientazioniTab == id && x.Cancellazione == false).ToList();
            if (ListaRegioni != null)
            {
                List<int> IDListaRegioni = new List<int>();
                foreach (RegioniTab item in ListaRegioni)
                {
                    item.Cancellazione = true;
                    if (item.Immagine != null)
                    {
                        System.IO.File.Delete(HttpContext.Current.Server.MapPath("~/Content/Images/DB/" + item.Immagine));
                        item.Immagine = null;
                    }
                    db.Entry(item).State = EntityState.Modified;
                    db.SaveChanges();
                    IDListaRegioni.Add(item.ID);
                }
                List<LuoghiTab> ListaLuoghi = db.LuoghiTab.Where(x => IDListaRegioni.Contains(x.IDRegioniTab) && x.Cancellazione == false).ToList();
                if (ListaLuoghi != null)
                {
                    List<int> IDListaLuoghi = new List<int>();
                    foreach (LuoghiTab item in ListaLuoghi)
                    {
                        item.Cancellazione = true;
                        if (item.Immagine != null)
                        {
                            System.IO.File.Delete(HttpContext.Current.Server.MapPath("~/Content/Images/DB/" + item.Immagine));
                            item.Immagine = null;
                        }
                        db.Entry(item).State = EntityState.Modified;
                        db.SaveChanges();
                        IDListaLuoghi.Add(item.ID);
                    }
                    List<PuntiInteresseTab> ListaPuntiInteresse = db.PuntiInteresseTab.Where(x => IDListaLuoghi.Contains(x.IDLuoghiTab) && x.Cancellazione == false).ToList();
                    if (ListaPuntiInteresse != null)
                    {
                        foreach (PuntiInteresseTab item in ListaPuntiInteresse)
                        {
                            item.Cancellazione = true;
                            if (item.Immagine != null)
                            {
                                System.IO.File.Delete(HttpContext.Current.Server.MapPath("~/Content/Images/DB/" + item.Immagine));
                                item.Immagine = null;
                            }
                            db.Entry(item).State = EntityState.Modified;
                            db.SaveChanges();
                        }
                    }
                }
            }
            db.Entry(ambientazioniTab).State = EntityState.Modified;
            db.SaveChanges();
        }

        public static void GetPrivateOnPW(int id, ModelDBContext db, AmbientazioniTab ambientazioniTab)
        {
            if (ambientazioniTab.Privata)
            {
                ambientazioniTab.Privata = false;
                List<RegioniTab> ListaRegioni = db.RegioniTab.Where(x => x.IDAmbientazioniTab == id && x.Cancellazione == false).ToList();
                if (ListaRegioni != null)
                {
                    List<int> IDListaRegioni = new List<int>();
                    foreach (RegioniTab item in ListaRegioni)
                    {
                        item.Privata = false;
                        db.Entry(item).State = EntityState.Modified;
                        db.SaveChanges();
                        IDListaRegioni.Add(item.ID);
                    }
                    List<LuoghiTab> ListaLuoghi = db.LuoghiTab.Where(x => IDListaRegioni.Contains(x.IDRegioniTab) && x.Cancellazione == false).ToList();
                    if (ListaLuoghi != null)
                    {
                        List<int> IDListaLuoghi = new List<int>();
                        foreach (LuoghiTab item in ListaLuoghi)
                        {
                            item.Privata = false;
                            db.Entry(item).State = EntityState.Modified;
                            db.SaveChanges();
                            IDListaLuoghi.Add(item.ID);
                        }
                        List<PuntiInteresseTab> ListaPuntiInteresse = db.PuntiInteresseTab.Where(x => IDListaLuoghi.Contains(x.IDLuoghiTab) && x.Cancellazione == false).ToList();
                        if (ListaPuntiInteresse != null)
                        {
                            foreach (PuntiInteresseTab item in ListaPuntiInteresse)
                            {
                                item.Privata = false;
                                db.Entry(item).State = EntityState.Modified;
                                db.SaveChanges();
                            }
                        }
                    }
                }
            }
            else
            {
                ambientazioniTab.Privata = true;
                List<RegioniTab> ListaRegioni = db.RegioniTab.Where(x => x.IDAmbientazioniTab == id && x.Cancellazione == false).ToList();
                if (ListaRegioni != null)
                {
                    List<int> IDListaRegioni = new List<int>();
                    foreach (RegioniTab item in ListaRegioni)
                    {
                        item.Privata = true;
                        db.Entry(item).State = EntityState.Modified;
                        db.SaveChanges();
                        IDListaRegioni.Add(item.ID);
                    }
                    List<LuoghiTab> ListaLuoghi = db.LuoghiTab.Where(x => IDListaRegioni.Contains(x.IDRegioniTab) && x.Cancellazione == false).ToList();
                    if (ListaLuoghi != null)
                    {
                        List<int> IDListaLuoghi = new List<int>();
                        foreach (LuoghiTab item in ListaLuoghi)
                        {
                            item.Privata = true;
                            db.Entry(item).State = EntityState.Modified;
                            db.SaveChanges();
                            IDListaLuoghi.Add(item.ID);
                        }
                        List<PuntiInteresseTab> ListaPuntiInteresse = db.PuntiInteresseTab.Where(x => IDListaLuoghi.Contains(x.IDLuoghiTab) && x.Cancellazione == false).ToList();
                        if (ListaPuntiInteresse != null)
                        {
                            foreach (PuntiInteresseTab item in ListaPuntiInteresse)
                            {
                                item.Privata = true;
                                db.Entry(item).State = EntityState.Modified;
                                db.SaveChanges();
                            }
                        }
                    }
                }
            }
            db.Entry(ambientazioniTab).State = EntityState.Modified;
            db.SaveChanges();
        }
    }
}
