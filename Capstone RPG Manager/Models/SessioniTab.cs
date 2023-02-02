namespace Capstone_RPG_Manager.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("SessioniTab")]
    public partial class SessioniTab
    {
        public int ID { get; set; }

        public string Descrizione { get; set; }

        [Column(TypeName = "date")]
        public DateTime Data { get; set; }

        public bool Cancellazione { get; set; }

        public int IDCampagna { get; set; }

        public virtual CampagneTab CampagneTab { get; set; }
    }
}
