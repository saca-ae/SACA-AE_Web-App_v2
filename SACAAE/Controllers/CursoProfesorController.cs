﻿using Newtonsoft.Json;
using SACAAE.Data_Access;
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
    public class CursoProfesorController : Controller
    {
        private SACAAEContext db = new SACAAEContext();
        private const string TempDataMessageKey = "Message";
        private const string TempDataMessageKeySuccess = "MessageSuccess";

        // GET: CursoProfesor/Asignar
        public ActionResult Asignar()
        {
            if (Request.UrlReferrer != null)
            {
                ViewBag.returnUrl = Request.UrlReferrer.ToString();
            }
            else
            {
                ViewBag.returnUrl = null;
            }

            /* get List of all teachers */
            ViewBag.Profesores = new SelectList(db.Professors, "ID", "Name");
            /*get List of all 'sedes' */
            ViewBag.Sedes = new SelectList(db.Sedes, "ID", "Name");
            /* get List of all 'modalidades' */
            ViewBag.Modalidades = new SelectList(db.Modalities, "ID", "Name"); 

            return View();
        }

        // POST: CursoProfesor/Asignar
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Asignar(AsignacionCursoProfesorViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                int idGrupo = viewModel.idGrupo;
                int idProfesor = viewModel.idProfesor;

                
                db.Groups.Find(idGrupo).Professor.ID = idProfesor;
                db.SaveChanges();

                TempData[TempDataMessageKeySuccess] = "Profesor asignado correctamente";
                return RedirectToAction("Curso","Index");
            }
            return View(viewModel);

        }
        

        // GET: CursoProfesor/revocar
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
            ViewBag.Profesores = new SelectList(db.Professors, "ID", "Name");

            return View();
        }

        // POST: CursoProfesor/revocar
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult revocar(int sltCursosImpartidos)
        {
            var revocado = false;
  

            if (revocado)
            {
                TempData[TempDataMessageKey] = "Profesor revocado del curso correctamente.";
            }
            else
            {
                TempData[TempDataMessageKey] = "Ocurrió un error al revocar el profesor.";
            }

            return RedirectToAction("Revocar");
        }

        #region Ajax Post

        /// <summary>
        ///  Get the ID and Description of Plan Studt according to pSede and pModalidad
        ///  
        /// ***************** NOT USED  ********************************* 
        /// </summary>
        /// <autor> Unknown </autor>
        /// <param name="pSede"> ID of Sede in database</param>
        /// <param name="pModalidad">ID of Sede in database</param>
        /// <returns>ID of Study Plan and Description of Study Plan</returns>
        [Route("CursoProfesor/Planes/List/{pSede:int}/{pModalidad:int}")]
        public ActionResult ObtenerPlanesEstudio(int pSede, int pModalidad)
        {
            if (HttpContext.Request.IsAjaxRequest())
            {
                var listaPlanes = from sedes in db.Sedes
                                  join planesporsede in db.StudyPlansXSedes on sedes.ID equals planesporsede.SedeID
                                  join planesestudio in db.StudyPlans on planesporsede.StudyPlanID equals planesestudio.ID
                                  join modalidades in db.Modalities on planesestudio.ModeID equals modalidades.ID
                                  where (sedes.ID == pSede) && (modalidades.ID == pModalidad)
                                  select new { planesestudio.ID, planesestudio.Name };

                return Json(listaPlanes, JsonRequestBehavior.AllowGet);
            }
            return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        }

        /// <summary>
        /// Get the ID and description of blocks according to ID of Plan 
        /// 
        /// ***************** NOT USED  *********************************   
        /// </summary>
        /// <autor>Unknown</autor>
        /// <param name="pPlan">ID of Study Plan in database</param>
        /// <returns>ID and Description block</returns>
        [Route("CursoProfesor/Bloques/List/{pPlan:int}")]
        public ActionResult ObtenerBloques(int pPlan)
        {
            if (HttpContext.Request.IsAjaxRequest())
            {
                var listaBloques = db.StudyPlans.Find(pPlan)
                                     .AcademicBlocksXStudyPlans
                                     .Select(p => new { p.AcademicBlock.ID, p.AcademicBlock.Description });

                return Json(listaBloques, JsonRequestBehavior.AllowGet);
            }
            return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        }

        /// <summary>
        /// Get a list of courses according to ID plan and ID block
        /// 
        /// ***************** NOT USED  *********************************  
        /// </summary>
        /// <param name="pPlan">ID of Study Plan in database</param>
        /// <param name="pBloque">ID of Block in database</param>
        /// <returns>ID, Code and Name of Course according a ID Plan and ID Bloc</returns>
        [Route("CursoProfesor/Cursos/List/{pPlan:int}/{pBloque:int}")]
        public ActionResult ObtenerCursos(int pPlan, int pBloque)
        {
            if (HttpContext.Request.IsAjaxRequest())
            {
                var listaCursos = db.AcademicBlocksXStudyPlans
                                    .Where(p => p.BlockID == pBloque && p.PlanID == pPlan).FirstOrDefault()
                                    .BlocksXPlansXCourses.Select(p => new { p.Course.ID, p.Course.Code, p.Course.Name });

                return Json(listaCursos, JsonRequestBehavior.AllowGet);
            }
            return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        }

    

        /// <summary>
        /// Get a list of groups without profesors in a determinated Sede,Plan,Block and Period
        /// </summary>
        /// 
        /// <autor>Unknow</autor>
        /// <changes>
        /// Esteban Segura Benavides 8/28/2015
        ///   
        ///   Route:
        ///   Before: CursoProfesor/GruposSinProfesor/List/{curso:int}/{plan:int}/{sede:int}/{bloque:int}
        ///   New: CursoProfesor/Sedes/{sede:int}/Planes/{plan:int}/Bloques/{bloque:int}/Cursos/{curso:int}/GroupWithoutProfesor
        ///
        ///   Method Name:
        ///   Before: ObtenerGruposSinProfesor
        ///   New: getGroupWithoutProfesor
        /// </changes>
        /// <param name="pCurso">ID of Course in database</param>
        /// <param name="pPlan">ID of Study Plan in database</param>
        /// <param name="pSede">ID of Sede in database</param>
        /// <param name="pBloque">ID of Block in database</param>
        /// <returns>ID and Number of a group according a parameters and a Period</returns>
        [Route("CursoProfesor/Sedes/{pSede:int}/Planes/{pPlan:int}/Bloques/{pBloque:int}/Cursos/{pCurso:int}/GroupWithoutProfesor")]
        public ActionResult getGroupWithoutProfesor(int pCurso, int pPlan, int pSede, int pBloque)
        {
            if (HttpContext.Request.IsAjaxRequest())
            {
                var vPeriod = Request.Cookies["Periodo"].Value;
                var vIDPeriod = db.Periods.Find(int.Parse(vPeriod)).ID;
                var vListGroup = db.Groups
                                    .Where(p => (p.ProfessorID == null || p.ProfessorID == 3)
                                             && p.PeriodID == vIDPeriod
                                             && p.BlockXPlanXCourse.CourseID == pCurso
                                             && p.BlockXPlanXCourse.AcademicBlockXStudyPlan.PlanID == pPlan
                                             && p.BlockXPlanXCourse.AcademicBlockXStudyPlan.BlockID == pBloque
                                             && p.BlockXPlanXCourse.SedeID == pSede
                                            )
                                    .Select(p => new{ p.ID, p.Number });

                return Json(vListGroup, JsonRequestBehavior.AllowGet);
            }
            return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        }

       

        /// <summary>
        /// Get info about a group
        /// </summary>
        /// <autor>Unknown</autor>
        /// <param name="cursoxgrupo"></param>
        /// <returns></returns>
        [Route("CursoProfesor/Grupos/Info/{cursoxgrupo:int}")]
        public ActionResult ObtenerInfo(int cursoxgrupo)
        {
            if (HttpContext.Request.IsAjaxRequest())
            {
                var grupo = db.Groups.Find(cursoxgrupo);
                var info = new { grupo.ID, grupo.Capacity,grupo.BlockXPlanXCourse.Course.TheoreticalHours };

                return Json(info, JsonRequestBehavior.AllowGet);
            }
            return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        }

        /// <summary>
        /// ***************** NOT USED  ********************************* 
        /// </summary>
        /// <autor>Unkown</autor>
        /// <param name="idProfesor"></param>
        /// <returns></returns>
        [Route("CursoProfesor/Profesor/Cursos/{idProfesor:int}")]
        public ActionResult ObtenerCursosPorProfesor(int idProfesor)
        {
            if (HttpContext.Request.IsAjaxRequest())
            {
                var listaCursos = from profesores in db.Professors
                                  join grupo in db.Groups on profesores.ID equals grupo.ProfessorID
                                  join bloqueXPlanXCurso in db.BlocksXPlansXCourses on grupo.BlockXPlanXCourseID equals bloqueXPlanXCurso.ID
                                  where profesores.ID == idProfesor
                                  select new { profesores.ID, bloqueXPlanXCurso.Course.Name, bloqueXPlanXCurso.Course.Code }; //Revisar

                var json = JsonConvert.SerializeObject(listaCursos);
                return Content(json);
            }
            return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        }

        /// <summary>
        /// Get a groups associated a determinated course 
        /// </summary>
        /// <autor>Esteban Segura Benavides</autor>
        /// <changes>
        /// Esteban Segura Benavides 8/28/2015
        ///   
        ///   Script Old:
        ///   from curso in db.Courses
