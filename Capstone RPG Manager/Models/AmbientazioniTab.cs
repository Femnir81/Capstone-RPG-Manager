namespace Capstone_RPG_Manager.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;
    using System.Linq;

    [Table("AmbientazioniTab")]
    public partial class AmbientazioniTab
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public AmbientazioniTab()
        {
            RegioniTab = new HashSet<RegioniTab>();
        }

        public int ID { get; set; }

        [Required]
        public string Nome { get; set; }

        public string Descrizione { get; set; }

        public string Immagine { get; set; }

        public bool Privata { get; set; }

        public bool Cancellazione { get; set; }

        public int IDCampagneTab { get; set; }

        public virtual CampagneTab CampagneTab { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<RegioniTab> RegioniTab { get; set; }

        //public static List<AmbientazioniTab> GetListSettingsByDM(int id, ModelDBContext db)
        //{
        //    List<AmbientazioniTab> ListaAmbientazioni = db.AmbientazioniTab.Where(x=> x.i == id).ToList();
        //}
    }
}
