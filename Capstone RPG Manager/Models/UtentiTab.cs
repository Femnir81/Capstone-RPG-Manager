namespace Capstone_RPG_Manager.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;
    using System.Linq;
    using System.Security.Policy;

    [Table("UtentiTab")]
    public partial class UtentiTab
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public UtentiTab()
        {
            AmicizieTab = new HashSet<AmicizieTab>();
            AmicizieTab1 = new HashSet<AmicizieTab>();
            CampagneTab = new HashSet<CampagneTab>();
            PermessiDMTab = new HashSet<PermessiDMTab>();
        }

        public int ID { get; set; }

        [Required(ErrorMessage = "Username is required")]
        public string Username { get; set; }

        [Required(ErrorMessage = "Password is required")]
        public string Password { get; set; }

        public bool DM { get; set; }

        [Required(ErrorMessage = "Email is required")]
        public string Email { get; set; }

        public int IDRuoliTab { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<AmicizieTab> AmicizieTab { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<AmicizieTab> AmicizieTab1 { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CampagneTab> CampagneTab { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<PermessiDMTab> PermessiDMTab { get; set; }

        public virtual RuoliTab RuoliTab { get; set; }

        public static bool IsLogged(UtentiTab utente, ModelDBContext db)
        {
            if (db.UtentiTab.Where(x => x.Username == utente.Username && x.Password == utente.Password).Count() == 1)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public static bool IsRegistered (UtentiTab utente, ModelDBContext db)
        {
            if (db.UtentiTab.Where(x => x.Username == utente.Username).Count() == 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
