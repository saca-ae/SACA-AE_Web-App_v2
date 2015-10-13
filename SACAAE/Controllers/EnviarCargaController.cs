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

namespace SACAAE.Controllers
{
    public class EnviarCargaController : Controller
    {
        private SACAAEContext db = new SACAAEContext();
        // GET: EnviarCarga
        public ActionResult Index()
        {
            var Professors = db.Professors.ToList();
            var viewModel = new List<ProfesorViewModel>();

            Professors.ForEach(p => viewModel.Add(
                new ProfesorViewModel()
                {
                    ID = p.ID,
                    Name = p.Name,
                    Link = p.Link,
                    Tel1 = p.Tel1,
                    Tel2 = p.Tel2,
                    StateID = p.StateID.GetValueOrDefault(),
                    Email = p.Email
                }));

            return View(viewModel);
        }

        public ActionResult SendAcademicLoad() 
        {
            ReporteInfo vReportInfo = new ReporteInfo();
            vReportInfo = setCourses(vReportInfo);
            vReportInfo = setProjects(vReportInfo);
            vReportInfo = setCommissions(vReportInfo);

            Profesor[] vAcademicLoad = vReportInfo.todo_profesores.ToArray();
            string profe_actual = "";

            Array.Sort(vAcademicLoad, delegate(Profesor user1, Profesor user2)
            {
                return user1.Profesor_Nombre.CompareTo(user2.Profesor_Nombre);
            });

            var Professors = db.Professors.ToList();

            String vMessageSubject = "Carga Académica";
            for (int vCont = 0; vCont < Professors.Count(); vCont++)
            {
                Professor vProf = Professors.ElementAt(vCont);
                String vMessageBody = "Carga Académica del Semestre \n Profesor: " + vProf.Name + "\n";
                int vActiveProfessor = 0;
                for (int vElement = 0; vElement < vAcademicLoad.Count(); vElement++)
                {
                    if (vAcademicLoad[vElement].Profesor_Nombre == vProf.Name)
                    {
                        vMessageBody += "Nombre: " + vAcademicLoad[vElement].Nombre + " Aula: " + vAcademicLoad[vElement].Aula 
                            + " Día:" + vAcademicLoad[vElement].Dia +" Hora Inicio: " + vAcademicLoad[vElement].HoraInicio 
                            +" Hora Fin:"+ vAcademicLoad[vElement].HoraFin + " Grupo: " + vAcademicLoad[vElement].Grupo + " Sede: " 
                            + vAcademicLoad[vElement].Sede + "\n";
                            vActiveProfessor = 1;
                     }
                }
                if ((vProf.Email != null) && (vActiveProfessor == 1)) 
                {
                    sendEmail("cristian.arayaf@gmail.com",vMessageSubject,vMessageBody);
                }
            }
            return null;
        }

        public void sendEmail(String pEmail, String pSubject, String pBody) 
        {
            string from = "cargaacademica@saca-ae.net";
            MailMessage message = new MailMessage(from, pEmail);
            message.Subject = pSubject;
            message.Body = pBody;
            SmtpClient client = new SmtpClient("mail.saca-ae.net", 587);
            client.UseDefaultCredentials = true;
            client.Credentials = new System.Net.NetworkCredential("cargaacademica@saca-ae.net", "sacapassword4_");

            client.Send(message);

        }


        #region Helpers

