namespace Capstone_RPG_Manager.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class TitleInSessionRequired : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.SessioniTab", "Titolo", c => c.String(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.SessioniTab", "Titolo", c => c.String());
        }
    }
}
