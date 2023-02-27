namespace Capstone_RPG_Manager.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class OraMessaggioInAmicizie : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AmicizieTab", "OraMessaggio", c => c.DateTime(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.AmicizieTab", "OraMessaggio");
        }
    }
}
