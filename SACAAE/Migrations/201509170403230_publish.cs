namespace SACAAE.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class publish : DbMigration
    {
        public override void Up()
        {
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
                "dbo.Course",
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
                        Temporal = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.ID);
            
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
                "dbo.Classroom",
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
                "dbo.Sede",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        GroupEnum = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ID);
            
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
                "dbo.HourAllocatedType",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.ID);
            
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
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.AspNetUserRoles", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserLogins", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserClaims", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserRoles", "RoleId", "dbo.AspNetRoles");
            DropForeignKey("dbo.AspNetUserRoles", "Role_Id", "dbo.AspNetRoles");
            DropForeignKey("dbo.AcademicBlockXStudyPlan", "PlanID", "dbo.StudyPlan");
            DropForeignKey("dbo.BlockXPlanXCourse", "SedeID", "dbo.Sede");
            DropForeignKey("dbo.Group", "ProfessorID", "dbo.Professor");
            DropForeignKey("dbo.Group", "PeriodID", "dbo.Period");
            DropForeignKey("dbo.Group", "HourAllocatedTypeID", "dbo.HourAllocatedType");
            DropForeignKey("dbo.GroupClassroom", "ScheduleID", "dbo.Schedule");
            DropForeignKey("dbo.GroupClassroom", "GroupID", "dbo.Group");
            DropForeignKey("dbo.GroupClassroom", "ClassroomID", "dbo.Classroom");
            DropForeignKey("dbo.Classroom", "SedeID", "dbo.Sede");
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
            DropIndex("dbo.AspNetUserLogins", new[] { "UserId" });
            DropIndex("dbo.AspNetUserClaims", new[] { "UserId" });
            DropIndex("dbo.AspNetUsers", "UserNameIndex");
            DropIndex("dbo.AspNetUserRoles", new[] { "Role_Id" });
            DropIndex("dbo.AspNetUserRoles", new[] { "RoleId" });
            DropIndex("dbo.AspNetUserRoles", new[] { "UserId" });
            DropIndex("dbo.AspNetRoles", "RoleNameIndex");
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
            DropIndex("dbo.Classroom", new[] { "SedeID" });
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
            DropTable("dbo.AspNetUserLogins");
            DropTable("dbo.AspNetUserClaims");
            DropTable("dbo.AspNetUsers");
            DropTable("dbo.AspNetUserRoles");
            DropTable("dbo.AspNetRoles");
            DropTable("dbo.Modality");
            DropTable("dbo.Schedule");
            DropTable("dbo.Project");
            DropTable("dbo.State");
            DropTable("dbo.Plaza");
            DropTable("dbo.PlazaXProfessor");
            DropTable("dbo.Professor");
            DropTable("dbo.ProjectXProfessor");
            DropTable("dbo.PeriodType");
            DropTable("dbo.PeriodNumber");
            DropTable("dbo.Period");
            DropTable("dbo.HourAllocatedType");
            DropTable("dbo.CommissionXProfessor");
            DropTable("dbo.Commission");
            DropTable("dbo.EntityType");
            DropTable("dbo.StudyPlan");
            DropTable("dbo.StudyPlanXSede");
            DropTable("dbo.Sede");
            DropTable("dbo.Classroom");
            DropTable("dbo.GroupClassroom");
            DropTable("dbo.Group");
            DropTable("dbo.Course");
            DropTable("dbo.BlockXPlanXCourse");
            DropTable("dbo.AcademicBlockXStudyPlan");
            DropTable("dbo.AcademicBlock");
        }
    }
}
