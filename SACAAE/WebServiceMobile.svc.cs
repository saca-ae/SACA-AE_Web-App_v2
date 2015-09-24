using SACAAE.Data_Access;
using SACAAE.WebService_Models;
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;
using System.Web.Mvc;
using System.Net;
using Newtonsoft.Json;
using SACAAE.Models.ViewModels;

namespace SACAAE
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "WebServiceMobile" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select WebServiceMobile.svc or WebServiceMobile.svc.cs at the Solution Explorer and start debugging.
    [System.ServiceModel.ServiceBehavior(IncludeExceptionDetailInFaults = true)]
    public class WebServiceMobile : IWebServiceMobile
    {
        private SACAAEContext db = new SACAAEContext();
        private const string MOVIL_CODE = "SACAAEProfesor";

        public bool LogIn(string pPassword)
        {
            var result = false;
            if (pPassword.Equals(MOVIL_CODE))
            {
                result = true;
            }

            return result;
        }

        public string LogInUser(string pUser, string pPassword)
        {
            var professors = db.Professors.Where(p => p.Email == pUser).ToList();
            var result = "";

            if(professors.Count > 0)
            {
                if (pPassword.Equals(MOVIL_CODE))
                {
                    result = professors[0].Name;
                }
            }

            return result;
        }

        public List<GroupScheduleWSModel> getCoursesPerProfe(string pPeriod, string pProfessor)
        {
            var period = int.Parse(pPeriod);
            var result = db.SP_getAllCoursesPerProf(period, pProfessor).ToList();

            return result;
                }

        public List<ProjectCommissionWSModel> getProjectsPerProfe(string pPeriod, string pProfessor)
        {
            var period = int.Parse(pPeriod);
            var result = db.SP_getAllProjectsPerProf(period, pProfessor).ToList();

            return result;
            }

        public List<ProjectCommissionWSModel> getCommissionsPerProfe(string pPeriod, string pProfessor)
        {
            var period = int.Parse(pPeriod);
            var result = db.SP_getAllCommissionsPerProf(period, pProfessor).ToList();

            return result;
        }

        public IQueryable<PeriodoViewModel> getPeriods()
        {
            return db.Periods.Select(p => new PeriodoViewModel
            {
                ID = p.ID,
                Name = (p.Year + " - " + p.Number.Type.Name + " " + p.Number.Number)
            });
        }

        public List<BasicInfoWSModel> getCourses(string pPeriod)
        {
            var period = int.Parse(pPeriod);
            var result = db.SP_getAllCourses(period).ToList();

            return result;
        }

        public List<BasicInfoWSModel> getProjects(string pPeriod)
        {
            var period = int.Parse(pPeriod);
            var result = db.SP_getAllProjects(period).ToList();

            return result;
        }

        public List<BasicInfoWSModel> getCommissions(string pPeriod)
        {
            var period = int.Parse(pPeriod);
            var result = db.SP_getAllCommissions(period).ToList();

            return result;
        }

        public List<BasicInfoWSModel> getGroups(string pPeriod, string pCourse)
        {
            var period = int.Parse(pPeriod);
            var course = int.Parse(pCourse);
            var result = db.SP_getAllGroups(period, course).ToList();

            return result;
        }

        public List<ProjectCommissionWSModel> getOneProject(string pPeriod, string pProject)
        {
            var period = int.Parse(pPeriod);
            var project = int.Parse(pProject);
            var result = db.SP_getOneProject(period, project).ToList();

            return result;
        }

        public List<ProjectCommissionWSModel> getOneCommission(string pPeriod, string pCommission)
        {
            var period = int.Parse(pPeriod);
            var commission = int.Parse(pCommission);
            var result = db.SP_getOneCommission(period, commission).ToList();

            return result;
        }

        public List<GroupWSModel> getOneGroup(string pPeriod, string pGroup)
        {
            var period = int.Parse(pPeriod);
            var group = int.Parse(pGroup);
            var result = db.SP_getOneGroup(period, group).ToList();

            return result;
        }

        //
        public List<NameWSModel> getCoursesName(string pStudyPlan, string pBlockNumber)
        {
            var blockNumber = int.Parse(pBlockNumber);
            var StudyPlan = int.Parse(pStudyPlan);
            var result = db.SP_GetCourses(StudyPlan, blockNumber).ToList();

            return result;
        }

        public List<PeriodInformationWSModel> getPeriodInformation(string pPeriod, string pStudyPlan, string pBlockLevel, string pCourse, string pProfessor)
        {
            var period = int.Parse(pPeriod);
            var blockLevel = int.Parse(pBlockLevel);
            var vStudyPlan = int.Parse(pStudyPlan);
            var vCourse = int.Parse(pCourse);
            var vProfessor = int.Parse(pProfessor);
            return db.SP_GetPeriodInformation(period, vStudyPlan, blockLevel, vCourse, vProfessor).ToList();
        }

        public List<BasicInfoWSModel> getStudyPlan()
        {
            var result = db.SP_GetStudyPlan().ToList();
            
            return result;
        }

        public List<BasicInfoWSModel> getProfessors(string pCourse)
        {
            var vCourse = int.Parse(pCourse);
            return db.SP_GetProfessor(vCourse).ToList();
        }

        public List<NameWSModel> getCoursesXBlockXPlan(string pStudyPlan, string pBlockLevel)
        {
            var blockLevel = int.Parse(pBlockLevel);
            var vStudyPlan = int.Parse(pStudyPlan);
            return db.SP_GetCoursesXBlockXPlan(vStudyPlan, blockLevel).ToList();
        }
    }
}
