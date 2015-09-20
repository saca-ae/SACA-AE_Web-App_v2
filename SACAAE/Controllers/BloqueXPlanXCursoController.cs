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
    public class BloqueXPlanXCursoController : Controller
    {
        private SACAAEContext gvDatabase = new SACAAEContext();
        private const string TempDataMessageKey = "MessageError";
        private const string TempDataMessageKeySuccess = "MessageSuccess";
        // GET: BloqueXPlanXCurso
        [Authorize]
        public ActionResult CrearBloqueXPlanXCurso(int plan)
        {
            ViewBag.Planes = ObtenerUnPlanDeEstudio(plan);
            ViewBag.Bloques = obtenerBloques(plan);
            ViewBag.Cursos = ObtenerCursos();
            return View();
        }

        [Authorize]
        [HttpPost]
        public ActionResult CrearBloqueXPlanXCurso(BlockXPlanXCourse pBloqueXPlanXCurso, string selectPlanDeEstudio, string selectBloqueAcademico, string selectCurso)
        {
            int PlanID = Int16.Parse(selectPlanDeEstudio);
            if (pBloqueXPlanXCurso != null && selectPlanDeEstudio != null && selectBloqueAcademico != null && selectCurso != null)
            {
                int BloqueID = Int16.Parse(selectBloqueAcademico);
                int CursoID = Int16.Parse(selectCurso);

                pBloqueXPlanXCurso.CourseID = CursoID;
                pBloqueXPlanXCurso.BlockXPlanID = idBloqueXPlan(PlanID, BloqueID);
                if (existeRelacionBloqueXPlanXCurso(pBloqueXPlanXCurso.BlockXPlanID, pBloqueXPlanXCurso.CourseID))
                {
                    TempData[TempDataMessageKey] = "El Bloque académico de este plan de estudio ya cuenta con el curso seleccionado. Por Favor intente de nuevo.";
                    return RedirectToAction("CrearBloqueXPlanXCurso", new { plan = PlanID });
                }
                if (existeRelacionCursoEnPlan(PlanID, pBloqueXPlanXCurso.CourseID))
                {
                    TempData[TempDataMessageKey] = "El Plan de estudio  ya cuenta con el curso seleccionado. Por Favor intente de nuevo.";
                    return RedirectToAction("CrearBloqueXPlanXCurso", new { plan = PlanID });
                }
                crearRelacionBloqueXPlanXCurso(pBloqueXPlanXCurso);
                TempData[TempDataMessageKeySuccess] = "El curso ha sido asignado al bloque académico del plan de estudio exitosamente";
                return RedirectToAction("CrearBloqueXPlanXCurso", new { plan = PlanID });

            }
            TempData[TempDataMessageKey] = "Datos ingresados son inválidos";
            return RedirectToAction("CrearBloqueXPlanXCurso", new { plan = PlanID });
        }

        public StudyPlan ObtenerUnPlanDeEstudio(int plan)
        {
            return (from PlanesDeEstudio in gvDatabase.StudyPlans
                    where PlanesDeEstudio.ID == plan
                    select PlanesDeEstudio).FirstOrDefault(); ;
        }

        public IQueryable<AcademicBlock> obtenerBloques(int PlanDeEstudio)
        {
            return from BloquesAcademicos in gvDatabase.AcademicBlocks
                   join BloquesXPlan in gvDatabase.AcademicBlocksXStudyPlans on BloquesAcademicos.ID equals BloquesXPlan.BlockID
                   where BloquesXPlan.PlanID == PlanDeEstudio
                   select BloquesAcademicos;
        }

        public int idBloqueXPlan(int pPlanID, int pBloqueID)
        {
            return (from BloquesXPlan in gvDatabase.AcademicBlocksXStudyPlans
                    where BloquesXPlan.PlanID == pPlanID && BloquesXPlan.BlockID == pBloqueID
                    select BloquesXPlan).FirstOrDefault().ID;
        }

        public IQueryable<Course> ObtenerCursos()
        {
            return from Curso in gvDatabase.Courses
                   select Curso;
        }

        public IQueryable<Course> ObtenerCursosXEntidad(int entidad)
        {
            if (entidad == 1)
            {
                return from Curso in gvDatabase.Courses
                       join BloqueXPlanXCursos in gvDatabase.BlocksXPlansXCourses on Curso.ID equals BloqueXPlanXCursos.ID
                       join BloquesXPlan in gvDatabase.AcademicBlocksXStudyPlans on BloqueXPlanXCursos.BlockXPlanID equals BloquesXPlan.ID
                       join PlanDeEstudio in gvDatabase.StudyPlans on BloquesXPlan.ID equals PlanDeEstudio.ID
                       where PlanDeEstudio.EntityTypeID == 1 || PlanDeEstudio.EntityTypeID == 2 ||
                       PlanDeEstudio.EntityTypeID == 3 || PlanDeEstudio.EntityTypeID == 4 || PlanDeEstudio.EntityTypeID == 10
                       select Curso;
            }
            else
            {
                return from Curso in gvDatabase.Courses
                       join BloqueXPlanXCursos in gvDatabase.BlocksXPlansXCourses on Curso.ID equals BloqueXPlanXCursos.ID
                       join BloquesXPlan in gvDatabase.AcademicBlocksXStudyPlans on BloqueXPlanXCursos.BlockXPlanID equals BloquesXPlan.ID
                       join PlanDeEstudio in gvDatabase.StudyPlans on BloquesXPlan.ID equals PlanDeEstudio.ID
                       where PlanDeEstudio.EntityTypeID == entidad
                       select Curso;
            }

        }

        public IQueryable<Course> ObtenerCursosXEntidad(int planDeEstudio, int bloque, int entidadID)
        {
            return from curso in gvDatabase.Courses
                   join BloqueXPlanXCursos in gvDatabase.BlocksXPlansXCourses on curso.ID equals BloqueXPlanXCursos.CourseID
                   join BloquesXPlan in gvDatabase.AcademicBlocksXStudyPlans on BloqueXPlanXCursos.BlockXPlanID equals BloquesXPlan.ID
                   join PlanDeEstudio in gvDatabase.StudyPlans on BloquesXPlan.PlanID equals PlanDeEstudio.ID
                   where BloquesXPlan.PlanID == planDeEstudio && BloquesXPlan.BlockID == bloque && PlanDeEstudio.EntityTypeID == entidadID
                   orderby curso.Name
                   select curso;
        }

        public bool existeRelacionBloqueXPlanXCurso(int pBloqueXPlanID, int pCursoID)
        {
            return (gvDatabase.BlocksXPlansXCourses.SingleOrDefault(relacion => relacion.BlockXPlanID == pBloqueXPlanID && relacion.CourseID == pCursoID) != null);
        }

        public bool existeRelacionCursoEnPlan(int pPlanID, int pCursoID)
        {
            var request = from cursos in gvDatabase.Courses
                          join BloqueXPlanXCursos in gvDatabase.BlocksXPlansXCourses on cursos.ID equals BloqueXPlanXCursos.CourseID
                          join BloqueXPlan in gvDatabase.AcademicBlocksXStudyPlans on BloqueXPlanXCursos.BlockXPlanID equals BloqueXPlan.ID
                          where cursos.ID == pCursoID && BloqueXPlan.PlanID == pPlanID
                          select BloqueXPlanXCursos;
            if (request.Any())
                return true;
            return false;
        }

        public void crearRelacionBloqueXPlanXCurso(BlockXPlanXCourse pBloqueXPlanXCurso)
        {
            if (existeRelacionBloqueXPlanXCurso(pBloqueXPlanXCurso.BlockXPlanID, pBloqueXPlanXCurso.CourseID))
                return;
            else
            {
                gvDatabase.BlocksXPlansXCourses.Add(pBloqueXPlanXCurso);
                gvDatabase.SaveChanges();
            }
        }

        [Route("BloqueXPlanXCurso/Cursos/List/{plan}/{bloque}")]
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

            IQueryable vListaCursos = ObtenerCursosXEntidad(plan, bloque, entidadID);
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