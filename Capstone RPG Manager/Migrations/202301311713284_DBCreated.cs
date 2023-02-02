namespace Capstone_RPG_Manager.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DBCreated : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.AmbientazioniTab",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Nome = c.String(nullable: false),
                        Descrizione = c.String(),
                        Immagine = c.String(),
                        Privata = c.Boolean(nullable: false),
                        Cancellazione = c.Boolean(nullable: false),
                        IDCampagneTab = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.CampagneTab", t => t.IDCampagneTab)
                .Index(t => t.IDCampagneTab);
            
            CreateTable(
                "dbo.CampagneTab",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Nome = c.String(nullable: false),
                        Descrizione = c.String(),
                        Data = c.DateTime(storeType: "date"),
                        Cancellazione = c.Boolean(nullable: false),
                        IDUtentiTab = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.UtentiTab", t => t.IDUtentiTab)
                .Index(t => t.IDUtentiTab);
            
            CreateTable(
                "dbo.PermessiDMTab",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Permesso = c.Boolean(nullable: false),
                        Cancellazione = c.Boolean(nullable: false),
                        IDUtentiTab = c.Int(nullable: false),
                        IDCampagnaTab = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.UtentiTab", t => t.IDUtentiTab)
                .ForeignKey("dbo.CampagneTab", t => t.IDCampagnaTab)
                .Index(t => t.IDUtentiTab)
                .Index(t => t.IDCampagnaTab);
            
            CreateTable(
                "dbo.UtentiTab",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Username = c.String(nullable: false),
                        Password = c.String(nullable: false),
                        DM = c.Boolean(nullable: false),
                        Email = c.String(nullable: false),
                        IDRuoliTab = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.RuoliTab", t => t.IDRuoliTab)
                .Index(t => t.IDRuoliTab);
            
            CreateTable(
                "dbo.AmicizieTab",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Cancellazione = c.Boolean(nullable: false),
                        IDUtentiTabA = c.Int(nullable: false),
                        IDUtentiTabB = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.UtentiTab", t => t.IDUtentiTabA)
                .ForeignKey("dbo.UtentiTab", t => t.IDUtentiTabB)
                .Index(t => t.IDUtentiTabA)
                .Index(t => t.IDUtentiTabB);
            
            CreateTable(
                "dbo.RuoliTab",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Ruolo = c.String(nullable: false, maxLength: 10, fixedLength: true),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.SessioniTab",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Descrizione = c.String(),
                        Data = c.DateTime(nullable: false, storeType: "date"),
                        Cancellazione = c.Boolean(nullable: false),
                        IDCampagna = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.CampagneTab", t => t.IDCampagna)
                .Index(t => t.IDCampagna);
            
            CreateTable(
                "dbo.RegioniTab",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Nome = c.String(nullable: false),
                        Descrizione = c.String(),
                        Immagine = c.String(),
                        Privata = c.Boolean(nullable: false),
                        Cancellazione = c.Boolean(nullable: false),
                        IDAmbientazioniTab = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.AmbientazioniTab", t => t.IDAmbientazioniTab)
                .Index(t => t.IDAmbientazioniTab);
            
            CreateTable(
                "dbo.LuoghiTab",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Nome = c.String(nullable: false),
                        Descrizione = c.String(),
                        Immagine = c.String(),
                        Citta = c.Boolean(nullable: false),
                        Privata = c.Boolean(nullable: false),
                        Cancellazione = c.Boolean(nullable: false),
                        IDRegioniTab = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.RegioniTab", t => t.IDRegioniTab)
                .Index(t => t.IDRegioniTab);
            
            CreateTable(
                "dbo.PuntiInteresseTab",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Nome = c.String(nullable: false),
                        Descrizione = c.String(),
                        Immagine = c.String(),
                        Privata = c.Boolean(nullable: false),
                        Cancellazione = c.Boolean(nullable: false),
                        IDLuoghiTab = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.LuoghiTab", t => t.IDLuoghiTab)
                .Index(t => t.IDLuoghiTab);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.RegioniTab", "IDAmbientazioniTab", "dbo.AmbientazioniTab");
            DropForeignKey("dbo.LuoghiTab", "IDRegioniTab", "dbo.RegioniTab");
            DropForeignKey("dbo.PuntiInteresseTab", "IDLuoghiTab", "dbo.LuoghiTab");
            DropForeignKey("dbo.SessioniTab", "IDCampagna", "dbo.CampagneTab");
            DropForeignKey("dbo.PermessiDMTab", "IDCampagnaTab", "dbo.CampagneTab");
            DropForeignKey("dbo.UtentiTab", "IDRuoliTab", "dbo.RuoliTab");
            DropForeignKey("dbo.PermessiDMTab", "IDUtentiTab", "dbo.UtentiTab");
            DropForeignKey("dbo.CampagneTab", "IDUtentiTab", "dbo.UtentiTab");
            DropForeignKey("dbo.AmicizieTab", "IDUtentiTabB", "dbo.UtentiTab");
            DropForeignKey("dbo.AmicizieTab", "IDUtentiTabA", "dbo.UtentiTab");
            DropForeignKey("dbo.AmbientazioniTab", "IDCampagneTab", "dbo.CampagneTab");
            DropIndex("dbo.PuntiInteresseTab", new[] { "IDLuoghiTab" });
            DropIndex("dbo.LuoghiTab", new[] { "IDRegioniTab" });
            DropIndex("dbo.RegioniTab", new[] { "IDAmbientazioniTab" });
            DropIndex("dbo.SessioniTab", new[] { "IDCampagna" });
            DropIndex("dbo.AmicizieTab", new[] { "IDUtentiTabB" });
            DropIndex("dbo.AmicizieTab", new[] { "IDUtentiTabA" });
            DropIndex("dbo.UtentiTab", new[] { "IDRuoliTab" });
            DropIndex("dbo.PermessiDMTab", new[] { "IDCampagnaTab" });
            DropIndex("dbo.PermessiDMTab", new[] { "IDUtentiTab" });
            DropIndex("dbo.CampagneTab", new[] { "IDUtentiTab" });
            DropIndex("dbo.AmbientazioniTab", new[] { "IDCampagneTab" });
            DropTable("dbo.PuntiInteresseTab");
            DropTable("dbo.LuoghiTab");
            DropTable("dbo.RegioniTab");
            DropTable("dbo.SessioniTab");
            DropTable("dbo.RuoliTab");
            DropTable("dbo.AmicizieTab");
            DropTable("dbo.UtentiTab");
            DropTable("dbo.PermessiDMTab");
            DropTable("dbo.CampagneTab");
            DropTable("dbo.AmbientazioniTab");
        }
    }
}
