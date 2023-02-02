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

        public int IDUtentiTab { get; set; }

        public int IDCampagnaTab { get; set; }

        public virtual CampagneTab CampagneTab { get; set; }

        public virtual UtentiTab UtentiTab { get; set; }
    }
}
