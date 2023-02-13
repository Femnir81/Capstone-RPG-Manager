using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Linq;

namespace Capstone_RPG_Manager.Models
{
    public partial class ModelDBContext : DbContext
    {
        public ModelDBContext()
            : base("name=ModelDBContext")
        {
        }

        public virtual DbSet<AmbientazioniTab> AmbientazioniTab { get; set; }
        public virtual DbSet<AmicizieTab> AmicizieTab { get; set; }
        public virtual DbSet<CampagneTab> CampagneTab { get; set; }
        public virtual DbSet<LuoghiTab> LuoghiTab { get; set; }
        public virtual DbSet<PermessiDMTab> PermessiDMTab { get; set; }
        public virtual DbSet<PuntiInteresseTab> PuntiInteresseTab { get; set; }
        public virtual DbSet<RegioniTab> RegioniTab { get; set; }
        public virtual DbSet<RuoliTab> RuoliTab { get; set; }
        public virtual DbSet<SessioniTab> SessioniTab { get; set; }
        public virtual DbSet<UtentiTab> UtentiTab { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<AmbientazioniTab>()
                .HasMany(e => e.RegioniTab)
                .WithRequired(e => e.AmbientazioniTab)
                .HasForeignKey(e => e.IDAmbientazioniTab)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<AmbientazioniTab>()
                .HasMany(e => e.CampagneTab)
                .WithRequired(e => e.AmbientazioniTab)
                .HasForeignKey(e => e.IDAmbientazioniTab)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<AmbientazioniTab>()
                .HasMany(e => e.PermessiDMTab)
                .WithRequired(e => e.AmbientazioniTab)
                .HasForeignKey(e => e.IDAmbientazioniTab)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<CampagneTab>()
                .HasMany(e => e.SessioniTab)
                .WithRequired(e => e.CampagneTab)
                .HasForeignKey(e => e.IDCampagna)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<LuoghiTab>()
                .HasMany(e => e.PuntiInteresseTab)
                .WithRequired(e => e.LuoghiTab)
                .HasForeignKey(e => e.IDLuoghiTab)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<RegioniTab>()
                .HasMany(e => e.LuoghiTab)
                .WithRequired(e => e.RegioniTab)
                .HasForeignKey(e => e.IDRegioniTab)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<RuoliTab>()
                .Property(e => e.Ruolo)
                .IsFixedLength();

            modelBuilder.Entity<RuoliTab>()
                .HasMany(e => e.UtentiTab)
                .WithRequired(e => e.RuoliTab)
                .HasForeignKey(e => e.IDRuoliTab)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<UtentiTab>()
                .HasMany(e => e.AmicizieTab)
                .WithRequired(e => e.UtentiTab)
                .HasForeignKey(e => e.IDUtentiTabA)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<UtentiTab>()
                .HasMany(e => e.AmicizieTab1)
                .WithRequired(e => e.UtentiTab1)
                .HasForeignKey(e => e.IDUtentiTabB)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<UtentiTab>()
                .HasMany(e => e.AmbientazioniTab)
                .WithRequired(e => e.UtentiTab)
                .HasForeignKey(e => e.IDUtentiTab)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<UtentiTab>()
                .HasMany(e => e.PermessiDMTab)
                .WithRequired(e => e.UtentiTab)
                .HasForeignKey(e => e.IDUtentiTabA)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<UtentiTab>()
                .HasMany(e => e.PermessiDMTab1)
                .WithRequired(e => e.UtentiTab1)
                .HasForeignKey(e => e.IDUtentiTabB)
                .WillCascadeOnDelete(false);
        }
    }
}
