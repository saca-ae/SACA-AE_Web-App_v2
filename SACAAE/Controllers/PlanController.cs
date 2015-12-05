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

namespace SACAAE.Controllers
{
    public class PlanController : Controller
    {
        private const string TempDataMessageKey = "MessageError";
        private const string TempDataMessageKeySuccess = "MessageSuccess";

        private SACAAEContext gvDatabase = new SACAAEContext();
        //
        // GET: /Plan/
        [Authorize]
        public ActionResult CrearPlan()
        {
            ViewBag.Modalidades = ObtenerTodosModalidades();
            ViewBag.Sedes = ObtenerTodosSedes();
            var model = new StudyPlan();
            return View();
        }

        public ActionResult Index()
        {
            ViewBag.Modalidades = ObtenerTodosModalidades();
            ViewBag.Sedes = ObtenerTodosSedes();

            String entidad = Request.Cookies["Entidad"].Value;

            if (entidad.Equals("TEC"))
            {
                var model = ObtenerPlanesDeEstudioXEntidad(1);
                return View(model);
            }
            else if (entidad.Equals("CIE"))
            {
                var model = ObtenerPlanesDeEstudioXEntidad(7);
                return View(model);
            }
            else if (entidad.Equals("TAE"))
            {
                var model = ObtenerPlanesDeEstudioXEntidad(5);
                return View(model);
            }
            else if (entidad.Equals("MAE"))
            {
                var model = ObtenerPlanesDeEstudioXEntidad(6);
                return View(model);
            }
            else if (entidad.Equals("DDE"))
            {
                var model = ObtenerPlanesDeEstudioXEntidad(11);
                return View(model);
            }
            else if (entidad.Equals("Emprendedores"))
            {
                var model = ObtenerPlanesDeEstudioXEntidad(12);
                return View(model);
            }
            else if (entidad.Equals("Actualizacion_Cartago"))
            {
                var model = ObtenerPlanesDeEstudioXEntidad(9);
                return View(model);
            }
            else
            {
                var model = ObtenerPlanesDeEstudioXEntidad(8); //Actualización San Carlos
                return View(model);
            }


        }

        [Authorize]
        [HttpPost]
        public ActionResult Index(string sltPlan)
        {
            int PlanID = Int16.Parse(sltPlan);
            if (sltPlan == null)
            {
                TempData[TempDataMessageKey] = "Seleccione un Plan";
                return RedirectToAction("CrearPlan");
            }
            return RedirectToAction("BloqueXPlan", new { plan = PlanID });
        }

        public ActionResult BloqueXPlan(int id)
        {
            var model = obtenerBloques(id);
            ViewBag.Plan = ObtenerUnPlanDeEstudio(id);
            return View(model);
        }

        [Authorize]
        [HttpPost]
        public ActionResult BloqueXPlan(string sltPlan, string sltBloque)
        {
            int PlanID = Int16.Parse(sltPlan);
            int BloqueID = Int16.Parse(sltBloque);
            return RedirectToAction("CursoXPlanXBloque", new { plan = PlanID, bloque = BloqueID });
        }

        [Authorize]
        public ActionResult CursoXPlanXBloque(int plan, int bloque)
        {
            ViewBag.Cursos = ObtenerCursos(plan, bloque);
            ViewBag.Bloque = obtenerBloqueAcademico(bloque);
            ViewBag.Plan = ObtenerUnPlanDeEstudio(plan);
            var model = new Course();
            return View(model);
        }

        [Authorize]
        public ActionResult ModificarCurso(int plan, int curso, int bloque)
        {
            ViewBag.Bloques = obtenerBloques(plan);
            ViewBag.plan = plan;
            ViewBag.bloque = bloque;
            ViewBag.curso = curso;
            var model = ObtenerCurso(curso);
            return View(model);
        }

