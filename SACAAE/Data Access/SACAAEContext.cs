using Microsoft.AspNet.Identity.EntityFramework;
using SACAAE.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Core.Objects;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;
using System.Web;

namespace SACAAE.Data_Access
{
    public class SACAAEContext : IdentityDbContext<User>
    {
        public SACAAEContext()
            : base("DefaultConnection", throwIfV1Schema: false)
        {
        }

        public static SACAAEContext Create()
        {
            return new SACAAEContext();
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
            base.OnModelCreating(modelBuilder);
        }

        //Tables
        public DbSet<Classroom> Classrooms { get; set; }
        public DbSet<AcademicBlock> AcademicBlocks { get; set; }
        public DbSet<AcademicBlockXStudyPlan> AcademicBlocksXStudyPlans { get; set; }
        public DbSet<BlockXPlanXCourse> BlocksXPlansXCourses { get; set; }
        public DbSet<Commission> Commissions { get; set; }
        public DbSet<CommissionXProfessor> CommissionsXProfessors { get; set; }
        public DbSet<Course> Courses { get; set; }
        public DbSet<State> States { get; set; }
        public DbSet<Group> Groups { get; set; }
        public DbSet<Schedule> Schedules { get; set; }
        public DbSet<Modality> Modalities { get; set; }
        public DbSet<Period> Periods { get; set; }
        public DbSet<PeriodType> PeriodTypes { get; set; }
        public DbSet<PeriodNumber> PeriodNumbers { get; set; }
        public DbSet<StudyPlan> StudyPlans { get; set; }
        public DbSet<StudyPlanXSede> StudyPlansXSedes { get; set; }
        public DbSet<Project> Projects { get; set; }
        public DbSet<ProjectXProfessor> ProjectsXProfessors { get; set; }
        public DbSet<Sede> Sedes { get; set; }
        public DbSet<Plaza> Plazas { get; set; }
        public DbSet<PlazaXProfessor> PlazasXProfessors { get; set; }
        public DbSet<Professor> Professors { get; set; }
        public DbSet<EntityType> EntityTypes { get; set; }
        public DbSet<HourAllocatedType> HourAllocatedTypes { get; set; }
        public DbSet<GroupClassroom> GroupClassrooms { get; set; }

        public virtual ObjectResult<Nullable<int>> SP_CreateGroupsinNewSemester(Nullable<int> pIdPeriod)
        {
            var pIdPeriodParameter = pIdPeriod.HasValue ?
                new ObjectParameter("pIdPeriod", pIdPeriod) :
                new ObjectParameter("pIdPeriod", typeof(int));

            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<Nullable<int>>("SP_CreateGroupsinNewSemester", pIdPeriodParameter);
        }
    }
}
/*
 * Rollback database
 * update-database -target:0
 */