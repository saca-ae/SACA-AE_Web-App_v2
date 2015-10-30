using Newtonsoft.Json;
using SACAAE.Data_Access;
using SACAAE.Models;
using SACAAE.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace SACAAE.Controllers
{
    [Authorize]
    public class ProyectoProfesorController : Controller
    {
        private SACAAEContext db = new SACAAEContext();
        private const string TempDataMessageKeySuccess = "MessageSuccess";
        private const string TempDataMessageKeyError = "MessageError";
        // GET: ProyectoProfesor/Asignar
        public ActionResult Asignar()
        {
            
            List<String> HorasInicio = new List<String>();
            List<String> HorasFin = new List<String>();
            for (int i = 7; i < 23; i++)
            {
                HorasInicio.Add(i.ToString() + ":00");
                HorasFin.Add(i.ToString() + ":00");
            }
            ViewBag.HorasInicio = HorasInicio;
            ViewBag.HorasFin = HorasFin;

            String entidad = Request.Cookies["Entidad"].Value;
            var entidadID = getEntityID(entidad);

            ViewBag.Professors = new SelectList(db.Professors, "ID", "Name"); ;

            if (entidadID == 1)
            {
                var Projects = db.Projects.Where(p => p.StateID == 1 && (p.EntityTypeID == 1 ||      //TEC
                                                         p.EntityTypeID == 2 || p.EntityTypeID == 3 || //TEC-VIC TEC-REC
                                                         p.EntityTypeID == 4 || p.EntityTypeID == 10)) //TEC-MIXTO TEC-Académico
                                             .OrderBy(p => p.Name);
                ViewBag.Projects = new SelectList(Projects, "ID", "Name");
            }
            else
            {
                var Projects = db.Projects.Where(p => p.EntityTypeID == entidadID).OrderBy(p => p.Name);
                ViewBag.Projects = new SelectList(Projects, "ID", "Name");
            }
            return View();
        }

        // POST: ProyectoProfesor/Asignar
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Asignar(ScheduleProjectViewModel pSchedule)
        {
            var vPeriod = Request.Cookies["Periodo"].Value;
            var vIDPeriod = db.Periods.Find(int.Parse(vPeriod)).ID;

            string vHourCharge = pSchedule.HourCharge;
            string vProjectID = pSchedule.Projects;
            string vProfessorID = pSchedule.Professors;
            List<ScheduleProject> vSchedules = pSchedule.ScheduleProject;
             bool vIsProfessorAssign = isProfessorAssign(Convert.ToInt32(vProjectID), Convert.ToInt32(vProfessorID));
            if (!vIsProfessorAssign)
            {
                //Check the schedule of the commissions related with the professor
                bool isCommissionShock = existShockScheduleCommission(Convert.ToInt32(vProfessorID), vSchedules);
                //if exist shock with the schedule, the system doesn't let assign new projects in that schedule
                if (!isCommissionShock)
                {
                    //if exist shock with the schedule, the system doesn't let assign new projects in that schedule
                    bool isProjectShock = existShockScheduleProject(Convert.ToInt32(vProfessorID), vSchedules);
                    if (!isProjectShock)
                    {
                        //if exist shock with the schedule, the system doesn't let assign new projects in that schedule
                        bool isGroupShock = existShockScheduleGroup(Convert.ToInt32(vProfessorID), vSchedules);
                        if (!isGroupShock)
                        {
                                int totalHourAssign = 0;

                                //Save Commission Professor
                                ProjectXProfessor vProjectProfessor = new ProjectXProfessor();
                                vProjectProfessor.ProjectID = Convert.ToInt32(vProjectID);
                                vProjectProfessor.ProfessorID = Convert.ToInt32(vProfessorID);
                                if (vHourCharge.Equals("1"))
                                {
                                    vProjectProfessor.HourAllocatedTypeID = Convert.ToInt32(vHourCharge);
                                }
                                vProjectProfessor.PeriodID = vIDPeriod;
                                vProjectProfessor.Schedule = new List<Schedule>();

                                //Calculate the total hour assign
                                foreach (ScheduleProject vSchedule in vSchedules)
                                {
                                    Schedule vTempSchedule = existSchedule(vSchedule.Day, vSchedule.StartHour, vSchedule.EndHour);
                                    if (vTempSchedule != null)
                                    {
                                        //Get id schedule

                                        vTempSchedule.ProjectsXProfessors.Add(vProjectProfessor);

                                    }

                                    //Convert StartHour to DateTime
                                    var HoraInicio = DateTime.Parse(vSchedule.StartHour);
                                    //Convert EndHour to DateTime
                                    var HoraFin = DateTime.Parse(vSchedule.EndHour);

                                    //Get the difference between StartHour and EndHour
                                    var vSTRDiff = Math.Ceiling(HoraFin.Subtract(HoraInicio).TotalHours);
                
                
                                    int vDiferencia = Convert.ToInt32(vSTRDiff);

                                    totalHourAssign = totalHourAssign + vDiferencia;
                                }
           

                                vProjectProfessor.Hours = totalHourAssign;

                                db.ProjectsXProfessors.Add(vProjectProfessor);

                                db.SaveChanges();
                                TempData[TempDataMessageKeySuccess] = "Profesor asignado correctamente.";

                                return RedirectToAction("Asignar");
                            }
                        else
                        {
                            TempData[TempDataMessageKeyError] = "Existe choque de horario con grupos, no se asigno al profesor al proyecto";
                            return RedirectToAction("Asignar");
                        }
                    }
                    else
                    {
                        TempData[TempDataMessageKeyError] = "Existe choque de horario con proyectos, no se asigno al profesor al proyecto";
                        return RedirectToAction("Asignar");
                    }
                }
                else
                {
                    TempData[TempDataMessageKeyError] = "Existe choque de horario con comisiones, no se asigno al profesor al proyecto";
                    return RedirectToAction("Asignar");
                }
            }
            else
            {
                TempData[TempDataMessageKeyError] = "El profesor ya esta asignado a este proyecto, no se permite asignar dos veces a un profesor a un proyecto ";
                return RedirectToAction("Asignar");
            }

        }

        //GET: ProyectoProfesor/Editar
        public ActionResult Editar()
        {
            if (Request.UrlReferrer != null)
            {
                ViewBag.returnUrl = Request.UrlReferrer.ToString();
            }
            else
            {
                ViewBag.returnUrl = null;
            }

            /* get List of all projects*/
            ViewBag.Projects = new SelectList(db.Projects, "ID", "Name");

            return View();
        }

        // GET: ProyectoProfesor/Revocar
        public ActionResult Revocar()
        {
            if (Request.UrlReferrer != null)
            {
                ViewBag.returnUrl = Request.UrlReferrer.ToString();
            }
            else
            {
                ViewBag.returnUrl = null;
            }

            /* Se obtiene la lista de profesores */
            ViewBag.profesores = new SelectList(db.Professors, "ID", "Name");

            return View();
        }

        // POST: ProyectoProfesor/Revocar
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Revocar(int sltProyectos)
        {
            var revocado = false;

            var proyecto = db.ProjectsXProfessors.Find(sltProyectos);

            if (proyecto != null)
            {
                db.ProjectsXProfessors.Remove(proyecto);
                db.SaveChanges();
                revocado = true;
            }

            if (revocado)
            {
                TempData[TempDataMessageKeySuccess] = "Profesor revocado de proyecto correctamente.";
            }
            else
            {
                TempData[TempDataMessageKeySuccess] = "Ocurrió un error al revocar el profesor.";
            }

            return RedirectToAction("Revocar");
        }

        #region Ajax Post
        [Route("ProyectoProfesor/Profesor/Proyecto/{idProfesor:int}")]
        public ActionResult ObtenerProyectosXProfesor(int idProfesor)
        {
            var vPeriod = Request.Cookies["Periodo"].Value;
            int vIDPeriod = db.Periods.Find(int.Parse(vPeriod)).ID;
            if (HttpContext.Request.IsAjaxRequest())
            {
                var listaProyectos = db.Professors.Find(idProfesor)
                                                  .ProjectsXProfessors
                                                  .Where(p => p.PeriodID == vIDPeriod)
                                                  .Select(p => new { p.ID, p.Project.Name });

                return Json(listaProyectos, JsonRequestBehavior.AllowGet);
            }
            return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        }
        #endregion

        #region Helpers
        private Schedule existSchedule(string pDay, string pStartHour, string pEndHour)
        {
            var vSchedule = db.Schedules.Where(p => p.Day == pDay && p.StartHour == pStartHour && p.EndHour == pEndHour).FirstOrDefault();

            if (vSchedule != null)
            {
                return vSchedule;
            }
            else
            {
                //Create schedule and get id
                Schedule vNewSchedule = new Schedule();
                vNewSchedule.Day = pDay;
                vNewSchedule.StartHour = pStartHour;
                vNewSchedule.EndHour = pEndHour;
                vNewSchedule.ProjectsXProfessors = new List<ProjectXProfessor>();

                db.Schedules.Add(vNewSchedule);
               
                return vNewSchedule;
            }
        }
        private int getEntityID(string entityName)
        {
            EntityType entity;
            switch (entityName)
            {
                case "TEC":
                    entity = db.EntityTypes.SingleOrDefault(p => p.Name == "TEC");
                    break;
                case "CIADEG":
                    entity = db.EntityTypes.SingleOrDefault(p => p.Name == "CIADEG");
                    break;
                case "TAE":
                    entity = db.EntityTypes.SingleOrDefault(p => p.Name == "FUNDA-TAE");
                    break;
                case "MAE":
                    entity = db.EntityTypes.SingleOrDefault(p => p.Name == "FUNDA-MAE");
                    break;
                case "MDE":
                    entity = db.EntityTypes.SingleOrDefault(p => p.Name == "FUNDA-MDE");
                    break;
                case "MGP":
                    entity = db.EntityTypes.SingleOrDefault(p => p.Name == "FUNDA-MGP");
                    break;
                case "DDE":
                    entity = db.EntityTypes.SingleOrDefault(p => p.Name == "FUNDA-Doctorado");
                    break;
                case "Emprendedores":
                    entity = db.EntityTypes.SingleOrDefault(p => p.Name == "FUNDA-Emprendedores");
                    break;
                case "CIE":
                    entity = db.EntityTypes.SingleOrDefault(p => p.Name == "FUNDA-CIE");
                    break;
                case "Actualizacion_Cartago":
                    entity = db.EntityTypes.SingleOrDefault(p => p.Name == "FUNDA-Actualizacion Cartago");
                    break;
                case "Actualizacion_San_Carlos":
                    entity = db.EntityTypes.SingleOrDefault(p => p.Name == "FUNDA-Actualizacion San Carlos");
                    break;
                default:
                    entity = db.EntityTypes.SingleOrDefault(p => p.Name == entityName);
                    break;
            }

            return (entity != null) ? entity.ID : 0;
        }

        /// <summary>
        ///  Remove a profesor from a group
        /// </summary>
        /// <autor> Esteban Segura Benavides </autor>
        /// <param name="pIDGrupo"> ID of group in database</param>
        /// <returns>Information about the action of remove a profesor from a group</returns>
        [Route("ProyectoProfesor/ProyectoProfesor/{pIDProyectoProfesor:int}/removeProfesor")]
        public ActionResult removeGroup(int pIDProyectoProfesor)
        {
            if (HttpContext.Request.IsAjaxRequest())
            {
                var vProjectProfesor = db.ProjectsXProfessors.Find(pIDProyectoProfesor);
                if (vProjectProfesor != null)
                {
                    db.ProjectsXProfessors.Remove(vProjectProfesor);
                    db.SaveChanges();
                    var respuesta = new { respuesta = "success" };
                    var json = JsonConvert.SerializeObject(respuesta);
                    return Content(json);
                }
                else
                {
                    var respuesta_error = new { respuesta = "error" };
                    var json_respuesta_error = JsonConvert.SerializeObject(respuesta_error);
                    return Content(json_respuesta_error);
                }
            }
            var error = new { respuesta = "error" };
            var json_error = JsonConvert.SerializeObject(error);
            return Content(json_error);
        }

        /// <summary>
        /// <autor>Esteban Segura Benavides</autor>
        /// Check posibles conflicts with the new project schedule and the all commission schedule related with determinated professor
        /// </summary>
        /// <param name="pProfessorID"></param>
        /// <param name="pSchedules"></param>
        /// <returns>true if found any problem with the schedules</returns>
        public bool existShockScheduleCommission(int pProfessorID, List<ScheduleProject> pSchedules)
        {
            var vPeriod = Request.Cookies["Periodo"].Value;
            var vPeriodID = db.Periods.Find(int.Parse(vPeriod)).ID;

            //Get the day, starthour and endhour where professor was assign in commission
            var commission_schedule = db.SP_getProfessorScheduleCommission(pProfessorID, vPeriodID).ToList();

            //Verify each scheedule with the new assign information
            foreach (var vNewSchedule in pSchedules)
            {
                foreach (var vActualScheduleCommission in commission_schedule)
                {
                    if (vNewSchedule.Day.Equals(vActualScheduleCommission.Day))
                    {
                        var vActualStartHour = DateTime.Parse(vActualScheduleCommission.StartHour);
                        var vActualEndHour = DateTime.Parse(vActualScheduleCommission.EndHour);
                        var vNewStartHour = DateTime.Parse(vNewSchedule.StartHour);
                        var vNewEndHour = DateTime.Parse(vNewSchedule.EndHour);

                        //Check the range of the schedule
                        if ((vActualStartHour <= vNewStartHour && vNewStartHour <= vActualEndHour) ||
                            (vActualStartHour <= vNewEndHour && vNewEndHour <= vActualEndHour) ||
                            (vNewStartHour <= vActualStartHour && vActualStartHour <= vNewEndHour) ||
                            (vNewStartHour <= vActualEndHour && vActualEndHour <= vNewEndHour))
                        {
                            return true;
                        }
                    }
                }
            }
            return false;
        }

        /// <summary>
        /// Esteban Segura Benavides
        /// Check posibles conflicts with the new project schedule and the all project schedule related with determinated professor
        /// </summary>
        /// <param name="pProfessorID"></param>
        /// <param name="pSchedules"></param>
        /// <returns>true if found any problem with the schedules</returns>
        public bool existShockScheduleProject(int pProfessorID, List<ScheduleProject> pSchedules)
        {
            var vPeriod = Request.Cookies["Periodo"].Value;
            var vPeriodID = db.Periods.Find(int.Parse(vPeriod)).ID;

            //Get the day, starthour and endhour where professor was assign in commission
            var project_schedule = db.SP_getProfessorScheduleProject(pProfessorID, vPeriodID).ToList();

            //Verify each scheedule with the new assign information
            foreach (var vNewSchedule in pSchedules)
            {
                foreach (var vActualScheduleProject in project_schedule)
                {
                    if (vNewSchedule.Day.Equals(vActualScheduleProject.Day))
                    {
                        var vActualStartHour = DateTime.Parse(vActualScheduleProject.StartHour);
                        var vActualEndHour = DateTime.Parse(vActualScheduleProject.EndHour);
                        var vNewStartHour = DateTime.Parse(vNewSchedule.StartHour);
                        var vNewEndHour = DateTime.Parse(vNewSchedule.EndHour);

                        //Check the range of the schedule
                        if ((vActualStartHour <= vNewStartHour && vNewStartHour <= vActualEndHour) ||
                            (vActualStartHour <= vNewEndHour && vNewEndHour <= vActualEndHour) ||
                            (vNewStartHour <= vActualStartHour && vActualStartHour <= vNewEndHour) ||
                            (vNewStartHour <= vActualEndHour && vActualEndHour <= vNewEndHour))
                        {
                            return true;
                        }
                    }
                }
            }
            return false;
        }
        

        /// <summary>
        /// Esteban Segura Benavides
        /// Check posibles conflicts with the new project schedule and the all group schedule related with determinated professor
        /// </summary>
        /// <param name="pProfessorID"></param>
        /// <param name="pSchedules"></param>
        /// <returns>true if found any problem with the schedules</returns>
        public bool existShockScheduleGroup(int pProfessorID, List<ScheduleProject> pSchedules)
        {
            var vPeriod = Request.Cookies["Periodo"].Value;
            var vPeriodID = db.Periods.Find(int.Parse(vPeriod)).ID;

            //Get the day, starthour and endhour where professor was assign in commission
            var project_schedule = db.SP_getProfessorScheduleGroup(pProfessorID, vPeriodID).ToList();

            //Verify each scheedule with the new assign information
            foreach (var vNewSchedule in pSchedules)
            {
                foreach (var vActualScheduleProject in project_schedule)
                {
                    if (vNewSchedule.Day.Equals(vActualScheduleProject.Day))
                    {
                        var vActualStartHour = DateTime.Parse(vActualScheduleProject.StartHour);
                        var vActualEndHour = DateTime.Parse(vActualScheduleProject.EndHour);
                        var vNewStartHour = DateTime.Parse(vNewSchedule.StartHour);
                        var vNewEndHour = DateTime.Parse(vNewSchedule.EndHour);

                        //Check the range of the schedule
                        if ((vActualStartHour <= vNewStartHour && vNewStartHour <= vActualEndHour) ||
                            (vActualStartHour <= vNewEndHour && vNewEndHour <= vActualEndHour) ||
                            (vNewStartHour <= vActualStartHour && vActualStartHour <= vNewEndHour) ||
                            (vNewStartHour <= vActualEndHour && vActualEndHour <= vNewEndHour))
                        {
                            return true;
                        }

                    }
                }
            }
            return false;
        }

        public bool isProfessorAssign(int pProjectID, int pProfessorID)
        {
            var vPeriod = Request.Cookies["Periodo"].Value;
            var vIDPeriod = db.Periods.Find(int.Parse(vPeriod)).ID;

            var getAssign = (from project_profesor in db.ProjectsXProfessors
                             join professor in db.Professors on project_profesor.ProfessorID equals professor.ID
                             join period in db.Periods on project_profesor.PeriodID equals period.ID
                             where project_profesor.ProjectID == pProjectID & period.ID == vIDPeriod
                             select new { professorID = professor.ID }).ToList();
            foreach (var professor in getAssign)
            {
                if (pProfessorID == professor.professorID)
                {
                    return true;
                }
            }
            return false;
        }
        #endregion
    }
}