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
        private const string TempDataMessageKey = "Message";

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
            string vProjects = pSchedule.Projects;
            string vProfessor = pSchedule.Professors;
            List<ScheduleProject> vSchedules = pSchedule.ScheduleProject;

            int totalHourAssign = 0;

            //Save Commission Professor
            ProjectXProfessor vProjectProfessor = new ProjectXProfessor();
            vProjectProfessor.ProjectID = Convert.ToInt32(vProjects);
            vProjectProfessor.ProfessorID = Convert.ToInt32(vProfessor);
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
                var CargaC = Math.Ceiling(HoraFin.Subtract(HoraInicio).TotalHours);
                
                
                int vDiferencia = Convert.ToInt32(CargaC);

                totalHourAssign = totalHourAssign + vDiferencia;
            }
           

            vProjectProfessor.Hours = totalHourAssign;

            db.ProjectsXProfessors.Add(vProjectProfessor);

            db.SaveChanges();
            TempData[TempDataMessageKey] = "Profesor asignado correctamente.";

            return RedirectToAction("Asignar");
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
                TempData[TempDataMessageKey] = "Profesor revocado de proyecto correctamente.";
            }
            else
            {
                TempData[TempDataMessageKey] = "Ocurrió un error al revocar el profesor.";
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
        #endregion
    }
}