using Microsoft.AspNet.Identity.EntityFramework;
using SACAAE.Models;
using SACAAE.Models.StoredProcedures;
using SACAAE.Models.ViewModels;
using SACAAE.WebService_Models;
using System;
using System.Collections.Generic;
using System.Data;
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

        public virtual ObjectResult<resultadoStoredProcedure> SPGetIDSchedule(string pDay, string pHStart, string pHEnd)
        {
            var vDay = new SqlParameter("pDay",pDay); 
            var vHStart = new SqlParameter("pHStart",pHStart); 
            var vHEnd = new SqlParameter("pHEnd",pHEnd);

            return ((IObjectContextAdapter)this).ObjectContext.ExecuteStoreQuery<resultadoStoredProcedure>("SPGetIDSchedule @pDay, @pHStart, @pHEnd", vDay, vHStart, vHEnd);
        }

        public virtual ObjectResult<DataCSV> SP_GetGroupsPeriod(Nullable<int> pIdPeriod, Nullable<int> pIdEntity)
        {
            var vIdPeriodParameter = pIdPeriod.HasValue ?
                new SqlParameter("pIdPeriod", pIdPeriod) :
                new SqlParameter("pIdPeriod", typeof(int));

            var vIdEntityParameter = pIdEntity.HasValue ?
                new SqlParameter("pIDEntity", pIdEntity) :
                new SqlParameter("pIDEntity", typeof(int));

            return ((IObjectContextAdapter)this).ObjectContext.ExecuteStoreQuery<DataCSV>("SP_GetGroupsPeriod @pIdPeriod, @pIDEntity", vIdPeriodParameter, vIdEntityParameter);
        }

        //Ejemplo SP
        public virtual ObjectResult<CourseGroupResult> SP_GroupCourse(Nullable<int> pCourseID, Nullable<int> pSedeID, Nullable<int> pPeriodID)
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

            return ((IObjectContextAdapter)this).ObjectContext.ExecuteStoreQuery<CourseGroupResult>("SP_GroupCourse @courseID, @sedeID, @periodID", vCourseParameter, vSedeParameter, vPeriodParameter);
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
        public virtual ObjectResult<CourseGroupResult> SP_Professor_Group(Nullable<int> pCourseID, Nullable<int> pSedeID, Nullable<int> pPeriodID, Nullable<int> pProfessorID)
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

             return ((IObjectContextAdapter)this).ObjectContext.ExecuteStoreQuery<CourseGroupResult>("SP_Professor_Group @courseID, @sedeID, @periodID,@professorID", 
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

        //Stored Procedures Mobile
        public virtual ObjectResult<PeriodWSModel> SP_getAllPeriod()
        {
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteStoreQuery<PeriodWSModel>("SP_getAllPeriod");
        }

        public virtual ObjectResult<BasicInfoWSModel> SP_getAllCourses(int? periodID)
        {
            var vPeriodParameter = periodID.HasValue ?
                 new SqlParameter("periodID", periodID) :
                 new SqlParameter("periodID", periodID);

            return ((IObjectContextAdapter)this).ObjectContext.ExecuteStoreQuery<BasicInfoWSModel>("SP_getAllCourses  @periodID",
                                                                                                                    vPeriodParameter);
        }
        public virtual ObjectResult<GroupScheduleWSModel> SP_getAllCoursesPerProf(int? periodID, string profID)
        {
            var vPeriodParameter = periodID.HasValue ?
                 new SqlParameter("periodID", periodID) :
                 new SqlParameter("periodID", periodID);
            var vProfeParameter = new SqlParameter("profID", profID);

            return ((IObjectContextAdapter)this).ObjectContext.ExecuteStoreQuery<GroupScheduleWSModel>("SP_getAllCoursesPerProf  @periodID, @profID",
                                                                                                            vPeriodParameter, vProfeParameter);
        }

        public virtual ObjectResult<BasicInfoWSModel> SP_getAllProjects(int? periodID)
        {
            var vPeriodParameter = periodID.HasValue ?
                 new SqlParameter("periodID", periodID) :
                 new SqlParameter("periodID", periodID);

            return ((IObjectContextAdapter)this).ObjectContext.ExecuteStoreQuery<BasicInfoWSModel>("SP_getAllProjects  @periodID",
                                                                                                        vPeriodParameter);
        }
        public virtual ObjectResult<ProjectCommissionWSModel> SP_getAllProjectsPerProf(int? periodID, string profID)
        {
            var vPeriodParameter = periodID.HasValue ?
                 new SqlParameter("periodID", periodID) :
                 new SqlParameter("periodID", periodID);
            var vProfeParameter = new SqlParameter("profID", profID);

            return ((IObjectContextAdapter)this).ObjectContext.ExecuteStoreQuery<ProjectCommissionWSModel>("SP_getAllProjectsPerProf  @periodID, @profID",
                                                                                                                vPeriodParameter, vProfeParameter);
        }

        public virtual ObjectResult<BasicInfoWSModel> SP_getAllCommissions(int? periodID)
        {
            var vPeriodParameter = periodID.HasValue ?
                 new SqlParameter("periodID", periodID) :
                 new SqlParameter("periodID", periodID);

            return ((IObjectContextAdapter)this).ObjectContext.ExecuteStoreQuery<BasicInfoWSModel>("SP_getAllCommissions  @periodID",
                                                                                                        vPeriodParameter);
        }
        public virtual ObjectResult<ProjectCommissionWSModel> SP_getAllCommissionsPerProf(int? periodID, string profID)
        {
            var vPeriodParameter = periodID.HasValue ?
                 new SqlParameter("periodID", periodID) :
                 new SqlParameter("periodID", periodID);
            var vProfeParameter = new SqlParameter("profID", profID);

            return ((IObjectContextAdapter)this).ObjectContext.ExecuteStoreQuery<ProjectCommissionWSModel>("SP_getAllCommissionsPerProf  @periodID, @profID",
                                                                                                                vPeriodParameter, vProfeParameter);
        }

        public virtual ObjectResult<BasicInfoWSModel> SP_getAllGroups(int? periodID, int? courseID)
        {
            var vPeriodParameter = periodID.HasValue ?
                 new SqlParameter("periodID", periodID) :
                 new SqlParameter("periodID", periodID);
            var vCourseParameter = courseID.HasValue ?
                 new SqlParameter("courseID", courseID) :
                 new SqlParameter("courseID", courseID);

            return ((IObjectContextAdapter)this).ObjectContext.ExecuteStoreQuery<BasicInfoWSModel>("SP_getAllGroups  @periodID, @courseID",
                                                                                                        vPeriodParameter, vCourseParameter);
        }

        public virtual ObjectResult<ProjectCommissionWSModel> SP_getOneProject(int? periodID, int? projectID)
        {
            var vPeriodParameter = periodID.HasValue ?
                 new SqlParameter("periodID", periodID) :
                 new SqlParameter("periodID", periodID);
            var vProjectParameter = projectID.HasValue ?
                 new SqlParameter("projectID", projectID) :
                 new SqlParameter("projectID", projectID);

            return ((IObjectContextAdapter)this).ObjectContext.ExecuteStoreQuery<ProjectCommissionWSModel>("SP_getOneProject  @periodID, @projectID",
                                                                                                                vPeriodParameter, vProjectParameter);
        }

        public virtual ObjectResult<ProjectCommissionWSModel> SP_getOneCommission(int? periodID, int? commissionID)
        {
            var vPeriodParameter = periodID.HasValue ?
                 new SqlParameter("periodID", periodID) :
                 new SqlParameter("periodID", periodID);
            var vCommissionParameter = commissionID.HasValue ?
                 new SqlParameter("commissionID", commissionID) :
                 new SqlParameter("commissionID", commissionID);

            return ((IObjectContextAdapter)this).ObjectContext.ExecuteStoreQuery<ProjectCommissionWSModel>("SP_getOneCommission  @periodID, @commissionID",
                                                                                                                vPeriodParameter, vCommissionParameter);
        }

        public virtual ObjectResult<GroupWSModel> SP_getOneGroup(int? periodID, int? groupID)
        {
            var vPeriodParameter = periodID.HasValue ?
                 new SqlParameter("periodID", periodID) :
                 new SqlParameter("periodID", periodID);
            var vGroupParameter = groupID.HasValue ?
                 new SqlParameter("groupID", groupID) :
                 new SqlParameter("groupID", groupID);

            return ((IObjectContextAdapter)this).ObjectContext.ExecuteStoreQuery<GroupWSModel>("SP_getOneGroup  @periodID, @groupID",
                                                                                                    vPeriodParameter, vGroupParameter);
        }

        public virtual ObjectResult<NameWSModel> SP_GetCourses(string pStudyPlan, int? PLevel)
        {
            var vStudyPlanParameter = new SqlParameter("pStudyPlan", pStudyPlan);

            var vLevelParameter = PLevel.HasValue ?
                 new SqlParameter("PLevel", PLevel) :
                 new SqlParameter("PLevel", PLevel);

            return ((IObjectContextAdapter)this).ObjectContext.ExecuteStoreQuery<NameWSModel>("SP_GetCourses  @pStudyPlan, @PLevel",
                                                                                                        vStudyPlanParameter, vLevelParameter);
        }

        public virtual ObjectResult<PeriodInformationWSModel> SP_GetPeriodInformation(int? periodID, string pStudyPlan, int? pBlockLevel, string pCourse, string pProf)
        {
            var vPeriodParameter = periodID.HasValue ?
                 new SqlParameter("periodID", periodID) :
                 new SqlParameter("periodID", periodID);
            var vStudyPlanParameter = new SqlParameter("pStudyPlan", pStudyPlan);
            var vBlockLevelParameter = pBlockLevel.HasValue ?
                 new SqlParameter("pBlockLevel", pBlockLevel) :
                 new SqlParameter("pBlockLevel", pBlockLevel);
            var vCourseParameter = new SqlParameter("pCourse", pCourse);
            var vProfParameter = new SqlParameter("pProf", pProf);

            return ((IObjectContextAdapter)this).ObjectContext.ExecuteStoreQuery<PeriodInformationWSModel>("SP_GetPeriodInformation @periodID, @pStudyPlan, @pBlockLevel, @pCourse, @pProf",
                                                                                                                vPeriodParameter, vStudyPlanParameter, vBlockLevelParameter, vCourseParameter, vProfParameter);
        }

        public virtual ObjectResult<NameWSModel> SP_GetStudyPlan()
        {
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteStoreQuery<NameWSModel>("SP_GetStudyPlan");
        }

        public virtual ObjectResult<PeriodInformationViewModel> SP_GetCoursesXBlockXPlan(string pStudyPlan, int? pBlockLevel)
        {
            var vStudyPlanParameter = new SqlParameter("pStudyPlan", pStudyPlan);
            var vBlockLevelParameter = pBlockLevel.HasValue ?
                 new SqlParameter("pBlockLevel", pBlockLevel) :
                 new SqlParameter("pBlockLevel", pBlockLevel);

            return ((IObjectContextAdapter)this).ObjectContext.ExecuteStoreQuery<PeriodInformationViewModel>("SP_GetCoursesXBlockXPlan @pStudyPlan, @pBlockLevel",
                                                                                                        vStudyPlanParameter, vBlockLevelParameter);
        }

        public virtual ObjectResult<NameWSModel> SP_GetProfessor(string pCourse)
        {
            var vCourseParameter = new SqlParameter("pCourse", pCourse);

            return ((IObjectContextAdapter)this).ObjectContext.ExecuteStoreQuery<NameWSModel>("SP_GetProfessor @pCourse", vCourseParameter);
        }
    }
}
/*
 * Rollback database
 * update-database -target:0
 */