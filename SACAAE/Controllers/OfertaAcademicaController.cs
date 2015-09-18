using SACAAE.Data_Access;
using SACAAE.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SACAAE.Controllers
{
    public class OfertaAcademicaController : Controller
    {
        private SACAAEContext gvDatabase = new SACAAEContext();
        private const string TempDataMessageKey = "MessageError";
        private const string TempDataMessageKeySuccess = "MessageSuccess";
        // GET: OfertaAcademica
        [Authorize]
        public ActionResult CrearOfertaAcademica()
        {
            ViewBag.Modalidades = ObtenerTodosModalidades();
            ViewBag.Sedes = ObtenerTodosSedes();
            var periodos = gvDatabase.Periods.Select(p => new
            {
                ID = p.ID,
                Name = (p.Year + " - " + p.Number.Type.Name + " " + p.Number.Number)
            }).ToList();
            ViewBag.Periodos = periodos;
            return View();
        }

        [Authorize]
        public ActionResult Index()
        {
            ViewBag.Modalidades = ObtenerTodosModalidades();
            ViewBag.Sedes = ObtenerTodosSedes();
            var periodos = gvDatabase.Periods.Select(p => new
            {
                ID = p.ID,
                Name = (p.Year + " - " + p.Number.Type.Name + " " + p.Number.Number)
            }).ToList();
            ViewBag.Periodos = periodos;
            return View();
        }

        [Authorize]
        public ActionResult EliminarOferta(int id)
        {
            var model = obtenerUnGrupo(id);
            return View(model);
        }


        [Authorize]
        [HttpPost]
        public ActionResult CrearOfertaAcademica(string sltPeriodo, string sltSede, string sltPlan,
            string sltBloque, string sltCurso, int cantidadGrupos)
        {
            if (String.IsNullOrEmpty(sltPeriodo))
            {
                TempData[TempDataMessageKey] = "Es necesario que seleccione un Periodo";
                return RedirectToAction("CrearOfertaAcademica");
            }
            if (String.IsNullOrEmpty(sltSede))
            {
                TempData[TempDataMessageKey] = "Es necesario que seleccione una Sede";
                return RedirectToAction("CrearOfertaAcademica");
            }
            if (String.IsNullOrEmpty(sltPlan))
            {
                TempData[TempDataMessageKey] = "Es necesario que seleccione un Plan";
                return RedirectToAction("CrearOfertaAcademica");
            }
            if (String.IsNullOrEmpty(sltBloque))
            {
                TempData[TempDataMessageKey] = "Es necesario que seleccione un Bloque";
                return RedirectToAction("CrearOfertaAcademica");
            }
            if (String.IsNullOrEmpty(sltCurso))
            {
                TempData[TempDataMessageKey] = "Es necesario que seleccione un Curso";
                return RedirectToAction("CrearOfertaAcademica");
            }

            int vPeriodoID = Int16.Parse(sltPeriodo);
            int vSedeID = Int16.Parse(sltSede);
            int vPlanID = Int16.Parse(sltPlan);
            int vBloqueID = Int16.Parse(sltBloque);
            int vCursoID = Int16.Parse(sltCurso);

            int vPlanXSedeID = tomarIDPlanXSede(vSedeID, vPlanID).ID;
            int vBloqueXPlanID = obtenerIdBloqueXPlan(vPlanID, vBloqueID);
            int vBloqueXPlanXCursoID = obtenerBloqueXPlanXCursoID(vBloqueXPlanID, vCursoID);
            for (int vContadorGrupos = 0; vContadorGrupos < cantidadGrupos; vContadorGrupos++)
            {
                int vNumeroGrupo = ObtenerUltimoNumeroGrupo(vPlanXSedeID, vPeriodoID, vBloqueXPlanXCursoID) + 1;
                Group vNewGrupo = new Group();
                vNewGrupo.Number = vNumeroGrupo;
                vNewGrupo.PeriodID = vPeriodoID;
                vNewGrupo.BlockXPlanXCourseID = vBloqueXPlanXCursoID;

                agregarGrupo(vNewGrupo);
            }
            TempData[TempDataMessageKeySuccess] = "Los Grupos fueron creados correctamente";
            return RedirectToAction("Index");
        }

        public ActionResult ObtenerOfertasAcademicas(int sede, int plan, int periodo)
        {
            IQueryable listaOfertas = ListarGruposXSedeXPeriodo(plan, periodo);
            if (HttpContext.Request.IsAjaxRequest())
            {
                return Json(listaOfertas, JsonRequestBehavior.AllowGet);
            }
            return View(listaOfertas);
        }

        [Authorize]
        [HttpPost]
        public ActionResult EliminarOferta(Group grupo)
        {
            eliminarGrupo(grupo);
            TempData[TempDataMessageKey] = "El registro ha sido borrado correctamente.";
            return RedirectToAction("Index");
        }

        public IQueryable<Modality> ObtenerTodosModalidades()
        {
            return from Modalidades in gvDatabase.Modalities
                   orderby Modalidades.Name
                   select Modalidades;
        }

        public IQueryable<Sede> ObtenerTodosSedes()
        {
            return from Sede in gvDatabase.Sedes
                   orderby Sede.Name
                   select Sede;
        }

        public StudyPlanXSede tomarIDPlanXSede(int idSede, int idPlan)
        {
            return gvDatabase.StudyPlansXSedes.SingleOrDefault(plansSede => plansSede.StudyPlanID == idPlan && plansSede.SedeID == idSede);
        }

        public int obtenerIdBloqueXPlan(int pPlanID, int pBloqueID)
        {
            return (gvDatabase.AcademicBlocksXStudyPlans.SingleOrDefault(relacion => relacion.PlanID == pPlanID && relacion.BlockID == pBloqueID).ID);
        }

        public int obtenerBloqueXPlanXCursoID(int pBloqueXPlanID, int pCursoID)
        {
            return (gvDatabase.BlocksXPlansXCourses.SingleOrDefault(relacion => relacion.BlockXPlanID == pBloqueXPlanID && relacion.CourseID == pCursoID).ID);
        }

        public Group obtenerUnGrupo(int id)
        {
            return gvDatabase.Groups.SingleOrDefault(grupo => grupo.ID == id);
        }

        public int ObtenerUltimoNumeroGrupo(int PlanXSedeID, int PeriodoID, int BloqueXPlanXCursoID)
        {
            // grupos.PlanDeEstudio == PlanXSedeID &&
            var vGrupos = from grupos in gvDatabase.Groups
                          where grupos.PeriodID == PeriodoID && grupos.BlockXPlanXCourseID == BloqueXPlanXCursoID
                          orderby grupos.Number descending
                          select grupos;
            if (vGrupos.Any())
                return vGrupos.First().Number;
            else
                return 0;
        }

        public void agregarGrupo(Group pGrupo)
        {
            gvDatabase.Groups.Add(pGrupo);
            gvDatabase.SaveChanges();
        }

        public IQueryable ListarGruposXSedeXPeriodo(int pPlanID, int pPeriodoID)
        {
            // Grupos.PlanDeEstudio == pPlanID &&
            return from Grupos in gvDatabase.Groups
                   join BloqueXPlanXCursos in gvDatabase.BlocksXPlansXCourses on Grupos.BlockXPlanXCourseID equals BloqueXPlanXCursos.ID
                   join Cursos in gvDatabase.Courses on BloqueXPlanXCursos.CourseID equals Cursos.ID
                   where Grupos.PeriodID == pPeriodoID
                   select new { Grupos.ID, Grupos.Number, Cursos.Name};
        }

        public void eliminarGrupo(Group pGrupo)
        {
            var vGrupo = gvDatabase.Groups.SingleOrDefault(grupo => grupo.ID == pGrupo.ID);
            if (vGrupo != null)
            {
                gvDatabase.Groups.Remove(vGrupo);
                gvDatabase.SaveChanges();
            }
            else
                return;
        }
    }
}