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

        // POST: CursoProfesor/Asignar
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AsignarProfesoraCurso(int Profesores, int Grupos_Disponibles)
        {
            if (ModelState.IsValid)
            {

                Grupo grupo = db.Grupos.Find(Grupos_Disponibles);
                grupo.ProfessorID = Profesores;
                db.SaveChanges();

                TempData[TempDataMessageKeySuccess] = "Profesor asignado correctamente";
                return RedirectToAction( "Index");
            }
            return View();
            //var creado = 0;
            //var idProfesorXCurso = 0;
            //var idDetalleGrupo = vRepositorioGrupos.obtenerUnDetalleGrupo(sltGrupo);
            //idProfesorXCurso = repositorioCursoProfesor.asignarProfesor(sltProfesor, txtHoras + txtHorasEstimadas);
            //if (idProfesorXCurso != 0)
            //{
            //    creado = repositorioCursoProfesor.actualizarDetalleGrupo(idProfesorXCurso, idDetalleGrupo.Id);

            //    if (creado != 0)
            //    {
            //        TempData[TempDataMessageKey] = "Profesor asignado correctamente.";
            //    }
            //    else
            //    {
            //        TempData[TempDataMessageKey] = "Ocurrió un error al asignar el profesor.";
            //    }
            //}
            //else
            //{
            //    TempData[TempDataMessageKey] = "No se pudo obtener el id de profesor x curso.";
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
        public ActionResult EditarAsignacion(String ID, int Profesores)
        {
            int idGrupo = Convert.ToInt32(ID);
            if (ModelState.IsValid)
            {

                Grupo grupo = db.Grupos.Find(idGrupo);
                grupo.ProfessorID = Profesores;
                db.SaveChanges();

                TempData[TempDataMessageKeySuccess] = "Profesor asignado correctamente";
                return RedirectToAction("Index");
            }
            return View();
        }

        /*-----------------------------------------------------------------*/
        /*
         * Esteban Segura Benavides
         * Metodo que llama a la vista para asignar un profesor a un curso definido seleccionado de 'Ver Detalle Curso'
         * */
        
        
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
            Curso curso = db.Cursos.Find(id);
            return View(curso);
        }
        /*-------------------------------------------------------------------*/
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        /*----------------------------------------------------------------------------*/
        /* Esteban Segura Benavides Creacion funciones ajax
         * Obtener informacion ajax*/
        #region Ajax
        /*Obtener infomracion completa de un grupo de acuerdo a su id*/
        [Route("Cursos/Grupo/{idGrupo:int}")]
        public ActionResult getInformationGroup(int idGrupo)
        {
            if (HttpContext.Request.IsAjaxRequest())
            {
                var listaCursos = (from curso in db.Cursos
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
                                   where grupo.ID == idGrupo

                                   select new
                                   {curso_id = curso.ID,curso_name = curso.Name,grupo.Number,profesor_name = profesor.Name,horario.StartHour,horario.EndHour,horario.Day,
                                       aula.Code,aula.Capacity,sede_id = sede.ID,sede_name = sede.Name,plan_id = plan_estudio.ID,plan_name = plan_estudio.Name,bloque_id = bloque_academico.ID,
                                       descripcion_bloque = bloque_academico.Description, modalidad_id=modalidad.ID,modalidad_name = modalidad.Name
                                   });
               
                var json = JsonConvert.SerializeObject(listaCursos);
                return Content(json);
            }
            return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        }

        /*Obtener infomracion completa de un grupo de acuerdo a su id*/
        [Route("Cursos/Grupo/EliminarGrupo/{idGrupo:int}")]
        public ActionResult removeGrupo(int idGrupo)
        {
            if (HttpContext.Request.IsAjaxRequest())
            {
                Grupo grupo = db.Grupos.Find(idGrupo);
                grupo.ProfessorID = null;
                db.SaveChanges();
                var respuesta =  new{respuesta = "success"};
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
