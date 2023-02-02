namespace Capstone_RPG_Manager.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

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
        public string Nome { get; set; }

        public string Descrizione { get; set; }

        public string Immagine { get; set; }

        public bool Citta { get; set; }

        public bool Privata { get; set; }

        public bool Cancellazione { get; set; }

        public int IDRegioniTab { get; set; }

        public virtual RegioniTab RegioniTab { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<PuntiInteresseTab> PuntiInteresseTab { get; set; }
    }
}
