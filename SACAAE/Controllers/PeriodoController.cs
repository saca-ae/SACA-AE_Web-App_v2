using Newtonsoft.Json;
using SACAAE.Data_Access;
using SACAAE.Models;
using SACAAE.Models.StoredProcedures;
using SACAAE.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace SACAAE.Controllers
{
    public class PeriodoController : Controller
    {
        private SACAAEContext gvDatabase = new SACAAEContext();
        private Period gPeriod = new Period();

        // GET: Periodo
        public ActionResult Index()
        {
            gPeriod = AddNewSemester();

            int vIdPeriod = getIDPeriod(gPeriod.Year, gPeriod.NumberID);
            var vResult = gvDatabase.SP_CreateGroupsinNewSemester(vIdPeriod);

            ViewBag.Period = "" + gPeriod.Year + " - " + gPeriod.NumberID + " Semestre";
            ViewBag.IdPeriod = vIdPeriod;
            return View(getGroupsList(vIdPeriod));
        }

        /// <summary>
        /// This function gives the list and the details of the groups in a given, period and entity.
        /// </summary>
        /// <author> Cristian Araya Fuentes </author> 
        /// <param name=pIdPeriod> </param>
        /// <param name=pIdEntity> </param>
        /// <returns> IQueryable<GroupsCreated> object, that contents the group's details (Group number, course name, study plan and headquarter) in the given period. </returns>
        private IQueryable<GroupsCreatedViewModel> getGroupsList(int pIdPeriod)
        {
            IQueryable<GroupsCreatedViewModel> vGroupsList = 
                from Group Gru in gvDatabase.Groups
                join BlockXPlanXCourse BPC in gvDatabase.BlocksXPlansXCourses on Gru.BlockXPlanXCourseID equals BPC.ID
                join AcademicBlockXStudyPlan BP in gvDatabase.AcademicBlocksXStudyPlans on BPC.BlockXPlanID equals BP.ID
                join StudyPlan P in gvDatabase.StudyPlans on BP.PlanID equals P.ID
                join Course C in gvDatabase.Courses on BPC.CourseID equals C.ID
                join Sede S in gvDatabase.Sedes on BPC.SedeID equals S.ID
                where Gru.PeriodID == pIdPeriod
                select new GroupsCreatedViewModel
                {
                    Grupo = Gru.Number,
                    Curso = C.Name,
                    PlandeEstudios = P.Name,
                    Sede = S.Name,
                };

            return vGroupsList;
        }

        /// <summary>
        /// Gets the new period
        /// </summary>
        /// <author> Cristian Araya Fuentes </author> 
        /// <param name=pPeriodType> Name of the period type</param>
        /// <returns></returns>
        public Period GetNextPeriod(String pPeriodType)
        {
            int vNumber = 0, vYear = 0;
            NuevoPeriodo vLastPeriod =
                (from PeriodType TP in gvDatabase.PeriodTypes
                 from PeriodNumber N in gvDatabase.PeriodNumbers
                 from Period P in gvDatabase.Periods
                 where TP.Name == pPeriodType
                 where N.ID == P.NumberID
                 where TP.ID == N.TypeID
                 orderby P.Year descending, N.Number descending
                 select new NuevoPeriodo { Number = N.Number, Year = P.Year }).FirstOrDefault();

            vNumber = vLastPeriod.Number;
            vYear = vLastPeriod.Year;

            if ((vNumber != 0) && (vYear != 0))
            {
                if (pPeriodType == "Semestre")
                {
                    if (vNumber == 2) { vNumber = 1; vYear += 1; }
                    else { vNumber = 2; }
                }
            }

            Period vPeriod = new Period();
            vPeriod.NumberID = vNumber;
            vPeriod.Year = vYear;

            return vPeriod;
        }

        public int getIDPeriodNumber(int pPeriodNumber, String pPeriodType)
        {
            return (from NumeroPeriodo in gvDatabase.PeriodNumbers
                    join TipoPeriodo in gvDatabase.PeriodTypes on NumeroPeriodo.TypeID equals TipoPeriodo.ID
                    where TipoPeriodo.Name == pPeriodType
                    where NumeroPeriodo.Number == pPeriodNumber
                    select NumeroPeriodo).FirstOrDefault().ID;
        }

        public int getIDPeriod(int pPeriodYear, int pPeriodNumberID)
        {
            return (from Period P in gvDatabase.Periods
                    where P.NumberID == pPeriodNumberID
                    where P.Year == pPeriodYear
                    select P).FirstOrDefault().ID;
        }

        public Period AddNewSemester()
        {
            Period vPeriod = GetNextPeriod("Semestre");
            AddPeriod(vPeriod);
            return vPeriod;
        }

        public void AddPeriod(Period pPeriod)
        {
            gvDatabase.Periods.Add(pPeriod);
            gvDatabase.SaveChanges();
        }

        public class NuevoPeriodo
        {
            public int Number { get; set; }
            public int Year { get; set; }
        }

        #region helpers
        private int getEntityID(string entityName)
        {
            EntityType entity;
            switch (entityName)
            {
                case "TEC":
                    entity = gvDatabase.EntityTypes.SingleOrDefault(p => p.Name == "TEC");
                    break;
                case "CIADEG":
                    entity = gvDatabase.EntityTypes.SingleOrDefault(p => p.Name == "CIADEG");
                    break;
                case "TAE":
                    entity = gvDatabase.EntityTypes.SingleOrDefault(p => p.Name == "FUNDA-TAE");
                    break;
                case "MAE":
                    entity = gvDatabase.EntityTypes.SingleOrDefault(p => p.Name == "FUNDA-MAE");
                    break;
                case "MDE":
                    entity = gvDatabase.EntityTypes.SingleOrDefault(p => p.Name == "FUNDA-MDE");
                    break;
                case "MGP":
                    entity = gvDatabase.EntityTypes.SingleOrDefault(p => p.Name == "FUNDA-MGP");
                    break;
                case "DDE":
                    entity = gvDatabase.EntityTypes.SingleOrDefault(p => p.Name == "FUNDA-Doctorado");
                    break;
                case "Emprendedores":
                    entity = gvDatabase.EntityTypes.SingleOrDefault(p => p.Name == "FUNDA-Emprendedores");
                    break;
                case "CIE":
                    entity = gvDatabase.EntityTypes.SingleOrDefault(p => p.Name == "FUNDA-CIE");
                    break;
                case "Actualizacion_Cartago":
                    entity = gvDatabase.EntityTypes.SingleOrDefault(p => p.Name == "FUNDA-Actualizacion Cartago");
                    break;
                case "Actualizacion_San_Carlos":
                    entity = gvDatabase.EntityTypes.SingleOrDefault(p => p.Name == "FUNDA-Actualizacion San Carlos");
                    break;
                default:
                    entity = gvDatabase.EntityTypes.SingleOrDefault(p => p.Name == entityName);
                    break;
            }

            return (entity != null) ? entity.ID : 0;
        }
        #endregion
    }
}