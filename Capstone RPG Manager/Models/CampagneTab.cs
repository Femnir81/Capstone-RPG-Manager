namespace Capstone_RPG_Manager.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;
    using System.Linq;

    [Table("CampagneTab")]
    public partial class CampagneTab
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public CampagneTab()
        {
            SessioniTab = new HashSet<SessioniTab>();
        }

        public int ID { get; set; }

        [Required]
        [Display(Name = "Campaign")]
        public string Nome { get; set; }

        [Display(Name = "Description")]
        public string Descrizione { get; set; }

        [Column(TypeName = "date")]
        [Display(Name = "Date")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime? Data { get; set; }

        public bool Cancellazione { get; set; }

        [Display(Name = "Setting")]
        public int IDAmbientazioniTab { get; set; }

        public virtual AmbientazioniTab AmbientazioniTab { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<SessioniTab> SessioniTab { get; set; }

        public static List<CampagneTab> GetListCampaignsByDM(int id, ModelDBContext db)
        {
            List<int> IdListaAmbientazioni = db.AmbientazioniTab.Where(x => x.IDUtentiTab == id && x.UtentiTab.DM == true && x.Cancellazione == false).Select(x => x.ID).ToList();
            List<CampagneTab> ListaCampagne = db.CampagneTab.Where(x => IdListaAmbientazioni.Contains(x.IDAmbientazioniTab) && x.Cancellazione == false).ToList();
            return ListaCampagne;
        }

        public static List<CampagneTab> GetListCampaignsAllowedByDM(int id, ModelDBContext db)
        {
            List<int> idListaAmbientazioni = new List<int>();
            List<AmbientazioniTab> ListaAmbientazioni = AmbientazioniTab.GetListSettingsAllowedByDM(id, db);
            if (ListaAmbientazioni != null)
            {
                foreach (AmbientazioniTab item in ListaAmbientazioni)
                {
                    idListaAmbientazioni.Add(item.ID);
                }
                List<CampagneTab> ListaCampagne = db.CampagneTab.Where( x => idListaAmbientazioni.Contains(x.IDAmbientazioniTab) && x.Cancellazione == false).ToList();
                return ListaCampagne;
            }
            else
            {
                List<CampagneTab> ListaCampagne = new List<CampagneTab>();
                ListaCampagne = null;
                return ListaCampagne;
            }
        }

        public static List<CampagneTab> GetListCampaignsBySetting(int id, int idLoggedUser, ModelDBContext db)
        {
            List<CampagneTab> ListaCampagne = db.CampagneTab.Where(x => x.IDAmbientazioniTab == id && x.AmbientazioniTab.UtentiTab.ID == idLoggedUser && x.Cancellazione == false).ToList();
            return ListaCampagne;
        }

        public static List<CampagneTab> GetListCampaignsAllowedBySetting(int id, int idLoggedUser, ModelDBContext db)
        {
            AmbientazioniTab ambientazioniTab = AmbientazioniTab.GetListSettingsAllowedByDM(idLoggedUser, db).Where(x => x.ID == id).FirstOrDefault();
            if (ambientazioniTab != null)
            {
                List<CampagneTab> ListaCampagne = db.CampagneTab.Where(x => x.IDAmbientazioniTab == ambientazioniTab.ID && x.Cancellazione == false).ToList();
                return ListaCampagne;
            }
            else
            {
                List<CampagneTab> ListaCampagne = new List<CampagneTab>();
                ListaCampagne = null;
                return ListaCampagne;
            }
        }


    }
}