        /// <author>Adonis Mora Angulo</author>
        /// <summary>
        /// Set all the data from professors and courses on the helper class
        /// </summary>
        /// <param name="pReportInfo">Helper class to save all data</param>
        /// <returns></returns>
        private ReporteInfo setCourses(ReporteInfo pReportInfo)
        {
            var vPeriodID = int.Parse(Request.Cookies["Periodo"].Value);
            var entidad_temp = "";

            var vGroups = db.Groups.Where(p => p.PeriodID == vPeriodID).OrderBy(p => p.Number).ToList();
            foreach (Group vGroup in vGroups)
            {
                var vDetail = db.GroupClassrooms.Where(p => p.GroupID == vGroup.ID).ToList();
                var vCargaEstimada = (int)Math.Floor(10.0 / vDetail.Count);

                foreach (var vSchedule in vDetail)
                {

                    string HoraInicio = vSchedule.Schedule.StartHour;
                    string HoraFin = vSchedule.Schedule.EndHour;
                    int Carga = vCargaEstimada;
                    var vCourseInfo = db.BlocksXPlansXCourses.Single(p => p.ID == vGroup.BlockXPlanXCourseID).Course;

                    entidad_temp = vGroup.BlockXPlanXCourse.AcademicBlockXStudyPlan.StudyPlan.EntityType.Name;
                    pReportInfo.todo_profesores.Add(new Profesor
                    {

                        Tipo = "Carga docente",
                        Grupo = vGroup.Number + "",
                        Nombre = vCourseInfo.Name,
                        Profesor_Nombre = (vGroup.Professor != null) ? vGroup.Professor.Name : "No asignado",
                        Dia = vSchedule.Schedule.Day,
                        HoraInicio = HoraInicio,
                        HoraFin = HoraFin,
                        Cupo = (vGroup.Capacity ?? vSchedule.Classroom.Capacity) + "",
                        PlanEstudio = vGroup.BlockXPlanXCourse.AcademicBlockXStudyPlan.StudyPlan.Name,
                        Modalidad = vGroup.BlockXPlanXCourse.AcademicBlockXStudyPlan.StudyPlan.Modality.Name,
                        Sede = vGroup.BlockXPlanXCourse.Sede.Name,
                        HoraTeoricas = vGroup.BlockXPlanXCourse.Course.TheoreticalHours + "",
                        HoraPractica = vGroup.BlockXPlanXCourse.Course.PracticeHours + "",
                        CargaEstimada = Carga + "",
                        Aula = vSchedule.Classroom.Code,
                        Entidad = entidad_temp
                    });
                    List<string> vProfessorEntity;
                    // All TEC entities
                    if (entidad_temp.Equals("TEC") || entidad_temp.Equals("TEC-VIC") || entidad_temp.Equals("TEC-REC") ||
                    entidad_temp.Equals("TEC-MIXTO") || entidad_temp.Equals("TEC-Académico") || entidad_temp.Equals("CIADEG "))
                    {
                        if (!entidad_temp.Equals("TEC-REC")) // Omit volunteer hours
                        {
                            if (vGroup.Professor == null)
                            {
                            }
                            else if (pReportInfo.profesores_carga_tec.ContainsKey(vGroup.Professor.Name))
                            {
                                vProfessorEntity = pReportInfo.profesores_cursos_asociados[vGroup.Professor.Name];  // get professor's courses
                                if (!vProfessorEntity.Contains(vCourseInfo.Name))                                   // omit duplicate counting
                                {
                                    vProfessorEntity.Add(vCourseInfo.Name);
                                    pReportInfo.profesores_cursos_asociados[vGroup.Professor.Name] = vProfessorEntity;
                                    pReportInfo.profesores_carga_tec[vGroup.Professor.Name] += Carga;
                                }
                            }
                            else
                            {
                                pReportInfo.profesores_carga_tec.Add(vGroup.Professor.Name, Carga);
                                if (pReportInfo.profesores_cursos_asociados.ContainsKey(vGroup.Professor.Name))     // ask if the course, project or commission has been counted
                                {
                                    vProfessorEntity = pReportInfo.profesores_cursos_asociados[vGroup.Professor.Name];
                                    vProfessorEntity.Add(vCourseInfo.Name);


                                    pReportInfo.profesores_cursos_asociados[vGroup.Professor.Name] = vProfessorEntity;
                                }
                                else
                                {
                                    pReportInfo.profesores_cursos_asociados.Add(vGroup.Professor.Name, new List<string>() { vCourseInfo.Name });
                                }
                            }
                        }
                    }
                    else
                    {
                        if (vGroup.Professor == null)
                        {
                        }
                        else if (pReportInfo.profesores_carga_fundatec.ContainsKey(vGroup.Professor.Name))
                        {
                            vProfessorEntity = pReportInfo.profesores_cursos_asociados[vGroup.Professor.Name];  // get professor's courses
                            if (!vProfessorEntity.Contains(vCourseInfo.Name))                                   // omit duplicate counting
                            {
                                vProfessorEntity.Add(vCourseInfo.Name);
                                pReportInfo.profesores_cursos_asociados[vGroup.Professor.Name] = vProfessorEntity;
                                pReportInfo.profesores_carga_fundatec[vGroup.Professor.Name] += Carga;
                            }
                        }
                        else
                        {
                            pReportInfo.profesores_carga_fundatec.Add(vGroup.Professor.Name, Carga);
                            if (pReportInfo.profesores_cursos_asociados.ContainsKey(vGroup.Professor.Name))     // ask if the course, project or commission has been counted
                            {
                                vProfessorEntity = pReportInfo.profesores_cursos_asociados[vGroup.Professor.Name];
                                vProfessorEntity.Add(vCourseInfo.Name);


                                pReportInfo.profesores_cursos_asociados[vGroup.Professor.Name] = vProfessorEntity;
                            }
                            else
                            {
                                pReportInfo.profesores_cursos_asociados.Add(vGroup.Professor.Name, new List<string>() { vCourseInfo.Name });
                            }
                        }
                    }
                }

            }
            return pReportInfo;
        }

