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
        public ActionResult Asignar(int sltProfesor, int sltGrupo, int txtHoras, int txtHorasEstimadas)
        {
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
            var temp = db.ProfesoresXCursos.Find(sltCursosImpartidos);
            if (temp != null)
            {
                db.ProfesoresXCursos.Remove(temp);
                db.SaveChanges();
                revocado = true;
            }

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
                                  join planesestudio in db.PlanesDeEstudio on planesporsede.StudyPlan equals planesestudio.ID
                                  join modalidades in db.Modalidades on planesestudio.Mode equals modalidades.ID
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
                var periodoID = db.Periodos.Where(p => p.Name == periodo).FirstOrDefault().ID;
                var listaGrupos = db.Grupos
                                    .Where(p => (p.DetalleGrupo.Professor == null || p.DetalleGrupo.ProfesorXCurso.Professor == 3)
                                             && p.Period == periodoID
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
                var info = new { grupo.ID, grupo.DetalleGrupo.Capacity };

                return Json(info, JsonRequestBehavior.AllowGet);
            }
            return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        }


        //public ActionResult ObtenerHorario(int cursoxgrupo)
        //{
        //    int idHorario = repositorioCursoProfesor.obtenerHorario(cursoxgrupo);
        //    IQueryable listaHorario = null;

        //    if (idHorario != 0)
        //    {
        //        listaHorario = repositorioCursoProfesor.obtenerInfoHorario(idHorario);

        //        var json = JsonConvert.SerializeObject(listaHorario);

        //        return Content(json);
        //    }

        //    return View(listaHorario);
        //}

        [Route("CursoProfesor/Profesor/Cursos/{idProfesor:int}")]
        public ActionResult ObtenerCursosPorProfesor(int idProfesor)
        {
            if (HttpContext.Request.IsAjaxRequest())
            {
                var listaCursos = from profesores in db.Profesores
                                  join profesoresxcurso in db.ProfesoresXCursos on profesores.ID equals profesoresxcurso.Professor
                                  join detallecurso in db.DetallesDelGrupo on profesoresxcurso.ID equals detallecurso.Professor
                                  join grupo in db.Grupos on detallecurso.ID equals grupo.ID
                                  join bloqueXPlanXCurso in db.BloquesXPlanesXCursos on grupo.BlockXPlanXCourse equals bloqueXPlanXCurso.ID
                                  where profesores.ID == idProfesor
                                  select new { profesoresxcurso.ID, bloqueXPlanXCurso.Curso.Name, bloqueXPlanXCurso.Curso.Code };

                var json = JsonConvert.SerializeObject(listaCursos);
                return Content(json);
            }
            return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        }

        #endregion
    }
}