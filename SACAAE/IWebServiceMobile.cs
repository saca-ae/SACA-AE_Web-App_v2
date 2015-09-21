using SACAAE.WebService_Models;
using System.Collections.Generic;
using System.ServiceModel;
using System.ServiceModel.Web;

namespace SACAAE
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IWebServiceMobile" in both code and config file together.
    [ServiceContract]
    public interface IWebServiceMobile
    {
        [OperationContract]
        [WebInvoke(Method = "GET", UriTemplate = "LogIn/{pPassword}", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json)]
        bool LogIn(string pPassword);

        [OperationContract]
        [WebInvoke(Method = "GET", UriTemplate = "LogInUser/{pUser}/{pPassword}", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json)]
        bool LogInUser(string pUser, string pPassword);

        //Esteban
        [OperationContract]
        [WebInvoke(Method = "GET", UriTemplate = "getPeriods", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json)]
        List<PeriodWSModel> getPeriods();

        [OperationContract]
        [WebInvoke(Method = "GET", UriTemplate = "getCourses/{pPeriod}", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json)]
        List<BasicInfoWSModel> getCourses(string pPeriod);

        [OperationContract]
        [WebInvoke(Method = "GET", UriTemplate = "getProjects/{pPeriod}", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json)]
        List<BasicInfoWSModel> getProjects(string pPeriod);

        [OperationContract]
        [WebInvoke(Method = "GET", UriTemplate = "getCommissions/{pPeriod}", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json)]
        List<BasicInfoWSModel> getCommissions(string pPeriod);

        [OperationContract]
        [WebInvoke(Method = "GET", UriTemplate = "getGroups/{pPeriod}/{pCourse}", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json)]
        List<BasicInfoWSModel> getGroups(string pPeriod, string pCourse);

        [OperationContract]
        [WebInvoke(Method = "GET", UriTemplate = "getOneProject/{pPeriod}/{pProject}", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json)]
        List<ProjectCommissionWSModel> getOneProject(string pPeriod, string pProject);

        [OperationContract]
        [WebInvoke(Method = "GET", UriTemplate = "getOneCommission/{pPeriod}/{pCommission}", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json)]
        List<ProjectCommissionWSModel> getOneCommission(string pPeriod, string pCommission);

        [OperationContract]
        [WebInvoke(Method = "GET", UriTemplate = "getOneGroup/{pPeriod}/{pGroup}", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json)]
        List<GroupWSModel> getOneGroup(string pPeriod, string pGroup);

        //Cristian
        [OperationContract]
        [WebInvoke(Method = "GET", UriTemplate = "getCoursesName/{pStudyPlan}/{pBlockNumber}", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json)]
        List<NameWSModel> getCoursesName(string pStudyPlan, string pBlockNumber);

        [OperationContract]
        [WebInvoke(Method = "GET", UriTemplate = "getPeriodInformation/{pPeriod}/{pStudyPlan}/{pBlockLevel}/{pCourse}/{pProfessor}", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json)]
        List<PeriodInformationWSModel> getPeriodInformation(string pPeriod, string pStudyPlan, string pBlockLevel, string pCourse, string pProfessor);

        //periodo usar "getPeriods"

        [OperationContract]
        [WebInvoke(Method = "GET", UriTemplate = "getStudyPlan", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json)]
        List<NameWSModel> getStudyPlan();

        [OperationContract]
        [WebInvoke(Method = "GET", UriTemplate = "getCoursesXBlockXPlan/{pStudyPlan}/{pBlockLevel}", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json)]
        List<NameWSModel> getCoursesXBlockXPlan(string pStudyPlan, string pBlockLevel);

        [OperationContract]
        [WebInvoke(Method = "GET", UriTemplate = "getProfessors/{pCourse}", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json)]
        List<NameWSModel> getProfessors(string pCourse);
    }
}
