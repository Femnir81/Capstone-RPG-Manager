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

    [Table("LuoghiTab")]
    public partial class LuoghiTab
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public LuoghiTab()
        {
            PuntiInteresseTab = new HashSet<PuntiInteresseTab>();
        }

        public int ID { get; set; }

        [Required]
        [Display(Name = "Area")]
        public string Nome { get; set; }

        [Display(Name = "Description")]
        public string Descrizione { get; set; }

        [Display(Name = "Image")]
        public string Immagine { get; set; }

        public bool Citta { get; set; }

        [Display(Name = "Private")]
        public bool Privata { get; set; }

        public bool Cancellazione { get; set; }

        public int IDRegioniTab { get; set; }

        public virtual RegioniTab RegioniTab { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<PuntiInteresseTab> PuntiInteresseTab { get; set; }

        public static List<LuoghiTab> GetListAreasByRegion(int id, ModelDBContext db, int idDM)
        {
            List<LuoghiTab> ListaLuoghi = db.LuoghiTab.Where(x => x.IDRegioniTab == id && x.Citta == false && x.Cancellazione == false && x.RegioniTab.AmbientazioniTab.IDUtentiTab == idDM).ToList();
            return ListaLuoghi;
        }

        public static List<LuoghiTab> GetListCitiesByRegion(int id, ModelDBContext db, int idDM)
        {
            List<LuoghiTab> ListaLuoghi = db.LuoghiTab.Where(x => x.IDRegioniTab == id && x.Citta == true && x.Cancellazione == false && x.RegioniTab.AmbientazioniTab.IDUtentiTab == idDM).ToList();
            return ListaLuoghi;
        }

        public static void Delete(int id, ModelDBContext db, LuoghiTab luoghiTab)
        {
            luoghiTab.Cancellazione = true;
            if (luoghiTab.Immagine != null)
            {
                System.IO.File.Delete(HttpContext.Current.Server.MapPath("~/Content/Images/DB/" + luoghiTab.Immagine));
                luoghiTab.Immagine = null;
            }
            List<PuntiInteresseTab> ListaPuntiInteresse = db.PuntiInteresseTab.Where(x => x.IDLuoghiTab == id && x.Cancellazione == false).ToList();
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
            db.Entry(luoghiTab).State = EntityState.Modified;
            db.SaveChanges();
        }

        public static void GetPrivateOnPW(int id, ModelDBContext db, LuoghiTab luoghiTab)
        {
            if (luoghiTab.Privata)
            {
                luoghiTab.Privata = false;
                List<PuntiInteresseTab> ListaPuntiInteresse = db.PuntiInteresseTab.Where(x => x.IDLuoghiTab == id && x.Cancellazione == false).ToList();
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
            else
            {
                luoghiTab.Privata = true;
                List<PuntiInteresseTab> ListaPuntiInteresse = db.PuntiInteresseTab.Where(x => x.IDLuoghiTab == id && x.Cancellazione == false).ToList();
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
            db.Entry(luoghiTab).State = EntityState.Modified;
            db.SaveChanges();
        }
    }
}