        [Authorize]
        [HttpPost]
        public ActionResult ModificarCurso(string sltBloques, int bloque, int plan, int curso, string button)
        {
            int bloqueXPlanID = obtenerIdBloqueXPlan(plan, bloque);
            if (button == "Eliminar")
            {
                eliminarCursoBloquePlan(bloqueXPlanID, curso);
                TempData[TempDataMessageKey] = "El registro ha sido borrado correctamente.";
                return RedirectToAction("CursoXPlanXBloque", new { plan = plan, bloque = bloqueXPlanID });
            }
            int bloqID = Int16.Parse(sltBloques);
            int BloqueID = obtenerIdBloqueXPlan(plan, bloqID);
            modificarCursoBloquePlan(bloqueXPlanID, BloqueID);
            TempData[TempDataMessageKey] = "El registro ha sido editado correctamente.";
            return RedirectToAction("CursoXPlanXBloque", new { plan = plan, bloque = bloqID });
        }

        [Authorize]
        [HttpPost]
        public ActionResult CrearPlan(StudyPlan plan, int Modalidades, List<int> Sedes)
        {
            String entidad = Request.Cookies["Entidad"].Value;
            int entidadID;

            if (entidad.Equals("TEC")) { entidadID = 1; }
            else if (entidad.Equals("CIE")) { entidadID = 7; }
            else if (entidad.Equals("TAE")) { entidadID = 5; }
            else if (entidad.Equals("MAE")) { entidadID = 6; }
            else if (entidad.Equals("DDE")) { entidadID = 11; }
            else if (entidad.Equals("Emprendedores")) { entidadID = 12; }
            else if (entidad.Equals("Actualizacion_Cartago")) { entidadID = 9; }
            else { entidadID = 8; }

            if (plan.Name == null)
            {
                TempData[TempDataMessageKey] = "Ingrese un Nombre";
                return RedirectToAction("CrearPlan");
            }
            if (existe(plan.Name, Modalidades) != null)
            {
                TempData[TempDataMessageKey] = "Ya existe ese plan de estudio";
                return RedirectToAction("CrearPlan");
            }
            if (Sedes == null)
            {
                TempData[TempDataMessageKey] = "Seleccione al menos una sede";
                return RedirectToAction("CrearPlan");
            }
            plan.ModeID = Modalidades;
            plan.EntityTypeID = entidadID;
            agregarPlan(plan);
            int idplan = IdPlanDeEstudioPorIdModalidad(plan.Name, Modalidades);
            StudyPlanXSede planXSede = new StudyPlanXSede();
            planXSede.StudyPlanID = idplan;
            foreach (int idsede in Sedes)
            {
                planXSede.SedeID = idsede;
                agregrarPlanXSede(planXSede);
            }
            TempData[TempDataMessageKeySuccess] = "Plan Creado Exitosamente";
            return RedirectToAction("CrearBloqueXPlan", "BloqueXPlan", new { plan = idplan });
        }

        [Authorize]
        public ActionResult EliminarBloque(int bloque, int plan)
        {
            ViewBag.Plan = ObtenerUnPlanDeEstudio(plan);
            ViewBag.Bloque = bloque;
            var model = obtenerBloqueAcademico(bloque);
            return View(model);
        }

        [Authorize]
        [HttpPost]
        public ActionResult EliminarBloque(int plan, int bloque, string button)
        {
            int bloqueXPlanID = obtenerIdBloqueXPlan(plan, bloque);

            TempData[TempDataMessageKey] = eliminarBloquePlan(bloqueXPlanID);
            return RedirectToAction("BloqueXPlan/" + plan);
        }

        [Authorize]
        public ActionResult EliminarPlanV(int id)
        {
            var model = ObtenerUnPlanDeEstudio(id);
            return View(model);
        }

        [Authorize]
        [HttpPost]
        public ActionResult EliminarPlan(StudyPlan plan, string button)
        {
            TempData[TempDataMessageKey] = EliminarPlan(plan.ID);
            return RedirectToAction("Index");
        }

        public Course ObtenerCurso(int id)
        {
            return gvDatabase.Courses.SingleOrDefault(curso => curso.ID == id);
        }

