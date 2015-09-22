using SACAAE.Models.ViewModels;
using SACAAE.WebService_Models;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Web;

namespace SACAAE
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IWebServiceMobile" in both code and config file together.
    [ServiceContract]
    public interface IWebServiceMobile
    {
        //Esteban


        [OperationContract]
        [WebInvoke(Method = "GET", UriTemplate = "getCourses/{pPeriod}", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json)]
        string getCourses(string pPeriod);

        [OperationContract]
        [WebInvoke(Method = "GET", UriTemplate = "getProjects/{pPeriod}", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json)]
        string getProjects(string pPeriod);

        [OperationContract]
        [WebInvoke(Method = "GET", UriTemplate = "getCommissions/{pPeriod}", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json)]
        string getCommissions(string pPeriod);

        [OperationContract]
        [WebInvoke(Method = "GET", UriTemplate = "getGroups/{pPeriod}/{pCourse}", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json)]
        string getGroups(string pPeriod, string pCourse);

        [OperationContract]
        [WebInvoke(Method = "GET", UriTemplate = "getOneProject/{pPeriod}/{pProject}", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json)]
        string getOneProject(string pPeriod, string pProject);

        [OperationContract]
        [WebInvoke(Method = "GET", UriTemplate = "getOneCommission/{pPeriod}/{pCommission}", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json)]
        string getOneCommission(string pPeriod, string pCommission);

        [OperationContract]
        [WebInvoke(Method = "GET", UriTemplate = "getOneGroup/{pPeriod}/{pGroup}", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json)]
        string getOneGroup(string pPeriod, string pGroup);

        //Cristian

        [OperationContract]
        [WebInvoke(Method = "GET", UriTemplate = "getStudyPlan", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json)]
        List<NameWSModel> getStudyPlan();

        [OperationContract]
        [WebInvoke(Method = "GET", UriTemplate = "getProfessors/{pCourse}", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json)]
        List<NameWSModel> getProfessors(string pCourse);

        [OperationContract]
        [WebInvoke(Method = "GET", UriTemplate = "getPeriods", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json)]
        IQueryable<PeriodoViewModel> getPeriods();

        [OperationContract]
        [WebInvoke(Method = "GET", UriTemplate = "getCoursesXBlockXPlan/{pStudyPlan}/{pBlockLevel}", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json)]
        List<PeriodInformationViewModel> getCoursesXBlockXPlan(string pStudyPlan, string pBlockLevel);

        [OperationContract]
        [WebInvoke(Method = "GET", UriTemplate = "getCoursesName/{pStudyPlan}/{pBlockNumber}", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json)]
        List<NameWSModel> getCoursesName(string pStudyPlan, string pBlockNumber);

        [OperationContract]
        [WebInvoke(Method = "GET", UriTemplate = "getPeriodInformation/{pPeriod}/{pStudyPlan}/{pBlockLevel}/{pCourse}/{pProfessor}", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json)]
        List<PeriodInformationWSModel> getPeriodInformation(string pPeriod, string pStudyPlan, string pBlockLevel, string pCourse, string pProfessor);

    }
}
