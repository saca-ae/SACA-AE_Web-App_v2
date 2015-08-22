namespace SACAAE.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class v01 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.ProyectoXProfesor", "Schedule", "dbo.Horario");
            DropForeignKey("dbo.ComisionXProfesor", "Schedule", "dbo.Horario");
            DropIndex("dbo.ComisionXProfesor", new[] { "Schedule" });
            DropIndex("dbo.ProyectoXProfesor", new[] { "Schedule" });
            CreateTable(
                "dbo.HorarioComisionXProfesor",
                c => new
                    {
                        Horario_ID = c.Int(nullable: false),
                        ComisionXProfesor_ID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Horario_ID, t.ComisionXProfesor_ID })
                .ForeignKey("dbo.Horario", t => t.Horario_ID, cascadeDelete: true)
                .ForeignKey("dbo.ComisionXProfesor", t => t.ComisionXProfesor_ID, cascadeDelete: true)
                .Index(t => t.Horario_ID)
                .Index(t => t.ComisionXProfesor_ID);
            
            CreateTable(
                "dbo.ProyectoXProfesorHorario",
                c => new
                    {
                        ProyectoXProfesor_ID = c.Int(nullable: false),
                        Horario_ID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.ProyectoXProfesor_ID, t.Horario_ID })
                .ForeignKey("dbo.ProyectoXProfesor", t => t.ProyectoXProfesor_ID, cascadeDelete: true)
                .ForeignKey("dbo.Horario", t => t.Horario_ID, cascadeDelete: true)
                .Index(t => t.ProyectoXProfesor_ID)
                .Index(t => t.Horario_ID);
            
            AlterColumn("dbo.Comision", "Start", c => c.DateTime(nullable: false, storeType: "date"));
            AlterColumn("dbo.Comision", "End", c => c.DateTime(nullable: false, storeType: "date"));
            DropColumn("dbo.ComisionXProfesor", "Schedule");
            DropColumn("dbo.ProyectoXProfesor", "Schedule");
        }
        
        public override void Down()
        {
            AddColumn("dbo.ProyectoXProfesor", "Schedule", c => c.Int());
            AddColumn("dbo.ComisionXProfesor", "Schedule", c => c.Int());
            DropForeignKey("dbo.ProyectoXProfesorHorario", "Horario_ID", "dbo.Horario");
            DropForeignKey("dbo.ProyectoXProfesorHorario", "ProyectoXProfesor_ID", "dbo.ProyectoXProfesor");
            DropForeignKey("dbo.HorarioComisionXProfesor", "ComisionXProfesor_ID", "dbo.ComisionXProfesor");
            DropForeignKey("dbo.HorarioComisionXProfesor", "Horario_ID", "dbo.Horario");
            DropIndex("dbo.ProyectoXProfesorHorario", new[] { "Horario_ID" });
            DropIndex("dbo.ProyectoXProfesorHorario", new[] { "ProyectoXProfesor_ID" });
            DropIndex("dbo.HorarioComisionXProfesor", new[] { "ComisionXProfesor_ID" });
            DropIndex("dbo.HorarioComisionXProfesor", new[] { "Horario_ID" });
            AlterColumn("dbo.Comision", "End", c => c.DateTime(nullable: false));
            AlterColumn("dbo.Comision", "Start", c => c.DateTime(nullable: false));
            DropTable("dbo.ProyectoXProfesorHorario");
            DropTable("dbo.HorarioComisionXProfesor");
            CreateIndex("dbo.ProyectoXProfesor", "Schedule");
            CreateIndex("dbo.ComisionXProfesor", "Schedule");
            AddForeignKey("dbo.ComisionXProfesor", "Schedule", "dbo.Horario", "ID");
            AddForeignKey("dbo.ProyectoXProfesor", "Schedule", "dbo.Horario", "ID");
        }
    }
}
