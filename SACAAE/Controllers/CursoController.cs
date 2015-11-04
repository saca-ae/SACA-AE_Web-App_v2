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
using SACAAE.Models.StoredProcedures;
using SACAAE.Helpers;

namespace SACAAE.Controllers
{
    public class CursoController : Controller
    {
        private SACAAEContext db = new SACAAEContext();
        private ScheduleHelper dbHelper = new ScheduleHelper();
        private const string TempDataMessageKeySuccess = "MessageSuccess";
        private const string TempDataMessageKeyError = "MessageError";
        // GET: Curso
        public ActionResult Index()
        {
            var cursos = db.Courses.ToList();
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
            var vPeriod = Request.Cookies["Periodo"].Value;
            var vIDPeriod = db.Periods.Find(int.Parse(vPeriod)).ID;
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Course curso = db.Courses.Find(id);
            if (curso == null)
            {
                return HttpNotFound();
            }
            ViewBag.Sedes = new SelectList(db.Sedes, "ID", "Name");
            return View(curso);
        }

        // GET: Curso/Create
        public ActionResult Create()
        {
            var model = new Course();
            return View(model);
        }

        // POST: Curso/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,Name,Code,TheoreticalHours,Block,External,PracticeHours,Credits")] Course curso, int HorasPracticas, int HorasTeoricas, int Bloque)
        {
            if (ModelState.IsValid)
            {
                curso.PracticeHours = HorasPracticas;
                curso.TheoreticalHours = HorasTeoricas;
                curso.Block = Bloque;
                db.Courses.Add(curso);
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
            Course curso = db.Courses.Find(id);
            if (curso == null)
            {
                return HttpNotFound();
            }
            return View(curso);
        }

        // POST: Curso/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,Name,Code,TheoreticalHours,Block,External,PracticeHours,Credits")] Course curso, String Name, String  Code)
        {
            
                if (curso.Name != Name)
                {
                    if (existeCursoPorNombre(curso.Name))
                    {
                        TempData[TempDataMessageKeyError] = "Es posible que exista un curso con el mismo nombre. Por Favor intente de nuevo.";
                        return RedirectToAction("Edit");
                    }
                }
                if (curso.Code != Code)
                {
                    if (existeCurso(curso.Code))
                    {
                        TempData[TempDataMessageKeyError] = "Es posible que exista un curso con el mismo codigo. Por Favor intente de nuevo.";
                        return RedirectToAction("Edit");
                    }
                }
                if (ModelState.IsValid)
                {
                    db.Entry(curso).State = EntityState.Modified;
                    db.SaveChanges();
                    TempData[TempDataMessageKeyError] = "El registro ha sido editado correctamente.";
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
            Course curso = db.Courses.Find(id);
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
            if (existeCurso(id))
            {
                var existeEnPlan = from vBlocksXPlansXCourses in db.BlocksXPlansXCourses
                                   where vBlocksXPlansXCourses.CourseID == id
                                   select vBlocksXPlansXCourses;
                if (!existeEnPlan.Any())
                {
                    Course curso = db.Courses.Find(id);
                    if (curso != null)
                    {
                        db.Courses.Remove(curso);
                        db.SaveChanges();
                        TempData[TempDataMessageKeyError] = "Curso removido satisfactoriamente";

                    }
                    else
                    {
                        TempData[TempDataMessageKeyError] = "El curso no existe";
                    }
                }
                else
                {
                    TempData[TempDataMessageKeyError] = "Debe remover el curso de los planes a los que esta asignado";
                }
            }
            else
            {
                TempData[TempDataMessageKeyError] = "El curso no existe";
            }
            return RedirectToAction("Index");
            
        }

        /// <summary>
        ///  Show the view for assigning Profesor to a Course
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
            ViewBag.Profesores = new SelectList(db.Professors, "ID", "Name");
            Course vCurso = db.Courses.Find(id);
            return View(vCurso);
        }

        /// <summary>
        ///  Saves the assign Profesor to a Course if it doesn't have problems with the profesor scheduler
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
                var vPeriod = Request.Cookies["Periodo"].Value;
                var vPeriodID = db.Periods.Find(int.Parse(vPeriod)).ID;

                //Verify if profesor have other assign in the same day and start hour, if don't have conflict with other group in the same hour and day return true, else
                //if found problem with other group in the same day and start hour return false and the assign is cancelled and the user receive information
                string validate = dbHelper.validations(Profesores, Grupos_Disponibles, vPeriodID);

                //If doesn't exist problems in the profesor schedule
                if (validate.Equals("true"))
                {
                    var grupo = db.Groups.Find(Grupos_Disponibles);
                    grupo.ProfessorID = Profesores;
                    if (HourCharge == 1)
                    {
                        
                        grupo.HourAllocatedTypeID = 1;
                    }
                   db.SaveChanges();

                    TempData[TempDataMessageKeySuccess] = "Profesor asignado correctamente";
                    return RedirectToAction("Details", new { id = ID });
                }
                // Exist problems in profesor schedule, so the assign is cancelled and the user recive the information of the problem
                else if (validate.Equals("falseIsGroupSchock"))
                {
                    TempData[TempDataMessageKeyError] = "Existe choque de horario con grupos, no se asigno al profesor al curso";
                    return RedirectToAction("AsignarProfesoraCurso");
                }
                else if (validate.Equals("falseIsProjectSchock"))
                {
                    TempData[TempDataMessageKeyError] = "Existe choque de horario con proyectos, no se asigno al profesor al curso";
                    return RedirectToAction("AsignarProfesoraCurso");
                }

                else if (validate.Equals("falseIsCommissionSchock"))
                {
                    TempData[TempDataMessageKeyError] = "Existe choque de horario con comisiones, no se asigno al profesor al curso";
                    return RedirectToAction("AsignarProfesoraCurso");
                }


               
            }
            return View();
        }

       
        // GET: Curso/DetalleAsignacion/5
        public ActionResult DetalleAsignacion(int? id)
        {
            if (Request.UrlReferrer != null)
            {
                ViewBag.returnUrl = Request.UrlReferrer.ToString();
            }
            else
            {
                ViewBag.returnUrl = null;
            }
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Group grupo = db.Groups.Find(id);
            if (grupo == null)
            {
                return HttpNotFound();
            }
            
            return View(grupo);
        }