        /// <author>Adonis Mora Angulo</author>
        /// <summary>
        /// Set all the data from professors and projects on the helper class
        /// </summary>
        /// <param name="pReportInfo">Helper class to save all data</param>
        /// <returns></returns>
        private ReporteInfo setProjects(ReporteInfo pReportInfo)
        {
            var entidad_temp = "";

            var vProjects = db.Projects.ToList();
            foreach (var vProject in vProjects)
            {
                var vProfessors = db.ProjectsXProfessors.Where(p => p.ProjectID == vProject.ID).ToList();
                foreach (var vProfe in vProfessors)
                {
                    var vSchedule = vProfe.Schedule.ToList();
                    foreach (var vDay in vSchedule)
                    {
                        var HoraInicio = DateTime.Parse(vDay.StartHour);
                        var HoraFin = DateTime.Parse(vDay.EndHour);

                        var CargaC = Math.Ceiling(HoraFin.Subtract(HoraInicio).TotalHours);
                        if (HoraInicio <= DateTime.Parse("12:00 PM") && HoraFin >= DateTime.Parse("01:00 PM"))
                        {
                            CargaC = CargaC - 1;
                        }

                        entidad_temp = vProfe.Project.EntityType.Name;
                        pReportInfo.todo_profesores.Add(new Profesor
                        {
                            Tipo = "Carga Investigación Extensión",
                            Grupo = "N/A",
                            Nombre = vProject.Name,
                            Profesor_Nombre = vProfe.Professor.Name,
                            Dia = vDay.Day,
                            HoraInicio = HoraInicio.ToShortTimeString(),
                            HoraFin = HoraFin.ToShortTimeString(),
                            Cupo = "N/A",
                            PlanEstudio = "N/A",
                            Modalidad = "N/A",
                            Sede = "N/A",
                            HoraTeoricas = "N/A",
                            HoraPractica = "N/A",
                            CargaEstimada = CargaC + "",
                            Aula = "N/A",
                            Entidad = entidad_temp
                        });


                        List<string> entidadesXProfesor;
                        // All TEC entities
                        if (entidad_temp.Equals("TEC") || entidad_temp.Equals("TEC-VIC") || entidad_temp.Equals("TEC-REC") ||
                        entidad_temp.Equals("TEC-MIXTO") || entidad_temp.Equals("TEC-Académico") || entidad_temp.Equals("CIADEG "))
                        {
                            if (!entidad_temp.Equals("TEC-REC")) // Omit volunteer hours
                            {
                                if (pReportInfo.profesores_carga_tec.ContainsKey(vProfe.Professor.Name))
                                {
                                    entidadesXProfesor = pReportInfo.profesores_cursos_asociados[vProfe.Professor.Name];    // get professor's courses
                                    if (!entidadesXProfesor.Contains(vProject.Name))                                        // omit duplicate counting
                                    {
                                        entidadesXProfesor.Add(vProject.Name);
                                        pReportInfo.profesores_cursos_asociados[vProfe.Professor.Name] = entidadesXProfesor;
                                        pReportInfo.profesores_carga_tec[vProfe.Professor.Name] += Convert.ToInt32(CargaC);
                                    }

                                }
                                else
                                {

                                    pReportInfo.profesores_carga_tec.Add(vProfe.Professor.Name, Convert.ToInt32(CargaC));
                                    if (pReportInfo.profesores_cursos_asociados.ContainsKey(vProfe.Professor.Name))         // ask if the course, project or commission has been counted
                                    {
                                        entidadesXProfesor = pReportInfo.profesores_cursos_asociados[vProfe.Professor.Name];
                                        entidadesXProfesor.Add(vProject.Name);


                                        pReportInfo.profesores_cursos_asociados[vProfe.Professor.Name] = entidadesXProfesor;
                                    }
                                    else
                                    {
                                        pReportInfo.profesores_cursos_asociados.Add(vProfe.Professor.Name, new List<string>() { vProject.Name });
                                    }
                                }
                            }
                        }
                        else
                        {
                            if (pReportInfo.profesores_carga_fundatec.ContainsKey(vProfe.Professor.Name))
                            {
                                entidadesXProfesor = pReportInfo.profesores_cursos_asociados[vProfe.Professor.Name];
                                if (!entidadesXProfesor.Contains(vProject.Name))                                        // omit duplicate counting
                                {
                                    entidadesXProfesor.Add(vProject.Name);
                                    pReportInfo.profesores_cursos_asociados[vProfe.Professor.Name] = entidadesXProfesor;
                                    pReportInfo.profesores_carga_fundatec[vProfe.Professor.Name] += Convert.ToInt32(CargaC);
                                }
                            }
                            else
                            {
                                pReportInfo.profesores_carga_fundatec.Add(vProfe.Professor.Name, Convert.ToInt32(CargaC));
                                if (pReportInfo.profesores_cursos_asociados.ContainsKey(vProfe.Professor.Name))         // ask if the course, project or commission has been counted
                                {
                                    entidadesXProfesor = pReportInfo.profesores_cursos_asociados[vProfe.Professor.Name];
                                    entidadesXProfesor.Add(vProject.Name);


                                    pReportInfo.profesores_cursos_asociados[vProfe.Professor.Name] = entidadesXProfesor;
                                }
                                else
                                {
                                    pReportInfo.profesores_cursos_asociados.Add(vProfe.Professor.Name, new List<string>() { vProject.Name });
                                }

                            }
                        }
                    }
                }
            }
            return pReportInfo;
        }

