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

    [Table("PuntiInteresseTab")]
    public partial class PuntiInteresseTab
    {
        public int ID { get; set; }

        [Required]
        [Display(Name = "Point of Interest")]
        public string Nome { get; set; }

        [Display(Name = "Description")]
        public string Descrizione { get; set; }

        [Display(Name = "Image")]
        public string Immagine { get; set; }

        [Display(Name = "Private")]
        public bool Privata { get; set; }

        public bool Cancellazione { get; set; }

        public int IDLuoghiTab { get; set; }

        public virtual LuoghiTab LuoghiTab { get; set; }

        public static List<PuntiInteresseTab> GetListPointsOfInterestByArea(int id, ModelDBContext db, int idDM)
        {
            List<PuntiInteresseTab> ListaPuntiInteresse = db.PuntiInteresseTab.Where(x => x.IDLuoghiTab == id && x.Cancellazione == false && x.LuoghiTab.RegioniTab.AmbientazioniTab.IDUtentiTab == idDM).ToList();
            return ListaPuntiInteresse;
        }

        public static List<PuntiInteresseTab> GetListPointsOfInterestAllowedByArea(int idArea, int idLoggedUser, ModelDBContext db) 
        {
            List<int> idListaAmbientazioni = db.PermessiDMTab.Where(x => x.IDUtentiTabB == idLoggedUser && x.Permesso == true).Select(x => x.IDAmbientazioniTab).ToList();
            if (idListaAmbientazioni != null)
            {
                List<int> idListaRegioni = db.RegioniTab.Where(x => idListaAmbientazioni.Contains(x.IDAmbientazioniTab) && x.Privata == false && x.Cancellazione == false).Select(x => x.ID).ToList();
                if (idListaRegioni != null)
                {
                    LuoghiTab luoghiTab = db.LuoghiTab.Where(x => idListaRegioni.Contains(x.IDRegioniTab) && x.ID == idArea && x.Privata == false && x.Cancellazione == false).FirstOrDefault();
                    if (luoghiTab != null)
                    {
                        List<PuntiInteresseTab> ListaPuntiInteresse = db.PuntiInteresseTab.Where(x => x.IDLuoghiTab == luoghiTab.ID && x.Privata == false && x.Cancellazione == false).ToList();
                        return ListaPuntiInteresse;
                    }
                    else
                    {
                        List<PuntiInteresseTab> ListaPuntiInteresse = new List<PuntiInteresseTab>();
                        ListaPuntiInteresse = null;
                        return ListaPuntiInteresse;
                    }
                }
                else
                {
                    List<PuntiInteresseTab> ListaPuntiInteresse = new List<PuntiInteresseTab>();
                    ListaPuntiInteresse = null;
                    return ListaPuntiInteresse;
                }
            }
            else
            {
                List<PuntiInteresseTab> ListaPuntiInteresse = new List<PuntiInteresseTab>();
                ListaPuntiInteresse = null;
                return ListaPuntiInteresse;
            }

            //List<PuntiInteresseTab> ListaPuntiInteresse = db.PuntiInteresseTab.Where(x => x.IDLuoghiTab == id && x.Cancellazione == false && x.LuoghiTab.RegioniTab.AmbientazioniTab.IDUtentiTab == idDM).ToList();
            //return ListaPuntiInteresse;
        }

        public static void Delete(ModelDBContext db, PuntiInteresseTab puntiInteresseTab)
        {
            puntiInteresseTab.Cancellazione = true;
            if (puntiInteresseTab.Immagine != null)
            {
                System.IO.File.Delete(HttpContext.Current.Server.MapPath("~/Content/Images/DB/" + puntiInteresseTab.Immagine));
                puntiInteresseTab.Immagine = null;
            }
            db.Entry(puntiInteresseTab).State = EntityState.Modified;
            db.SaveChanges();
        }

        public static void GetPrivateOnPW(ModelDBContext db, PuntiInteresseTab puntiInteresseTab)
        {
            if (puntiInteresseTab.Privata)
            {
                puntiInteresseTab.Privata = false;
            }
            else
            {
                puntiInteresseTab.Privata = true;
            }
            db.Entry(puntiInteresseTab).State = EntityState.Modified;
            db.SaveChanges();
        }
    }
}
