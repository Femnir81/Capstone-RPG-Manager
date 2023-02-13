namespace Capstone_RPG_Manager.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class TitleInSession : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.SessioniTab", "Titolo", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.SessioniTab", "Titolo");
        }
    }
}