        /// <author>Adonis Mora Angulo</author>
        /// <summary>
        /// Set all the data from professors and commisions on the helper class
        /// </summary>
        /// <param name="pReportInfo">Helper class to save all data</param>
        /// <returns></returns>
        private ReporteInfo setCommissions(ReporteInfo pReportInfo)
        {
            var entidad_temp = "";

            var vCommissions = db.Commissions.ToList();
            foreach (var vCommission in vCommissions)
            {
                var vProfessors = db.CommissionsXProfessors.Where(p => p.CommissionID == vCommission.ID).ToList();
                foreach (var vProfe in vProfessors)
                {
                    var vSchedule = vProfe.Schedule.ToList();
                    foreach (var vDay in vSchedule)
                    {
                        var HoraInicio = DateTime.Parse(vDay.StartHour);
                        var HoraFin = DateTime.Parse(vDay.EndHour);

                        var CargaC = Math.Ceiling(HoraFin.Subtract(HoraInicio).TotalHours);
                        if (HoraInicio <= DateTime.Parse("12:00 PM") && HoraFin >= DateTime.Parse("01:00 PM"))
                        {
                            CargaC = CargaC - 1;
                        }

                        entidad_temp = vCommission.EntityType.Name;
                        pReportInfo.todo_profesores.Add(new Profesor
                        {
                            Tipo = "Carga Académico Administrativo",
                            Grupo = "N/A",
                            Nombre = vCommission.Name,
                            Profesor_Nombre = vProfe.Professor.Name,
                            Dia = vDay.Day,
                            HoraInicio = HoraInicio.ToShortTimeString(),
                            HoraFin = HoraFin.ToShortTimeString(),
                            Cupo = "N/A",
                            PlanEstudio = "N/A",
                            Modalidad = "N/A",
                            Sede = "N/A",
                            HoraTeoricas = "N/A",
                            HoraPractica = "N/A",
                            CargaEstimada = CargaC + "",
                            Aula = "N/A",
                            Entidad = entidad_temp
                        });

                        List<string> entidadesXProfesor;
                        // All TEC entities
                        if (entidad_temp.Equals("TEC") || entidad_temp.Equals("TEC-VIC") || entidad_temp.Equals("TEC-REC") ||
                        entidad_temp.Equals("TEC-MIXTO") || entidad_temp.Equals("TEC-Académico") || entidad_temp.Equals("CIADEG "))
                        {
                            if (!entidad_temp.Equals("TEC-REC"))     // Omit volunteer hours
                            {
                                if (pReportInfo.profesores_carga_tec.ContainsKey(vProfe.Professor.Name))
                                {
                                    entidadesXProfesor = pReportInfo.profesores_cursos_asociados[vProfe.Professor.Name];
                                    if (!entidadesXProfesor.Contains(vCommission.Name))                                     // omit duplicate counting
                                    {
                                        entidadesXProfesor.Add(vCommission.Name);
                                        pReportInfo.profesores_cursos_asociados[vProfe.Professor.Name] = entidadesXProfesor;
                                        pReportInfo.profesores_carga_tec[vProfe.Professor.Name] += Convert.ToInt32(CargaC);
                                    }

                                }
                                else
                                {

                                    pReportInfo.profesores_carga_tec.Add(vProfe.Professor.Name, Convert.ToInt32(CargaC));
                                    if (pReportInfo.profesores_cursos_asociados.ContainsKey(vProfe.Professor.Name))         // ask if the course, project or commission has been counted
                                    {
                                        entidadesXProfesor = pReportInfo.profesores_cursos_asociados[vProfe.Professor.Name];
                                        entidadesXProfesor.Add(vCommission.Name);


                                        pReportInfo.profesores_cursos_asociados[vProfe.Professor.Name] = entidadesXProfesor;
                                    }
                                    else
                                    {
                                        pReportInfo.profesores_cursos_asociados.Add(vProfe.Professor.Name, new List<string>() { vCommission.Name });
                                    }
                                }
                            }
                        }
                        else
                        {
                            if (pReportInfo.profesores_carga_fundatec.ContainsKey(vProfe.Professor.Name))
                            {
                                entidadesXProfesor = pReportInfo.profesores_cursos_asociados[vProfe.Professor.Name];
                                if (!entidadesXProfesor.Contains(vCommission.Name))                                          // omit duplicate counting
                                {
                                    entidadesXProfesor.Add(vCommission.Name);
                                    pReportInfo.profesores_cursos_asociados[vProfe.Professor.Name] = entidadesXProfesor;
                                    pReportInfo.profesores_carga_fundatec[vProfe.Professor.Name] += Convert.ToInt32(CargaC);
                                }
                            }
                            else
                            {
                                pReportInfo.profesores_carga_fundatec.Add(vProfe.Professor.Name, Convert.ToInt32(CargaC));
                                if (pReportInfo.profesores_cursos_asociados.ContainsKey(vProfe.Professor.Name))             // ask if the course, project or commission has been counted
                                {
                                    entidadesXProfesor = pReportInfo.profesores_cursos_asociados[vProfe.Professor.Name];
                                    entidadesXProfesor.Add(vCommission.Name);


                                    pReportInfo.profesores_cursos_asociados[vProfe.Professor.Name] = entidadesXProfesor;
                                }
                                else
                                {
                                    pReportInfo.profesores_cursos_asociados.Add(vProfe.Professor.Name, new List<string>() { vCommission.Name });
                                }
                            }
                        }
                    }
                }
            }
            return pReportInfo;
        }

