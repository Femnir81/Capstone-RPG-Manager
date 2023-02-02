namespace Capstone_RPG_Manager.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

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
        public string Nome { get; set; }

        public string Descrizione { get; set; }

        public string Immagine { get; set; }

        public bool Privata { get; set; }

        public bool Cancellazione { get; set; }

        public int IDAmbientazioniTab { get; set; }

        public virtual AmbientazioniTab AmbientazioniTab { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<LuoghiTab> LuoghiTab { get; set; }
    }
}
