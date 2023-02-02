namespace Capstone_RPG_Manager.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("RuoliTab")]
    public partial class RuoliTab
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public RuoliTab()
        {
            UtentiTab = new HashSet<UtentiTab>();
        }

        public int ID { get; set; }

        [Required]
        [StringLength(10)]
        public string Ruolo { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<UtentiTab> UtentiTab { get; set; }
    }
}
