namespace Capstone_RPG_Manager.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("PuntiInteresseTab")]
    public partial class PuntiInteresseTab
    {
        public int ID { get; set; }

        [Required]
        public string Nome { get; set; }

        public string Descrizione { get; set; }

        public string Immagine { get; set; }

        public bool Privata { get; set; }

        public bool Cancellazione { get; set; }

        public int IDLuoghiTab { get; set; }

        public virtual LuoghiTab LuoghiTab { get; set; }
    }
}
