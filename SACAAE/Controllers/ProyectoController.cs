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

namespace SACAAE.Controllers
{
    public class ProyectoController : Controller
    {
        private SACAAEContext db = new SACAAEContext();
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
            Project project = db.Projects.Find(id);
            ScheduleProjectViewModel projectViewModel = new ScheduleProjectViewModel();
            projectViewModel.Projects = project.Name;

            ViewBag.Professors = new SelectList(db.Professors, "ID", "Name"); 
            return View(projectViewModel);
        }

        // POST: /Proyecto/AsignarProfesorProyecto
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AsignarProfesorProyecto(ScheduleProjectViewModel pSchedule)
        {
            var vPeriod = Request.Cookies["Periodo"].Value;
            var vIDPeriod = db.Periods.Find(int.Parse(vPeriod)).ID;

            string vHourCharge = pSchedule.HourCharge;
            string vProjectID = pSchedule.Projects;
            string vProfessorID = pSchedule.Professors;
            List<ScheduleProject> vSchedules = pSchedule.ScheduleProject;

            //Check the schedule of the commissions related with the professor
            bool isCommissionShock = existShockScheduleCommission(Convert.ToInt32(vProfessorID),vSchedules);
            //if exist shock with the schedule, the system doesn't let assign new projects in that schedule
            if (!isCommissionShock)
            {
                //if exist shock with the schedule, the system doesn't let assign new projects in that schedule
                bool isProjectShock = existShockScheduleProject(Convert.ToInt32(vProfessorID), vSchedules);
                if (!isProjectShock)
                {
                    //if exist shock with the schedule, the system doesn't let assign new projects in that schedule
                    bool isGroupShock = existShockScheduleGroup(Convert.ToInt32(vProfessorID), vSchedules);
                    if(!isGroupShock)
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
                            var vStartHour = DateTime.Parse(vSchedule.StartHour);
                            var vEndHour = DateTime.Parse(vSchedule.EndHour);

                            var CargaC = Math.Ceiling(vEndHour.Subtract(vStartHour).TotalHours);

                            int vDiferencia = Convert.ToInt32(CargaC);


                            totalHourAssign = totalHourAssign + vDiferencia;
                        }


                        vProjectProfessor.Hours = totalHourAssign;

                        db.ProjectsXProfessors.Add(vProjectProfessor);

                        db.SaveChanges();
                        TempData[TempDataMessageKeySuccess] = "Profesor asignado correctamente.";

                        return RedirectToAction("AsignarProfesorProyecto");
                    }
                    else
                    {
                        TempData[TempDataMessageKeyError] = "Existe choque de horario con grupos, no se asigno al profesor al proyecto";
                        return RedirectToAction("AsignarProfesorProyecto");
                    }
                }
                else
                {
                    TempData[TempDataMessageKeyError] = "Existe choque de horario con proyectos, no se asigno al profesor al proyecto";
                    return RedirectToAction("AsignarProfesorProyecto");
                }
            }
            else
            {
                TempData[TempDataMessageKeyError] = "Existe choque de horario con comisiones, no se asigno al profesor al proyecto";
                return RedirectToAction("AsignarProfesorProyecto");
            }


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
                db.Entry(temp).Property(p => p.StateID).CurrentValue = 2;
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
            ProjectXProfessor project_profesor = db.ProjectsXProfessors.Find(id);

            ViewBag.Professors = new SelectList(db.Professors, "ID", "Name");

            return View(project_profesor);
        }

        // POST: /Comision/EditarAsignacion/5
        [HttpPost, ActionName("EditarAsignacion")]
        [ValidateAntiForgeryToken]
        public ActionResult EditarAsignacion(String ID, int Professors, String editHourCharge)
        {

            int ProjectXProfesorID = Convert.ToInt32(ID);
            int vHourChange = Convert.ToInt32(editHourCharge);
            if (ModelState.IsValid)
            {
               
                ProjectXProfessor project_profesor = db.ProjectsXProfessors.Find(ProjectXProfesorID);
                project_profesor.ProfessorID = Professors;
                if (vHourChange == 1)
                {
                    project_profesor.HourAllocatedTypeID = 1;
                }
                else
                {
                    project_profesor.HourAllocatedTypeID = null;
                }
                db.SaveChanges();

                TempData[TempDataMessageKeySuccess] = "Profesor asignado correctamente";
                return RedirectToAction("Details", new { id = project_profesor.ProjectID });
            }
            else
            {
                /*The user recive the information about the problem*/
                TempData[TempDataMessageKeyError] = "No se puede asignar al profesor al curso\n porque existe choque de horario";

                /* Get list of professor related a  project*/
                ProjectXProfessor project_profesor = db.ProjectsXProfessors.Find(ProjectXProfesorID);
                ViewBag.Profesores = new SelectList(db.Professors, "ID", "Name");
                return View(project_profesor);
            }
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
                       where proyecto.StateID == 1 && proyecto.EntityTypeID == 1 || //TEC
                       proyecto.EntityTypeID == 2 || proyecto.EntityTypeID== 3 || //TEC-VIC TEC-REC
                       proyecto.EntityTypeID == 4 || proyecto.EntityTypeID == 10 //TEC-MIXTO TEC Acádemico
                       select proyecto;
            }
            else
            {
                return from proyecto in db.Projects
                       orderby proyecto.Name
                       where proyecto.StateID== 1 && proyecto.EntityTypeID == entidad
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

        /// <summary>
        /// Esteban Segura Benavides
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
                        if (vActualStartHour <= vNewStartHour && vNewStartHour <= vActualEndHour)
                        {
                            return true;
                        }
                        else if (vActualStartHour <= vNewEndHour && vNewEndHour <= vActualEndHour)
                        {
                            return true;
                        }
                        else if (vNewStartHour <= vActualStartHour && vActualStartHour <= vNewEndHour)
                        {
                            return true;
                        }
                        else if (vNewStartHour <= vActualEndHour && vActualEndHour <= vNewEndHour)
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
                        if (vActualStartHour <= vNewStartHour && vNewStartHour <= vActualEndHour)
                        {
                            return true;
                        }
                        else if (vActualStartHour <= vNewEndHour && vNewEndHour <= vActualEndHour)
                        {
                            return true;
                        }
                        else if (vNewStartHour <= vActualStartHour && vActualStartHour <= vNewEndHour)
                        {
                            return true;
                        }
                        else if (vNewStartHour <= vActualEndHour && vActualEndHour <= vNewEndHour)
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
                        if (vActualStartHour <= vNewStartHour && vNewStartHour <= vActualEndHour)
                        {
                            return true;
                        }
                        else if (vActualStartHour <= vNewEndHour && vNewEndHour <= vActualEndHour)
                        {
                            return true;
                        }
                        else if (vNewStartHour <= vActualStartHour && vActualStartHour <= vNewEndHour)
                        {
                            return true;
                        }
                        else if (vNewStartHour <= vActualEndHour && vActualEndHour <= vNewEndHour)
                        {
                            return true;
                        }
                    }
                }
            }
            return false;
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
