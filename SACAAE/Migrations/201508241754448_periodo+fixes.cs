namespace SACAAE.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class periodofixes : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.ProfesorXGrupo", "Professor", "dbo.Profesor");
            DropForeignKey("dbo.Grupo", "Professor", "dbo.ProfesorXGrupo");
            DropIndex("dbo.Grupo", new[] { "Professor" });
            DropIndex("dbo.ProfesorXGrupo", new[] { "Professor" });
            RenameColumn(table: "dbo.GrupoAula", name: "Classroom", newName: "ClassroomID");
            RenameColumn(table: "dbo.GrupoAula", name: "Schedule", newName: "ScheduleID");
            RenameColumn(table: "dbo.Grupo", name: "BlockXPlanXCourse", newName: "BlockXPlanXCourseID");
            RenameColumn(table: "dbo.Grupo", name: "Period", newName: "PeriodID");
            RenameColumn(table: "dbo.PlanDeEstudio", name: "Mode", newName: "ModeID");
            RenameColumn(table: "dbo.PlanDeEstudioXSede", name: "StudyPlan", newName: "StudyPlanID");
            RenameColumn(table: "dbo.PlanDeEstudio", name: "EntityType", newName: "EntityTypeID");
            RenameColumn(table: "dbo.Comision", name: "EntityType", newName: "EntityTypeID");
            RenameColumn(table: "dbo.Proyecto", name: "EntityType", newName: "EntityTypeID");
            RenameColumn(table: "dbo.ComisionXProfesor", name: "Commission", newName: "CommissionID");
            RenameColumn(table: "dbo.Comision", name: "State", newName: "StateID");
            RenameColumn(table: "dbo.ComisionXProfesor", name: "Period", newName: "PeriodID");
            RenameColumn(table: "dbo.ComisionXProfesor", name: "Professor", newName: "ProfessorID");
            RenameColumn(table: "dbo.ProyectoXProfesor", name: "Period", newName: "PeriodID");
            RenameColumn(table: "dbo.ProyectoXProfesor", name: "Professor", newName: "ProfessorID");
            RenameColumn(table: "dbo.ProyectoXProfesor", name: "Project", newName: "ProjectID");
            RenameColumn(table: "dbo.Profesor", name: "State", newName: "StateID");
            RenameColumn(table: "dbo.PlazaXProfesor", name: "Professor", newName: "ProfessorID");
            RenameColumn(table: "dbo.Proyecto", name: "State", newName: "StateID");
            RenameIndex(table: "dbo.GrupoAula", name: "IX_Classroom", newName: "IX_ClassroomID");
            RenameIndex(table: "dbo.GrupoAula", name: "IX_Schedule", newName: "IX_ScheduleID");
            RenameIndex(table: "dbo.Grupo", name: "IX_Period", newName: "IX_PeriodID");
            RenameIndex(table: "dbo.Grupo", name: "IX_BlockXPlanXCourse", newName: "IX_BlockXPlanXCourseID");
            RenameIndex(table: "dbo.PlanDeEstudio", name: "IX_Mode", newName: "IX_ModeID");
            RenameIndex(table: "dbo.PlanDeEstudio", name: "IX_EntityType", newName: "IX_EntityTypeID");
            RenameIndex(table: "dbo.PlanDeEstudioXSede", name: "IX_StudyPlan", newName: "IX_StudyPlanID");
            RenameIndex(table: "dbo.Comision", name: "IX_State", newName: "IX_StateID");
            RenameIndex(table: "dbo.Comision", name: "IX_EntityType", newName: "IX_EntityTypeID");
            RenameIndex(table: "dbo.ComisionXProfesor", name: "IX_Commission", newName: "IX_CommissionID");
            RenameIndex(table: "dbo.ComisionXProfesor", name: "IX_Professor", newName: "IX_ProfessorID");
            RenameIndex(table: "dbo.ComisionXProfesor", name: "IX_Period", newName: "IX_PeriodID");
            RenameIndex(table: "dbo.ProyectoXProfesor", name: "IX_Project", newName: "IX_ProjectID");
            RenameIndex(table: "dbo.ProyectoXProfesor", name: "IX_Professor", newName: "IX_ProfessorID");
            RenameIndex(table: "dbo.ProyectoXProfesor", name: "IX_Period", newName: "IX_PeriodID");
            RenameIndex(table: "dbo.Profesor", name: "IX_State", newName: "IX_StateID");
            RenameIndex(table: "dbo.Proyecto", name: "IX_State", newName: "IX_StateID");
            RenameIndex(table: "dbo.Proyecto", name: "IX_EntityType", newName: "IX_EntityTypeID");
            RenameIndex(table: "dbo.PlazaXProfesor", name: "IX_Professor", newName: "IX_ProfessorID");
            CreateTable(
                "dbo.NumeroPeriodo",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Number = c.Int(nullable: false),
                        TypeID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.TipoPeriodo", t => t.TypeID)
                .Index(t => t.TypeID);
            
            CreateTable(
                "dbo.TipoPeriodo",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.ID);
            
            AddColumn("dbo.Grupo", "ProfessorID", c => c.Int());
            AddColumn("dbo.Periodo", "Year", c => c.Int(nullable: false));
            AddColumn("dbo.Periodo", "NumberID", c => c.Int(nullable: false));
            CreateIndex("dbo.Grupo", "ProfessorID");
            CreateIndex("dbo.Periodo", "NumberID");
            AddForeignKey("dbo.Periodo", "NumberID", "dbo.NumeroPeriodo", "ID");
            AddForeignKey("dbo.Grupo", "ProfessorID", "dbo.Profesor", "ID");
            DropColumn("dbo.Grupo", "Professor");
            DropColumn("dbo.Periodo", "Name");
            DropTable("dbo.ProfesorXGrupo");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.ProfesorXGrupo",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Professor = c.Int(nullable: false),
                        Hours = c.Int(),
                    })
                .PrimaryKey(t => t.ID);
            
            AddColumn("dbo.Periodo", "Name", c => c.String());
            AddColumn("dbo.Grupo", "Professor", c => c.Int());
            DropForeignKey("dbo.Grupo", "ProfessorID", "dbo.Profesor");
            DropForeignKey("dbo.Periodo", "NumberID", "dbo.NumeroPeriodo");
            DropForeignKey("dbo.NumeroPeriodo", "TypeID", "dbo.TipoPeriodo");
            DropIndex("dbo.NumeroPeriodo", new[] { "TypeID" });
            DropIndex("dbo.Periodo", new[] { "NumberID" });
            DropIndex("dbo.Grupo", new[] { "ProfessorID" });
            DropColumn("dbo.Periodo", "NumberID");
            DropColumn("dbo.Periodo", "Year");
            DropColumn("dbo.Grupo", "ProfessorID");
            DropTable("dbo.TipoPeriodo");
            DropTable("dbo.NumeroPeriodo");
            RenameIndex(table: "dbo.PlazaXProfesor", name: "IX_ProfessorID", newName: "IX_Professor");
            RenameIndex(table: "dbo.Proyecto", name: "IX_EntityTypeID", newName: "IX_EntityType");
            RenameIndex(table: "dbo.Proyecto", name: "IX_StateID", newName: "IX_State");
            RenameIndex(table: "dbo.Profesor", name: "IX_StateID", newName: "IX_State");
            RenameIndex(table: "dbo.ProyectoXProfesor", name: "IX_PeriodID", newName: "IX_Period");
            RenameIndex(table: "dbo.ProyectoXProfesor", name: "IX_ProfessorID", newName: "IX_Professor");
            RenameIndex(table: "dbo.ProyectoXProfesor", name: "IX_ProjectID", newName: "IX_Project");
            RenameIndex(table: "dbo.ComisionXProfesor", name: "IX_PeriodID", newName: "IX_Period");
            RenameIndex(table: "dbo.ComisionXProfesor", name: "IX_ProfessorID", newName: "IX_Professor");
            RenameIndex(table: "dbo.ComisionXProfesor", name: "IX_CommissionID", newName: "IX_Commission");
            RenameIndex(table: "dbo.Comision", name: "IX_EntityTypeID", newName: "IX_EntityType");
            RenameIndex(table: "dbo.Comision", name: "IX_StateID", newName: "IX_State");
            RenameIndex(table: "dbo.PlanDeEstudioXSede", name: "IX_StudyPlanID", newName: "IX_StudyPlan");
            RenameIndex(table: "dbo.PlanDeEstudio", name: "IX_EntityTypeID", newName: "IX_EntityType");
            RenameIndex(table: "dbo.PlanDeEstudio", name: "IX_ModeID", newName: "IX_Mode");
            RenameIndex(table: "dbo.Grupo", name: "IX_BlockXPlanXCourseID", newName: "IX_BlockXPlanXCourse");
            RenameIndex(table: "dbo.Grupo", name: "IX_PeriodID", newName: "IX_Period");
            RenameIndex(table: "dbo.GrupoAula", name: "IX_ScheduleID", newName: "IX_Schedule");
            RenameIndex(table: "dbo.GrupoAula", name: "IX_ClassroomID", newName: "IX_Classroom");
            RenameColumn(table: "dbo.Proyecto", name: "StateID", newName: "State");
            RenameColumn(table: "dbo.PlazaXProfesor", name: "ProfessorID", newName: "Professor");
            RenameColumn(table: "dbo.Profesor", name: "StateID", newName: "State");
            RenameColumn(table: "dbo.ProyectoXProfesor", name: "ProjectID", newName: "Project");
            RenameColumn(table: "dbo.ProyectoXProfesor", name: "ProfessorID", newName: "Professor");
            RenameColumn(table: "dbo.ProyectoXProfesor", name: "PeriodID", newName: "Period");
            RenameColumn(table: "dbo.ComisionXProfesor", name: "ProfessorID", newName: "Professor");
            RenameColumn(table: "dbo.ComisionXProfesor", name: "PeriodID", newName: "Period");
            RenameColumn(table: "dbo.Comision", name: "StateID", newName: "State");
            RenameColumn(table: "dbo.ComisionXProfesor", name: "CommissionID", newName: "Commission");
            RenameColumn(table: "dbo.Proyecto", name: "EntityTypeID", newName: "EntityType");
            RenameColumn(table: "dbo.Comision", name: "EntityTypeID", newName: "EntityType");
            RenameColumn(table: "dbo.PlanDeEstudio", name: "EntityTypeID", newName: "EntityType");
            RenameColumn(table: "dbo.PlanDeEstudioXSede", name: "StudyPlanID", newName: "StudyPlan");
            RenameColumn(table: "dbo.PlanDeEstudio", name: "ModeID", newName: "Mode");
            RenameColumn(table: "dbo.Grupo", name: "PeriodID", newName: "Period");
            RenameColumn(table: "dbo.Grupo", name: "BlockXPlanXCourseID", newName: "BlockXPlanXCourse");
            RenameColumn(table: "dbo.GrupoAula", name: "ScheduleID", newName: "Schedule");
            RenameColumn(table: "dbo.GrupoAula", name: "ClassroomID", newName: "Classroom");
            CreateIndex("dbo.ProfesorXGrupo", "Professor");
            CreateIndex("dbo.Grupo", "Professor");
            AddForeignKey("dbo.Grupo", "Professor", "dbo.ProfesorXGrupo", "ID");
            AddForeignKey("dbo.ProfesorXGrupo", "Professor", "dbo.Profesor", "ID", cascadeDelete: true);
        }
    }
}
