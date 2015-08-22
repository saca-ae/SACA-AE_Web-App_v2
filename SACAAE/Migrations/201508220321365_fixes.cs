namespace SACAAE.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class fixes : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.GrupoAula", "Group", "dbo.Grupo");
            DropIndex("dbo.GrupoAula", new[] { "Group" });
            AlterColumn("dbo.GrupoAula", "Group", c => c.Int(nullable: false));
            CreateIndex("dbo.GrupoAula", "Group");
            AddForeignKey("dbo.GrupoAula", "Group", "dbo.Grupo", "ID", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.GrupoAula", "Group", "dbo.Grupo");
            DropIndex("dbo.GrupoAula", new[] { "Group" });
            AlterColumn("dbo.GrupoAula", "Group", c => c.Int());
            CreateIndex("dbo.GrupoAula", "Group");
            AddForeignKey("dbo.GrupoAula", "Group", "dbo.Grupo", "ID");
        }
    }
}
