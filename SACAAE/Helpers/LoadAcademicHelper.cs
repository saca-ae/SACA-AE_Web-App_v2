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

namespace SACAAE.Helpers
{
    public class LoadAcademicHelper
    {
        private SACAAEContext db = new SACAAEContext();

        /// <author></author>
        /// <summary>
        /// Set all the data from professors and courses on the helper class
        /// </summary>
        /// <param name="pReportInfo">Helper class to save all data</param>
        /// <returns></returns>
        public ReporteInfo setCourses(ReporteInfo pReportInfo, int pPeriodID)
        {
            var vPeriodID = pPeriodID;
            var entidad_temp = "";
            var vGroups = db.Groups.Where(p => p.PeriodID == vPeriodID).OrderBy(p => p.Number).ToList();
            
            foreach (Group vGroup in vGroups)
            {
                var vDetail = db.GroupClassrooms.Where(p => p.GroupID == vGroup.ID).ToList();
                var vCargaEstimada = (int)Math.Floor(10.0 / vDetail.Count);
                String vTypeHour = "";
                if (vGroup.HourAllocatedTypeID != null) vTypeHour = vGroup.HourAllocatedType.Name; 
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
                        PlanEstudio = vGroup.BlockXPlanXCourse.AcademicBlockXStudyPlan.StudyPlan.Name,
                        Sede = vGroup.BlockXPlanXCourse.Sede.Name,
                        CargaEstimada = Carga + "",
                        TipoHora = vTypeHour,
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
                                vProfessorEntity.Add(vCourseInfo.Name); 
                                pReportInfo.profesores_cursos_asociados[vGroup.Professor.Name] = vProfessorEntity;
                                if (vTypeHour != "Reconocimiento")  //
                                    pReportInfo.profesores_carga_tec[vGroup.Professor.Name] += Carga;
                            }
                            else
                            {
                                if (vTypeHour != "Reconocimiento")
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
                            vProfessorEntity.Add(vCourseInfo.Name);
                            pReportInfo.profesores_cursos_asociados[vGroup.Professor.Name] = vProfessorEntity;
                            pReportInfo.profesores_carga_fundatec[vGroup.Professor.Name] += Carga;
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

        /// <author></author>
        /// <summary>
        /// Set all the data from professors and projects on the helper class
        /// </summary>
        /// <param name="pReportInfo">Helper class to save all data</param>
        /// <returns></returns>
        public ReporteInfo setProjects(ReporteInfo pReportInfo, int pPeriodID)
        {
            var entidad_temp = "";
            var vPeriodID = pPeriodID;
            var vProjects = db.Projects.ToList();

            foreach (var vProject in vProjects)
            {
                var vProfessors = db.ProjectsXProfessors.Where(p => p.ProjectID == vProject.ID && p.PeriodID == vPeriodID).ToList();
                foreach (var vProfe in vProfessors)
                {
                    String vTypeHour = "";    //
                    if ( vProfe.HourAllocatedTypeID != null) vTypeHour = vProfe.HourAllocatedType.Name;  //
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
                            PlanEstudio = "N/A",
                            Sede = "N/A",
                            CargaEstimada = CargaC + "",
                            TipoHora = vTypeHour,
                            Aula = "N/A",
                            Entidad = entidad_temp
                        });


                        List<string> entidadesXProfesor;
                        // Separates FundaTEC -- ??
                        if (entidad_temp.Equals("TEC") || entidad_temp.Equals("TEC-VIC") || entidad_temp.Equals("TEC-REC") ||
                        entidad_temp.Equals("TEC-MIXTO") || entidad_temp.Equals("TEC-Académico") || entidad_temp.Equals("CIADEG "))
                        {
                            if (pReportInfo.profesores_carga_tec.ContainsKey(vProfe.Professor.Name)) //Proffesor has assigned academic load
                            {
                                entidadesXProfesor = pReportInfo.profesores_cursos_asociados[vProfe.Professor.Name];
                                entidadesXProfesor.Add(vProject.Name);
                                pReportInfo.profesores_cursos_asociados[vProfe.Professor.Name] = entidadesXProfesor;
                                if (vTypeHour != "Reconocimiento")  //
                                    pReportInfo.profesores_carga_tec[vProfe.Professor.Name] += Convert.ToInt32(CargaC);
                            }
                            else
                            {
                                if (vTypeHour != "Reconocimiento") //
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
                        else
                        {
                            if (pReportInfo.profesores_carga_fundatec.ContainsKey(vProfe.Professor.Name))
                            {
                                entidadesXProfesor = pReportInfo.profesores_cursos_asociados[vProfe.Professor.Name];
                                entidadesXProfesor.Add(vProject.Name);
                                pReportInfo.profesores_cursos_asociados[vProfe.Professor.Name] = entidadesXProfesor;
                                pReportInfo.profesores_carga_fundatec[vProfe.Professor.Name] += Convert.ToInt32(CargaC);
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

        /// <author> Cristian Araya Fuentes </author>
        /// <summary>
        /// Set all the data from professors and commisions on the helper class
        /// </summary>
        /// <param name="pReportInfo">Helper class to save all data</param>
        /// <returns></returns>
        public ReporteInfo setCommissions(ReporteInfo pReportInfo, int pPeriodID)
        {
            var entidad_temp = "";
            var vPeriodID = pPeriodID;

            var vCommissions = db.Commissions.ToList();
            foreach (var vCommission in vCommissions)
            {
                var vProfessors = db.CommissionsXProfessors.Where(p => p.CommissionID == vCommission.ID && p.PeriodID == vPeriodID).ToList();
                foreach (var vProfe in vProfessors)
                {
                    String vTypeHour = "";    //
                    if (vProfe.HourAllocatedTypeID != null) vTypeHour = vProfe.HourAllocatedType.Name;  //               
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
                            PlanEstudio = "N/A",
                            Sede = "N/A",
                            CargaEstimada = CargaC + "",
                            TipoHora = vTypeHour,
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
                                    entidadesXProfesor.Add(vCommission.Name);
                                    pReportInfo.profesores_cursos_asociados[vProfe.Professor.Name] = entidadesXProfesor;
                                    if (vTypeHour != "Reconocimiento")  //
                                        pReportInfo.profesores_carga_tec[vProfe.Professor.Name] += Convert.ToInt32(CargaC);
                                }
                                else
                                {
                                    if (vTypeHour != "Reconocimiento")  //
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
                                entidadesXProfesor.Add(vCommission.Name);
                                pReportInfo.profesores_cursos_asociados[vProfe.Professor.Name] = entidadesXProfesor;
                                pReportInfo.profesores_carga_fundatec[vProfe.Professor.Name] += Convert.ToInt32(CargaC);
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
        public class ReporteInfo
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
        public class Profesor
        {
            public string Tipo { get; set; }
            public string Grupo { get; set; }
            public string Nombre { get; set; }
            public string Profesor_Nombre { get; set; }
            public string Dia { get; set; }
            public string HoraInicio { get; set; }
            public string HoraFin { get; set; }
            public string PlanEstudio { get; set; }
            public string Sede { get; set; }
            public string CargaEstimada { get; set; }
            public string TipoHora { get; set; }
            public string Aula { get; set; }
            public string Entidad { get; set; }

            public string toStr()
            {
                return Tipo + ";" + Grupo + ";" + Nombre + ";" + Profesor_Nombre + ";" +
                       Dia + ";" + HoraInicio + ";" + HoraFin + ";" + Sede + ";" + Aula + ";" +
                       PlanEstudio + ";" + CargaEstimada + ";" + TipoHora + ";" + Entidad + ";";
            }
        }

      
    }
}