        /// <author>Adonis Mora Angulo</author>
        /// <summary>
        /// Helper class to save all courses, projects and commissions data
        /// </summary>
        private class ReporteInfo
        {
            public ReporteInfo()
            {
                profesores_carga_tec = new Dictionary<string, int>();
                profesores_carga_fundatec = new Dictionary<string, int>();
                profesores_cursos_asociados = new Dictionary<string, List<string>>();
                todo_profesores = new List<Profesor>();
            }

            public Dictionary<string, int> profesores_carga_tec { get; set; }
            public Dictionary<string, int> profesores_carga_fundatec { get; set; }
            public Dictionary<string, List<string>> profesores_cursos_asociados { get; set; }
            public List<Profesor> todo_profesores { get; set; }
        }

        /// <author>Adonis Mora Angulo</author>
        /// <summary>
        /// Helper class to write each line on the csv file
        /// </summary>
        private class Profesor
        {
            public string Tipo { get; set; }
            public string Grupo { get; set; }
            public string Nombre { get; set; }
            public string Profesor_Nombre { get; set; }
            public string Dia { get; set; }
            public string HoraInicio { get; set; }
            public string HoraFin { get; set; }
            public string Cupo { get; set; }
            public string PlanEstudio { get; set; }
            public string Modalidad { get; set; }
            public string Sede { get; set; }
            public string HoraTeoricas { get; set; }
            public string HoraPractica { get; set; }
            public string CargaEstimada { get; set; }
            public string Aula { get; set; }
            public string Entidad { get; set; }

            public string toStr()
            {
                return Tipo + ";" + Grupo + ";" + Nombre + ";" + Profesor_Nombre + ";" +
                       Dia + ";" + HoraInicio + ";" + HoraFin + ";" + Sede + ";" + Aula + ";" +
                       Cupo + ";" + PlanEstudio + ";" + Modalidad + ";" + HoraTeoricas + ";" +
                       HoraPractica + ";" + CargaEstimada + ";" + Entidad + ";";
            }
        }

        #endregion
    }
}