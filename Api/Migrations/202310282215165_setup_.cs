namespace Api.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class setup_ : DbMigration
    {
        public override void Up()
        {
            DropPrimaryKey("dbo.Users");
            AlterColumn("dbo.Users", "Id", c => c.Guid(nullable: false));
            AddPrimaryKey("dbo.Users", "Id");
            DropColumn("dbo.Users", "Username");
            DropColumn("dbo.Users", "Email");
            DropColumn("dbo.Users", "IpAddress");
            DropColumn("dbo.Users", "Segment");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Users", "Segment", c => c.String());
            AddColumn("dbo.Users", "IpAddress", c => c.String());
            AddColumn("dbo.Users", "Email", c => c.String());
            AddColumn("dbo.Users", "Username", c => c.String());
            DropPrimaryKey("dbo.Users");
            AlterColumn("dbo.Users", "Id", c => c.String(nullable: false, maxLength: 128));
            AddPrimaryKey("dbo.Users", "Id");
        }
    }
}
