namespace Capstone_RPG_Manager.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("PermessiDMTab")]
    public partial class PermessiDMTab
    {
        public int ID { get; set; }

        public bool Permesso { get; set; }

        public bool Cancellazione { get; set; }

        public int IDUtentiTabA { get; set; }

        public int IDUtentiTabB { get; set; }

        public int IDAmbientazioniTab { get; set; }

        public virtual AmbientazioniTab AmbientazioniTab { get; set; }

        public virtual UtentiTab UtentiTab { get; set; }

        public virtual UtentiTab UtentiTab1 { get; set; }
    }
}
