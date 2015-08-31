namespace SACAAE.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class names : DbMigration
    {
        public override void Up()
        {
            RenameTable(name: "dbo.Aula", newName: "Classroom");
            RenameTable(name: "dbo.TipoPeriodo", newName: "HourAllocatedType");
            RenameTable(name: "dbo.Curso", newName: "Course");
            DropForeignKey("dbo.GrupoAula", "ClassroomID", "dbo.Aula");
            DropForeignKey("dbo.BloqueAcademicoXPlanDeEstudio", "BlockID", "dbo.BloqueAcademico");
            DropForeignKey("dbo.PlanDeEstudio", "ModeID", "dbo.Modalidad");
            DropForeignKey("dbo.PlanDeEstudioXSede", "StudyPlanID", "dbo.PlanDeEstudio");
            DropForeignKey("dbo.PlanDeEstudioXSede", "SedeID", "dbo.Sede");
            DropForeignKey("dbo.ComisionXProfesor", "CommissionID", "dbo.Comision");
            DropForeignKey("dbo.HorarioComisionXProfesor", "Horario_ID", "dbo.Horario");
            DropForeignKey("dbo.HorarioComisionXProfesor", "ComisionXProfesor_ID", "dbo.ComisionXProfesor");
            DropForeignKey("dbo.ProyectoXProfesorHorario", "ProyectoXProfesor_ID", "dbo.ProyectoXProfesor");
            DropForeignKey("dbo.ProyectoXProfesorHorario", "Horario_ID", "dbo.Horario");
            DropForeignKey("dbo.NumeroPeriodo", "TypeID", "dbo.TipoPeriodo");
            DropForeignKey("dbo.Periodo", "NumberID", "dbo.NumeroPeriodo");
            DropForeignKey("dbo.ProyectoXProfesor", "PeriodID", "dbo.Periodo");
            DropForeignKey("dbo.Proyecto", "StateID", "dbo.Estado");
            DropForeignKey("dbo.Proyecto", "EntityTypeID", "dbo.TipoEntidad");
            DropForeignKey("dbo.Profesor", "StateID", "dbo.Estado");
            DropForeignKey("dbo.PlazaXProfesor", "PlazaID", "dbo.Plaza");
            DropForeignKey("dbo.PlazaXProfesor", "ProfessorID", "dbo.Profesor");
            DropForeignKey("dbo.ProyectoXProfesor", "ProfessorID", "dbo.Profesor");
            DropForeignKey("dbo.ProyectoXProfesor", "ProjectID", "dbo.Proyecto");
            DropForeignKey("dbo.ProyectoXProfesor", "AssignProfessorTypeID", "dbo.TipoAsignacionProfesor");
            DropForeignKey("dbo.ComisionXProfesor", "PeriodID", "dbo.Periodo");
            DropForeignKey("dbo.ComisionXProfesor", "ProfessorID", "dbo.Profesor");
            DropForeignKey("dbo.ComisionXProfesor", "AssignProfessorTypeID", "dbo.TipoAsignacionProfesor");
            DropForeignKey("dbo.Comision", "StateID", "dbo.Estado");
            DropForeignKey("dbo.Comision", "EntityTypeID", "dbo.TipoEntidad");
            DropForeignKey("dbo.PlanDeEstudio", "EntityTypeID", "dbo.TipoEntidad");
            DropForeignKey("dbo.BloqueAcademicoXPlanDeEstudio", "PlanID", "dbo.PlanDeEstudio");
            DropForeignKey("dbo.BloqueXPlanXCurso", "BlockXPlanID", "dbo.BloqueAcademicoXPlanDeEstudio");
            DropForeignKey("dbo.BloqueXPlanXCurso", "CourseID", "dbo.Curso");
            DropForeignKey("dbo.BloqueXPlanXCurso", "SedeID", "dbo.Sede");
            DropForeignKey("dbo.Grupo", "BlockXPlanXCourseID", "dbo.BloqueXPlanXCurso");
            DropForeignKey("dbo.Grupo", "PeriodID", "dbo.Periodo");
            DropForeignKey("dbo.Grupo", "ProfessorID", "dbo.Profesor");
            DropForeignKey("dbo.Grupo", "AssignProfessorTypeID", "dbo.TipoAsignacionProfesor");
            DropForeignKey("dbo.GrupoAula", "GroupID", "dbo.Grupo");
            DropForeignKey("dbo.GrupoAula", "ScheduleID", "dbo.Horario");
            DropIndex("dbo.GrupoAula", new[] { "GroupID" });
            DropIndex("dbo.GrupoAula", new[] { "ClassroomID" });
            DropIndex("dbo.GrupoAula", new[] { "ScheduleID" });
            DropIndex("dbo.Grupo", new[] { "PeriodID" });
            DropIndex("dbo.Grupo", new[] { "BlockXPlanXCourseID" });
            DropIndex("dbo.Grupo", new[] { "ProfessorID" });
            DropIndex("dbo.Grupo", new[] { "AssignProfessorTypeID" });
            DropIndex("dbo.BloqueXPlanXCurso", new[] { "BlockXPlanID" });
            DropIndex("dbo.BloqueXPlanXCurso", new[] { "CourseID" });
            DropIndex("dbo.BloqueXPlanXCurso", new[] { "SedeID" });
            DropIndex("dbo.BloqueAcademicoXPlanDeEstudio", new[] { "PlanID" });
            DropIndex("dbo.BloqueAcademicoXPlanDeEstudio", new[] { "BlockID" });
            DropIndex("dbo.PlanDeEstudio", new[] { "ModeID" });
            DropIndex("dbo.PlanDeEstudio", new[] { "EntityTypeID" });
            DropIndex("dbo.PlanDeEstudioXSede", new[] { "SedeID" });
            DropIndex("dbo.PlanDeEstudioXSede", new[] { "StudyPlanID" });
            DropIndex("dbo.Comision", new[] { "StateID" });
            DropIndex("dbo.Comision", new[] { "EntityTypeID" });
            DropIndex("dbo.ComisionXProfesor", new[] { "CommissionID" });
            DropIndex("dbo.ComisionXProfesor", new[] { "ProfessorID" });
            DropIndex("dbo.ComisionXProfesor", new[] { "AssignProfessorTypeID" });
            DropIndex("dbo.ComisionXProfesor", new[] { "PeriodID" });
            DropIndex("dbo.ProyectoXProfesor", new[] { "ProjectID" });
            DropIndex("dbo.ProyectoXProfesor", new[] { "ProfessorID" });
            DropIndex("dbo.ProyectoXProfesor", new[] { "AssignProfessorTypeID" });
            DropIndex("dbo.ProyectoXProfesor", new[] { "PeriodID" });
            DropIndex("dbo.Periodo", new[] { "NumberID" });
            DropIndex("dbo.NumeroPeriodo", new[] { "TypeID" });
            DropIndex("dbo.Profesor", new[] { "StateID" });
            DropIndex("dbo.Proyecto", new[] { "StateID" });
            DropIndex("dbo.Proyecto", new[] { "EntityTypeID" });
            DropIndex("dbo.PlazaXProfesor", new[] { "PlazaID" });
            DropIndex("dbo.PlazaXProfesor", new[] { "ProfessorID" });
            DropIndex("dbo.HorarioComisionXProfesor", new[] { "Horario_ID" });
            DropIndex("dbo.HorarioComisionXProfesor", new[] { "ComisionXProfesor_ID" });
            DropIndex("dbo.ProyectoXProfesorHorario", new[] { "ProyectoXProfesor_ID" });
            DropIndex("dbo.ProyectoXProfesorHorario", new[] { "Horario_ID" });
            CreateTable(
                "dbo.AcademicBlock",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Description = c.String(),
                        Level = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.AcademicBlockXStudyPlan",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        PlanID = c.Int(nullable: false),
                        BlockID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.AcademicBlock", t => t.BlockID, cascadeDelete: true)
                .ForeignKey("dbo.StudyPlan", t => t.PlanID, cascadeDelete: true)
                .Index(t => t.PlanID)
                .Index(t => t.BlockID);
            
            CreateTable(
                "dbo.BlockXPlanXCourse",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        BlockXPlanID = c.Int(nullable: false),
                        CourseID = c.Int(nullable: false),
                        SedeID = c.Int(),
                        GroupsPerPeriods = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.AcademicBlockXStudyPlan", t => t.BlockXPlanID, cascadeDelete: true)
                .ForeignKey("dbo.Course", t => t.CourseID, cascadeDelete: true)
                .ForeignKey("dbo.Sede", t => t.SedeID)
                .Index(t => t.BlockXPlanID)
                .Index(t => t.CourseID)
                .Index(t => t.SedeID);
            
            CreateTable(
                "dbo.Group",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Number = c.Int(nullable: false),
                        PeriodID = c.Int(nullable: false),
                        BlockXPlanXCourseID = c.Int(nullable: false),
                        ProfessorID = c.Int(),
                        HourAllocatedTypeID = c.Int(),
                        Capacity = c.Int(),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.BlockXPlanXCourse", t => t.BlockXPlanXCourseID, cascadeDelete: true)
                .ForeignKey("dbo.HourAllocatedType", t => t.HourAllocatedTypeID)
                .ForeignKey("dbo.Period", t => t.PeriodID, cascadeDelete: true)
                .ForeignKey("dbo.Professor", t => t.ProfessorID)
                .Index(t => t.PeriodID)
                .Index(t => t.BlockXPlanXCourseID)
                .Index(t => t.ProfessorID)
                .Index(t => t.HourAllocatedTypeID);
            
            CreateTable(
                "dbo.GroupClassroom",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        GroupID = c.Int(nullable: false),
                        ClassroomID = c.Int(),
                        ScheduleID = c.Int(),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Classroom", t => t.ClassroomID)
                .ForeignKey("dbo.Group", t => t.GroupID, cascadeDelete: true)
                .ForeignKey("dbo.Schedule", t => t.ScheduleID)
                .Index(t => t.GroupID)
                .Index(t => t.ClassroomID)
                .Index(t => t.ScheduleID);
            
            CreateTable(
                "dbo.StudyPlanXSede",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        SedeID = c.Int(nullable: false),
                        StudyPlanID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Sede", t => t.SedeID, cascadeDelete: true)
                .ForeignKey("dbo.StudyPlan", t => t.StudyPlanID, cascadeDelete: true)
                .Index(t => t.SedeID)
                .Index(t => t.StudyPlanID);
            
            CreateTable(
                "dbo.StudyPlan",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        ModeID = c.Int(nullable: false),
                        EntityTypeID = c.Int(),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.EntityType", t => t.EntityTypeID)
                .ForeignKey("dbo.Modality", t => t.ModeID, cascadeDelete: true)
                .Index(t => t.ModeID)
                .Index(t => t.EntityTypeID);
            
            CreateTable(
                "dbo.EntityType",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.Commission",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Start = c.DateTime(nullable: false, storeType: "date"),
                        End = c.DateTime(nullable: false, storeType: "date"),
                        StateID = c.Int(),
                        EntityTypeID = c.Int(),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.EntityType", t => t.EntityTypeID)
                .ForeignKey("dbo.State", t => t.StateID)
                .Index(t => t.StateID)
                .Index(t => t.EntityTypeID);
            
            CreateTable(
                "dbo.CommissionXProfessor",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        CommissionID = c.Int(nullable: false),
                        ProfessorID = c.Int(nullable: false),
                        HourAllocatedTypeID = c.Int(),
                        PeriodID = c.Int(nullable: false),
                        Hours = c.Int(),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Commission", t => t.CommissionID, cascadeDelete: true)
                .ForeignKey("dbo.HourAllocatedType", t => t.HourAllocatedTypeID)
                .ForeignKey("dbo.Period", t => t.PeriodID, cascadeDelete: true)
                .ForeignKey("dbo.Professor", t => t.ProfessorID, cascadeDelete: true)
                .Index(t => t.CommissionID)
                .Index(t => t.ProfessorID)
                .Index(t => t.HourAllocatedTypeID)
                .Index(t => t.PeriodID);
            
            CreateTable(
                "dbo.Period",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Year = c.Int(nullable: false),
                        NumberID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.PeriodNumber", t => t.NumberID, cascadeDelete: true)
                .Index(t => t.NumberID);
            
            CreateTable(
                "dbo.PeriodNumber",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Number = c.Int(nullable: false),
                        TypeID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.PeriodType", t => t.TypeID, cascadeDelete: true)
                .Index(t => t.TypeID);
            
            CreateTable(
                "dbo.PeriodType",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.ProjectXProfessor",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        ProjectID = c.Int(nullable: false),
                        ProfessorID = c.Int(nullable: false),
                        HourAllocatedTypeID = c.Int(),
                        PeriodID = c.Int(nullable: false),
                        Hours = c.Int(),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.HourAllocatedType", t => t.HourAllocatedTypeID)
                .ForeignKey("dbo.Period", t => t.PeriodID, cascadeDelete: true)
                .ForeignKey("dbo.Professor", t => t.ProfessorID, cascadeDelete: true)
                .ForeignKey("dbo.Project", t => t.ProjectID, cascadeDelete: true)
                .Index(t => t.ProjectID)
                .Index(t => t.ProfessorID)
                .Index(t => t.HourAllocatedTypeID)
                .Index(t => t.PeriodID);
            
            CreateTable(
                "dbo.Professor",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Link = c.String(),
                        StateID = c.Int(),
                        Tel1 = c.String(),
                        Tel2 = c.String(),
                        Email = c.String(),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.State", t => t.StateID)
                .Index(t => t.StateID);
            
            CreateTable(
                "dbo.PlazaXProfessor",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        PlazaID = c.Int(nullable: false),
                        ProfessorID = c.Int(nullable: false),
                        PercentHours = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Plaza", t => t.PlazaID, cascadeDelete: true)
                .ForeignKey("dbo.Professor", t => t.ProfessorID, cascadeDelete: true)
                .Index(t => t.PlazaID)
                .Index(t => t.ProfessorID);
            
            CreateTable(
                "dbo.State",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.Project",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Start = c.DateTime(),
                        End = c.DateTime(),
                        StateID = c.Int(),
                        Link = c.String(),
                        EntityTypeID = c.Int(),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.EntityType", t => t.EntityTypeID)
                .ForeignKey("dbo.State", t => t.StateID)
                .Index(t => t.StateID)
                .Index(t => t.EntityTypeID);
            
            CreateTable(
                "dbo.Schedule",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Day = c.String(),
                        StartHour = c.String(),
                        EndHour = c.String(),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.Modality",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.ScheduleCommissionXProfessor",
                c => new
                    {
                        Schedule_ID = c.Int(nullable: false),
                        CommissionXProfessor_ID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Schedule_ID, t.CommissionXProfessor_ID })
                .ForeignKey("dbo.Schedule", t => t.Schedule_ID, cascadeDelete: true)
                .ForeignKey("dbo.CommissionXProfessor", t => t.CommissionXProfessor_ID, cascadeDelete: true)
                .Index(t => t.Schedule_ID)
                .Index(t => t.CommissionXProfessor_ID);
            
            CreateTable(
                "dbo.ScheduleProjectXProfessor",
                c => new
                    {
                        Schedule_ID = c.Int(nullable: false),
                        ProjectXProfessor_ID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Schedule_ID, t.ProjectXProfessor_ID })
                .ForeignKey("dbo.Schedule", t => t.Schedule_ID, cascadeDelete: true)
                .ForeignKey("dbo.ProjectXProfessor", t => t.ProjectXProfessor_ID, cascadeDelete: true)
                .Index(t => t.Schedule_ID)
                .Index(t => t.ProjectXProfessor_ID);
            
            DropTable("dbo.GrupoAula");
            DropTable("dbo.Grupo");
            DropTable("dbo.BloqueXPlanXCurso");
            DropTable("dbo.BloqueAcademicoXPlanDeEstudio");
            DropTable("dbo.BloqueAcademico");
            DropTable("dbo.PlanDeEstudioXSede");
            DropTable("dbo.PlanDeEstudio");
            DropTable("dbo.Modalidad");
            DropTable("dbo.ComisionXProfesor");
            DropTable("dbo.Comision");
            DropTable("dbo.Horario");
            DropTable("dbo.ProyectoXProfesor");
            DropTable("dbo.Periodo");
            DropTable("dbo.NumeroPeriodo");
            DropTable("dbo.PlazaXProfesor");
            DropTable("dbo.TipoAsignacionProfesor");
            DropTable("dbo.HorarioComisionXProfesor");
            DropTable("dbo.ProyectoXProfesorHorario");
            DropTable("dbo.Proyecto");
            DropTable("dbo.Profesor");
            DropTable("dbo.Estado");
            DropTable("dbo.TipoEntidad");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.ProyectoXProfesorHorario",
                c => new
                    {
                        ProyectoXProfesor_ID = c.Int(nullable: false),
                        Horario_ID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.ProyectoXProfesor_ID, t.Horario_ID });
            
            CreateTable(
                "dbo.HorarioComisionXProfesor",
                c => new
                    {
                        Horario_ID = c.Int(nullable: false),
                        ComisionXProfesor_ID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Horario_ID, t.ComisionXProfesor_ID });
            
            CreateTable(
                "dbo.TipoAsignacionProfesor",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.PlazaXProfesor",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        PlazaID = c.Int(nullable: false),
                        ProfessorID = c.Int(nullable: false),
                        PercentHours = c.Int(nullable: false),
                        PropertyHours = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.Proyecto",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Start = c.DateTime(),
                        End = c.DateTime(),
                        StateID = c.Int(),
                        Link = c.String(),
                        EntityTypeID = c.Int(),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.Estado",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.Profesor",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Link = c.String(),
                        StateID = c.Int(),
                        Tel1 = c.String(),
                        Tel2 = c.String(),
                        Email = c.String(),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.NumeroPeriodo",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Number = c.Int(nullable: false),
                        TypeID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.Periodo",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Year = c.Int(nullable: false),
                        NumberID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.ProyectoXProfesor",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        ProjectID = c.Int(nullable: false),
                        ProfessorID = c.Int(nullable: false),
                        AssignProfessorTypeID = c.Int(),
                        PeriodID = c.Int(nullable: false),
                        Hours = c.Int(),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.Horario",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Day = c.String(),
                        StartHour = c.String(),
                        EndHour = c.String(),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.ComisionXProfesor",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        CommissionID = c.Int(nullable: false),
                        ProfessorID = c.Int(nullable: false),
                        AssignProfessorTypeID = c.Int(),
                        PeriodID = c.Int(nullable: false),
                        Hours = c.Int(),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.Comision",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Start = c.DateTime(nullable: false, storeType: "date"),
                        End = c.DateTime(nullable: false, storeType: "date"),
                        StateID = c.Int(),
                        EntityTypeID = c.Int(),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.TipoEntidad",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.PlanDeEstudioXSede",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        SedeID = c.Int(nullable: false),
                        StudyPlanID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.Modalidad",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.PlanDeEstudio",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        ModeID = c.Int(nullable: false),
                        EntityTypeID = c.Int(),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.BloqueAcademico",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Description = c.String(),
                        Level = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.BloqueAcademicoXPlanDeEstudio",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        PlanID = c.Int(nullable: false),
                        BlockID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.BloqueXPlanXCurso",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        BlockXPlanID = c.Int(nullable: false),
                        CourseID = c.Int(nullable: false),
                        SedeID = c.Int(),
                        GroupsPerPeriods = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.Grupo",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Number = c.Int(nullable: false),
                        PeriodID = c.Int(nullable: false),
                        BlockXPlanXCourseID = c.Int(nullable: false),
                        ProfessorID = c.Int(),
                        AssignProfessorTypeID = c.Int(),
                        Capacity = c.Int(),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.GrupoAula",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        GroupID = c.Int(nullable: false),
                        ClassroomID = c.Int(),
                        ScheduleID = c.Int(),
                    })
                .PrimaryKey(t => t.ID);
            
            DropForeignKey("dbo.AcademicBlockXStudyPlan", "PlanID", "dbo.StudyPlan");
            DropForeignKey("dbo.BlockXPlanXCourse", "SedeID", "dbo.Sede");
            DropForeignKey("dbo.Group", "ProfessorID", "dbo.Professor");
            DropForeignKey("dbo.Group", "PeriodID", "dbo.Period");
            DropForeignKey("dbo.Group", "HourAllocatedTypeID", "dbo.HourAllocatedType");
            DropForeignKey("dbo.GroupClassroom", "ScheduleID", "dbo.Schedule");
            DropForeignKey("dbo.GroupClassroom", "GroupID", "dbo.Group");
            DropForeignKey("dbo.GroupClassroom", "ClassroomID", "dbo.Classroom");
            DropForeignKey("dbo.StudyPlanXSede", "StudyPlanID", "dbo.StudyPlan");
            DropForeignKey("dbo.StudyPlan", "ModeID", "dbo.Modality");
            DropForeignKey("dbo.StudyPlan", "EntityTypeID", "dbo.EntityType");
            DropForeignKey("dbo.Commission", "StateID", "dbo.State");
            DropForeignKey("dbo.Commission", "EntityTypeID", "dbo.EntityType");
            DropForeignKey("dbo.CommissionXProfessor", "ProfessorID", "dbo.Professor");
            DropForeignKey("dbo.CommissionXProfessor", "PeriodID", "dbo.Period");
            DropForeignKey("dbo.ScheduleProjectXProfessor", "ProjectXProfessor_ID", "dbo.ProjectXProfessor");
            DropForeignKey("dbo.ScheduleProjectXProfessor", "Schedule_ID", "dbo.Schedule");
            DropForeignKey("dbo.ScheduleCommissionXProfessor", "CommissionXProfessor_ID", "dbo.CommissionXProfessor");
            DropForeignKey("dbo.ScheduleCommissionXProfessor", "Schedule_ID", "dbo.Schedule");
            DropForeignKey("dbo.ProjectXProfessor", "ProjectID", "dbo.Project");
            DropForeignKey("dbo.ProjectXProfessor", "ProfessorID", "dbo.Professor");
            DropForeignKey("dbo.Professor", "StateID", "dbo.State");
            DropForeignKey("dbo.Project", "StateID", "dbo.State");
            DropForeignKey("dbo.Project", "EntityTypeID", "dbo.EntityType");
            DropForeignKey("dbo.PlazaXProfessor", "ProfessorID", "dbo.Professor");
            DropForeignKey("dbo.PlazaXProfessor", "PlazaID", "dbo.Plaza");
            DropForeignKey("dbo.ProjectXProfessor", "PeriodID", "dbo.Period");
            DropForeignKey("dbo.ProjectXProfessor", "HourAllocatedTypeID", "dbo.HourAllocatedType");
            DropForeignKey("dbo.Period", "NumberID", "dbo.PeriodNumber");
            DropForeignKey("dbo.PeriodNumber", "TypeID", "dbo.PeriodType");
            DropForeignKey("dbo.CommissionXProfessor", "HourAllocatedTypeID", "dbo.HourAllocatedType");
            DropForeignKey("dbo.CommissionXProfessor", "CommissionID", "dbo.Commission");
            DropForeignKey("dbo.StudyPlanXSede", "SedeID", "dbo.Sede");
            DropForeignKey("dbo.Group", "BlockXPlanXCourseID", "dbo.BlockXPlanXCourse");
            DropForeignKey("dbo.BlockXPlanXCourse", "CourseID", "dbo.Course");
            DropForeignKey("dbo.BlockXPlanXCourse", "BlockXPlanID", "dbo.AcademicBlockXStudyPlan");
            DropForeignKey("dbo.AcademicBlockXStudyPlan", "BlockID", "dbo.AcademicBlock");
            DropIndex("dbo.ScheduleProjectXProfessor", new[] { "ProjectXProfessor_ID" });
            DropIndex("dbo.ScheduleProjectXProfessor", new[] { "Schedule_ID" });
            DropIndex("dbo.ScheduleCommissionXProfessor", new[] { "CommissionXProfessor_ID" });
            DropIndex("dbo.ScheduleCommissionXProfessor", new[] { "Schedule_ID" });
            DropIndex("dbo.Project", new[] { "EntityTypeID" });
            DropIndex("dbo.Project", new[] { "StateID" });
            DropIndex("dbo.PlazaXProfessor", new[] { "ProfessorID" });
            DropIndex("dbo.PlazaXProfessor", new[] { "PlazaID" });
            DropIndex("dbo.Professor", new[] { "StateID" });
            DropIndex("dbo.ProjectXProfessor", new[] { "PeriodID" });
            DropIndex("dbo.ProjectXProfessor", new[] { "HourAllocatedTypeID" });
            DropIndex("dbo.ProjectXProfessor", new[] { "ProfessorID" });
            DropIndex("dbo.ProjectXProfessor", new[] { "ProjectID" });
            DropIndex("dbo.PeriodNumber", new[] { "TypeID" });
            DropIndex("dbo.Period", new[] { "NumberID" });
            DropIndex("dbo.CommissionXProfessor", new[] { "PeriodID" });
            DropIndex("dbo.CommissionXProfessor", new[] { "HourAllocatedTypeID" });
            DropIndex("dbo.CommissionXProfessor", new[] { "ProfessorID" });
            DropIndex("dbo.CommissionXProfessor", new[] { "CommissionID" });
            DropIndex("dbo.Commission", new[] { "EntityTypeID" });
            DropIndex("dbo.Commission", new[] { "StateID" });
            DropIndex("dbo.StudyPlan", new[] { "EntityTypeID" });
            DropIndex("dbo.StudyPlan", new[] { "ModeID" });
            DropIndex("dbo.StudyPlanXSede", new[] { "StudyPlanID" });
            DropIndex("dbo.StudyPlanXSede", new[] { "SedeID" });
            DropIndex("dbo.GroupClassroom", new[] { "ScheduleID" });
            DropIndex("dbo.GroupClassroom", new[] { "ClassroomID" });
            DropIndex("dbo.GroupClassroom", new[] { "GroupID" });
            DropIndex("dbo.Group", new[] { "HourAllocatedTypeID" });
            DropIndex("dbo.Group", new[] { "ProfessorID" });
            DropIndex("dbo.Group", new[] { "BlockXPlanXCourseID" });
            DropIndex("dbo.Group", new[] { "PeriodID" });
            DropIndex("dbo.BlockXPlanXCourse", new[] { "SedeID" });
            DropIndex("dbo.BlockXPlanXCourse", new[] { "CourseID" });
            DropIndex("dbo.BlockXPlanXCourse", new[] { "BlockXPlanID" });
            DropIndex("dbo.AcademicBlockXStudyPlan", new[] { "BlockID" });
            DropIndex("dbo.AcademicBlockXStudyPlan", new[] { "PlanID" });
            DropTable("dbo.ScheduleProjectXProfessor");
            DropTable("dbo.ScheduleCommissionXProfessor");
            DropTable("dbo.Modality");
            DropTable("dbo.Schedule");
            DropTable("dbo.Project");
            DropTable("dbo.State");
            DropTable("dbo.PlazaXProfessor");
            DropTable("dbo.Professor");
            DropTable("dbo.ProjectXProfessor");
            DropTable("dbo.PeriodType");
            DropTable("dbo.PeriodNumber");
            DropTable("dbo.Period");
            DropTable("dbo.CommissionXProfessor");
            DropTable("dbo.Commission");
            DropTable("dbo.EntityType");
            DropTable("dbo.StudyPlan");
            DropTable("dbo.StudyPlanXSede");
            DropTable("dbo.GroupClassroom");
            DropTable("dbo.Group");
            DropTable("dbo.BlockXPlanXCourse");
            DropTable("dbo.AcademicBlockXStudyPlan");
            DropTable("dbo.AcademicBlock");
            CreateIndex("dbo.ProyectoXProfesorHorario", "Horario_ID");
            CreateIndex("dbo.ProyectoXProfesorHorario", "ProyectoXProfesor_ID");
            CreateIndex("dbo.HorarioComisionXProfesor", "ComisionXProfesor_ID");
            CreateIndex("dbo.HorarioComisionXProfesor", "Horario_ID");
            CreateIndex("dbo.PlazaXProfesor", "ProfessorID");
            CreateIndex("dbo.PlazaXProfesor", "PlazaID");
            CreateIndex("dbo.Proyecto", "EntityTypeID");
            CreateIndex("dbo.Proyecto", "StateID");
            CreateIndex("dbo.Profesor", "StateID");
            CreateIndex("dbo.NumeroPeriodo", "TypeID");
            CreateIndex("dbo.Periodo", "NumberID");
            CreateIndex("dbo.ProyectoXProfesor", "PeriodID");
            CreateIndex("dbo.ProyectoXProfesor", "AssignProfessorTypeID");
            CreateIndex("dbo.ProyectoXProfesor", "ProfessorID");
            CreateIndex("dbo.ProyectoXProfesor", "ProjectID");
            CreateIndex("dbo.ComisionXProfesor", "PeriodID");
            CreateIndex("dbo.ComisionXProfesor", "AssignProfessorTypeID");
            CreateIndex("dbo.ComisionXProfesor", "ProfessorID");
            CreateIndex("dbo.ComisionXProfesor", "CommissionID");
            CreateIndex("dbo.Comision", "EntityTypeID");
            CreateIndex("dbo.Comision", "StateID");
            CreateIndex("dbo.PlanDeEstudioXSede", "StudyPlanID");
            CreateIndex("dbo.PlanDeEstudioXSede", "SedeID");
            CreateIndex("dbo.PlanDeEstudio", "EntityTypeID");
            CreateIndex("dbo.PlanDeEstudio", "ModeID");
            CreateIndex("dbo.BloqueAcademicoXPlanDeEstudio", "BlockID");
            CreateIndex("dbo.BloqueAcademicoXPlanDeEstudio", "PlanID");
            CreateIndex("dbo.BloqueXPlanXCurso", "SedeID");
            CreateIndex("dbo.BloqueXPlanXCurso", "CourseID");
            CreateIndex("dbo.BloqueXPlanXCurso", "BlockXPlanID");
            CreateIndex("dbo.Grupo", "AssignProfessorTypeID");
            CreateIndex("dbo.Grupo", "ProfessorID");
            CreateIndex("dbo.Grupo", "BlockXPlanXCourseID");
            CreateIndex("dbo.Grupo", "PeriodID");
            CreateIndex("dbo.GrupoAula", "ScheduleID");
            CreateIndex("dbo.GrupoAula", "ClassroomID");
            CreateIndex("dbo.GrupoAula", "GroupID");
            AddForeignKey("dbo.GrupoAula", "ScheduleID", "dbo.Horario", "ID");
            AddForeignKey("dbo.GrupoAula", "GroupID", "dbo.Grupo", "ID", cascadeDelete: true);
            AddForeignKey("dbo.Grupo", "AssignProfessorTypeID", "dbo.TipoAsignacionProfesor", "ID");
            AddForeignKey("dbo.Grupo", "ProfessorID", "dbo.Profesor", "ID");
            AddForeignKey("dbo.Grupo", "PeriodID", "dbo.Periodo", "ID", cascadeDelete: true);
            AddForeignKey("dbo.Grupo", "BlockXPlanXCourseID", "dbo.BloqueXPlanXCurso", "ID", cascadeDelete: true);
            AddForeignKey("dbo.BloqueXPlanXCurso", "SedeID", "dbo.Sede", "ID");
            AddForeignKey("dbo.BloqueXPlanXCurso", "CourseID", "dbo.Curso", "ID", cascadeDelete: true);
            AddForeignKey("dbo.BloqueXPlanXCurso", "BlockXPlanID", "dbo.BloqueAcademicoXPlanDeEstudio", "ID", cascadeDelete: true);
            AddForeignKey("dbo.BloqueAcademicoXPlanDeEstudio", "PlanID", "dbo.PlanDeEstudio", "ID", cascadeDelete: true);
            AddForeignKey("dbo.PlanDeEstudio", "EntityTypeID", "dbo.TipoEntidad", "ID");
            AddForeignKey("dbo.Comision", "EntityTypeID", "dbo.TipoEntidad", "ID");
            AddForeignKey("dbo.Comision", "StateID", "dbo.Estado", "ID");
            AddForeignKey("dbo.ComisionXProfesor", "AssignProfessorTypeID", "dbo.TipoAsignacionProfesor", "ID");
            AddForeignKey("dbo.ComisionXProfesor", "ProfessorID", "dbo.Profesor", "ID", cascadeDelete: true);
            AddForeignKey("dbo.ComisionXProfesor", "PeriodID", "dbo.Periodo", "ID", cascadeDelete: true);
            AddForeignKey("dbo.ProyectoXProfesor", "AssignProfessorTypeID", "dbo.TipoAsignacionProfesor", "ID");
            AddForeignKey("dbo.ProyectoXProfesor", "ProjectID", "dbo.Proyecto", "ID", cascadeDelete: true);
            AddForeignKey("dbo.ProyectoXProfesor", "ProfessorID", "dbo.Profesor", "ID", cascadeDelete: true);
            AddForeignKey("dbo.PlazaXProfesor", "ProfessorID", "dbo.Profesor", "ID", cascadeDelete: true);
            AddForeignKey("dbo.PlazaXProfesor", "PlazaID", "dbo.Plaza", "ID", cascadeDelete: true);
            AddForeignKey("dbo.Profesor", "StateID", "dbo.Estado", "ID");
            AddForeignKey("dbo.Proyecto", "EntityTypeID", "dbo.TipoEntidad", "ID");
            AddForeignKey("dbo.Proyecto", "StateID", "dbo.Estado", "ID");
            AddForeignKey("dbo.ProyectoXProfesor", "PeriodID", "dbo.Periodo", "ID", cascadeDelete: true);
            AddForeignKey("dbo.Periodo", "NumberID", "dbo.NumeroPeriodo", "ID", cascadeDelete: true);
            AddForeignKey("dbo.NumeroPeriodo", "TypeID", "dbo.TipoPeriodo", "ID", cascadeDelete: true);
            AddForeignKey("dbo.ProyectoXProfesorHorario", "Horario_ID", "dbo.Horario", "ID", cascadeDelete: true);
            AddForeignKey("dbo.ProyectoXProfesorHorario", "ProyectoXProfesor_ID", "dbo.ProyectoXProfesor", "ID", cascadeDelete: true);
            AddForeignKey("dbo.HorarioComisionXProfesor", "ComisionXProfesor_ID", "dbo.ComisionXProfesor", "ID", cascadeDelete: true);
            AddForeignKey("dbo.HorarioComisionXProfesor", "Horario_ID", "dbo.Horario", "ID", cascadeDelete: true);
            AddForeignKey("dbo.ComisionXProfesor", "CommissionID", "dbo.Comision", "ID", cascadeDelete: true);
            AddForeignKey("dbo.PlanDeEstudioXSede", "SedeID", "dbo.Sede", "ID", cascadeDelete: true);
            AddForeignKey("dbo.PlanDeEstudioXSede", "StudyPlanID", "dbo.PlanDeEstudio", "ID", cascadeDelete: true);
            AddForeignKey("dbo.PlanDeEstudio", "ModeID", "dbo.Modalidad", "ID", cascadeDelete: true);
            AddForeignKey("dbo.BloqueAcademicoXPlanDeEstudio", "BlockID", "dbo.BloqueAcademico", "ID", cascadeDelete: true);
            AddForeignKey("dbo.GrupoAula", "ClassroomID", "dbo.Aula", "ID");
            RenameTable(name: "dbo.Course", newName: "Curso");
            RenameTable(name: "dbo.HourAllocatedType", newName: "TipoPeriodo");
            RenameTable(name: "dbo.Classroom", newName: "Aula");
        }
    }
}
