namespace Capstone_RPG_Manager.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ScambioAmbientazioneCampagna : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.CampagneTab", "IDUtentiTab", "dbo.UtentiTab");
            DropIndex("dbo.AmbientazioniTab", new[] { "IDCampagneTab" });
            DropIndex("dbo.CampagneTab", new[] { "IDUtentiTab" });
            RenameColumn(table: "dbo.CampagneTab", name: "IDCampagneTab", newName: "IDAmbientazioniTab");
            AddColumn("dbo.AmbientazioniTab", "IDUtentiTab", c => c.Int(nullable: false));
            CreateIndex("dbo.AmbientazioniTab", "IDUtentiTab");
            CreateIndex("dbo.CampagneTab", "IDAmbientazioniTab");
            AddForeignKey("dbo.AmbientazioniTab", "IDUtentiTab", "dbo.UtentiTab", "ID");
            DropColumn("dbo.AmbientazioniTab", "IDCampagneTab");
            DropColumn("dbo.CampagneTab", "IDUtentiTab");
        }
        
        public override void Down()
        {
            AddColumn("dbo.CampagneTab", "IDUtentiTab", c => c.Int(nullable: false));
            AddColumn("dbo.AmbientazioniTab", "IDCampagneTab", c => c.Int(nullable: false));
            DropForeignKey("dbo.AmbientazioniTab", "IDUtentiTab", "dbo.UtentiTab");
            DropIndex("dbo.CampagneTab", new[] { "IDAmbientazioniTab" });
            DropIndex("dbo.AmbientazioniTab", new[] { "IDUtentiTab" });
            DropColumn("dbo.AmbientazioniTab", "IDUtentiTab");
            RenameColumn(table: "dbo.CampagneTab", name: "IDAmbientazioniTab", newName: "IDCampagneTab");
            CreateIndex("dbo.CampagneTab", "IDUtentiTab");
            CreateIndex("dbo.AmbientazioniTab", "IDCampagneTab");
            AddForeignKey("dbo.CampagneTab", "IDUtentiTab", "dbo.UtentiTab", "ID");
        }
    }
}
