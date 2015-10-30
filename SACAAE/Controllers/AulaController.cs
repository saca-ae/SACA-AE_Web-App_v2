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
using Newtonsoft.Json;
using SACAAE.Models.ViewModels;

namespace SACAAE.Controllers
{
    [Authorize]
    public class AulaController : Controller
    {
        private SACAAEContext db = new SACAAEContext();
        private const string TempDataMessageKey = "MessageError";
        private const string TempDataMessageKeySuccess = "MessageSuccess";

        // GET: Aula
        public ActionResult Index()
        {
            return View(db.Classrooms.ToList());
        }

        // GET: Aula/Create
        public ActionResult Create()
        {
            var model = new Classroom();
            ViewBag.Sedes = new SelectList(db.Sedes, "ID", "Name");
            return View(model);
        }

        // POST: Aula/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,SedeID,Code,Capacity,Active")] Classroom aula)
        {
            if (ModelState.IsValid)
            {
                if (aula.Code == null)
                {
                    TempData[TempDataMessageKey] = "Ingrese un Código Válido";
                    ViewBag.SedeID = new SelectList(db.Sedes, "ID", "Name", aula.SedeID);
                    return View(aula);
                }
                if (db.Classrooms.Where(p => p.Code == aula.Code).Count() > 0)
                {
                    TempData[TempDataMessageKey] = "Esta sede ya cuenta con un aula con el código provisto. Por Favor intente de nuevo.";
                    ViewBag.SedeID = new SelectList(db.Sedes, "ID", "Name", aula.SedeID);
                    return View(aula);
                }

                aula.SedeID = Convert.ToInt32(Request.Form["Sedes"].ToString());
                aula.Active = true;
                db.Classrooms.Add(aula);
                db.SaveChanges();
                TempData[TempDataMessageKeySuccess] = "El aula ha sido creada exitosamente";
                return RedirectToAction("Index");
            }

            ViewBag.SedeID = new SelectList(db.Sedes, "ID", "Name", aula.SedeID);
            return View(aula);
        }

