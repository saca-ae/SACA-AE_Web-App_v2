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

        public string getCourses(string pPeriod)
        {
            var period = int.Parse(pPeriod);
            var result = db.SP_getAllCourses(period).ToList();

            return JsonConvert.SerializeObject(result);
        }

        public string getProjects(string pPeriod)
        {
            var period = int.Parse(pPeriod);
            var result = db.SP_getAllProjects(period).ToList();

            return JsonConvert.SerializeObject(result);
        }

        public string getCommissions(string pPeriod)
        {
            var period = int.Parse(pPeriod);
            var result = db.SP_getAllCommissions(period).ToList();

            return JsonConvert.SerializeObject(result);
        }

        public string getGroups(string pPeriod, string pCourse)
        {
            var period = int.Parse(pPeriod);
            var course = int.Parse(pCourse);
            var result = db.SP_getAllGroups(period, course).ToList();

            return JsonConvert.SerializeObject(result);
        }

        public string getOneProject(string pPeriod, string pProject)
        {
            var period = int.Parse(pPeriod);
            var project = int.Parse(pProject);
            var result = db.SP_getOneProject(period, project).ToList();

            return JsonConvert.SerializeObject(result);
        }

        public string getOneCommission(string pPeriod, string pCommission)
        {
            var period = int.Parse(pPeriod);
            var commission = int.Parse(pCommission);
            var result = db.SP_getOneCommission(period, commission).ToList();

            return JsonConvert.SerializeObject(result);
        }

        public string getOneGroup(string pPeriod, string pGroup)
        {
            var period = int.Parse(pPeriod);
            var group = int.Parse(pGroup);
            var result = db.SP_getOneGroup(period, group).ToList();

            return JsonConvert.SerializeObject(result);
        }

        public List<NameWSModel> getStudyPlan()
        {
            return db.SP_GetStudyPlan().ToList();
        }

        public List<NameWSModel> getProfessors(string pCourse)
        {
            return db.SP_GetProfessor(pCourse).ToList();
        }

        public IQueryable<PeriodoViewModel> getPeriods()
        {
            return db.Periods.Select(p => new PeriodoViewModel
            {
                ID = p.ID,
                Name = (p.Year + " - " + p.Number.Type.Name + " " + p.Number.Number)
            });
        }

        public List<PeriodInformationViewModel> getCoursesXBlockXPlan(string pStudyPlan, string pBlockLevel)
        {
            var blockLevel = int.Parse(pBlockLevel);
            return db.SP_GetCoursesXBlockXPlan(pStudyPlan, blockLevel).ToList();
        }

        public List<NameWSModel> getCoursesName(string pStudyPlan, string pBlockNumber)
        {
            var blockNumber = int.Parse(pBlockNumber);
            return db.SP_GetCourses(pStudyPlan, blockNumber).ToList();
        }

        public List<PeriodInformationWSModel> getPeriodInformation(string pPeriod, string pStudyPlan, string pBlockLevel, string pCourse, string pProfessor)
        {
            var period = int.Parse(pPeriod);
            var blockLevel = int.Parse(pBlockLevel);
            return db.SP_GetPeriodInformation(period, pStudyPlan, blockLevel, pCourse, pProfessor).ToList();
        }
    }
}