        public IQueryable<Course> ObtenerCursos(int PlanDeEstudio, int bloque)
        {
            return from curso in gvDatabase.Courses
                   join BloqueXPlanXCursos in gvDatabase.BlocksXPlansXCourses on curso.ID equals BloqueXPlanXCursos.CourseID
                   join BloquesXPlan in gvDatabase.AcademicBlocksXStudyPlans on BloqueXPlanXCursos.BlockXPlanID equals BloquesXPlan.ID
                   where BloquesXPlan.PlanID == PlanDeEstudio && BloquesXPlan.BlockID == bloque
                   orderby curso.Name
                   select curso;
        }

        public int obtenerIdBloqueXPlan(int pPlanID, int pBloqueID)
        {
            return (gvDatabase.AcademicBlocksXStudyPlans.SingleOrDefault(relacion => relacion.PlanID == pPlanID && relacion.BlockID == pBloqueID).ID);
        }

        public string eliminarBloquePlan(int pBloqueXPlanID)
        {

            var vBloques = (from BloquesXPlanXCursos in gvDatabase.BlocksXPlansXCourses
                            where BloquesXPlanXCursos.BlockXPlanID == pBloqueXPlanID
                            select BloquesXPlanXCursos);
            if (vBloques != null)
            {
                foreach (var vBloque in vBloques)
                {
                    var CursosAsignados = from Grupos in gvDatabase.Groups
                                          where Grupos.BlockXPlanXCourseID == vBloque.ID
                                          select Grupos;
                    if (CursosAsignados.Any())
                        return "Debe eliminar los grupos de este curso";
                    gvDatabase.BlocksXPlansXCourses.Remove(vBloque);
                }
                var vBloquePlan = gvDatabase.AcademicBlocksXStudyPlans.SingleOrDefault(bloquePlan => bloquePlan.ID == pBloqueXPlanID);
                gvDatabase.AcademicBlocksXStudyPlans.Remove(vBloquePlan);
                gvDatabase.SaveChanges();
            }
            else
                return "El registro ha sido borrado correctamente.";
            return "El registro no ha sido borrado.";
            //throw new Exception("Se ha producido un error, no se ha encontrado referencia del registro seleccionado. Por Favor comuniquese con un administrador.");
        }

        public AcademicBlock obtenerBloqueAcademico(int id)
        {
            return gvDatabase.AcademicBlocks.SingleOrDefault(bloque => bloque.ID == id);
        }

        public IQueryable<AcademicBlock> obtenerBloques(int PlanDeEstudio)
        {
            return from BloquesAcademicos in gvDatabase.AcademicBlocks
                   join BloquesXPlan in gvDatabase.AcademicBlocksXStudyPlans on BloquesAcademicos.ID equals BloquesXPlan.BlockID
                   where BloquesXPlan.PlanID == PlanDeEstudio
                   select BloquesAcademicos;
        }

        public void modificarCursoBloquePlan(int idBloqueXplan, int idBloque)
        {
            var vBloqueXPlanXCurso = gvDatabase.BlocksXPlansXCourses.SingleOrDefault(bloqueXplanXcurso => bloqueXplanXcurso.BlockXPlanID == idBloqueXplan);
            if (vBloqueXPlanXCurso != null)
            {
                gvDatabase.Entry(vBloqueXPlanXCurso).Property(bloqueXplanXcurso => bloqueXplanXcurso.BlockXPlanID).CurrentValue = idBloque;
                gvDatabase.SaveChanges();
            }
            else
                return;
        }

        public void eliminarCursoBloquePlan(int pBloqueXPlanID, int curso)
        {
            var vBloque = gvDatabase.BlocksXPlansXCourses.SingleOrDefault(bloque => bloque.BlockXPlanID == pBloqueXPlanID && bloque.CourseID == curso);
            if (vBloque != null)
            {
                gvDatabase.BlocksXPlansXCourses.Remove(vBloque);
                gvDatabase.SaveChanges();
            }
            else
                return;
            //throw new Exception("Se ha producido un error, no se ha encontrado referencia del registro seleccionado. Por Favor comuniquese con un administrador.");
        }

