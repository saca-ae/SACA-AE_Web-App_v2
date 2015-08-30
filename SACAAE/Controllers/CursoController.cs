using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using SACAAE.Data_Access;
using SACAAE.Models;
using SACAAE.Models.ViewModels;
using Newtonsoft.Json;

namespace SACAAE.Controllers
{
    public class CursoController : Controller
    {
        private SACAAEContext db = new SACAAEContext();
        private const string TempDataMessageKeySuccess = "MessageSuccess";
        private const string TempDataMessageKey = "MessageError";
        // GET: Curso
        public ActionResult Index()
        {
            var cursos = db.Cursos.ToList();
            var viewModel = new List<CursoIndexViewModel>();

            cursos.ForEach(p => viewModel.Add(
                new CursoIndexViewModel()
                {
                    ID = p.ID,
                    Name = p.Name,
                    TheoreticalHours = p.TheoreticalHours,
                    PracticeHours = p.PracticeHours.GetValueOrDefault(),
                    Block = p.Block,
                    Code = p.Code,
                    Credits = p.Credits.GetValueOrDefault(),
                    External = p.External
                }));

            return View(viewModel);
        }

        // GET: Curso/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Curso curso = db.Cursos.Find(id);
            if (curso == null)
            {
                return HttpNotFound();
            }

            return View(curso);
        }

        // GET: Curso/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Curso/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,Name,Code,TheoreticalHours,Block,External,PracticeHours,Credits")] Curso curso)
        {
            if (ModelState.IsValid)
            {
                db.Cursos.Add(curso);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(curso);
        }

        // GET: Curso/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Curso curso = db.Cursos.Find(id);
            if (curso == null)
            {
                return HttpNotFound();
            }
            return View(curso);
        }

        // POST: Curso/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,Name,Code,TheoreticalHours,Block,External,PracticeHours,Credits")] Curso curso)
        {
            if (ModelState.IsValid)
            {
                db.Entry(curso).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(curso);
        }

        // GET: Curso/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Curso curso = db.Cursos.Find(id);
            if (curso == null)
            {
                return HttpNotFound();
            }
            return View(curso);
        }

        // POST: Curso/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Curso curso = db.Cursos.Find(id);
            db.Cursos.Remove(curso);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        /// <summary>
        ///  Show the view for assign Profesor to a Course
        /// </summary>
        /// <autor> Esteban Segura Benavides </autor>
        /// <param name="id"> ID of Course in database</param>
        /// <returns>View for assign profesor to a course with the information of the course</returns>

