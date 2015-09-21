using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SACAAE.Data_Access;
using SACAAE.Models;
using SACAAE.Models.ViewModels;

namespace SACAAE.Controllers
{
    public class BloqueXPlanController : Controller
    {
        private SACAAEContext gvDatabase = new SACAAEContext();
        private const string TempDataMessageKey = "MessageError";
        private const string TempDataMessageKeySuccess = "MessageSuccess";
        // GET: BloqueXPlan
        [Authorize]
        public ActionResult CrearBloqueXPlan(int plan)
        {
            ViewBag.Planes = ObtenerUnPlanDeEstudio(plan);
            ViewBag.Bloques = ListarBloquesAcademicos();
            return View();
        }

        [Authorize]
        [HttpPost]
        public ActionResult CrearBloqueXPlan(string button, AcademicBlockXStudyPlan pBloqueXPlan, string selectPlanDeEstudio, string selectBloqueAcademico)
        {
            int PlanID = Int16.Parse(selectPlanDeEstudio);
            if (button == "Asignar Curso")
                return RedirectToAction("CrearBloqueXPlanXCurso", "BloqueXPlanXCurso", new { plan = PlanID });

            if (pBloqueXPlan != null && selectPlanDeEstudio != null && selectBloqueAcademico != null)
            {
                int BloqueID = Int16.Parse(selectBloqueAcademico);
                pBloqueXPlan.PlanID = PlanID;
                pBloqueXPlan.BlockID = BloqueID;
                if (existeRelacionBloqueXPlan(pBloqueXPlan.PlanID, pBloqueXPlan.BlockID))
                {
                    TempData[TempDataMessageKey] = "Este plan ya cuenta con el bloque seleccionado. Por Favor intente de nuevo.";
                    return RedirectToAction("CrearBloqueXPlan", new { plan = PlanID });
                }
                crearRelacionBloqueXPlan(pBloqueXPlan);
                TempData[TempDataMessageKeySuccess] = "El bloque ha sido asignado al plan de estudio exitosamente";
                return RedirectToAction("CrearBloqueXPlan", new { plan = PlanID });

            }
            TempData[TempDataMessageKey] = "Datos ingresados son inválidos";
            return RedirectToAction("CrearBloqueXPlan", new { plan = PlanID });
        }


        [Route("BloqueXPlan/Bloques/List/{plan:int}")]
        public ActionResult ObtenerBloques(int plan)
        {
            IQueryable listaBloques = ListarBloquesXPlan(plan);
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

        public StudyPlan ObtenerUnPlanDeEstudio(int plan)
        {
            return (from PlanesDeEstudio in gvDatabase.StudyPlans
                    where PlanesDeEstudio.ID == plan
                    select PlanesDeEstudio).FirstOrDefault(); ;
        }

        public IQueryable<AcademicBlock> ListarBloquesAcademicos()
        {
            return from BloquesAcademicos in gvDatabase.AcademicBlocks
                   select BloquesAcademicos;
        }

        public IQueryable<AcademicBlock> ListarBloquesXPlan(int pPlanID)
        {
            return from Bloques in gvDatabase.AcademicBlocks
                   join BloquesXPlan in gvDatabase.AcademicBlocksXStudyPlans on Bloques.ID equals BloquesXPlan.BlockID
                   where BloquesXPlan.PlanID == pPlanID
                   select Bloques;
        }

        public bool existeRelacionBloqueXPlan(int pPlanID, int pBloqueID)
        {
            return (gvDatabase.AcademicBlocksXStudyPlans.SingleOrDefault(relacion => relacion.PlanID == pPlanID && relacion.BlockID == pBloqueID) != null);
        }

        public void crearRelacionBloqueXPlan(AcademicBlockXStudyPlan pBloqueXPlan)
        {
            if (existeRelacionBloqueXPlan(pBloqueXPlan.PlanID, pBloqueXPlan.BlockID))
                return;
            else
            {
                gvDatabase.AcademicBlocksXStudyPlans.Add(pBloqueXPlan);
                gvDatabase.SaveChanges();
            }
        }
    }
}