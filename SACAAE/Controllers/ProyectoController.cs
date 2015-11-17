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
using SACAAE.Helpers;

namespace SACAAE.Controllers
{
    public class ProyectoController : Controller
    {
        private SACAAEContext db = new SACAAEContext();
        private ScheduleHelper dbHelper = new ScheduleHelper();
        private const string TempDataMessageKeySuccess = "MessageSuccess";
        private const string TempDataMessageKeyError = "MessageError";
        // GET: /Proyecto/
        public ActionResult Index()
        {
            String entidad = Request.Cookies["Entidad"].Value;

            if (entidad.Equals("TEC"))
            {
                var model = ObtenerProyectoXEntidad(1);
                return View(model);
            }
            else if (entidad.Equals("CIE"))
            {
                var model = ObtenerProyectoXEntidad(7);
                return View(model);
            }
            else if (entidad.Equals("TAE"))
            {
                var model = ObtenerProyectoXEntidad(5);
                return View(model);
            }
            else if (entidad.Equals("MAE"))
            {
                var model = ObtenerProyectoXEntidad(6);
                return View(model);
            }
            else if (entidad.Equals("DDE"))
            {
                var model = ObtenerProyectoXEntidad(11);
                return View(model);
            }
            else if (entidad.Equals("Emprendedores"))
            {
                var model = ObtenerProyectoXEntidad(12);
                return View(model);
            }
            else if (entidad.Equals("Actualizacion_Cartago"))
            {
                var model = ObtenerProyectoXEntidad(9);
                return View(model);
            }
            else
            {
                var model = ObtenerProyectoXEntidad(8); //Actualización San Carlos
                return View(model);
            }
        }

        // GET: /Proyecto/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Project project = db.Projects.Find(id);
            //****************
            if (project == null)
            {
                return HttpNotFound();
            }

            ProfessorAssignViewModel proyectViewModel = new ProfessorAssignViewModel();
            proyectViewModel.ID = project.ID;
            proyectViewModel.Name = project.Name;

            var vStart = project.Start.ToString();
            proyectViewModel.Start = vStart;

            var vEnd = project.End.ToString();


            proyectViewModel.End = vEnd;

            return View(proyectViewModel);
        }

        // GET: /Proyecto/Create
        public ActionResult Create()
        {

            var model = new Project();
            ViewBag.EntityTypeID = new SelectList(db.EntityTypes, "ID", "Name");
            ViewBag.StateID = new SelectList(db.States, "ID", "Name");
            return View(model);
        }

        // POST: /Proyecto/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include="ID,Name,Start,End,StateID,Link,EntityTypeID")] Project project)
        {
            String entidad = Request.Cookies["Entidad"].Value;
            int entidadID;

            if (entidad.Equals("TEC")) { entidadID = 1; }
            else if (entidad.Equals("CIE")) { entidadID = 7; }
            else if (entidad.Equals("TAE")) { entidadID = 5; }
            else if (entidad.Equals("MAE")) { entidadID = 6; }
            else if (entidad.Equals("DDE")) { entidadID = 11; }
            else if (entidad.Equals("Emprendedores")) { entidadID = 12; }
            else if (entidad.Equals("Actualizacion_Cartago")) { entidadID = 9; }
            else { entidadID = 8; }


            if (ModelState.IsValid)
            {
                Project proyectoNuevo = new Project()
                {
                    Name = project.Name,
                    Start = project.Start,
                    End = project.End,
                    EntityTypeID = entidadID,
                    Link = project.Link,
                    StateID = 1
                };
                db.Projects.Add(proyectoNuevo);
                db.SaveChanges();
                TempData[TempDataMessageKeySuccess] = "Proyecto creado correctamente.";
                return RedirectToAction("Index");
            }
            
            return RedirectToAction("Create");
        }