        // GET: Curso/AsignarProfesoraCurso/{id:int}
        public ActionResult AsignarProfesoraCurso(int id)
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
            ViewBag.Profesores = new SelectList(db.Profesores, "ID", "Name");
            Curso vCurso = db.Cursos.Find(id);
            return View(vCurso);
        }

        /// <summary>
        ///  Save the assign Profesor to a Course if don't have problems with the profesor scheduler
        /// </summary>
        /// <autor> Esteban Segura Benavides </autor>
        /// <param name="id"> ID of Course in database</param>
        /// <returns>View for assign profesor to a course with the information of the course</returns>
        // POST: CursoProfesor/Asignar
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AsignarProfesoraCurso(int ID, int Profesores, int Grupos_Disponibles, int HourCharge)
        {
            if (ModelState.IsValid)
            {
                //Verify if profesor have other assign in the same day and start hour, if don't have conflict with other group in the same hour and day return true, else
                //if found problem with other group in the same day and start hour return false and the assign is cancelled and the user recive information
                Boolean vChoqueHorario = isScheduleProfesorValidate(Grupos_Disponibles, Profesores);

                //If doesn't exist problems in the profesor schedule
                if (!vChoqueHorario)
                {
                    var grupo = db.Grupos.Find(Grupos_Disponibles);
                    grupo.ProfessorID = Profesores;
                    if (HourCharge == 1)
                    {
                        
                        grupo.AssignProfessorTypeID = 1;
                    }
                   db.SaveChanges();

                    TempData[TempDataMessageKeySuccess] = "Profesor asignado correctamente";
                    return RedirectToAction("Details", new { id = ID });
                }
                // Exist problems in profesor schedule, so the assign is cancelled and the user recive the information of the problem
                else
                {
                    TempData[TempDataMessageKey] = "No se puede asignar al profesor al curso\n porque existe choque de horario";

                    /* Se obtiene la lista de profesores */
                    ViewBag.Profesores = new SelectList(db.Profesores, "ID", "Name");
                    /*get the assign type list*/
                    ViewBag.AssignType = new SelectList(db.TipoAsignacionesProfesores, "ID", "Name");
                    Curso vCurso = db.Cursos.Find(ID);
                    return View(vCurso);
                }


               
            }
            return View();
        }

       
        // GET: Curso/DetalleAsignacion/5
        public ActionResult DetalleAsignacion(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Grupo grupo = db.Grupos.Find(id);
            if (grupo == null)
            {
                return HttpNotFound();
            }
            
            return View(grupo);
        }

        // GET: Curso/EditarAsignacion/5
        public ActionResult EditarAsignacion(int? id)
        {
            
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Grupo grupo = db.Grupos.Find(id);
            if (grupo == null)
            {
                return HttpNotFound();
            }
           

            /* Se obtiene la lista de profesores */
            ViewBag.Profesores = new SelectList(db.Profesores, "ID", "Name");
            return View(grupo);
        }


        // POST Curso/EditarAsignacion/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditarAsignacion(String ID, int Profesores, String editHourCharge)
        {
            int idGrupo = Convert.ToInt32(ID);
            int vHourChange = Convert.ToInt32(editHourCharge);
            if (ModelState.IsValid)
            {
                //Verify if profesor have other assign in the same day and start hour, if don't have conflict with other group in the same hour and day return true, else
                //if found problem with other group in the same day and start hour return false and the assign is cancelled and the user recive information
                Boolean vChoqueHorario = isScheduleProfesorValidate(idGrupo, Profesores);
                if (!vChoqueHorario)
                {
                    Grupo grupo = db.Grupos.Find(idGrupo);
                    grupo.ProfessorID = Profesores;
                    if (vHourChange == 1)
                    {
                        grupo.AssignProfessorTypeID = 1;
                    }
                    else
                    {
                        grupo.AssignProfessorTypeID = null;
                    }
                    db.SaveChanges();

                    TempData[TempDataMessageKeySuccess] = "Profesor asignado correctamente";
                    return RedirectToAction("Details", new { id = grupo.BloqueXPlanXCurso.CourseID });
                }
                else
                {
                    /*The user recive the information about the problem*/
                    TempData[TempDataMessageKey] = "No se puede asignar al profesor al curso\n porque existe choque de horario";

                    /* Se obtiene la lista de profesores */
                    Grupo grupo = db.Grupos.Find(idGrupo);
                    ViewBag.Profesores = new SelectList(db.Profesores, "ID", "Name");
                    return View(grupo);
                }
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

        /// <summary>
        /// Verify if profesor have other assign in the same day and start hour, if don't have conflict with other group in the same hour and day return
        /// if found problem with other group in the same day and start hour return false and the assign is cancelled and the user recive information
        /// </summary>
        /// <autor>Esteban Segura Benavides</autor>
        /// <param name="pIDGrupo"></param>
        /// <param name="pIDProfesor"></param>
        /// <returns>true if don't have conflicts with the schedule else return false if exist other grupo in the same start hour and day</returns>
        private Boolean isScheduleProfesorValidate(int pIDGrupo, int pIDProfesor)
        {
            /*Get Group from database accordin to idGrupo*/
            var vListScheduleGroup = (from grupo in db.Grupos
                                      join grupo_aula in db.GrupoAula on grupo.ID equals grupo_aula.GroupID
                                      join horario in db.Horarios on grupo_aula.ScheduleID equals horario.ID
                                      where (grupo.ID == pIDGrupo)
                                      select new { horario.StartHour, horario.EndHour, horario.Day }).ToList();


            /*Get all profesor schedules*/
            var vListScheduleProfesors = (from profesor in db.Profesores
                                          join grupo in db.Grupos on profesor.ID equals grupo.ProfessorID
                                          join grupo_aula in db.GrupoAula on grupo.ID equals grupo_aula.GroupID
                                          join horario in db.Horarios on grupo_aula.ScheduleID equals horario.ID
                                          where (profesor.ID == pIDProfesor)
                                          select new { horario.StartHour, horario.EndHour, horario.Day }).ToList();
            //Bandera que determina si el horario esta repetido o no
            Boolean vChoqueHoraio = false;
            foreach (var vHorarioProfesor in vListScheduleProfesors)
            {
                foreach (var vScheduleGroup in vListScheduleGroup)
                {
                    //If the schedule of profesor is equals to group schedule this is a problem because is assign a group but with the same schedule
                    if (vHorarioProfesor.StartHour.Equals(vScheduleGroup.StartHour) && vHorarioProfesor.Day.Equals(vScheduleGroup.Day))
                    {
                        vChoqueHoraio = true;
                    }
                }
            }

            return vChoqueHoraio;
        }
        /*----------------------------------------------------------------------------*/

        
        #region Ajax
        
        /// <summary>
        ///  Get information of a group
        /// </summary>
        /// <autor> Esteban Segura Benavides </autor>
        /// <param name="pIDGrupo"> ID of group in database</param>
        /// <returns>Information related with the group like course, profesor, scheduler, classroom, block, modality</returns>
        [Route("Cursos/Group/{pIDGrupo:int}")]
        public ActionResult getInformationGroup(int pIDGrupo)
        {
            if (HttpContext.Request.IsAjaxRequest())
            {
                var vInformationGroup = (from curso in db.Cursos
                                   join bloque_planes_curso in db.BloquesXPlanesXCursos on curso.ID equals bloque_planes_curso.CourseID
                                   join bloque_planes in db.BloquesAcademicosXPlanesDeEstudio on bloque_planes_curso.BlockXPlanID equals bloque_planes.ID
                                   join bloque_academico in db.BloquesAcademicos on bloque_planes.BlockID equals bloque_academico.ID
                                   join grupo in db.Grupos on bloque_planes_curso.ID equals grupo.BlockXPlanXCourseID
                                  
                                   join profesor in db.Profesores on grupo.ProfessorID equals profesor.ID
                                   join plan_estudio in db.PlanesDeEstudio on bloque_planes.PlanID equals plan_estudio.ID
                                   join modalidad in db.Modalidades on plan_estudio.ModeID equals modalidad.ID
                                   join plan_sede in db.PlanesDeEstudioXSedes on plan_estudio.ID equals plan_sede.StudyPlanID
                                   join sede in db.Sedes on plan_sede.SedeID equals sede.ID
                                   join grupo_aula in db.GrupoAula on grupo.ID equals grupo_aula.GroupID
                                   join aula in db.Aulas on grupo_aula.ClassroomID equals aula.ID
                                   join horario in db.Horarios on grupo_aula.ScheduleID equals horario.ID
                                   where grupo.ID == pIDGrupo 

                                   select new
                                   {curso_id = curso.ID,curso_name = curso.Name,curso.TheoreticalHours,grupo.Number,profesor_name = profesor.Name,horario.StartHour,horario.EndHour,horario.Day,
                                       aula.Code,aula.Capacity,sede_id = sede.ID,sede_name = sede.Name,plan_id = plan_estudio.ID,plan_name = plan_estudio.Name,bloque_id = bloque_academico.ID,
                                       descripcion_bloque = bloque_academico.Description, modalidad_id=modalidad.ID,modalidad_name = modalidad.Name, asignacion_id=grupo.AssignProfessorTypeID
                                   });
               
                var json = JsonConvert.SerializeObject(vInformationGroup);
                return Content(json);
            }
            return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        }

        
        #endregion

    }
}
