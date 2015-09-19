/*
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SACAAE.Controllers
{
    public class BloqueXPlanXCursoController : Controller
    {
        private RepositorioPlanesDeEstudio vRepoPlanes = new RepositorioPlanesDeEstudio();
        private RepositorioBloqueAcademico vRepoBloques = new RepositorioBloqueAcademico();
        private RepositorioBloqueXPlan vRepoBloqueXPlan = new RepositorioBloqueXPlan();
        private RepositorioCursos vRepoCursos = new RepositorioCursos();
        private RepositorioBloqueXPlanXCurso vRepoBloquesXPlanXCurso = new RepositorioBloqueXPlanXCurso();
        private const string TempDataMessageKey = "MessageError";
        private const string TempDataMessageKeySuccess = "MessageSuccess";
        // GET: BloqueXPlanXCurso
        [Authorize]
        public ActionResult CrearBloqueXPlanXCurso(int plan)
        {
            var vBloqueXPlan = new BloqueAcademicoXPlanDeEstudio();
            ViewBag.Planes = vRepoPlanes.ObtenerUnPlanDeEstudio(plan);
            ViewBag.Bloques = vRepoBloques.obtenerBloques(plan);
            ViewBag.Cursos = vRepoCursos.ObtenerCursos();
            return View();
        }

        [Authorize]
        [HttpPost]
        public ActionResult CrearBloqueXPlanXCurso(BloqueXPlanXCurso pBloqueXPlanXCurso, string selectPlanDeEstudio, string selectBloqueAcademico, string selectCurso)
        {
            int PlanID = Int16.Parse(selectPlanDeEstudio);
            if (pBloqueXPlanXCurso != null && selectPlanDeEstudio != null && selectBloqueAcademico != null && selectCurso != null)
            {
                int BloqueID = Int16.Parse(selectBloqueAcademico);
                int CursoID = Int16.Parse(selectCurso);

                pBloqueXPlanXCurso.CursoID = CursoID;
                pBloqueXPlanXCurso.BloqueXPlanID = vRepoBloqueXPlan.idBloqueXPlan(PlanID, BloqueID);
                if (vRepoBloquesXPlanXCurso.existeRelacionBloqueXPlanXCurso(pBloqueXPlanXCurso.BloqueXPlanID, pBloqueXPlanXCurso.CursoID))
                {
                    TempData[TempDataMessageKey] = "El Bloque académico de este plan de estudio ya cuenta con el curso seleccionado. Por Favor intente de nuevo.";
                    return RedirectToAction("CrearBloqueXPlanXCurso", new { plan = PlanID });
                }
                if (vRepoBloquesXPlanXCurso.existeRelacionCursoEnPlan(PlanID, pBloqueXPlanXCurso.CursoID))
                {
                    TempData[TempDataMessageKey] = "El Plan de estudio  ya cuenta con el curso seleccionado. Por Favor intente de nuevo.";
                    return RedirectToAction("CrearBloqueXPlanXCurso", new { plan = PlanID });
                }
                vRepoBloquesXPlanXCurso.crearRelacionBloqueXPlanXCurso(pBloqueXPlanXCurso);
                TempData[TempDataMessageKeySuccess] = "El curso ha sido asignado al bloque académico del plan de estudio exitosamente";
                return RedirectToAction("CrearBloqueXPlanXCurso", new { plan = PlanID });

            }
            TempData[TempDataMessageKey] = "Datos ingresados son inválidos";
            return RedirectToAction("CrearBloqueXPlanXCurso", new { plan = PlanID });
        }

        public ActionResult ObtenerCursos(int plan, int bloque)  //Por entidad
        {
            String entidad = Request.Cookies["Entidad"].Value;
            int entidadID = 0;

            if (entidad.Equals("TEC"))
            {
                entidadID = 1;

            }
            else if (entidad.Equals("CIE"))
            {
                entidadID = 7;
            }
            else if (entidad.Equals("TAE"))
            {
                entidadID = 5;
            }
            else if (entidad.Equals("MAE"))
            {
                entidadID = 6;
            }
            else if (entidad.Equals("DDE"))
            {
                entidadID = 11;
            }
            else if (entidad.Equals("Emprendedores"))
            {
                entidadID = 12;
            }
            else if (entidad.Equals("Actualizacion_Cartago"))
            {
                entidadID = 9;
            }
            else
            {
                entidadID = 8;
            }

            IQueryable vListaCursos = vRepoCursos.ObtenerCursosXEntidad(plan, bloque, entidadID);
            if (HttpContext.Request.IsAjaxRequest())
            {
                return Json(new SelectList(
                        vListaCursos,
                        "ID",
                        "Nombre"), JsonRequestBehavior.AllowGet
                        );
            }
            return View(vListaCursos);
        }
    }
}

*/