///           join bloque_planes_curso in db.BlocksXPlansXCourses on curso.ID equals bloque_planes_curso.CourseID
///           join bloque_planes in db.AcademicBlocksXStudyPlans on bloque_planes_curso.BlockXPlanID equals bloque_planes.ID
///           join grupo in db.Groups on bloque_planes_curso.ID equals grupo.BlockXPlanXCourseID
///           join profesor in db.Professors on grupo.ProfessorID equals profesor.ID
///           join plan_estudio in db.StudyPlans on bloque_planes.PlanID equals plan_estudio.ID
//////        join plan_sede in db.StudyPlansXSedes on plan_estudio.ID equals plan_sede.StudyPlanID
///           join sede in db.Sedes on plan_sede.SedeID equals sede.ID
///           join grupo_aula in db.GroupClassrooms on grupo.ID equals grupo_aula.GroupID
///           join aula in db.Classrooms on grupo_aula.ClassroomID equals aula.ID
///           join horario in db.Schedules on grupo_aula.ScheduleID equals horario.ID
///           where curso.ID == pIDCurso && sede.Name == "Cartago"
 ///           select new { grupo.ID, grupo.Number, profesor.Name, aula.Code, horario.StartHour, horario.EndHour, horario.Day });
///           Script New:
///           
        ///
        ///                           select new { grupo.ID, grupo.Number, profesor.Name, aula.Code, horario.StartHour, horario.EndHour, horario.Day });
        /// </changes>
        /// <param name="pIDCurso">ID Course in database</param>
        /// <returns>ID, Number of Group, Name of Profesor, Code of Aula and StartHour, EndHour and Day of Schedule</returns>
        [Route("CursoProfesor/Cursos/{pIDCurso:int}/{pSede:int}")]
        public ActionResult getCourseGroups(int pIDCurso, int pSede)
        {
            if (HttpContext.Request.IsAjaxRequest())
            {
                var json="";
                var vListGroupCourseLeft = (from course in db.Courses
                                   join block_plan_course in db.BlocksXPlansXCourses on course.ID equals block_plan_course.CourseID
                                   join groups in db.Groups on block_plan_course.ID equals groups.BlockXPlanXCourseID
                                   join period in db.Periods on groups.PeriodID equals period.ID
                                   join sede in db.Sedes on block_plan_course.SedeID equals sede.ID
                                   //join professor in db.Professors on groups.ProfessorID equals professor.ID
                                   join group_classroom in db.GroupClassrooms on groups.ID equals group_classroom.GroupID
                                   join schedule in db.Schedules on group_classroom.ScheduleID equals schedule.ID
                                   join classroom in db.Classrooms on group_classroom.ClassroomID equals classroom.ID
                                   where course.ID == pIDCurso && sede.ID == pSede
                                            select new { groups.ID, groups.Number, sede_name = sede.Name, Name = "", classroom.Code, schedule.StartHour, schedule.EndHour, schedule.Day }).ToList();
                
                var vListGroupCourseRight = (from course in db.Courses
                                           join block_plan_course in db.BlocksXPlansXCourses on course.ID equals block_plan_course.CourseID
                                           join groups in db.Groups on block_plan_course.ID equals groups.BlockXPlanXCourseID
                                           join period in db.Periods on groups.PeriodID equals period.ID
                                           join sede in db.Sedes on block_plan_course.SedeID equals sede.ID
                                           join professor in db.Professors on groups.ProfessorID equals professor.ID
                                           where course.ID == pIDCurso && sede.ID == pSede
                                             select new { groups.ID, groups.Number, sede_name = sede.Name, professor.Name, Code="", StartHour="", EndHour="", Day =""}).ToList();
                /*
                if (vListGroupCourseLeft.Count != 0 && vListGroupCourseRight.Count != 0)
                {
                    var vInnerJoin = (from course in db.Courses
                                    join block_plan_course in db.BlocksXPlansXCourses on course.ID equals block_plan_course.CourseID
                                    join groups in db.Groups on block_plan_course.ID equals groups.BlockXPlanXCourseID
                                    join period in db.Periods on groups.PeriodID equals period.ID
                                    join sede in db.Sedes on block_plan_course.SedeID equals sede.ID
                                    join professor in db.Professors on groups.ProfessorID equals professor.ID
                                    join group_classroom in db.GroupClassrooms on groups.ID equals group_classroom.GroupID
                                    join schedule in db.Schedules on group_classroom.ScheduleID equals schedule.ID
                                    join classroom in db.Classrooms on group_classroom.ClassroomID equals classroom.ID
                                    where course.ID == pIDCurso && sede.ID == pSede
                                    select new { groups.ID, groups.Number, sede_name = sede.Name, professor.Name, classroom.Code, schedule.StartHour, schedule.EndHour, schedule.Day }).ToList();
                    json = JsonConvert.SerializeObject(vInnerJoin);
                    return Content(json);
                }
                */
               // var fullouterjoin = vListGroupCourseLeft.Union(vListGroupCourseRight).Where(p=>p.ID.Equals(p.ID)) ;
                json = JsonConvert.SerializeObject(vListGroupCourseRight);
                return Content(json);

            
            }
            return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        }

        // <summary>
        /// return all sedes in database
        /// </summary>
        /// <autor>Esteban Segura Benavides</autor>
        /// <param name="pIDCurso"></param>
        /// <returns>ID and Name Headquarter</returns>
        [Route("CursoProfesor/Cursos/Sedes")]
        public ActionResult getAllSede()
        {
            if (HttpContext.Request.IsAjaxRequest())
            {
                var vListaSedesForCourse = (
                                        from sedes in db.Sedes
                                        select new { sedes.ID, sedes.Name }).Distinct();
                var json = JsonConvert.SerializeObject(vListaSedesForCourse);
                return Content(json);
            }
            return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        }


        /// <summary>
        /// Get the headquarter according a ID of a Course
        /// </summary>
        /// <autor>Esteban Segura Benavides</autor>
        /// <param name="pIDCurso">ID of a Course in database</param>
        /// <returns>ID and Name Headquarter</returns>
        [Route("CursoProfesor/Cursos/{pIDCurso:int}/Sedes")]
        public ActionResult getSede(int pIDCurso)
        {
            if (HttpContext.Request.IsAjaxRequest())
            {
                var vListaSedesForCourse = (
                                        from curso in db.Courses
                                        join bloque_planes_curso in db.BlocksXPlansXCourses on curso.ID equals bloque_planes_curso.CourseID
                                        join sede in db.Sedes on bloque_planes_curso.SedeID equals sede.ID
                                        where curso.ID == pIDCurso
                                        select new { sede.ID,sede.Name }).Distinct();
                var json = JsonConvert.SerializeObject(vListaSedesForCourse);
                return Content(json);
            }
            return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        }

        /// <summary>
        ///  Get modality ('Diurna' | 'Nocturna') according to Course and Headquarter
        /// </summary>
        /// <autor>Esteban Segura Benavides</autor>
        /// <param name="pIDCurso">ID Course of the database</param>
        /// <param name="pIDSede">ID Headquarter of the database</param>
        /// <returns>ID and Name of Modality</returns>
        [Route("CursoProfesor/Cursos/{pIDCurso:int}/Sedes/{pIDSede:int}/Modalidades")]
        public ActionResult getModality(int pIDCurso, int pIDSede)
        {
            if (HttpContext.Request.IsAjaxRequest())
            { 
                 var vListaModalidades = (
                                        from curso in db.Courses
                                        join bloque_planes_curso in db.BlocksXPlansXCourses on curso.ID equals bloque_planes_curso.CourseID
                                        join bloque_plan in db.AcademicBlocksXStudyPlans on bloque_planes_curso.BlockXPlanID equals bloque_plan.ID
                                        join plan_estudio in db.StudyPlans on bloque_plan.PlanID equals plan_estudio.ID
                                        join sede in db.Sedes on bloque_planes_curso.SedeID equals sede.ID
                                        join modalidad in db.Modalities on plan_estudio.ModeID equals modalidad.ID
                                        where curso.ID == pIDCurso  && sede.ID == pIDSede
                                        select new { modalidad.ID,modalidad.Name }).Distinct();
                var json = JsonConvert.SerializeObject(vListaModalidades);
                return Content(json);
            }
            return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        }

        /// <summary>
        /// Get Study Plan according a Course, Headquarter and Modality
        /// </summary>
        /// <autor>Esteban Segura Benavides</autor>
        /// <param name="pIDCurso">ID of course in database</param>
        /// <param name="pIDSede">ID  of headquarter in database</param>
        /// <param name="pIDModalidad">ID of modality in database</param>
        /// <returns>ID and Name of Study Plan</returns>
        [Route("CursoProfesor/Cursos/{pIDCurso:int}/Sedes/{pIDSede:int}/Modalidades/{pIDModalidad:int}/Planes")]
        public ActionResult getPlan(int pIDCurso, int pIDSede, int pIDModalidad)
        {
            if (HttpContext.Request.IsAjaxRequest())
            {
                var vListaPlan = (from curso in db.Courses
                                                          join bloque_planes_curso in db.BlocksXPlansXCourses on curso.ID equals bloque_planes_curso.CourseID
                                                          join bloque_plan in db.AcademicBlocksXStudyPlans on bloque_planes_curso.BlockXPlanID equals bloque_plan.ID
                                                          join plan_estudio in db.StudyPlans on bloque_plan.PlanID equals plan_estudio.ID
                                                          join sede in db.Sedes on bloque_planes_curso.SedeID equals sede.ID
                                                          join modalidad in db.Modalities on plan_estudio.ModeID equals modalidad.ID
                                                          where curso.ID == pIDCurso && sede.ID == pIDSede&& modalidad.ID==pIDModalidad
                                                          select new { plan_estudio.ID, plan_estudio.Name}).Distinct();

                var json = JsonConvert.SerializeObject(vListaPlan);
                return Content(json);
            }
            return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        }

        /// <summary>
        /// Get ID and Description of Block according Course, Headquarter, Plan and Modality in database
        /// </summary>
        /// <autor>Esteban Segura Benavides</autor>
        /// <param name="pIDCurso">id of course in database</param>
        /// <param name="pIDSede">id of sede in database</param>
        /// <param name="pIDModalidad">id of modality in database</param>
        /// <param name="pIDPlan">id of plan study in database</param>
        /// <returns>ID and Description of Block</returns>
        [Route("CursoProfesor/Cursos/{pIDCurso:int}/Sedes/{pIDSede:int}/Modalidades/{pIDModalidad:int}/Planes/{pIDPlan:int}/Bloques")]
        public ActionResult getBlock(int pIDCurso, int pIDSede, int pIDModalidad, int pIDPlan)
        {
            if (HttpContext.Request.IsAjaxRequest())
            {
                var vListaBlocks = (from curso in db.Courses
                                        join bloque_planes_curso in db.BlocksXPlansXCourses on curso.ID equals bloque_planes_curso.CourseID
                                        join bloque_plan in db.AcademicBlocksXStudyPlans on bloque_planes_curso.BlockXPlanID equals bloque_plan.ID
                                        join bloque in db.AcademicBlocks on bloque_plan.BlockID equals bloque.ID
                                        join plan_estudio in db.StudyPlans on bloque_plan.PlanID equals plan_estudio.ID
                                        
                                        join sede in db.Sedes on bloque_planes_curso.SedeID equals sede.ID
                                        join modalidad in db.Modalities on plan_estudio.ModeID equals modalidad.ID
                                        where curso.ID == pIDCurso && sede.ID == pIDSede && modalidad.ID == pIDModalidad && plan_estudio.ID == pIDPlan
                                        select new { bloque.ID, bloque.Description }).Distinct();

                var json = JsonConvert.SerializeObject(vListaBlocks);
                return Content(json);
            }
            return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        }

        /*Esteban Segura Benavides
         Obtener el horario de un grupo especifico
         de acuerdo a su codigo de curso, sede, modalidad, plan de estudio y numero de grupo*/

        /// <summary>
        /// Get Schedule of Group according a id of Group, Course, Plan, Headquarter, Modality
        /// </summary>
        /// <autor>Esteban Segura Benavides</autor>
        /// <param name="pIDCurso">id of course in database</param>
        /// <param name="pIDSede">id of sede in database</param>
        /// <param name="pIDModalidad">id of modality in database</param>
        /// <param name="pIDPlan">id of plan study in database</param>
        /// <param name="pIDGrupo">id of group in database</param>
        /// <returns>Code of Classroom and StartHour, EndHour and Day of Group</returns>
        [Route("CursoProfesor/Cursos/{pIDCurso:int}/Sedes/{pIDSede:int}/Modalidades/{pIDModalidad:int}/Planes/{pIDPlan:int}/Grupos/{pIDGrupo:int}/Horario")]
        public ActionResult getSchedule(int pIDCurso,int pIDSede, int pIDModalidad, int pIDPlan, int pIDGrupo)
        {

            if (HttpContext.Request.IsAjaxRequest())
            {
                var vListaSchedule = from curso in db.Courses
                                        join bloque_planes_curso in db.BlocksXPlansXCourses on curso.ID equals bloque_planes_curso.CourseID
                                        join bloque_plan in db.AcademicBlocksXStudyPlans on bloque_planes_curso.BlockXPlanID equals bloque_plan.ID     
                                        join plan_estudio in db.StudyPlans on bloque_plan.PlanID equals plan_estudio.ID
                                        join plan_estudio_sede in db.StudyPlansXSedes on plan_estudio.ID equals plan_estudio_sede.StudyPlanID
                                        join sede in db.Sedes on plan_estudio_sede.SedeID equals sede.ID
                                        join modalidad in db.Modalities on plan_estudio.ModeID equals modalidad.ID
                                        join grupo in db.Groups on bloque_planes_curso.ID equals grupo.BlockXPlanXCourseID
                                        join grupo_aula in db.GroupClassrooms on grupo.ID equals grupo_aula.GroupID
                                        join horario in db.Schedules on grupo_aula.ScheduleID equals horario.ID
                                        join aula in db.Classrooms on grupo_aula.ClassroomID equals aula.ID 
                                        where curso.ID == pIDCurso && sede.ID == pIDSede && modalidad.ID == pIDModalidad && plan_estudio.ID == pIDPlan &&
                                        grupo.ID == pIDGrupo
                                                                  select new { horario.Day, horario.StartHour, horario.EndHour,aula.Code };

                var json = JsonConvert.SerializeObject(vListaSchedule);
                return Content(json);
            }
            return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        }

        /// <summary>
        ///  Remove a profesor from a group
        /// </summary>
        /// <autor> Esteban Segura Benavides </autor>
        /// <param name="pIDGrupo"> ID of group in database</param>
        /// <returns>Information about the action of remove a profesor from a group</returns>
        [Route("CursoProfesor/Group/{pIDGroup:int}/removeProfesor")]
        public ActionResult removeGroup(int pIDGroup)
        {
            if (HttpContext.Request.IsAjaxRequest())
            {
                var grupo = db.Groups.Find(pIDGroup);
                grupo.ProfessorID = null;
                grupo.HourAllocatedTypeID = null;
                db.SaveChanges();
                var respuesta = new { respuesta = "success" };
                var json = JsonConvert.SerializeObject(respuesta);
                return Content(json);
            }
            var respuesta_error = new { respuesta = "error" };
            var json_error = JsonConvert.SerializeObject(respuesta_error);
            return Content(json_error);
        }
      
        #endregion
    }
}