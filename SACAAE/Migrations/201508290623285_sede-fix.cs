namespace SACAAE.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class sedefix : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Sede", "GroupEnum", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Sede", "GroupEnum");
        }
    }
}
