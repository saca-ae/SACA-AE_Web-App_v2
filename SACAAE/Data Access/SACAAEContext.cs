using Microsoft.AspNet.Identity.EntityFramework;
using SACAAE.Models;
using SACAAE.Models.StoredProcedures;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Core.Objects;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Data.SqlClient;
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

        public virtual ObjectResult<resultadoStoredProcedure> SP_CreateGroupsinNewSemester(Nullable<int> pIdPeriod)
        {
            var vIdPeriodParameter = pIdPeriod.HasValue ?
                new SqlParameter("pIdPeriod", pIdPeriod) :
                new SqlParameter("pIdPeriod", typeof(int));

            return ((IObjectContextAdapter)this).ObjectContext.ExecuteStoreQuery<resultadoStoredProcedure>("SP_CreateGroupsinNewSemester @pIdPeriod", vIdPeriodParameter);
        }

        //Ejemplo SP
        public virtual ObjectResult<ejemplo> SP_Ejemplo(Nullable<int> pCourseID, Nullable<int> pSedeID, Nullable<int> pPeriodID)
        {
            var vCourseParameter = pCourseID.HasValue ?
                new SqlParameter("courseID", pCourseID) :
                new SqlParameter("courseID", typeof(int));
            var vSedeParameter = pSedeID.HasValue ?
                new SqlParameter("sedeID", pSedeID) :
                new SqlParameter("sedeID", typeof(int));
            var vPeriodParameter = pPeriodID.HasValue ?
                new SqlParameter("periodID", pPeriodID) :
                new SqlParameter("periodID", typeof(int));

            return ((IObjectContextAdapter)this).ObjectContext.ExecuteStoreQuery<ejemplo>("SP_Ejemplo @courseID, @sedeID, @periodID", vCourseParameter, vSedeParameter, vPeriodParameter);
        }

        /// <summary>
        /// Store Procedure return a information about a group according to CourseID, pSedeID,PeriodID and ProfessorID
        /// </summary>
        /// <autor>Esteban Segura Benavides</autor>
        /// <param name="pIDCurso">ID Course in database</param>
        /// <param name="pIDSede">ID Sede in database</param>
        ///<param name="pIDPeriod">ID Period in database</param>
        /// <param name="pIDProfessor">ID Professor in database</param>
        /// <returns>ID, Number of Group, Name of Profesor, Code of Aula and StartHour, EndHour and Day of Schedule</returns>
         public virtual ObjectResult<ejemplo> SP_Professor_Group(Nullable<int> pCourseID, Nullable<int> pSedeID, Nullable<int> pPeriodID, Nullable<int> pProfessorID)
        {
            var vCourseParameter = pCourseID.HasValue ?
                new SqlParameter("courseID", pCourseID) :
                new SqlParameter("courseID", typeof(int));
            var vSedeParameter = pSedeID.HasValue ?
                new SqlParameter("sedeID", pSedeID) :
                new SqlParameter("sedeID", typeof(int));
            var vPeriodParameter = pPeriodID.HasValue ?
                new SqlParameter("periodID", pPeriodID) :
                new SqlParameter("periodID", typeof(int));
             var vProfessorParameter = pProfessorID.HasValue ?
                 new SqlParameter("professorID", pProfessorID):
                 new SqlParameter("professorID",pProfessorID);

             return ((IObjectContextAdapter)this).ObjectContext.ExecuteStoreQuery<ejemplo>("SP_Professor_Group @courseID, @sedeID, @periodID,@professorID", 
                                                                                            vCourseParameter, vSedeParameter, vPeriodParameter,vProfessorParameter);
        }

        /// <summary>
        /// Store Procedure return courses related to a professor
        /// </summary>
        /// <autor>Esteban Segura Benavides</autor>
        /// <param name="pIDProfessor">ID Professor in database</param>
        /// <returns>Course.*</returns>
         public virtual ObjectResult<Course> SP_Professor_Course(Nullable<int> pPeriodID, Nullable<int> pProfessorID)
         {
             var vPeriodParameter = pPeriodID.HasValue ?
                new SqlParameter("periodID", pPeriodID) :
                new SqlParameter("periodID", typeof(int));

             var vProfessorParameter = pProfessorID.HasValue ?
                  new SqlParameter("professorID", pProfessorID) :
                  new SqlParameter("professorID", pProfessorID);

             return ((IObjectContextAdapter)this).ObjectContext.ExecuteStoreQuery<Course>("SP_Professor_Course  @periodID,@professorID",
                                                                                             vPeriodParameter, vProfessorParameter);
         }

    }
}
/*
 * Rollback database
 * update-database -target:0
 */