        public IQueryable<StudyPlan> ObtenerTodosPlanes()
        {
            return from plan in gvDatabase.StudyPlans
                   orderby plan.Name
                   select plan;
        }

        public IQueryable ObtenerPlanesConModalidad(int IDModalidad, int IDSede)
        {
            return from plan in gvDatabase.StudyPlans
                   join planXsede in gvDatabase.StudyPlansXSedes
                   on plan.ID equals planXsede.StudyPlanID
                   where plan.ModeID == IDModalidad && planXsede.SedeID == IDSede
                   orderby plan.Name
                   select new { plan.ID, plan.Name, plan.Modality };
        }

        public IQueryable<StudyPlan> ObtenerTodosPlanesDeEstudio()
        {
            return from PlanesDeEstudio in gvDatabase.StudyPlans
                   orderby PlanesDeEstudio.ID
                   select PlanesDeEstudio;
        }

        public IQueryable<StudyPlan> ObtenerPlanesDeEstudioXEntidad(int entidadID)
        {
            if (entidadID == 1)
            {
                return from plan in gvDatabase.StudyPlans
                       orderby plan.Name
                       where plan.EntityTypeID == 1 || //TEC
                       plan.EntityTypeID == 2 || plan.EntityTypeID == 3 || //TEC-VIC TEC-REC
                       plan.EntityTypeID == 4 || plan.EntityTypeID == 10 //TEC-MIXTO TEC-Académico
                       select plan;
            }
            else
            {
                return from plan in gvDatabase.StudyPlans
                       orderby plan.Name
                       where plan.EntityTypeID == entidadID
                       select plan;

            }
        }

        public StudyPlan ObtenerUnPlanDeEstudio(int plan)
        {
            return (from PlanesDeEstudio in gvDatabase.StudyPlans
                    where PlanesDeEstudio.ID == plan
                    select PlanesDeEstudio).FirstOrDefault(); ;
        }

        public void agregarPlan(StudyPlan Plan)
        {
            gvDatabase.StudyPlans.Add(Plan);
            Save();
        }

        private void Save()
        {
            gvDatabase.SaveChanges();
        }

        public StudyPlan existe(string nombre, int modalidad)
        {
            return (from PlanesDeEstudio in gvDatabase.StudyPlans
                    where PlanesDeEstudio.Name == nombre && PlanesDeEstudio.ModeID == modalidad
                    select PlanesDeEstudio).FirstOrDefault();
        }
        public int IdPlanDeEstudioPorIdModalidad(String Nombre, int IdModalidad)
        {
            IQueryable<StudyPlan> result = from PlanesDeEstudio in gvDatabase.StudyPlans
                                           orderby PlanesDeEstudio.Name
                                           where PlanesDeEstudio.Name == Nombre && PlanesDeEstudio.ModeID == IdModalidad
                                           select PlanesDeEstudio;
            try
            {
                return result.FirstOrDefault().ID;
            }
            catch (Exception e)
            {
                return -1;
            }
        }

        public int IdPlanDeEstudio(String Nombre, string Modalidad)
        {
            int IdModalidad = (from Modalidade in gvDatabase.Modalities
                               where Modalidade.Name == Modalidad
                               select Modalidade).FirstOrDefault().ID;

            IQueryable<StudyPlan> result = from PlanesDeEstudio in gvDatabase.StudyPlans
                                           orderby PlanesDeEstudio.Name
                                           where PlanesDeEstudio.Name == Nombre && PlanesDeEstudio.ModeID == IdModalidad
                                           select PlanesDeEstudio;
            try
            {
                return result.FirstOrDefault().ID;
            }
            catch (Exception e)
            {
                return -1;
            }
        }

