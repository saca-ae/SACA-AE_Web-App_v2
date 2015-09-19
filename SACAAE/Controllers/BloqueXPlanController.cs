/*
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SACAAE.Controllers
{
    public class BloqueXPlanController : Controller
    {
        private RepositorioPlanesDeEstudio vRepoPlanes = new RepositorioPlanesDeEstudio();
        private RepositorioBloqueAcademico vRepoBloques = new RepositorioBloqueAcademico();
        private RepositorioBloqueXPlan vRepoBloquesXPlan = new RepositorioBloqueXPlan();
        private const string TempDataMessageKey = "MessageError";
        private const string TempDataMessageKeySuccess = "MessageSuccess";
        // GET: BloqueXPlan
        [Authorize]
        public ActionResult CrearBloqueXPlan(int plan)
        {
            var vBloqueXPlan = new BloqueAcademicoXPlanDeEstudio();
            ViewBag.Planes = vRepoPlanes.ObtenerUnPlanDeEstudio(plan);
            ViewBag.Bloques = vRepoBloques.ListarBloquesAcademicos();
            return View();
        }

        [Authorize]
        [HttpPost]
        public ActionResult CrearBloqueXPlan(string button, BloqueAcademicoXPlanDeEstudio pBloqueXPlan, string selectPlanDeEstudio, string selectBloqueAcademico)
        {
            int PlanID = Int16.Parse(selectPlanDeEstudio);
            if (button == "Asignar Curso")
                return RedirectToAction("CrearBloqueXPlanXCurso", "BloqueXPlanXCurso", new { plan = PlanID });

            if (pBloqueXPlan != null && selectPlanDeEstudio != null && selectBloqueAcademico != null)
            {
                int BloqueID = Int16.Parse(selectBloqueAcademico);
                pBloqueXPlan.PlanID = PlanID;
                pBloqueXPlan.BloqueID = BloqueID;
                if (vRepoBloquesXPlan.existeRelacionBloqueXPlan(pBloqueXPlan.PlanID, pBloqueXPlan.BloqueID))
                {
                    TempData[TempDataMessageKey] = "Este plan ya cuenta con el bloque seleccionado. Por Favor intente de nuevo.";
                    return RedirectToAction("CrearBloqueXPlan", new { plan = PlanID });
                }
                vRepoBloquesXPlan.crearRelacionBloqueXPlan(pBloqueXPlan);
                TempData[TempDataMessageKeySuccess] = "El bloque ha sido asignado al plan de estudio exitosamente";
                return RedirectToAction("CrearBloqueXPlan", new { plan = PlanID });

            }
            TempData[TempDataMessageKey] = "Datos ingresados son inválidos";
            return RedirectToAction("CrearBloqueXPlan", new { plan = PlanID });
        }

        public ActionResult ObtenerBloques(int plan)
        {
            IQueryable listaBloques = vRepoBloques.ListarBloquesXPlan(plan);
            if (HttpContext.Request.IsAjaxRequest())
            {
                return Json(new SelectList(
                        listaBloques,
                        "ID",
                        "Descripcion"), JsonRequestBehavior.AllowGet
                        );
            }
            return View(listaBloques);
        }
    }
}
*/