namespace Capstone_RPG_Manager.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity;
    using System.Data.Entity.Spatial;
    using System.Drawing;
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

        public static List<LuoghiTab> GetListAreasAllowedByRegion(int idRegion, int idLoggedUser, ModelDBContext db)
        {
            List<int> idListaAmbientazioni = db.PermessiDMTab.Where(x => x.IDUtentiTabB == idLoggedUser && x.Permesso == true).Select(x => x.IDAmbientazioniTab).ToList();
            if (idListaAmbientazioni != null)
            {
                RegioniTab regioniTab = db.RegioniTab.Where(x => idListaAmbientazioni.Contains(x.IDAmbientazioniTab) && x.ID == idRegion && x.Privata == false && x.Cancellazione == false).FirstOrDefault();
                if ( regioniTab != null)
                {
                    List<LuoghiTab> ListaLuoghi = db.LuoghiTab.Where(x => x.IDRegioniTab == regioniTab.ID && x.Citta == false && x.Privata == false && x.Cancellazione == false).ToList();
                    return ListaLuoghi;
                }
                else
                {
                    List<LuoghiTab> ListaLuoghi = new List<LuoghiTab>();
                    ListaLuoghi = null;
                    return ListaLuoghi;
                }
            }
            else
            {
                List<LuoghiTab> ListaLuoghi = new List<LuoghiTab>();
                ListaLuoghi = null;
                return ListaLuoghi;
            }

            //RegioniTab regioniTab = RegioniTab.GetListRegionsAllowedByCampaign(id, idLoggedUser, db).Where(x => x.ID == id).FirstOrDefault();
            //if (regioniTab != null)
            //{
            //    List<LuoghiTab> ListaLuoghi = db.LuoghiTab.Where(x => x.IDRegioniTab == regioniTab.ID && x.Citta == false && x.Privata == false && x.Cancellazione == false).ToList();
            //    return ListaLuoghi;
            //}
            //else
            //{
            //    List<LuoghiTab> ListaLuoghi = new List<LuoghiTab>();
            //    ListaLuoghi = null;
            //    return ListaLuoghi;
            //}
        }

        public static List<LuoghiTab> GetListCitiesByRegion(int id, ModelDBContext db, int idDM)
        {
            List<LuoghiTab> ListaLuoghi = db.LuoghiTab.Where(x => x.IDRegioniTab == id && x.Citta == true && x.Cancellazione == false && x.RegioniTab.AmbientazioniTab.IDUtentiTab == idDM).ToList();
            return ListaLuoghi;
        }

        public static List<LuoghiTab> GetListCitiesAllowedByRegion(int idRegion, int idLoggedUser, ModelDBContext db)
        {
            List<int> idListaAmbientazioni = db.PermessiDMTab.Where(x => x.IDUtentiTabB == idLoggedUser && x.Permesso == true).Select(x => x.IDAmbientazioniTab).ToList();
            if (idListaAmbientazioni != null)
            {
                RegioniTab regioniTab = db.RegioniTab.Where(x => idListaAmbientazioni.Contains(x.IDAmbientazioniTab) && x.ID == idRegion && x.Privata == false && x.Cancellazione == false).FirstOrDefault();
                if (regioniTab != null)
                {
                    List<LuoghiTab> ListaLuoghi = db.LuoghiTab.Where(x => x.IDRegioniTab == regioniTab.ID && x.Citta == true && x.Privata == false && x.Cancellazione == false).ToList();
                    return ListaLuoghi;
                }
                else
                {
                    List<LuoghiTab> ListaLuoghi = new List<LuoghiTab>();
                    ListaLuoghi = null;
                    return ListaLuoghi;
                }
            }
            else
            {
                List<LuoghiTab> ListaLuoghi = new List<LuoghiTab>();
                ListaLuoghi = null;
                return ListaLuoghi;
            }
            //RegioniTab regioniTab = RegioniTab.GetListRegionsAllowedByCampaign(id, idLoggedUser, db).Where(x => x.ID == id).FirstOrDefault();
            //if (regioniTab != null)
            //{
            //    List<LuoghiTab> ListaLuoghi = db.LuoghiTab.Where(x => x.IDRegioniTab == regioniTab.ID && x.Citta == true && x.Privata == false && x.Cancellazione == false).ToList();
            //    return ListaLuoghi;
            //}
            //else
            //{
            //    List<LuoghiTab> ListaLuoghi = new List<LuoghiTab>();
            //    ListaLuoghi = null;
            //    return ListaLuoghi;
            //}
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
