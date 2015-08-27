namespace SACAAE.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class fixgrupo : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Grupo", "SedeID", "dbo.Sede");
            DropIndex("dbo.Grupo", new[] { "SedeID" });
            AddColumn("dbo.BloqueXPlanXCurso", "SedeID", c => c.Int());
            CreateIndex("dbo.BloqueXPlanXCurso", "SedeID");
            AddForeignKey("dbo.BloqueXPlanXCurso", "SedeID", "dbo.Sede", "ID");
            DropColumn("dbo.Grupo", "SedeID");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Grupo", "SedeID", c => c.Int());
            DropForeignKey("dbo.BloqueXPlanXCurso", "SedeID", "dbo.Sede");
            DropIndex("dbo.BloqueXPlanXCurso", new[] { "SedeID" });
            DropColumn("dbo.BloqueXPlanXCurso", "SedeID");
            CreateIndex("dbo.Grupo", "SedeID");
            AddForeignKey("dbo.Grupo", "SedeID", "dbo.Sede", "ID");
        }
    }
}
