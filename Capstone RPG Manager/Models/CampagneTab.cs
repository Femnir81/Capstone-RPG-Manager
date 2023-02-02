namespace Capstone_RPG_Manager.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("CampagneTab")]
    public partial class CampagneTab
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public CampagneTab()
        {
            PermessiDMTab = new HashSet<PermessiDMTab>();
            SessioniTab = new HashSet<SessioniTab>();
        }

        public int ID { get; set; }

        [Required]
        public string Nome { get; set; }

        public string Descrizione { get; set; }

        [Column(TypeName = "date")]
        public DateTime? Data { get; set; }

        public bool Cancellazione { get; set; }

        public int IDAmbientazioniTab { get; set; }

        public virtual AmbientazioniTab AmbientazioniTab { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<PermessiDMTab> PermessiDMTab { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<SessioniTab> SessioniTab { get; set; }
    }
}
