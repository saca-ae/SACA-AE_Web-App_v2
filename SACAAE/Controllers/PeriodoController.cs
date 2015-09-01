using Newtonsoft.Json;
using SACAAE.Data_Access;
using SACAAE.Models;
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
        //private SACAAE_SP gvStoredProcedure = new SACAAE_SP();
        private Period gPeriod = new Period();

        // GET: Periodo
        public ActionResult Index()
        {
            gPeriod = gPeriod.AddNewSemester();
            
            int vIdPeriod = gPeriod.getIDPeriod(gPeriod.Year, gPeriod.NumberID);
            //gvStoredProcedure.SP_CreateGroupsinNewSemester(vIdPeriod);

            IQueryable<GroupsCreatedViewModel> vGroupsList = getGroupsList(vIdPeriod);

            ViewBag.Period = "" + gPeriod.Year + " - " + gPeriod.NumberID + " Semestre";
            ViewBag.IdPeriod = vIdPeriod;
            ViewBag.Grupos = vGroupsList; 
            return View(vGroupsList.ToList());
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