        // GET: Aula/Edit/5
        public ActionResult Edit(int? ID)
        {
            if (ID == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Classroom aula = db.Classrooms.Find(ID);
            if (aula == null)
            {
                return HttpNotFound();
            }
            ViewBag.Sedes = new SelectList(db.Sedes, "ID", "Name", aula.SedeID);
            return View(aula);
        }

        // POST: Aula/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,SedeID,Code,Capacity,Active")] Classroom aula)
        {
            if (ModelState.IsValid)
            {
                if (db.Classrooms.Where(p => p.Code == aula.Code && p.SedeID == aula.SedeID).Count() == 0)
                {
                    aula.SedeID = Convert.ToInt32(Request.Form["Sedes"].ToString());
                    db.Entry(aula).State = EntityState.Modified;
                    db.SaveChanges();
                    TempData[TempDataMessageKeySuccess] = "Aula código: "+aula.Code+" editada correctamente";
                    return RedirectToAction("Index");
                }
                else
                {
                    TempData[TempDataMessageKey] = "Esta sede ya cuenta con un aula con el código provisto. Por Favor intente de nuevo." + aula.Code;
                }
            }
            ViewBag.SedeID = new SelectList(db.Sedes, "ID", "Name", aula.SedeID);
            return View(aula);
        }

        // GET: Aula/Delete/5
        public ActionResult Delete(int? ID)
        {
            if (ID == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Classroom aula = db.Classrooms.Find(ID);
            if (aula == null)
            {
                return HttpNotFound();
            }
            return View(aula);
        }

        // POST: Aula/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int ID)
        {
            Classroom aula = db.Classrooms.Find(ID);
            db.Classrooms.Remove(aula);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        // GET: Aula/Schedule/5
        public ActionResult Schedule(int? ID)
        {
            if (ID == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Classroom classroom = db.Classrooms.Find(ID);
            if (classroom == null)
            {
                return HttpNotFound();
            }

            var periodo_actual = int.Parse(Request.Cookies["Periodo"].Value);
            
                var scheduleClassroom = (from aula in db.Classrooms
                                  join grupo_aula in db.GroupClassrooms on aula.ID equals grupo_aula.ClassroomID
                                  join horario in db.Schedules on grupo_aula.ScheduleID equals horario.ID
                                  join sede in db.Sedes on classroom.SedeID equals sede.ID
                                  join grupo in db.Groups on grupo_aula.GroupID equals grupo.ID
                                  join plan_bloque_curso in db.BlocksXPlansXCourses on grupo.BlockXPlanXCourseID equals plan_bloque_curso.ID
                                  join curso in db.Courses on plan_bloque_curso.CourseID equals curso.ID
                                  join periodo in db.Periods on grupo.PeriodID equals periodo.ID
                                  where (aula.ID == ID) && (periodo.ID == periodo_actual) && (horario.StartHour != "700" && horario.StartHour != "900")

                                  select new
                                  {
                                      curso.Name,
                                      grupo.Number,
                                      horario.StartHour,
                                      horario.EndHour,
                                      horario.Day
                                  }).ToList();

                var data = new ScheduleDataList();

                scheduleClassroom.ForEach(p => data.add(
                p.Day,
                (p.Name + "\nGrupo: " + p.Number),
                DateTime.Parse(p.StartHour),
                DateTime.Parse(p.EndHour),
                "Curso")
            );

                var viewModel = new ScheduleProfessorViewModel()
                {
                    Name = classroom.Code,
                    ScheduleData = data.getData()
                };

            return View(viewModel);
        }

        //[Route("ObtenerHorarioAula/List/{aula}/{periodo:int}")]
        //public ActionResult ObtenerHorarioAula(string aula, int periodo)
        //{
        //    IQueryable listaHorarioAula = from Dias in entidades.Dias
        //                                  join detallesGrupo in entidades.Detalle_Grupo on Dias.Horario equals detallesGrupo.Horario
        //                                  join grupos in entidades.Grupoes on detallesGrupo.Grupo equals grupos.ID
        //                                  join bloqueXPlanXCurso in entidades.BloqueXPlanXCursoes on grupos.BloqueXPlanXCursoID equals bloqueXPlanXCurso.ID
        //                                  join cursos in entidades.Cursos on bloqueXPlanXCurso.CursoID equals cursos.ID
        //                                  where detallesGrupo.Aula == aula && grupos.Periodo == periodo
        //                                  select new { Dias.Dia1, Dias.Hora_Inicio, Dias.Hora_Fin, cursos.Nombre, grupos.Numero, grupos.ID, detallesGrupo.Aula, bloqueXPlanXCurso.BloqueAcademicoXPlanDeEstudio.BloqueAcademico.Descripcion };
        //    var json = JsonConvert.SerializeObject(listaHorarioAula);

        //    return Content(json);
        //}

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        #region helpers
        private int getEntityID(string entityName)
        {
            EntityType entity;
            switch (entityName)
            {
                case "TEC":
                    entity = db.EntityTypes.SingleOrDefault(p => p.Name == "TEC");
                    break;
                case "CIADEG":
                    entity = db.EntityTypes.SingleOrDefault(p => p.Name == "CIADEG");
                    break;
                case "TAE":
                    entity = db.EntityTypes.SingleOrDefault(p => p.Name == "FUNDA-TAE");
                    break;
                case "MAE":
                    entity = db.EntityTypes.SingleOrDefault(p => p.Name == "FUNDA-MAE");
                    break;
                case "MDE":
                    entity = db.EntityTypes.SingleOrDefault(p => p.Name == "FUNDA-MDE");
                    break;
                case "MGP":
                    entity = db.EntityTypes.SingleOrDefault(p => p.Name == "FUNDA-MGP");
                    break;
                case "DDE":
                    entity = db.EntityTypes.SingleOrDefault(p => p.Name == "FUNDA-Doctorado");
                    break;
                case "Emprendedores":
                    entity = db.EntityTypes.SingleOrDefault(p => p.Name == "FUNDA-Emprendedores");
                    break;
                case "CIE":
                    entity = db.EntityTypes.SingleOrDefault(p => p.Name == "FUNDA-CIE");
                    break;
                case "Actualizacion_Cartago":
                    entity = db.EntityTypes.SingleOrDefault(p => p.Name == "FUNDA-Actualizacion Cartago");
                    break;
                case "Actualizacion_San_Carlos":
                    entity = db.EntityTypes.SingleOrDefault(p => p.Name == "FUNDA-Actualizacion San Carlos");
                    break;
                default:
                    entity = db.EntityTypes.SingleOrDefault(p => p.Name == entityName);
                    break;
            }

            return (entity != null) ? entity.ID : 0;
        }
        
        // Revisar desde acá
        public IQueryable<Classroom> ListarAulas()
        {
            
            return from Aulas in db.Classrooms
                   select Aulas;
        }

        public IQueryable<Classroom> ListarAulasXEntidad(int entidadID)
        {
            if (entidadID == 1) {
                return from aula in db.Classrooms
                       join Sedes in db.Sedes on aula.SedeID equals Sedes.ID
                       join planXSede in db.StudyPlansXSedes on Sedes.ID equals planXSede.SedeID
                       join planDeEstudio in db.StudyPlans on planXSede.StudyPlanID equals planDeEstudio.ID
                       where planDeEstudio.EntityType.ID == 1 || planDeEstudio.EntityType.ID == 2 ||
                       planDeEstudio.EntityType.ID == 3 || planDeEstudio.EntityType.ID == 4 || planDeEstudio.EntityType.ID == 10
                       select aula;
            }
            else {
                return from aula in db.Classrooms
                       join Sedes in db.Sedes on aula.SedeID equals Sedes.ID
                       join planXSede in db.StudyPlansXSedes on Sedes.ID equals planXSede.SedeID
                       join planDeEstudio in db.StudyPlans on planXSede.StudyPlanID equals planDeEstudio.ID
                       where planDeEstudio.EntityType.ID == entidadID
                       select aula;
            }
        }


        public IQueryable ListarAulasXSede(int pSedeID)
        {
            return from Aulas in db.Classrooms
                   where Aulas.SedeID == pSedeID
                   select new { Aulas.ID, Aulas.Code, Aulas.Capacity, Aulas.Active };
        }

        public IQueryable ListarAulasXSedeCompleta(int pSedeID)
        {
            return from Aulas in db.Classrooms
                   where Aulas.SedeID == pSedeID
                   select Aulas;
        }

        
        //public IQueryable obtenerInfoAula(string aula, int periodo)
        //{
        //    return from Dias in db.Dias
        //           join detallesGrupo in db.Detalle_Grupo on Dias.Horario equals detallesGrupo.Horario
        //           join grupos in db.Grupoes on detallesGrupo.Grupo equals grupos.ID
        //           join bloqueXPlanXCurso in db.BloqueXPlanXCursoes on grupos.BloqueXPlanXCursoID equals bloqueXPlanXCurso.ID
        //           join cursos in db.Cursos on bloqueXPlanXCurso.CursoID equals cursos.ID
        //           where detallesGrupo.Aula == aula && grupos.Periodo == periodo
        //           select new { Dias.Dia1, Dias.Hora_Inicio, Dias.Hora_Fin, cursos.Nombre, grupos.Numero, grupos.ID, detallesGrupo.Aula, bloqueXPlanXCurso.BloqueAcademicoXPlanDeEstudio.BloqueAcademico.Descripcion };
        //}

        public int idAula(string pCodigoAula)
        {
            return (from Aulas in db.Classrooms
                    where Aulas.Code == pCodigoAula
                    select Aulas).FirstOrDefault().ID;
        }

        public Classroom ObtenerAula(int ID)
        {
            return db.Classrooms.SingleOrDefault(aula => aula.ID == ID);
        }

        public bool existeAula(int pSede, string pCodigoAula)
        {
            return (db.Classrooms.SingleOrDefault(c => c.Code == pCodigoAula && c.SedeID == pSede) != null);
        }


        public void agregarAula(Classroom pAula)
        {
            if (existeAula(pAula.SedeID, pAula.Code))
                return;
            else {
                db.Classrooms.Add(pAula);
                Save();
            }
        }

        public void eliminarAula(Classroom pAula)
        {
            var vAula = db.Classrooms.SingleOrDefault(aula => aula.ID == pAula.ID);
            if (vAula != null){
                    db.Classrooms.Remove(vAula);
                    Save();
            }
            else
                return;
                //throw new Exception("Se ha producido un error, no se ha encontrado referencia del registro seleccionado. Por Favor comuniquese con un administrador.");
        }

        public void ModificarAula(Classroom pAula)
        {
            var vAula = db.Classrooms.SingleOrDefault(aula => aula.ID == pAula.ID);
            if (vAula != null)
            {
                db.Entry(vAula).Property(aula => aula.Code).CurrentValue = pAula.Code;
                db.Entry(vAula).Property(aula => aula.Capacity).CurrentValue = pAula.Capacity;
                db.Entry(vAula).Property(aula => aula.SedeID).CurrentValue = pAula.SedeID;
                db.Entry(vAula).Property(aula => aula.Active).CurrentValue = pAula.Active;
                Save();
            }
            else
                return;
        }

        private void Save()
        {
            db.SaveChanges();
        }

        private class ScheduleDataList
        {
            private List<List<ScheduleData>> data;

            public ScheduleDataList()
            {
                this.data = new List<List<ScheduleData>>();
                for(int i=0; i<6; i++)
                {
                    this.data.Add(new List<ScheduleData>());
                }
            }

            public void add(string day, string name, DateTime startHour, DateTime endHour, string type)
            {
                var index = 0;
                switch (day)
                {
                    case "Lunes":   index = 0; break;
                    case "Martes":  index = 1; break;
                    case "Miercoles": index = 2; break;
                    case "Jueves":  index = 3; break;
                    case "Viernes": index = 4; break;
                    case "Sabado":  index = 5; break;
                }

                this.data[index].Add(new ScheduleData()
                {
                    Name = name,
                    Type = type,
                    Difference = getDifference(startHour, endHour),
                    StartBlock = getHourBlock(startHour)
                });
            }

            public List<List<ScheduleData>> getData()
            {
                return this.data;
            }

            private int getDifference(DateTime startHour, DateTime endHour)
            {
                return (int)Math.Ceiling(endHour.Subtract(startHour).TotalHours);
            }

            private int getHourBlock(DateTime time)
            {
                var hh = 7;
                var mm = 00;
                var tt = "am";

                for (int i = 0; i < 16; i++)
                {
                    var start = DateTime.Parse(hh + ":" + mm + tt);
                    var end = DateTime.Parse((hh + 1) + ":" + mm + tt);

                    if (betweenDates(start, end, time)) return i;

                    hh++;
                    if (hh == 12) tt = "pm";
                }
                return 0;
            }

            private bool betweenDates(DateTime beginDate, DateTime endDate, DateTime date)
            {
                return (beginDate <= date && date < endDate);
            }
        }
        #endregion

        #region Ajax
            /*Obtener horario segun id del aula*/
            [Route("Aula/Schedules/{idAula:int}")]
        public ActionResult getScheduleAula(int idAula)
        {
            var periodo_actual = int.Parse(Request.Cookies["Periodo"].Value);
            if (HttpContext.Request.IsAjaxRequest())
            {
                var listaPlanes = from aula in db.Classrooms
                                  join grupo_aula in db.GroupClassrooms on aula.ID equals grupo_aula.ClassroomID
                                  join horario in db.Schedules on grupo_aula.ScheduleID equals horario.ID
                                  join sede in db.Sedes on aula.SedeID equals sede.ID
                                  join grupo in db.Groups on grupo_aula.GroupID equals grupo.ID
                                  join plan_bloque_curso in db.BlocksXPlansXCourses on grupo.BlockXPlanXCourseID equals plan_bloque_curso.ID
                                  join curso in db.Courses on plan_bloque_curso.CourseID equals curso.ID
                                  join periodo in db.Periods on grupo.PeriodID equals periodo.ID
                                  where (aula.ID == idAula) && (periodo.ID == periodo_actual) && (horario.StartHour != "700" && horario.StartHour != "900")
                                  
                                  select new { curso.Name, grupo.Number, StartHour = 
                                                                         horario.StartHour=="07:30 am"?1:
                                                                         horario.StartHour == "08:30 am" ? 2:
                                                                         horario.StartHour == "09:30 am" ? 3 :
                                                                         horario.StartHour == "10:30 am" ? 4 :
                                                                         horario.StartHour == "11:30 am" ? 5 :
                                                                         horario.StartHour == "12:30 pm" ? 6 :
                                                                         horario.StartHour == "01:00 pm" ? 7 :
                                                                         horario.StartHour == "02:00 pm" ? 8 :
                                                                         horario.StartHour == "03:00 pm" ? 9 :
                                                                         horario.StartHour == "04:00 pm" ? 10 :
                                                                         horario.StartHour == "05:00 pm" ? 11 :
                                                                         horario.StartHour == "06:00 pm" ? 12 :
                                                                         horario.StartHour == "07:00 pm" ? 13 :
                                                                         horario.StartHour == "08:00 pm" ? 14 :
                                                                         horario.StartHour == "09:00 pm" ? 15 :
                                                                         0,
                                                                         
                                                                         horario.EndHour,
                                                                         
                                                                         Day=horario.Day== "Lunes"?1:
                                                                                                                horario.Day=="Martes"?2:
                                                                                                                horario.Day=="Miércoles"?3:
                                                                                                                horario.Day == "Jueves" ? 4 :
                                                                                                                horario.Day=="Viernes"?5:
                                                                                                                horario.Day=="Sábado"?6:
                                                                                                                0};
                //listaPlanes.Where(p => p.Day == "lunes").OrderBy().ToList();
                listaPlanes = listaPlanes.OrderBy(c => c.StartHour).ThenBy(c => c.StartHour).ThenBy(c=>c.Day);
                /*Es necesario remover elementos de la lista cuando los horarios no son correctos*/
               
                var json = JsonConvert.SerializeObject(listaPlanes);
                return Content(json);
            }
            return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

        }

        #endregion
    }
}
