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