        // GET: /Proyecto/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Project project = db.Projects.Find(id);
            if (project == null)
            {
                return HttpNotFound();
            }
            ViewBag.EntityTypeID = new SelectList(db.EntityTypes, "ID", "Name", project.EntityTypeID);
            ViewBag.StateID = new SelectList(db.States, "ID", "Name", project.StateID);
            return View(project);
        }

        // POST: /Proyecto/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,Name,Start,End,StateID,Link,EntityTypeID")] Project proyecto)
        {
            if (ModelState.IsValid)
            {
                if (!ExisteProyecto(proyecto))
                    db.Projects.Add(proyecto);

                var temp = db.Projects.Find(proyecto.ID);

                if (temp != null)
                {
                    db.Entry(temp).Property(p => p.Name).CurrentValue = proyecto.Name;
                    db.Entry(temp).Property(p => p.Start).CurrentValue = proyecto.Start;
                    db.Entry(temp).Property(p => p.End).CurrentValue = proyecto.End;
                }
                db.SaveChanges();
                TempData[TempDataMessageKeySuccess] = "Proyecto editado correctamente.";
                
                return RedirectToAction("Index");
            }
            
            return RedirectToAction("Edit");
        }

        //GET: /Proyecto/AsignarProfesorProyecto/5
        public ActionResult AsignarProfesorProyecto(int? id)
        {
            if (Request.UrlReferrer != null)
            {
                ViewBag.returnUrl = Request.UrlReferrer.ToString();
            }
            else
            {
                ViewBag.returnUrl = null;
            }

            Project project = db.Projects.Find(id);
            ScheduleProjectViewModel projectViewModel = new ScheduleProjectViewModel();
            projectViewModel.Projects = project.Name;

            ViewBag.Professors = new SelectList(db.Professors.OrderBy(p => p.Name), "ID", "Name");
            ViewBag.ProjectID = project.ID.ToString();
            return View(projectViewModel);
        }

        // POST: /Proyecto/AsignarProfesorProyecto
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AsignarProfesorProyecto(ScheduleProjectViewModel pSchedule)
        {
            var vPeriod = Request.Cookies["Periodo"].Value;
            var vPeriodID = db.Periods.Find(int.Parse(vPeriod)).ID;

            string vHourCharge = pSchedule.HourCharge;
            int vProjectID = Convert.ToInt32(pSchedule.Projects);
            int vProfessorID = Convert.ToInt32(pSchedule.Professors);

            List<ScheduleProject> vSchedules = pSchedule.ScheduleProject;
            
            string validate = dbHelper.validationProject(vProjectID,vProfessorID,vPeriodID,vSchedules);
            if(validate.Equals("true"))
            {
                int totalHourAssign = 0;

                //Save Projectvbg Professor
                ProjectXProfessor vProjectProfessor = new ProjectXProfessor();
                vProjectProfessor.ProjectID = Convert.ToInt32(vProjectID);
                vProjectProfessor.ProfessorID = Convert.ToInt32(vProfessorID);
                if (vHourCharge.Equals("1"))
                {
                    vProjectProfessor.HourAllocatedTypeID = Convert.ToInt32(vHourCharge);
                }
                vProjectProfessor.PeriodID = vPeriodID;
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
                    var vStartHour = DateTime.Parse(vSchedule.StartHour);
                    var vEndHour = DateTime.Parse(vSchedule.EndHour);

                    var vSTRDiffHours = Math.Ceiling(vEndHour.Subtract(vStartHour).TotalHours);

                    int vDiffHours = Convert.ToInt32(vSTRDiffHours);


                    totalHourAssign = totalHourAssign + vDiffHours;
                }


                vProjectProfessor.Hours = totalHourAssign;

                db.ProjectsXProfessors.Add(vProjectProfessor);

                db.SaveChanges();
                TempData[TempDataMessageKeySuccess] = "El profesor fue asignado correctamente.";

               

                return RedirectToAction("Details", new { id = vProjectID });
            }
            else if(validate.Equals("falseIsGroupShock"))
            {
                TempData[TempDataMessageKeyError] = "Existe choque de horario con grupos, no se asigno al profesor al proyecto";
                return RedirectToAction("AsignarProfesorProyecto");
            }
            else if(validate.Equals("falseIsProjectShock"))
            {
                TempData[TempDataMessageKeyError] = "Existe choque de horario con proyectos, no se asigno al profesor al proyecto";
                return RedirectToAction("AsignarProfesorProyecto");
            }    
            else if(validate.Equals("falseIsCommissionShock"))
            {
                TempData[TempDataMessageKeyError] = "Existe choque de horario con comisiones, no se asigno al profesor al proyecto";
                return RedirectToAction("AsignarProfesorProyecto");
            }
            else if (validate.Equals("falseIsProfessorShock"))
            {
                TempData[TempDataMessageKeyError] = "El profesor ya esta asignado a este proyecto, no se permite asignar dos veces a un profesor a un proyecto ";
                return RedirectToAction("AsignarProfesorProyecto");
            }
            return View();
        }
    
        // GET: /Proyecto/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Project project = db.Projects.Find(id);
            if (project == null)
            {
                return HttpNotFound();
            }
            return View(project);
        }

        // POST: /Proyecto/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Project proyecto = db.Projects.Find(id);
            if (!ExisteProyecto(proyecto))
                throw new ArgumentException("Proyecto no existe");

            var temp = db.Projects.Find(proyecto.ID);
            if (temp != null)
            {
                //db.Entry(temp).Property(p => p.StateID).CurrentValue = 2;
                db.Projects.Remove(proyecto);
            }
            TempData[TempDataMessageKeyError] = "Proyecto eliminado correctamente.";
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        //GET: Proyecto /DetalleAsignacion/5
        public ActionResult DetalleAsignacion(int id)
        {
            ProjectXProfessor project_profesor = db.ProjectsXProfessors.Find(id);
            return View(project_profesor);
        }

        //GET: Comision / EditarAsignacion/5
        public ActionResult EditarAsignacion(int id)
        {
            if (Request.UrlReferrer != null)
            {
                ViewBag.returnUrl = Request.UrlReferrer.ToString();
            }
            else
            {
                ViewBag.returnUrl = null;
            }
            var project_professor = db.ProjectsXProfessors.Find(id);
            var projectID = project_professor.ProjectID;

            Project project = db.Projects.Find(projectID);

            ScheduleProjectViewModel projectViewModel = new ScheduleProjectViewModel();
            projectViewModel.Projects = project.Name;

            ViewBag.Professors = new SelectList(db.Professors, "ID", "Name");
            ViewBag.ProjectID = projectID.ToString();
            return View(projectViewModel);
        }

        // POST: /Comision/EditarAsignacion/5
        [HttpPost, ActionName("EditarAsignacion")]
        [ValidateAntiForgeryToken]
        public ActionResult EditarAsignacion(ScheduleProjectViewModel pSchedule)
        {
            var vPeriod = Request.Cookies["Periodo"].Value;
            var vPeriodID = db.Periods.Find(int.Parse(vPeriod)).ID;

            var vProjectXProfessorID = Convert.ToInt32(pSchedule.Projects);
            var vHourChange = Convert.ToInt32(pSchedule.HourCharge);
            var vProfessorID = Convert.ToInt32(pSchedule.Professors);

            if (ModelState.IsValid)
            {

                ProjectXProfessor vProjectXProfessor = db.ProjectsXProfessors.Find(vProjectXProfessorID);
                var vProjectID = vProjectXProfessor.ProjectID;

                List<ScheduleProject> vSchedules = pSchedule.ScheduleProject;
                string validate = dbHelper.validationsEditProject(vProjectID, vProfessorID,vPeriodID, vSchedules);
                if (validate.Equals("true"))
                {
                    vProjectXProfessor.ProfessorID = vProfessorID;
                    if (vHourChange == 1)
                    {
                        vProjectXProfessor.HourAllocatedTypeID = 1;
                    }
                    else
                    {
                        vProjectXProfessor.HourAllocatedTypeID = null;
                    }
                    var totalHourAssign = 0;
                    vProjectXProfessor.Schedule.Clear();

                    //Calculate the total hour assign
                    foreach (ScheduleProject vSchedule in vSchedules)
                    {
                        Schedule vTempSchedule = existSchedule(vSchedule.Day, vSchedule.StartHour, vSchedule.EndHour);


                        if (vTempSchedule != null)
                        {
                            //Get id schedule

                            vTempSchedule.ProjectsXProfessors.Add(vProjectXProfessor);
                            vProjectXProfessor.Schedule.Add(vTempSchedule);
                        }


                        //Convert StartHour to DateTime
                        var vStartHour = DateTime.Parse(vSchedule.StartHour);
                        var vEndHour = DateTime.Parse(vSchedule.EndHour);

                        var CargaC = Math.Ceiling(vEndHour.Subtract(vStartHour).TotalHours);

                        int vDiferencia = Convert.ToInt32(CargaC);


                        totalHourAssign = totalHourAssign + vDiferencia;
                    }


                    vProjectXProfessor.Hours = totalHourAssign;
                    db.SaveChanges();

                    TempData[TempDataMessageKeySuccess] = "Profesor asignado correctamente";
                    return RedirectToAction("Details", new { id = vProjectXProfessor.ProjectID });
                }
                else if (validate.Equals("falseIsProjectShock"))
                    TempData[TempDataMessageKeyError] = "Existe choque de horario con proyectos, no se asigno al profesor al proyecto";
                    
                
                else if (validate.Equals("falseIsGroupShock"))
                    TempData[TempDataMessageKeyError] = "Existe choque de horario con grupos, no se asigno al profesor al proyecto";
                    
                else if (validate.Equals("falseIsCommissionShock"))
                    TempData[TempDataMessageKeyError] = "Existe choque de horario con comisiones, no se asigno al profesor a la comisión";

                /* Get the list of professor related with project */
                if (Request.UrlReferrer != null)
                {
                    ViewBag.returnUrl = Request.UrlReferrer.ToString();
                }
                else
                {
                    ViewBag.returnUrl = null;
                }

                var ProjectsXProfessors = db.ProjectsXProfessors.Find(vProjectXProfessorID);
                var projectID = ProjectsXProfessors.ProjectID;
                Project project = db.Projects.Find(projectID);
                ScheduleProjectViewModel projectViewModel = new ScheduleProjectViewModel();
                projectViewModel.Projects = project.Name;

                ViewBag.Professors = new SelectList(db.Professors, "ID", "Name");
                ViewBag.ProjectID = projectID.ToString();
                return View(projectViewModel);
            }
            
            return View();
        }


        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Helpers
        public IQueryable<Project> ObtenerProyectoXEntidad(int entidad)
        {
            if (entidad == 1)
            {
                return from proyecto in db.Projects
                       orderby proyecto.Name
                       where proyecto.EntityTypeID == 1 || //TEC
                       proyecto.EntityTypeID == 2 || proyecto.EntityTypeID== 3 || //TEC-VIC TEC-REC
                       proyecto.EntityTypeID == 4 || proyecto.EntityTypeID == 10 //TEC-MIXTO TEC Acádemico
                       select proyecto;
            }
            else
            {
                return from proyecto in db.Projects
                       orderby proyecto.Name
                       where proyecto.EntityTypeID == entidad
                       select proyecto;
            }
        }

        public bool ExisteProyecto(Project proyecto)
        {
            if (proyecto == null)
                return false;
            return (db.Projects.SingleOrDefault(p => p.ID == proyecto.ID ||
                p.Name == proyecto.Name) != null);
        }

        /// <summary>
        /// Check if a schedule with pDay, pStartHour and pEndHour exist, if exist return the value include id from database
        /// else create and return values from database
        /// </summary>
        /// <param name="pDay"></param>
        /// <param name="pStartHour"></param>
        /// <param name="pEndHour"></param>
        /// <returns></returns>
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

                
        #endregion

        #region Ajax
        /// <summary>
        /// Remove professor from project accordinf to id project (pProyectoID) and id professor (pProfessorID)
        /// </summary>
        /// <param name="pProyectoID"></param>
        /// <param name="pProfessorID"></param>
        /// <returns></returns>
        [Route("Proyecto/{pProyectoID:int}/Professor/{pProfessorID:int}/removeProfesor")]
        public ActionResult removeProfessorsCommission(int pProyectoID, int pProfessorID)
        {
            if (HttpContext.Request.IsAjaxRequest())
            {
                var vPeriod = Request.Cookies["Periodo"].Value;
                var vIDPeriod = db.Periods.Find(int.Parse(vPeriod)).ID;

                var deleteProjectProfesor = (from projectProfessor in db.ProjectsXProfessors
                                               where projectProfessor.ProfessorID == pProfessorID &&
                                                     projectProfessor.ProjectID == pProyectoID &&
                                                     projectProfessor.PeriodID == vIDPeriod
                                               select projectProfessor);

                foreach (var projectProfesor in deleteProjectProfesor)
                {
                    db.ProjectsXProfessors.Remove(projectProfesor);
                }
                db.SaveChanges();
                var respuesta = new { respuesta = "success" };
                var json = JsonConvert.SerializeObject(respuesta);
                return Content(json);
            }
            var respuesta_error = new { respuesta = "error" };
            var json_error = JsonConvert.SerializeObject(respuesta_error);
            return Content(json_error);
        }
        /// <summary>
        /// Esteban Segura Benavides
        /// Get professor information in project
        /// </summary>
        /// <param name="pProyectoProfesorID"></param>
        /// <returns></returns>
        [Route("Proyecto/{pProyectoProfesorID:int}/Profesor")]
        public ActionResult getProfessorAssign(int pProyectoProfesorID)
        {
            var vPeriod = Request.Cookies["Periodo"].Value;
            var vIDPeriod = db.Periods.Find(int.Parse(vPeriod)).ID;

            var getAssign = (from project_profesor in db.ProjectsXProfessors
                             join professor in db.Professors on project_profesor.ProfessorID equals professor.ID
                             join period in db.Periods on project_profesor.PeriodID equals period.ID
                             where project_profesor.ID == pProyectoProfesorID & period.ID == vIDPeriod
                             select new { professorID = professor.ID, professorName = professor.Name, project_profesor.HourAllocatedTypeID }).ToList();

            var json = JsonConvert.SerializeObject(getAssign);
            return Content(json);

        }

        /// <summary>
        /// Get all professor related with a determinate commission according a id project
        /// </summary>
        /// <param name="pProjectID"></param>
        /// <returns>Information about a professor assign</returns>
        [Route("Project/{pProjectID:int}/Professors")]
        public ActionResult getProfesorsProject(int pProjectID)
        {
            var vPeriod = Request.Cookies["Periodo"].Value;
            var vIDPeriod = db.Periods.Find(int.Parse(vPeriod)).ID;

            var getAssign = (from project_profesor in db.ProjectsXProfessors
                             join professor in db.Professors on project_profesor.ProfessorID equals professor.ID
                             join period in db.Periods on project_profesor.PeriodID equals period.ID
                             where project_profesor.ProjectID == pProjectID & period.ID == vIDPeriod
                             select new { project_profesor.Hours, projectProfessorID = project_profesor.ID, professorID = professor.ID, professor.Name, project_profesor.HourAllocatedTypeID }).ToList();

            var json = JsonConvert.SerializeObject(getAssign);
            return Content(json);
        }

        /// <summary>
        /// Get the schedulle of a professor in a determinate project
        /// </summary>
        /// <param name="pProyectoProfessorID"></param>
        /// <returns>Day, StartHour and EndHour in a schedule in project</returns>
        [Route("Proyecto/getScheduleProfesor/{pProyectoProfessorID:int}")]
        public ActionResult getScheduleProfesor(int pProyectoProfessorID)
        {
            var json = "";

            var schedule_professor = db.SP_getScheduleProjectProfessor(pProyectoProfessorID).ToList();

            json = JsonConvert.SerializeObject(schedule_professor);
            return Content(json);

        }
        #endregion
    }
}
