using Newtonsoft.Json;
using SACAAE.Data_Access;
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

            /* Se obtiene la lista de profesores */
            ViewBag.Profesores = new SelectList(db.Profesores, "ID", "Name");
            /* Se obtiene la lista de sedes */
            ViewBag.Sedes = new SelectList(db.Sedes, "ID", "Name");
            /* Se obtiene la lista de modalidades */
            ViewBag.Modalidades = new SelectList(db.Modalidades, "ID", "Name"); 

            return View();
        }

        // POST: CursoProfesor/Asignar
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Asignar(int idProfesor, int idGrupo, int txtHoras, int txtHorasEstimadas)
        {
            db.Grupos.Find(idGrupo).Professor=idProfesor;
            db.SaveChanges();
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
            //}

            return RedirectToAction("Asignar");
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
            ViewBag.Profesores = new SelectList(db.Profesores, "ID", "Name");

            return View();
        }

        // POST: CursoProfesor/revocar
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult revocar(int sltCursosImpartidos)
        {
            var revocado = false;
            //var temp = db.ProfesoresXCursos.Find(sltCursosImpartidos);
            //if (temp != null)
            //{
            //    db.ProfesoresXCursos.Remove(temp);
            //    db.SaveChanges();
            //    revocado = true;
            //}

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

        [Route("CursoProfesor/Planes/List/{sede:int}/{modalidad:int}")]
        public ActionResult ObtenerPlanesEstudio(int sede, int modalidad)
        {
            if (HttpContext.Request.IsAjaxRequest())
            {
                var listaPlanes = from sedes in db.Sedes
                                  join planesporsede in db.PlanesDeEstudioXSedes on sedes.ID equals planesporsede.SedeID
                                  join planesestudio in db.PlanesDeEstudio on planesporsede.StudyPlanID equals planesestudio.ID
                                  join modalidades in db.Modalidades on planesestudio.ModeID equals modalidades.ID
                                  where (sedes.ID == sede) && (modalidades.ID == modalidad)
                                  select new { planesestudio.ID, planesestudio.Name };

                return Json(listaPlanes, JsonRequestBehavior.AllowGet);
            }
            return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        }

        [Route("CursoProfesor/Bloques/List/{plan:int}")]
        public ActionResult ObtenerBloques(int plan)
        {
            if (HttpContext.Request.IsAjaxRequest())
            {
                var listaBloques = db.PlanesDeEstudio.Find(plan)
                                     .BloquesAcademicosXPlanesDeEstudio
                                     .Select(p => new { p.BloqueAcademico.ID, p.BloqueAcademico.Description });

                return Json(listaBloques, JsonRequestBehavior.AllowGet);
            }
            return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        }

        [Route("CursoProfesor/Cursos/List/{plan:int}/{bloque:int}")]
        public ActionResult ObtenerCursos(int plan, int bloque)
        {
            if (HttpContext.Request.IsAjaxRequest())
            {
                var listaCursos = db.BloquesAcademicosXPlanesDeEstudio
                                    .Where(p => p.BlockID == bloque && p.PlanID == plan).FirstOrDefault()
                                    .BloquesXPlanesXCursos.Select(p => new { p.Curso.ID, p.Curso.Code, p.Curso.Name });

                return Json(listaCursos, JsonRequestBehavior.AllowGet);
            }
            return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        }

        [Route("CursoProfesor/GruposSinProfesor/List/{curso:int}/{plan:int}/{sede:int}/{bloque:int}")]
        public ActionResult ObtenerGruposSinProfe(int curso, int plan, int sede, int bloque)
        {
            if (HttpContext.Request.IsAjaxRequest())
            {
                var periodo = Request.Cookies["Periodo"].Value;
                var periodoID = db.Periodos.Find(int.Parse(periodo)).ID;
                var listaGrupos = db.Grupos
                                    .Where(p => (p.ProfessorID == null || p.ProfessorID == 3)
                                             && p.PeriodID == periodoID
                                             && p.BloqueXPlanXCurso.CourseID == curso
                                             && p.BloqueXPlanXCurso.BloqueAcademicoXPlanDeEstudio.PlanID == plan
                                             && p.BloqueXPlanXCurso.BloqueAcademicoXPlanDeEstudio.BlockID == bloque
                                            )
                                    .Select(p => new{ p.ID, p.Number });

                return Json(listaGrupos, JsonRequestBehavior.AllowGet);
            }
            return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        }

        [Route("CursoProfesor/Grupos/Info/{cursoxgrupo:int}")]
        public ActionResult ObtenerInfo(int cursoxgrupo)
        {
            if (HttpContext.Request.IsAjaxRequest())
            {
                var grupo = db.Grupos.Find(cursoxgrupo);
                var info = new { grupo.ID, grupo.Capacity,grupo.BloqueXPlanXCurso.Curso.TheoreticalHours };

                return Json(info, JsonRequestBehavior.AllowGet);
            }
            return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        }

        
        [Route("CursoProfesor/Profesor/Cursos/{idProfesor:int}")]
        public ActionResult ObtenerCursosPorProfesor(int idProfesor)
        {
            if (HttpContext.Request.IsAjaxRequest())
            {
                var listaCursos = from profesores in db.Profesores
                                  join grupo in db.Grupos on profesores.ID equals grupo.ProfessorID
                                  join bloqueXPlanXCurso in db.BloquesXPlanesXCursos on grupo.BlockXPlanXCourseID equals bloqueXPlanXCurso.ID
                                  where profesores.ID == idProfesor
                                  select new { profesores.ID, bloqueXPlanXCurso.Curso.Name, bloqueXPlanXCurso.Curso.Code }; //Revisar

                var json = JsonConvert.SerializeObject(listaCursos);
                return Content(json);
            }
            return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        }

        /*Esteban Segura Benavides 
         * Obtener grupos de un curso de acuerdo al id del curso*/

        [Route("CursoProfesor/Cursos/{idCurso:int}")]
        public ActionResult ObtenerGruposdeCurso(int idCurso)
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
                                   join plan_sede in db.PlanesDeEstudioXSedes on plan_estudio.ID equals plan_sede.StudyPlanID
                                   join sede in db.Sedes on plan_sede.SedeID equals sede.ID
                                   join grupo_aula in db.GrupoAula on grupo.ID equals grupo_aula.GroupID
                                   join aula in db.Aulas on grupo_aula.ClassroomID equals aula.ID
                                   join horario in db.Horarios on grupo_aula.ScheduleID equals horario.ID
                                   where curso.ID == idCurso && sede.Name == "San Carlos"

                                   select new { grupo.ID, grupo.Number, profesor.Name, aula.Code, horario.StartHour, horario.EndHour, horario.Day });
                                    
                  
                 
                var json = JsonConvert.SerializeObject(listaCursos);
                return Content(json);
            }
            return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        }


        /*Esteban Segura Benavides
        * Obtener la sede de curso especifico de acuerdo a su id*/
        [Route("CursoProfesor/Cursos/Sedes/{idCurso:int}")]
        public ActionResult ObtenerSedesegunCurso(int idCurso)
        {
            if (HttpContext.Request.IsAjaxRequest())
            {
                var listaSedesxCurso = (
                                        from curso in db.Cursos
                                        join bloque_planes_curso in db.BloquesXPlanesXCursos on curso.ID equals bloque_planes_curso.CourseID
                                        join bloque_plan in db.BloquesAcademicosXPlanesDeEstudio on bloque_planes_curso.BlockXPlanID equals bloque_plan.ID
                                        join plan_estudio in db.PlanesDeEstudio on bloque_plan.PlanID equals plan_estudio.ID
                                        join plan_estudio_sede in db.PlanesDeEstudioXSedes on plan_estudio.ID equals plan_estudio_sede.StudyPlanID
                                        join sede in db.Sedes on plan_estudio_sede.SedeID equals sede.ID
                                        where curso.ID == idCurso
                                        select new { sede.ID,sede.Name }).Distinct();
                var json = JsonConvert.SerializeObject(listaSedesxCurso);
                return Content(json);
            }
            return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        }

        /*Esteban Segura Benavides
       * Obtener la modalidad de curso especifico de acuerdo a su id*/
        [Route("CursoProfesor/Cursos/Sedes/Modalidad/{idCurso:int}/{idSede:int}")]
        public ActionResult ObtenerModalidadsegunCursoySede(int idCurso, int idSede)
        {
            if (HttpContext.Request.IsAjaxRequest())
            { 
                 var listaModalidadxCursoxSede = (
                                        from curso in db.Cursos
                                        join bloque_planes_curso in db.BloquesXPlanesXCursos on curso.ID equals bloque_planes_curso.CourseID
                                        join bloque_plan in db.BloquesAcademicosXPlanesDeEstudio on bloque_planes_curso.BlockXPlanID equals bloque_plan.ID
                                        join plan_estudio in db.PlanesDeEstudio on bloque_plan.PlanID equals plan_estudio.ID
                                        join plan_estudio_sede in db.PlanesDeEstudioXSedes on plan_estudio.ID equals plan_estudio_sede.StudyPlanID
                                        join sede in db.Sedes on plan_estudio_sede.SedeID equals sede.ID
                                        join modalidad in db.Modalidades on plan_estudio.ModeID equals modalidad.ID
                                        where curso.ID == idCurso  && sede.ID == idSede
                                        select new { modalidad.ID,modalidad.Name }).Distinct();
                var json = JsonConvert.SerializeObject(listaModalidadxCursoxSede);
                return Content(json);
            }
            return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        }


        /*Esteban Segura Benavides
       * Obtener el plan de estudio segun el curso especifico de acuerdo a su id, sede y modalidad*/
        /*CursoProfesor/Curso/{idCurso:int}/Sedes/{idSedes:int}/Modalida
         
        */
        [Route("CursoProfesor/Cursos/Sedes/Modalidad/Plan/{idCurso:int}/{idSede:int}/{idModalidad:int}")]
        public ActionResult ObtenerModalidadsegunCursoySedeyModalidad(int idCurso, int idSede, int idModalidad)
        {
            if (HttpContext.Request.IsAjaxRequest())
            {
                var listaPlansegunCursoxSedexModalidad = (from curso in db.Cursos
                                                          join bloque_planes_curso in db.BloquesXPlanesXCursos on curso.ID equals bloque_planes_curso.CourseID
                                                          join bloque_plan in db.BloquesAcademicosXPlanesDeEstudio on bloque_planes_curso.BlockXPlanID equals bloque_plan.ID
                                                          join plan_estudio in db.PlanesDeEstudio on bloque_plan.PlanID equals plan_estudio.ID
                                                          join plan_estudio_sede in db.PlanesDeEstudioXSedes on plan_estudio.ID equals plan_estudio_sede.StudyPlanID
                                                          join sede in db.Sedes on plan_estudio_sede.SedeID equals sede.ID
                                                          join modalidad in db.Modalidades on plan_estudio.ModeID equals modalidad.ID
                                                          where curso.ID == idCurso && sede.ID == idSede&& modalidad.ID==idModalidad
                                                          select new { plan_estudio.ID, plan_estudio.Name}).Distinct();

                var json = JsonConvert.SerializeObject(listaPlansegunCursoxSedexModalidad);
                return Content(json);
            }
            return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        }

        /*Esteban Segura Benavides
       * Obtener el bloque segun el curso especifico de acuerdo a su id, sede y modalidad y plan de estudio*/

        [Route("CursoProfesor/Cursos/Sedes/Modalidad/Plan/Bloque/{idCurso:int}/{idSede:int}/{idModalidad:int}/{idPlan:int}")]
        public ActionResult ObtenerModalidadsegunCursoySedeyModalidadyPlan(int idCurso, int idSede, int idModalidad, int idPlan)
        {
            if (HttpContext.Request.IsAjaxRequest())
            {
                var listaPlansegunCursoxSedexModalidad = (from curso in db.Cursos
                                                          join bloque_planes_curso in db.BloquesXPlanesXCursos on curso.ID equals bloque_planes_curso.CourseID
                                                          join bloque_plan in db.BloquesAcademicosXPlanesDeEstudio on bloque_planes_curso.BlockXPlanID equals bloque_plan.ID
                                                          join bloque in db.BloquesAcademicos on bloque_plan.BlockID equals bloque.ID
                                                          join plan_estudio in db.PlanesDeEstudio on bloque_plan.PlanID equals plan_estudio.ID
                                                          join plan_estudio_sede in db.PlanesDeEstudioXSedes on plan_estudio.ID equals plan_estudio_sede.StudyPlanID
                                                          join sede in db.Sedes on plan_estudio_sede.SedeID equals sede.ID
                                                          join modalidad in db.Modalidades on plan_estudio.ModeID equals modalidad.ID
                                                          where curso.ID == idCurso && sede.ID == idSede && modalidad.ID == idModalidad && plan_estudio.ID == idPlan
                                                          select new { bloque.ID, bloque.Description }).Distinct();

                var json = JsonConvert.SerializeObject(listaPlansegunCursoxSedexModalidad);
                return Content(json);
            }
            return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        }

        /*Esteban Segura Benavides
         Obtener el horario de un grupo especifico
         de acuerdo a su codigo de curso, sede, modalidad, plan de estudio y numero de grupo*/
        [Route("CursoProfesor/Cursos/Sedes/Modalidad/Plan/Grupo/Horario/{idCurso:int}/{idSede:int}/{idModalidad:int}/{idPlan:int}/{idGrupo:int}")]
        public ActionResult ObtenerHorario(int idCurso,int idSede, int idModalidad, int idPlan, int idGrupo)
        {

            if (HttpContext.Request.IsAjaxRequest())
            {
                var HorariosegunCursoxGrupoxSedexModalidadxPlan = from curso in db.Cursos
                                                                  join bloque_planes_curso in db.BloquesXPlanesXCursos on curso.ID equals bloque_planes_curso.CourseID
                                                                  join bloque_plan in db.BloquesAcademicosXPlanesDeEstudio on bloque_planes_curso.BlockXPlanID equals bloque_plan.ID
                                                          
                                                                  join plan_estudio in db.PlanesDeEstudio on bloque_plan.PlanID equals plan_estudio.ID
                                                                  join plan_estudio_sede in db.PlanesDeEstudioXSedes on plan_estudio.ID equals plan_estudio_sede.StudyPlanID
                                                                  join sede in db.Sedes on plan_estudio_sede.SedeID equals sede.ID
                                                                  join modalidad in db.Modalidades on plan_estudio.ModeID equals modalidad.ID
                                                                  join grupo in db.Grupos on bloque_planes_curso.ID equals grupo.BlockXPlanXCourseID
                                                                  join grupo_aula in db.GrupoAula on grupo.ID equals grupo_aula.GroupID
                                                                  join horario in db.Horarios on grupo_aula.ScheduleID equals horario.ID
                                                                  join aula in db.Aulas on grupo_aula.ClassroomID equals aula.ID 
                                                                  where curso.ID == idCurso && sede.ID == idSede && modalidad.ID == idModalidad && plan_estudio.ID == idPlan &&
                                                                  grupo.ID == idGrupo
                                                                  select new { horario.Day, horario.StartHour, horario.EndHour,aula.Code };

                var json = JsonConvert.SerializeObject(HorariosegunCursoxGrupoxSedexModalidadxPlan);
                return Content(json);
            }
            return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        }

        #endregion
    }
}