        public string EliminarPlan(int plan)
        {
            var bloques = from bloqueXPlan in gvDatabase.AcademicBlocksXStudyPlans
                          where bloqueXPlan.PlanID == plan
                          select bloqueXPlan;
            if (bloques.Any())
                return "Debe eliminar los bloques asignados al plan";
            else
            {
                var vSedes = from SedesXPlan in gvDatabase.StudyPlansXSedes
                             where SedesXPlan.StudyPlanID == plan
                             select SedesXPlan;
                if (vSedes != null)
                {
                    foreach (var vSede in vSedes)
                    {
                        gvDatabase.StudyPlansXSedes.Remove(vSede);
                    }
                    var vPlan = gvDatabase.StudyPlans.SingleOrDefault(Plan => Plan.ID == plan);
                    gvDatabase.StudyPlans.Remove(vPlan);
                    Save();
                    return "Registro borrado satisfactoriamente";
                }
            }
            return "Hubo problemas al eliminar el registro del plan";

        }
        public int IdPlanDeEstudioXSede(int sede, int plan)
        {
            return (from planXSede in gvDatabase.StudyPlansXSedes
                    where planXSede.SedeID == sede && planXSede.StudyPlanID == plan
                    select planXSede).FirstOrDefault().ID;
        }
        public int IdPlanDeEstudioXSede(String Nombre, String Modalidad, String Sede)
        {
            int IdModalidad = (from Modalidade in gvDatabase.Modalities
                               where Modalidade.Name == Modalidad
                               select Modalidade).FirstOrDefault().ID;
            int IdSede = (from Sedes in gvDatabase.Sedes
                          where Sedes.Name == Sede
                          select Sedes).FirstOrDefault().ID;

            int IdPlanDeEstudio = this.IdPlanDeEstudio(Nombre, Modalidad);

            IQueryable<StudyPlanXSede> result = from PlanesDeEstudioXSede in gvDatabase.StudyPlansXSedes
                                                where PlanesDeEstudioXSede.SedeID == IdSede && PlanesDeEstudioXSede.StudyPlanID == IdPlanDeEstudio
                                                select PlanesDeEstudioXSede;
            try
            {
                return result.FirstOrDefault().ID;
            }
            catch (Exception e)
            {
                return -1;
            }
        }


        public StudyPlanXSede tomarIDPlanXSede(int idSede, int idPlan)
        {
            return gvDatabase.StudyPlansXSedes.SingleOrDefault(plansSede => plansSede.StudyPlanID == idPlan && plansSede.SedeID == idSede);
        }
        public void agregrarPlanXSede(StudyPlanXSede planXSede)
        {
            gvDatabase.StudyPlansXSedes.Add(planXSede);
            gvDatabase.SaveChanges();
        }

        public IQueryable<Modality> tomarModalidades()
        {
            return from modalidades in gvDatabase.Modalities
                   orderby modalidades.Name
                   select modalidades;
        }

        public IQueryable<Modality> ObtenerTodosModalidades()
        {
            return from Modalidades in gvDatabase.Modalities
                   orderby Modalidades.Name
                   select Modalidades;
        }

        public int idModalidad(string nombre)
        {
            return (from Modalidade in gvDatabase.Modalities
                    where Modalidade.Name == nombre
                    select Modalidade).FirstOrDefault().ID;
        }

        public IQueryable<Sede> ObtenerTodosSedes()
        {
            return from Sede in gvDatabase.Sedes
                   orderby Sede.Name
                   select Sede;
        }

        [Route("Plan/PlanesSede/List/{pSede:int}/{pModalidad:int}")]
        public ActionResult ObtenerPlanesEstudio(int pSede, int pModalidad)
        {
            if (HttpContext.Request.IsAjaxRequest())
            {
                var listaPlanes = from sedes in gvDatabase.Sedes
                                  join planesporsede in gvDatabase.StudyPlansXSedes on sedes.ID equals planesporsede.SedeID
                                  join planesestudio in gvDatabase.StudyPlans on planesporsede.StudyPlanID equals planesestudio.ID
                                  join modalidades in gvDatabase.Modalities on planesestudio.ModeID equals modalidades.ID
                                  where (sedes.ID == pSede) && (modalidades.ID == pModalidad)
                                  select new { planesestudio.ID, planesestudio.Name };

                return Json(listaPlanes, JsonRequestBehavior.AllowGet);
            }
            return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        }
    }
}
