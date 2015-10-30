using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using SACAAE.Models;
using SACAAE.Data_Access;
using SACAAE.Models.ViewModels;
using Newtonsoft.Json;
using SACAAE.WebService_Models;
using System.Net.Mail;
using System.Text;
using SACAAE.Helpers;

namespace SACAAE.Controllers
{
    public class EnviarCargaController : Controller
    {
        private SACAAEContext db = new SACAAEContext();
        private LoadAcademicHelper LAHelper = new LoadAcademicHelper();
        private const string TempDataMessageKeySuccess = "MessageSuccess";
        private const string TempDataMessageKeyError = "MessageError";
        SACAAE.Helpers.LoadAcademicHelper.ReporteInfo vReportInfo = new SACAAE.Helpers.LoadAcademicHelper.ReporteInfo();
        SACAAE.Helpers.LoadAcademicHelper.Profesor[] vAcademicLoad;

        // GET: EnviarCarga
        public ActionResult Index()
        {
            int vPeriodID = int.Parse(Request.Cookies["Periodo"].Value);

            vReportInfo = LAHelper.setCourses(vReportInfo, vPeriodID);
            vReportInfo = LAHelper.setProjects(vReportInfo, vPeriodID);
            vReportInfo = LAHelper.setCommissions(vReportInfo, vPeriodID);
            vAcademicLoad = vReportInfo.todo_profesores.ToArray();

            var Professors = db.Professors.ToList();
            var viewModel = new ListLoadViewModel();
            viewModel.Items = new List<LoadViewModel>();

            for (int vCont = 0; vCont < Professors.Count(); vCont++)
            {
                Professor vProf = Professors.ElementAt(vCont);
                int vActiveProfessor = 0; 
                bool vCourses = false, vProjects = false, vComission = false ;
                for (int vElement = 0; vElement < vAcademicLoad.Count(); vElement++)
                {
                    if (vAcademicLoad[vElement].Profesor_Nombre == vProf.Name)
                    {
                        vActiveProfessor = 1;
                        if (vAcademicLoad[vElement].Tipo == "Carga docente") { vCourses = true; }
                        if (vAcademicLoad[vElement].Tipo == "Carga Investigación Extensión") { vProjects = true; }
                        if (vAcademicLoad[vElement].Tipo == "Carga Académico Administrativo") { vComission = true; }
                    }
                }
                if (vActiveProfessor == 1)
                {
                    viewModel.Items.Add(
                        new LoadViewModel()
                        {
                            Selected = true,
                            Name = vProf.Name,
                            Course = vCourses,
                            Project = vProjects,
                            Comission = vComission
                        });
                }
            }
            return View(viewModel);
        }

        /// <summary>
        /// Sends all academy load from the actual period
        /// </summary>
        /// <author> Cristian Araya Fuentes </author> 
        /// <returns></returns>
        public void SendAcademicLoad(ListLoadViewModel pSelectedList) 
        {
            var Professors = db.Professors.ToList();
            int vPeriod = int.Parse(Request.Cookies["Periodo"].Value);
            String vMessageSubject = "Carga Académica";
            vReportInfo = LAHelper.setCourses(vReportInfo, vPeriod);
            vReportInfo = LAHelper.setProjects(vReportInfo, vPeriod);
            vReportInfo = LAHelper.setCommissions(vReportInfo, vPeriod);
            vAcademicLoad = vReportInfo.todo_profesores.ToArray();
            for (int vCont = 0; vCont < Professors.Count(); vCont++)
            {
                Professor vProf = Professors.ElementAt(vCont);
                String vMessageBody = "<html> <body>";
                vMessageBody += "<h2> Carga Académica del siguiente periodo lectivo </h2> \n<h3>Profesor: " + vProf.Name + "</h3>\n";
                vMessageBody += "<table rules='all' style='border-color: #666;' cellpadding='10'>";
                vMessageBody += "<tr style='background: #eee;'> <td><strong>Nombre</strong> </td>   <td><strong>Aula</strong></td> <td><strong>Dia</strong></td>  <td><strong>Hora Inicio</strong></td> <td><strong>Hora Fin</strong></td> <td><strong>Grupo</strong></td> <td><strong>Sede</strong></td> <td><strong>Tipo</strong></td></tr>";
                int vActiveProfessor = 0;
                for (int vElement = 0; vElement < vAcademicLoad.Count(); vElement++)
                {
                    if (vAcademicLoad[vElement].Profesor_Nombre == vProf.Name)
                    {
                        if (pSelectedList.Items.Exists(match => match.Name == vProf.Name && match.Selected == true))
                        {
                            vMessageBody += "<tr><td>" + vAcademicLoad[vElement].Nombre + "</td><td>" + vAcademicLoad[vElement].Aula
                                + "</td><td>" + vAcademicLoad[vElement].Dia + "</td><td>" + vAcademicLoad[vElement].HoraInicio
                                + "</td><td>" + vAcademicLoad[vElement].HoraFin + "</td><td>" + vAcademicLoad[vElement].Grupo + "</td><td>"
                                + vAcademicLoad[vElement].Sede + "</td><td>" + vAcademicLoad[vElement].Tipo + "</td></tr>";
                            vActiveProfessor = 1;
                        }
                     }
                }
                vMessageBody += "</table> </body> </html>";
                if ((vProf.Email != null) && (vActiveProfessor == 1)) 
                {
                    sendEmail("esteban.1703@gmail.com",vMessageSubject,vMessageBody);
                }
            }
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Index(ListLoadViewModel model)
        {
            if (model != null)
            {
                SendAcademicLoad(model);
                TempData[TempDataMessageKeySuccess] = "Se ha enviado la carga académica correctamente";
            }
            return View(model);
        }

       /// <summary>
       /// This function sends an email
       /// </summary>
       /// <author> Cristian Araya Fuentes </author> 
       /// <param name="pEmail"> Email To </param>
       /// <param name="pSubject"> Email's Subject </param>
       /// <param name="pBody"> Email's Body </param>
        public void sendEmail(String pEmail, String pSubject, String pBody) 
        {
            string vFrom = "cargaacademica@saca-ae.net";
            MailMessage vMail = new MailMessage(vFrom, pEmail);
            vMail.Subject = pSubject;
            vMail.Body = pBody;
            vMail.IsBodyHtml = true;
            SmtpClient vSMTPClient = new SmtpClient("mail.saca-ae.net", 587);
            vSMTPClient.UseDefaultCredentials = true;
            vSMTPClient.Credentials = new System.Net.NetworkCredential("cargaacademica@saca-ae.net", "sacapassword4_");

            try { vSMTPClient.Send(vMail); }
            catch { }
        }
    }
}