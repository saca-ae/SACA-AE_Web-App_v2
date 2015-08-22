namespace SACAAE.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class deleteDetalleGrupofixes : DbMigration
    {
        public override void Up()
        {
            RenameTable(name: "dbo.ProfesorXCurso", newName: "ProfesorXGrupo");
            DropForeignKey("dbo.DetalleGrupo", "ID", "dbo.Grupo");
            DropForeignKey("dbo.DetalleGrupo", "Professor", "dbo.ProfesorXCurso");
            DropForeignKey("dbo.GrupoAula", "Group", "dbo.DetalleGrupo");
            DropIndex("dbo.GrupoAula", new[] { "Group" });
            DropIndex("dbo.DetalleGrupo", new[] { "ID" });
            DropIndex("dbo.DetalleGrupo", new[] { "Professor" });
            AddColumn("dbo.Grupo", "Professor", c => c.Int());
            AddColumn("dbo.Grupo", "Capacity", c => c.Int());
            AddColumn("dbo.ComisionXProfesor", "Hours", c => c.Int());
            AddColumn("dbo.ProyectoXProfesor", "Hours", c => c.Int());
            AddColumn("dbo.Curso", "Temporal", c => c.Boolean(nullable: false));
            AlterColumn("dbo.GrupoAula", "Group", c => c.Int(nullable: false));
            CreateIndex("dbo.GrupoAula", "Group");
            CreateIndex("dbo.Grupo", "Professor");
            AddForeignKey("dbo.Grupo", "Professor", "dbo.ProfesorXGrupo", "ID");
            AddForeignKey("dbo.GrupoAula", "Group", "dbo.Grupo", "ID");
            DropColumn("dbo.Profesor", "Password");
            DropTable("dbo.DetalleGrupo");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.DetalleGrupo",
                c => new
                    {
                        ID = c.Int(nullable: false),
                        Professor = c.Int(),
                        Capacity = c.Int(),
                    })
                .PrimaryKey(t => t.ID);
            
            AddColumn("dbo.Profesor", "Password", c => c.String());
            DropForeignKey("dbo.GrupoAula", "Group", "dbo.Grupo");
            DropForeignKey("dbo.Grupo", "Professor", "dbo.ProfesorXGrupo");
            DropIndex("dbo.Grupo", new[] { "Professor" });
            DropIndex("dbo.GrupoAula", new[] { "Group" });
            AlterColumn("dbo.GrupoAula", "Group", c => c.Int(nullable: false));
            DropColumn("dbo.Curso", "Temporal");
            DropColumn("dbo.ProyectoXProfesor", "Hours");
            DropColumn("dbo.ComisionXProfesor", "Hours");
            DropColumn("dbo.Grupo", "Capacity");
            DropColumn("dbo.Grupo", "Professor");
            CreateIndex("dbo.DetalleGrupo", "Professor");
            CreateIndex("dbo.DetalleGrupo", "ID");
            CreateIndex("dbo.GrupoAula", "Group");
            AddForeignKey("dbo.GrupoAula", "Group", "dbo.DetalleGrupo", "ID", cascadeDelete: true);
            AddForeignKey("dbo.DetalleGrupo", "Professor", "dbo.ProfesorXCurso", "ID");
            AddForeignKey("dbo.DetalleGrupo", "ID", "dbo.Grupo", "ID");
            RenameTable(name: "dbo.ProfesorXGrupo", newName: "ProfesorXCurso");
        }
    }
}
