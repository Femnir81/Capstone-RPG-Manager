namespace Capstone_RPG_Manager.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class PermessiFromCampagneToAmbientazioni : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.PermessiDMTab", "IDCampagnaTab", "dbo.CampagneTab");
            DropIndex("dbo.PermessiDMTab", new[] { "IDCampagnaTab" });
            AddColumn("dbo.PermessiDMTab", "IDAmbientazioniTab", c => c.Int(nullable: false));
            CreateIndex("dbo.PermessiDMTab", "IDAmbientazioniTab");
            AddForeignKey("dbo.PermessiDMTab", "IDAmbientazioniTab", "dbo.AmbientazioniTab", "ID");
            DropColumn("dbo.PermessiDMTab", "IDCampagnaTab");
        }
        
        public override void Down()
        {
            AddColumn("dbo.PermessiDMTab", "IDCampagnaTab", c => c.Int(nullable: false));
            DropForeignKey("dbo.PermessiDMTab", "IDAmbientazioniTab", "dbo.AmbientazioniTab");
            DropIndex("dbo.PermessiDMTab", new[] { "IDAmbientazioniTab" });
            DropColumn("dbo.PermessiDMTab", "IDAmbientazioniTab");
            CreateIndex("dbo.PermessiDMTab", "IDCampagnaTab");
            AddForeignKey("dbo.PermessiDMTab", "IDCampagnaTab", "dbo.CampagneTab", "ID");
        }
    }
}
