namespace Capstone_RPG_Manager.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity;
    using System.Data.Entity.Spatial;
    using System.Linq;
    using System.Web;

    [Table("RegioniTab")]
    public partial class RegioniTab
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public RegioniTab()
        {
            LuoghiTab = new HashSet<LuoghiTab>();
        }

        public int ID { get; set; }

        [Required]
        [Display(Name = "Region")]
        public string Nome { get; set; }

        [Display(Name = "Description")]
        public string Descrizione { get; set; }

        [Display(Name = "Image")]
        public string Immagine { get; set; }

        [Display(Name = "Private")]
        public bool Privata { get; set; }

        public bool Cancellazione { get; set; }

        public int IDAmbientazioniTab { get; set; }

        public virtual AmbientazioniTab AmbientazioniTab { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<LuoghiTab> LuoghiTab { get; set; }

        public static List<RegioniTab> GetListRegionsBySetting(int id, ModelDBContext db, int idDM)
        {
            List<RegioniTab> ListaRegioni = db.RegioniTab.Where(x => x.IDAmbientazioniTab == id && x.Cancellazione == false && x.AmbientazioniTab.IDUtentiTab == idDM).ToList();
            return ListaRegioni;
        }

        public static void Delete(int id, ModelDBContext db, RegioniTab regioniTab)
        {
            regioniTab.Cancellazione = true;
            if (regioniTab.Immagine != null)
            {
                System.IO.File.Delete(HttpContext.Current.Server.MapPath("~/Content/Images/DB/" + regioniTab.Immagine));
                regioniTab.Immagine = null;
            }
            List<LuoghiTab> ListaLuoghi = db.LuoghiTab.Where(x => x.IDRegioniTab == id && x.Cancellazione == false).ToList();
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
            db.Entry(regioniTab).State = EntityState.Modified;
            db.SaveChanges();
        }

        public static void GetPrivateOnPW(int id, ModelDBContext db, RegioniTab regioniTab)
        {
            if (regioniTab.Privata)
            {
                regioniTab.Privata = false;
                List<LuoghiTab> ListaLuoghi = db.LuoghiTab.Where(x => x.IDRegioniTab == id && x.Cancellazione == false).ToList();
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
            else
            {
                regioniTab.Privata = true;
                List<LuoghiTab> ListaLuoghi = db.LuoghiTab.Where(x => x.IDRegioniTab == id && x.Cancellazione == false).ToList();
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
            db.Entry(regioniTab).State = EntityState.Modified;
            db.SaveChanges();
        }
    }
}
