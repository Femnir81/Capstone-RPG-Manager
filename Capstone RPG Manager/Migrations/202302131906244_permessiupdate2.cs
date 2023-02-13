namespace Capstone_RPG_Manager.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class permessiupdate2 : DbMigration
    {
        public override void Up()
        {
            DropIndex("dbo.PermessiDMTab", new[] { "UtentiTabDM_ID" });
            RenameColumn(table: "dbo.PermessiDMTab", name: "IDUtentiTabUser", newName: "IDUtentiTabA");
            RenameColumn(table: "dbo.PermessiDMTab", name: "UtentiTabDM_ID", newName: "IDUtentiTabB");
            RenameIndex(table: "dbo.PermessiDMTab", name: "IX_IDUtentiTabUser", newName: "IX_IDUtentiTabA");
            AlterColumn("dbo.PermessiDMTab", "IDUtentiTabB", c => c.Int(nullable: false));
            CreateIndex("dbo.PermessiDMTab", "IDUtentiTabB");
            DropColumn("dbo.PermessiDMTab", "IDUtentiTabDM");
        }
        
        public override void Down()
        {
            AddColumn("dbo.PermessiDMTab", "IDUtentiTabDM", c => c.Int(nullable: false));
            DropIndex("dbo.PermessiDMTab", new[] { "IDUtentiTabB" });
            AlterColumn("dbo.PermessiDMTab", "IDUtentiTabB", c => c.Int());
            RenameIndex(table: "dbo.PermessiDMTab", name: "IX_IDUtentiTabA", newName: "IX_IDUtentiTabUser");
            RenameColumn(table: "dbo.PermessiDMTab", name: "IDUtentiTabB", newName: "UtentiTabDM_ID");
            RenameColumn(table: "dbo.PermessiDMTab", name: "IDUtentiTabA", newName: "IDUtentiTabUser");
            CreateIndex("dbo.PermessiDMTab", "UtentiTabDM_ID");
        }
    }
}
