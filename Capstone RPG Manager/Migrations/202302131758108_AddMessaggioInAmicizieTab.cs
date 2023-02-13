namespace Capstone_RPG_Manager.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddMessaggioInAmicizieTab : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AmicizieTab", "Messaggio", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.AmicizieTab", "Messaggio");
        }
    }
}
