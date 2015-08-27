namespace SACAAE.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class changes26Ago : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.TipoAsignacionProfesor",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.ID);
            
            AddColumn("dbo.Grupo", "SedeID", c => c.Int());
            AddColumn("dbo.Grupo", "AssignProfessorTypeID", c => c.Int());
            AddColumn("dbo.BloqueXPlanXCurso", "GroupsPerPeriods", c => c.Int(nullable: false));
            AddColumn("dbo.ComisionXProfesor", "AssignProfessorTypeID", c => c.Int());
            AddColumn("dbo.ProyectoXProfesor", "AssignProfessorTypeID", c => c.Int());
            CreateIndex("dbo.Grupo", "SedeID");
            CreateIndex("dbo.Grupo", "AssignProfessorTypeID");
            CreateIndex("dbo.ComisionXProfesor", "AssignProfessorTypeID");
            CreateIndex("dbo.ProyectoXProfesor", "AssignProfessorTypeID");
            AddForeignKey("dbo.ProyectoXProfesor", "AssignProfessorTypeID", "dbo.TipoAsignacionProfesor", "ID");
            AddForeignKey("dbo.ComisionXProfesor", "AssignProfessorTypeID", "dbo.TipoAsignacionProfesor", "ID");
            AddForeignKey("dbo.Grupo", "SedeID", "dbo.Sede", "ID");
            AddForeignKey("dbo.Grupo", "AssignProfessorTypeID", "dbo.TipoAsignacionProfesor", "ID");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Grupo", "AssignProfessorTypeID", "dbo.TipoAsignacionProfesor");
            DropForeignKey("dbo.Grupo", "SedeID", "dbo.Sede");
            DropForeignKey("dbo.ComisionXProfesor", "AssignProfessorTypeID", "dbo.TipoAsignacionProfesor");
            DropForeignKey("dbo.ProyectoXProfesor", "AssignProfessorTypeID", "dbo.TipoAsignacionProfesor");
            DropIndex("dbo.ProyectoXProfesor", new[] { "AssignProfessorTypeID" });
            DropIndex("dbo.ComisionXProfesor", new[] { "AssignProfessorTypeID" });
            DropIndex("dbo.Grupo", new[] { "AssignProfessorTypeID" });
            DropIndex("dbo.Grupo", new[] { "SedeID" });
            DropColumn("dbo.ProyectoXProfesor", "AssignProfessorTypeID");
            DropColumn("dbo.ComisionXProfesor", "AssignProfessorTypeID");
            DropColumn("dbo.BloqueXPlanXCurso", "GroupsPerPeriods");
            DropColumn("dbo.Grupo", "AssignProfessorTypeID");
            DropColumn("dbo.Grupo", "SedeID");
            DropTable("dbo.TipoAsignacionProfesor");
        }
    }
}
