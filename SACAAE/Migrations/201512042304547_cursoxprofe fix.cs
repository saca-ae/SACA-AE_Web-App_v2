namespace SACAAE.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class cursoxprofefix : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Group", "EstimatedHour", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Group", "EstimatedHour");
        }
    }
}
