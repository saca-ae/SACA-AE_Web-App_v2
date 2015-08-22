namespace SACAAE.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class init : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Aula",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        SedeID = c.Int(nullable: false),
                        Code = c.String(),
                        Capacity = c.Int(nullable: false),
                        Active = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Sede", t => t.SedeID, cascadeDelete: true)
                .Index(t => t.SedeID);
            
            CreateTable(
                "dbo.GrupoAula",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Group = c.Int(nullable: false),
                        Classroom = c.Int(),
                        Schedule = c.Int(),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Aula", t => t.Classroom)
                .ForeignKey("dbo.DetalleGrupo", t => t.Group, cascadeDelete: true)
                .ForeignKey("dbo.Horario", t => t.Schedule)
                .Index(t => t.Group)
                .Index(t => t.Classroom)
                .Index(t => t.Schedule);
            
            CreateTable(
                "dbo.DetalleGrupo",
                c => new
                    {
                        ID = c.Int(nullable: false),
                        Professor = c.Int(),
                        Capacity = c.Int(),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Grupo", t => t.ID)
                .ForeignKey("dbo.ProfesorXCurso", t => t.Professor)
                .Index(t => t.ID)
                .Index(t => t.Professor);
            
            CreateTable(
                "dbo.Grupo",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Number = c.Int(nullable: false),
                        Period = c.Int(nullable: false),
                        BlockXPlanXCourse = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.BloqueXPlanXCurso", t => t.BlockXPlanXCourse, cascadeDelete: true)
                .ForeignKey("dbo.Periodo", t => t.Period, cascadeDelete: true)
                .Index(t => t.Period)
                .Index(t => t.BlockXPlanXCourse);
            
            CreateTable(
                "dbo.BloqueXPlanXCurso",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        BlockXPlanID = c.Int(nullable: false),
                        CourseID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.BloqueAcademicoXPlanDeEstudio", t => t.BlockXPlanID, cascadeDelete: true)
                .ForeignKey("dbo.Curso", t => t.CourseID, cascadeDelete: true)
                .Index(t => t.BlockXPlanID)
                .Index(t => t.CourseID);
            
            CreateTable(
                "dbo.BloqueAcademicoXPlanDeEstudio",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        PlanID = c.Int(nullable: false),
                        BlockID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.BloqueAcademico", t => t.BlockID, cascadeDelete: true)
                .ForeignKey("dbo.PlanDeEstudio", t => t.PlanID, cascadeDelete: true)
                .Index(t => t.PlanID)
                .Index(t => t.BlockID);
            
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
                "dbo.PlanDeEstudio",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Mode = c.Int(nullable: false),
                        EntityType = c.Int(),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Modalidad", t => t.Mode, cascadeDelete: true)
                .ForeignKey("dbo.TipoEntidad", t => t.EntityType)
                .Index(t => t.Mode)
                .Index(t => t.EntityType);
            
            CreateTable(
                "dbo.Modalidad",
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
                        StudyPlan = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.PlanDeEstudio", t => t.StudyPlan, cascadeDelete: true)
                .ForeignKey("dbo.Sede", t => t.SedeID, cascadeDelete: true)
                .Index(t => t.SedeID)
                .Index(t => t.StudyPlan);
            
            CreateTable(
                "dbo.Sede",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Name = c.String(),
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
                "dbo.Comision",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Start = c.DateTime(nullable: false),
                        End = c.DateTime(nullable: false),
                        State = c.Int(),
                        EntityType = c.Int(),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Estado", t => t.State)
                .ForeignKey("dbo.TipoEntidad", t => t.EntityType)
                .Index(t => t.State)
                .Index(t => t.EntityType);
            
            CreateTable(
                "dbo.ComisionXProfesor",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Commission = c.Int(nullable: false),
                        Professor = c.Int(nullable: false),
                        Period = c.Int(nullable: false),
                        Schedule = c.Int(),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Comision", t => t.Commission, cascadeDelete: true)
                .ForeignKey("dbo.Horario", t => t.Schedule)
                .ForeignKey("dbo.Periodo", t => t.Period, cascadeDelete: true)
                .ForeignKey("dbo.Profesor", t => t.Professor, cascadeDelete: true)
                .Index(t => t.Commission)
                .Index(t => t.Professor)
                .Index(t => t.Period)
                .Index(t => t.Schedule);
            
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
                "dbo.ProyectoXProfesor",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Project = c.Int(nullable: false),
                        Professor = c.Int(nullable: false),
                        Period = c.Int(nullable: false),
                        Schedule = c.Int(),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Horario", t => t.Schedule)
                .ForeignKey("dbo.Periodo", t => t.Period, cascadeDelete: true)
                .ForeignKey("dbo.Profesor", t => t.Professor, cascadeDelete: true)
                .ForeignKey("dbo.Proyecto", t => t.Project, cascadeDelete: true)
                .Index(t => t.Project)
                .Index(t => t.Professor)
                .Index(t => t.Period)
                .Index(t => t.Schedule);
            
            CreateTable(
                "dbo.Periodo",
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
                        State = c.Int(),
                        Tel1 = c.String(),
                        Tel2 = c.String(),
                        Email = c.String(),
                        Password = c.String(),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Estado", t => t.State)
                .Index(t => t.State);
            
            CreateTable(
                "dbo.Estado",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Name = c.String(),
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
                        State = c.Int(),
                        Link = c.String(),
                        EntityType = c.Int(),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Estado", t => t.State)
                .ForeignKey("dbo.TipoEntidad", t => t.EntityType)
                .Index(t => t.State)
                .Index(t => t.EntityType);
            
            CreateTable(
                "dbo.PlazaXProfesor",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        PlazaID = c.Int(nullable: false),
                        Professor = c.Int(nullable: false),
                        PercentHours = c.Int(nullable: false),
                        PropertyHours = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Plaza", t => t.PlazaID, cascadeDelete: true)
                .ForeignKey("dbo.Profesor", t => t.Professor, cascadeDelete: true)
                .Index(t => t.PlazaID)
                .Index(t => t.Professor);
            
            CreateTable(
                "dbo.Plaza",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Code = c.String(),
                        PlazaType = c.String(),
                        TimeType = c.String(),
                        TotalHours = c.Int(),
                        EffectiveTime = c.Int(),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.ProfesorXCurso",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Professor = c.Int(nullable: false),
                        Hours = c.Int(),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Profesor", t => t.Professor, cascadeDelete: true)
                .Index(t => t.Professor);
            
            CreateTable(
                "dbo.Curso",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Code = c.String(),
                        TheoreticalHours = c.Int(nullable: false),
                        Block = c.Int(nullable: false),
                        External = c.Boolean(nullable: false),
                        PracticeHours = c.Int(),
                        Credits = c.Int(),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.AspNetRoles",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Name = c.String(nullable: false, maxLength: 256),
                        Description = c.String(),
                        Discriminator = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.Name, unique: true, name: "RoleNameIndex");
            
            CreateTable(
                "dbo.AspNetUserRoles",
                c => new
                    {
                        UserId = c.String(nullable: false, maxLength: 128),
                        RoleId = c.String(nullable: false, maxLength: 128),
                        Discriminator = c.String(nullable: false, maxLength: 128),
                        Role_Id = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => new { t.UserId, t.RoleId })
                .ForeignKey("dbo.AspNetRoles", t => t.Role_Id)
                .ForeignKey("dbo.AspNetRoles", t => t.RoleId, cascadeDelete: true)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId)
                .Index(t => t.RoleId)
                .Index(t => t.Role_Id);
            
            CreateTable(
                "dbo.AspNetUsers",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Name = c.String(),
                        Email = c.String(maxLength: 256),
                        EmailConfirmed = c.Boolean(nullable: false),
                        PasswordHash = c.String(),
                        SecurityStamp = c.String(),
                        PhoneNumber = c.String(),
                        PhoneNumberConfirmed = c.Boolean(nullable: false),
                        TwoFactorEnabled = c.Boolean(nullable: false),
                        LockoutEndDateUtc = c.DateTime(),
                        LockoutEnabled = c.Boolean(nullable: false),
                        AccessFailedCount = c.Int(nullable: false),
                        UserName = c.String(nullable: false, maxLength: 256),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.UserName, unique: true, name: "UserNameIndex");
            
            CreateTable(
                "dbo.AspNetUserClaims",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserId = c.String(nullable: false, maxLength: 128),
                        ClaimType = c.String(),
                        ClaimValue = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.AspNetUserLogins",
                c => new
                    {
                        LoginProvider = c.String(nullable: false, maxLength: 128),
                        ProviderKey = c.String(nullable: false, maxLength: 128),
                        UserId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.LoginProvider, t.ProviderKey, t.UserId })
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.AspNetUserRoles", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserLogins", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserClaims", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserRoles", "RoleId", "dbo.AspNetRoles");
            DropForeignKey("dbo.AspNetUserRoles", "Role_Id", "dbo.AspNetRoles");
            DropForeignKey("dbo.Aula", "SedeID", "dbo.Sede");
            DropForeignKey("dbo.GrupoAula", "Schedule", "dbo.Horario");
            DropForeignKey("dbo.GrupoAula", "Group", "dbo.DetalleGrupo");
            DropForeignKey("dbo.DetalleGrupo", "Professor", "dbo.ProfesorXCurso");
            DropForeignKey("dbo.DetalleGrupo", "ID", "dbo.Grupo");
            DropForeignKey("dbo.Grupo", "Period", "dbo.Periodo");
            DropForeignKey("dbo.Grupo", "BlockXPlanXCourse", "dbo.BloqueXPlanXCurso");
            DropForeignKey("dbo.BloqueXPlanXCurso", "CourseID", "dbo.Curso");
            DropForeignKey("dbo.BloqueXPlanXCurso", "BlockXPlanID", "dbo.BloqueAcademicoXPlanDeEstudio");
            DropForeignKey("dbo.BloqueAcademicoXPlanDeEstudio", "PlanID", "dbo.PlanDeEstudio");
            DropForeignKey("dbo.PlanDeEstudio", "EntityType", "dbo.TipoEntidad");
            DropForeignKey("dbo.Comision", "EntityType", "dbo.TipoEntidad");
            DropForeignKey("dbo.Comision", "State", "dbo.Estado");
            DropForeignKey("dbo.ComisionXProfesor", "Professor", "dbo.Profesor");
            DropForeignKey("dbo.ComisionXProfesor", "Period", "dbo.Periodo");
            DropForeignKey("dbo.ComisionXProfesor", "Schedule", "dbo.Horario");
            DropForeignKey("dbo.ProyectoXProfesor", "Project", "dbo.Proyecto");
            DropForeignKey("dbo.ProyectoXProfesor", "Professor", "dbo.Profesor");
            DropForeignKey("dbo.ProfesorXCurso", "Professor", "dbo.Profesor");
            DropForeignKey("dbo.PlazaXProfesor", "Professor", "dbo.Profesor");
            DropForeignKey("dbo.PlazaXProfesor", "PlazaID", "dbo.Plaza");
            DropForeignKey("dbo.Profesor", "State", "dbo.Estado");
            DropForeignKey("dbo.Proyecto", "EntityType", "dbo.TipoEntidad");
            DropForeignKey("dbo.Proyecto", "State", "dbo.Estado");
            DropForeignKey("dbo.ProyectoXProfesor", "Period", "dbo.Periodo");
            DropForeignKey("dbo.ProyectoXProfesor", "Schedule", "dbo.Horario");
            DropForeignKey("dbo.ComisionXProfesor", "Commission", "dbo.Comision");
            DropForeignKey("dbo.PlanDeEstudioXSede", "SedeID", "dbo.Sede");
            DropForeignKey("dbo.PlanDeEstudioXSede", "StudyPlan", "dbo.PlanDeEstudio");
            DropForeignKey("dbo.PlanDeEstudio", "Mode", "dbo.Modalidad");
            DropForeignKey("dbo.BloqueAcademicoXPlanDeEstudio", "BlockID", "dbo.BloqueAcademico");
            DropForeignKey("dbo.GrupoAula", "Classroom", "dbo.Aula");
            DropIndex("dbo.AspNetUserLogins", new[] { "UserId" });
            DropIndex("dbo.AspNetUserClaims", new[] { "UserId" });
            DropIndex("dbo.AspNetUsers", "UserNameIndex");
            DropIndex("dbo.AspNetUserRoles", new[] { "Role_Id" });
            DropIndex("dbo.AspNetUserRoles", new[] { "RoleId" });
            DropIndex("dbo.AspNetUserRoles", new[] { "UserId" });
            DropIndex("dbo.AspNetRoles", "RoleNameIndex");
            DropIndex("dbo.ProfesorXCurso", new[] { "Professor" });
            DropIndex("dbo.PlazaXProfesor", new[] { "Professor" });
            DropIndex("dbo.PlazaXProfesor", new[] { "PlazaID" });
            DropIndex("dbo.Proyecto", new[] { "EntityType" });
            DropIndex("dbo.Proyecto", new[] { "State" });
            DropIndex("dbo.Profesor", new[] { "State" });
            DropIndex("dbo.ProyectoXProfesor", new[] { "Schedule" });
            DropIndex("dbo.ProyectoXProfesor", new[] { "Period" });
            DropIndex("dbo.ProyectoXProfesor", new[] { "Professor" });
            DropIndex("dbo.ProyectoXProfesor", new[] { "Project" });
            DropIndex("dbo.ComisionXProfesor", new[] { "Schedule" });
            DropIndex("dbo.ComisionXProfesor", new[] { "Period" });
            DropIndex("dbo.ComisionXProfesor", new[] { "Professor" });
            DropIndex("dbo.ComisionXProfesor", new[] { "Commission" });
            DropIndex("dbo.Comision", new[] { "EntityType" });
            DropIndex("dbo.Comision", new[] { "State" });
            DropIndex("dbo.PlanDeEstudioXSede", new[] { "StudyPlan" });
            DropIndex("dbo.PlanDeEstudioXSede", new[] { "SedeID" });
            DropIndex("dbo.PlanDeEstudio", new[] { "EntityType" });
            DropIndex("dbo.PlanDeEstudio", new[] { "Mode" });
            DropIndex("dbo.BloqueAcademicoXPlanDeEstudio", new[] { "BlockID" });
            DropIndex("dbo.BloqueAcademicoXPlanDeEstudio", new[] { "PlanID" });
            DropIndex("dbo.BloqueXPlanXCurso", new[] { "CourseID" });
            DropIndex("dbo.BloqueXPlanXCurso", new[] { "BlockXPlanID" });
            DropIndex("dbo.Grupo", new[] { "BlockXPlanXCourse" });
            DropIndex("dbo.Grupo", new[] { "Period" });
            DropIndex("dbo.DetalleGrupo", new[] { "Professor" });
            DropIndex("dbo.DetalleGrupo", new[] { "ID" });
            DropIndex("dbo.GrupoAula", new[] { "Schedule" });
            DropIndex("dbo.GrupoAula", new[] { "Classroom" });
            DropIndex("dbo.GrupoAula", new[] { "Group" });
            DropIndex("dbo.Aula", new[] { "SedeID" });
            DropTable("dbo.AspNetUserLogins");
            DropTable("dbo.AspNetUserClaims");
            DropTable("dbo.AspNetUsers");
            DropTable("dbo.AspNetUserRoles");
            DropTable("dbo.AspNetRoles");
            DropTable("dbo.Curso");
            DropTable("dbo.ProfesorXCurso");
            DropTable("dbo.Plaza");
            DropTable("dbo.PlazaXProfesor");
            DropTable("dbo.Proyecto");
            DropTable("dbo.Estado");
            DropTable("dbo.Profesor");
            DropTable("dbo.Periodo");
            DropTable("dbo.ProyectoXProfesor");
            DropTable("dbo.Horario");
            DropTable("dbo.ComisionXProfesor");
            DropTable("dbo.Comision");
            DropTable("dbo.TipoEntidad");
            DropTable("dbo.Sede");
            DropTable("dbo.PlanDeEstudioXSede");
            DropTable("dbo.Modalidad");
            DropTable("dbo.PlanDeEstudio");
            DropTable("dbo.BloqueAcademico");
            DropTable("dbo.BloqueAcademicoXPlanDeEstudio");
            DropTable("dbo.BloqueXPlanXCurso");
            DropTable("dbo.Grupo");
            DropTable("dbo.DetalleGrupo");
            DropTable("dbo.GrupoAula");
            DropTable("dbo.Aula");
        }
    }
}
