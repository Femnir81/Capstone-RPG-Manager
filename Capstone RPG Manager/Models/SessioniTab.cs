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

    [Table("SessioniTab")]
    public partial class SessioniTab
    {
        public int ID { get; set; }

        [Required]
        [Display(Name = "Title")]
        public string Titolo { get; set; }

        [Display(Name = "Description")]
        public string Descrizione { get; set; }

        [Column(TypeName = "date")]
        [Display(Name = "Date")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime Data { get; set; }

        public bool Cancellazione { get; set; }

        public int IDCampagna { get; set; }

        public virtual CampagneTab CampagneTab { get; set; }

        public static List<SessioniTab> GetListSessionsByCampaign(int id, ModelDBContext db, int idDM)
        {
            List<SessioniTab> ListaSessioni = db.SessioniTab.Where(x => x.IDCampagna == id && x.Cancellazione == false && x.CampagneTab.AmbientazioniTab.UtentiTab.ID == idDM).OrderByDescending(x => x.Data).ToList();
            return ListaSessioni;
        }

        public static void Delete(ModelDBContext db, SessioniTab sessioniTab)
        {
            sessioniTab.Cancellazione = true;
            db.Entry(sessioniTab).State = EntityState.Modified;
            db.SaveChanges();
        }
    }
}