        // GET: Curso/EditarAsignacion/5
        public ActionResult EditarAsignacion(int? id)
        {
            if (Request.UrlReferrer != null)
            {
                ViewBag.returnUrl = Request.UrlReferrer.ToString();
            }
            else
            {
                ViewBag.returnUrl = null;
            }
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Group grupo = db.Groups.Find(id);
            if (grupo == null)
            {
                return HttpNotFound();
            }
           

            /* Se obtiene la lista de profesores */
            ViewBag.Profesores = new SelectList(db.Professors, "ID", "Name");
            return View(grupo);
        }


        // POST Curso/EditarAsignacion/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditarAsignacion(String ID, int Profesores, String editHourCharge)
        {
            var vPeriod = Request.Cookies["Periodo"].Value;
            var vPeriodID = db.Periods.Find(int.Parse(vPeriod)).ID;

            int idGrupo = Convert.ToInt32(ID);
            int vHourChange = Convert.ToInt32(editHourCharge);
            if (ModelState.IsValid)
            {
                //Verify if profesor have other assign in the same day and start hour, if don't have conflict with other group in the same hour and day return true, else
                //if found problem with other group in the same day and start hour return false and the assign is cancelled and the user recive information
                string validate = dbHelper.validationsEditGroup(idGrupo, Profesores,vPeriodID);
                if (validate.Equals("true"))
                {
                    Group grupo = db.Groups.Find(idGrupo);
                    grupo.ProfessorID = Profesores;
                    if (vHourChange == 1)
                    {
                        grupo.HourAllocatedTypeID = 1;
                        
                    }
                    else
                    {
                        grupo.HourAllocatedTypeID = null;
                    }
                    db.SaveChanges();

                    TempData[TempDataMessageKeySuccess] = "Profesor asignado correctamente";
                    return RedirectToAction("Details", new { id = grupo.BlockXPlanXCourse.CourseID });
                }
                // Exist problems in profesor schedule, so the assign is cancelled and the user recive the information of the problem
                else if (validate.Equals("falseIsGroupSchock"))
                {
                    /*The user recive the information about the problem*/
                    TempData[TempDataMessageKeyError] = "Existe choque de horario con grupos, no se asigno al profesor al curso"; ;

                    /* Se obtiene la lista de profesores */
                    Group grupo = db.Groups.Find(idGrupo);
                    ViewBag.Profesores = new SelectList(db.Professors, "ID", "Name");
                    return View(grupo);
                }
                else if (validate.Equals("falseIsProjectSchock"))
                {
                    /*The user recive the information about the problem*/
                    TempData[TempDataMessageKeyError] = "Existe choque de horario con proyectos, no se asigno al profesor al curso";

                    /* Se obtiene la lista de profesores */
                    Group grupo = db.Groups.Find(idGrupo);
                    ViewBag.Profesores = new SelectList(db.Professors, "ID", "Name");
                    return View(grupo);
                }

                else if (validate.Equals("falseIsCommissionSchock"))
                {
                    /*The user recive the information about the problem*/
                    TempData[TempDataMessageKeyError] = "Existe choque de horario con comisiones, no se asigno al profesor al curso";

                    /* Se obtiene la lista de profesores */
                    Group grupo = db.Groups.Find(idGrupo);
                    ViewBag.Profesores = new SelectList(db.Professors, "ID", "Name");
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

        #region Helpers
            public bool existeCurso(string Codigo)
            {
                return (db.Courses.SingleOrDefault(c => c.Code == Codigo) != null);

            }

            public bool existeCursoPorNombre(String Nombre)
            {
                return (db.Courses.SingleOrDefault(c => c.Name == Nombre) != null);
            }

            public bool existeCurso(int Curso)
            {
                return (db.Courses.SingleOrDefault(c => c.ID == Curso) != null);
            }
        #endregion

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
                var vInformationGroup = (from curso in db.Courses
                                   join bloque_planes_curso in db.BlocksXPlansXCourses on curso.ID equals bloque_planes_curso.CourseID
                                   join bloque_planes in db.AcademicBlocksXStudyPlans on bloque_planes_curso.BlockXPlanID equals bloque_planes.ID
                                   join bloque_academico in db.AcademicBlocks on bloque_planes.BlockID equals bloque_academico.ID
                                   join grupo in db.Groups on bloque_planes_curso.ID equals grupo.BlockXPlanXCourseID
                                  
                                   join profesor in db.Professors on grupo.ProfessorID equals profesor.ID
                                   join plan_estudio in db.StudyPlans on bloque_planes.PlanID equals plan_estudio.ID
                                   join modalidad in db.Modalities on plan_estudio.ModeID equals modalidad.ID
                                   join sede in db.Sedes on bloque_planes_curso.SedeID equals sede.ID
                                   join grupo_aula in db.GroupClassrooms on grupo.ID equals grupo_aula.GroupID
                                   join aula in db.Classrooms on grupo_aula.ClassroomID equals aula.ID
                                   join horario in db.Schedules on grupo_aula.ScheduleID equals horario.ID
                                   where grupo.ID == pIDGrupo 

                                   select new
                                   {curso_id = curso.ID,curso_name = curso.Name,curso.TheoreticalHours,grupo.Number,profesor_id = profesor.ID,profesor_name = profesor.Name,horario.StartHour,horario.EndHour,horario.Day,
                                       aula.Code,aula.Capacity,sede_id = sede.ID,sede_name = sede.Name,plan_id = plan_estudio.ID,plan_name = plan_estudio.Name,bloque_id = bloque_academico.ID,
                                       descripcion_bloque = bloque_academico.Description, modalidad_id=modalidad.ID,modalidad_name = modalidad.Name, asignacion_id=grupo.HourAllocatedTypeID
                                   });

                var lista = vInformationGroup.ToList();
                /*If dont exist classroom related with a course*/
                if (lista.Count == 0)
                {
                   var vInformationGroup2 = (from curso in db.Courses
                                         join bloque_planes_curso in db.BlocksXPlansXCourses on curso.ID equals bloque_planes_curso.CourseID
                                         join bloque_planes in db.AcademicBlocksXStudyPlans on bloque_planes_curso.BlockXPlanID equals bloque_planes.ID
                                         join bloque_academico in db.AcademicBlocks on bloque_planes.BlockID equals bloque_academico.ID
                                         join grupo in db.Groups on bloque_planes_curso.ID equals grupo.BlockXPlanXCourseID
                                         join profesor in db.Professors on grupo.ProfessorID equals profesor.ID
                                         join plan_estudio in db.StudyPlans on bloque_planes.PlanID equals plan_estudio.ID
                                         join modalidad in db.Modalities on plan_estudio.ModeID equals modalidad.ID
                                         join sede in db.Sedes on bloque_planes_curso.SedeID equals sede.ID
                                         select new {curso_id = curso.ID,curso_name = curso.Name,curso.TheoreticalHours,grupo.Number, profesor_name = profesor.Name,
                                             StartHour = "No asignado",EndHour = "No asignado",Day="No asignado",Code="No asignado",Capacity="?",
                                             sede_id = sede.ID,plan_id = plan_estudio.ID, plan_name = plan_estudio.Name, bloque_id = bloque_academico.ID, descripcion_bloque = bloque_academico.Description,
                                             modalidad_id = modalidad.ID, modalidad_name = modalidad.Name, asignacion_id = grupo.HourAllocatedTypeID
                                         });
                   var json2 = JsonConvert.SerializeObject(vInformationGroup2);
                   return Content(json2);
                }

                var json = JsonConvert.SerializeObject(vInformationGroup);
                return Content(json);
            }
            return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        }

        
        #endregion

    }
}
