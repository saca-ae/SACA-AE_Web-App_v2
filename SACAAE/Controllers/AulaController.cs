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

namespace SACAAE.Controllers
{
    public class AulaController : Controller
    {
        private SACAAEContext db = new SACAAEContext();
        private const string TempDataMessageKey = "MessageError";
        private const string TempDataMessageKeySuccess = "MessageSuccess";

        // GET: Aula
        public ActionResult Index()
        {
            String entity = Request.Cookies["Entidad"].Value;
            var entityID = getEntityID(entity);
            IQueryable<Aula> result;
            if (entity == "TEC")
            {
                result = from aula in db.Aulas
                         join sedes in db.Sedes on aula.SedeID equals sedes.ID
                         join planXSede in db.PlanesDeEstudioXSedes on sedes.ID equals planXSede.SedeID
                         join planDeEstudio in db.PlanesDeEstudio on planXSede.PlanDeEstudio.ID equals planDeEstudio.ID
                         where planDeEstudio.TipoEntidad.ID == 1 || planDeEstudio.TipoEntidad.ID == 2 ||
                         planDeEstudio.TipoEntidad.ID == 3 || planDeEstudio.TipoEntidad.ID == 4 || planDeEstudio.TipoEntidad.ID == 10
                         select aula;
            }
            else
            {
                result = from aula in db.Aulas
                         join Sedes in db.Sedes on aula.SedeID equals Sedes.ID
                         join planXSede in db.PlanesDeEstudioXSedes on Sedes.ID equals planXSede.SedeID
                         join planDeEstudio in db.PlanesDeEstudio on planXSede.PlanDeEstudio.ID equals planDeEstudio.ID
                         where planDeEstudio.TipoEntidad.ID == entityID
                         select aula;
            }

            return View(result.Distinct().ToList());
        }

        // GET: Aula/Create
        public ActionResult Create()
        {
            var model = new Aula();
            ViewBag.Sedes = new SelectList(db.Sedes, "ID", "Name");
            return View(model);
        }

        // POST: Aula/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,SedeID,Code,Capacity,Active")] Aula aula)
        {
            if (ModelState.IsValid)
            {
                if (aula.Code == null)
                {
                    TempData[TempDataMessageKey] = "Ingrese un Código Válido";
                    ViewBag.SedeID = new SelectList(db.Sedes, "ID", "Name", aula.SedeID);
                    return View(aula);
                }
                if (db.Aulas.Where(p => p.Code == aula.Code).Count() > 0)
                {
                    TempData[TempDataMessageKey] = "Esta sede ya cuenta con un aula con el código provisto. Por Favor intente de nuevo.";
                    ViewBag.SedeID = new SelectList(db.Sedes, "ID", "Name", aula.SedeID);
                    return View(aula);
                }

                aula.Active = true;
                db.Aulas.Add(aula);
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
            Aula aula = db.Aulas.Find(ID);
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
        public ActionResult Edit([Bind(Include = "ID,SedeID,Code,Capacity,Active")] Aula aula)
        {
            if (ModelState.IsValid)
            {
                if (db.Aulas.Where(p => p.Code == aula.Code && p.SedeID == aula.SedeID).Count() == 0)
                {
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
            Aula aula = db.Aulas.Find(ID);
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
            Aula aula = db.Aulas.Find(ID);
            db.Aulas.Remove(aula);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        // GET: Aula/Schedule/5
        public ActionResult Schedule(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Aula aula = db.Aulas.Find(id);
            if (aula == null)
            {
                return HttpNotFound();
            }
            return View(aula);
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
            TipoEntidad entity;
            switch (entityName)
            {
                case "TEC":
                    entity = db.TipoEntidades.SingleOrDefault(p => p.Name == "TEC");
                    break;
                case "CIADEG":
                    entity = db.TipoEntidades.SingleOrDefault(p => p.Name == "CIADEG");
                    break;
                case "TAE":
                    entity = db.TipoEntidades.SingleOrDefault(p => p.Name == "FUNDA-TAE");
                    break;
                case "MAE":
                    entity = db.TipoEntidades.SingleOrDefault(p => p.Name == "FUNDA-MAE");
                    break;
                case "MDE":
                    entity = db.TipoEntidades.SingleOrDefault(p => p.Name == "FUNDA-MDE");
                    break;
                case "MGP":
                    entity = db.TipoEntidades.SingleOrDefault(p => p.Name == "FUNDA-MGP");
                    break;
                case "DDE":
                    entity = db.TipoEntidades.SingleOrDefault(p => p.Name == "FUNDA-Doctorado");
                    break;
                case "Emprendedores":
                    entity = db.TipoEntidades.SingleOrDefault(p => p.Name == "FUNDA-Emprendedores");
                    break;
                case "CIE":
                    entity = db.TipoEntidades.SingleOrDefault(p => p.Name == "FUNDA-CIE");
                    break;
                case "Actualizacion_Cartago":
                    entity = db.TipoEntidades.SingleOrDefault(p => p.Name == "FUNDA-Actualizacion Cartago");
                    break;
                case "Actualizacion_San_Carlos":
                    entity = db.TipoEntidades.SingleOrDefault(p => p.Name == "FUNDA-Actualizacion San Carlos");
                    break;
                default:
                    entity = db.TipoEntidades.SingleOrDefault(p => p.Name == entityName);
                    break;
            }

            return (entity != null) ? entity.ID : 0;
        }
        
        // Revisar desde aca
        public IQueryable<Aula> ListarAulas()
        {
            
            return from Aulas in db.Aulas
                   select Aulas;
        }

        public IQueryable<Aula> ListarAulasXEntidad(int entidadID)
        {
            if (entidadID == 1) {
                return from aula in db.Aulas
                       join Sedes in db.Sedes on aula.SedeID equals Sedes.ID
                       join planXSede in db.PlanesDeEstudioXSedes on Sedes.ID equals planXSede.SedeID
                       join planDeEstudio in db.PlanesDeEstudio on planXSede.StudyPlanID equals planDeEstudio.ID
                       where planDeEstudio.TipoEntidad.ID == 1 || planDeEstudio.TipoEntidad.ID == 2 ||
                       planDeEstudio.TipoEntidad.ID == 3 || planDeEstudio.TipoEntidad.ID == 4 || planDeEstudio.TipoEntidad.ID == 10
                       select aula;
            }
            else {
                return from aula in db.Aulas
                       join Sedes in db.Sedes on aula.SedeID equals Sedes.ID
                       join planXSede in db.PlanesDeEstudioXSedes on Sedes.ID equals planXSede.SedeID
                       join planDeEstudio in db.PlanesDeEstudio on planXSede.StudyPlanID equals planDeEstudio.ID
                       where planDeEstudio.TipoEntidad.ID == entidadID
                       select aula;
            }
        }


        public IQueryable ListarAulasXSede(int pSedeID)
        {
            return from Aulas in db.Aulas
                   where Aulas.SedeID == pSedeID
                   select new { Aulas.ID, Aulas.Code, Aulas.Capacity, Aulas.Active };
        }

        public IQueryable ListarAulasXSedeCompleta(int pSedeID)
        {
            return from Aulas in db.Aulas
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
            return (from Aulas in db.Aulas
                    where Aulas.Code == pCodigoAula
                    select Aulas).FirstOrDefault().ID;
        }

        public Aula ObtenerAula(int ID)
        {
            return db.Aulas.SingleOrDefault(aula => aula.ID == ID);
        }

        public bool existeAula(int pSede, string pCodigoAula)
        {
            return (db.Aulas.SingleOrDefault(c => c.Code == pCodigoAula && c.SedeID == pSede) != null);
        }


        public void agregarAula(Aula pAula)
        {
            if (existeAula(pAula.SedeID, pAula.Code))
                return;
            else {
                db.Aulas.Add(pAula);
                Save();
            }
        }

        public void eliminarAula(Aula pAula)
        {
            var vAula = db.Aulas.SingleOrDefault(aula => aula.ID == pAula.ID);
            if (vAula != null){
                    db.Aulas.Remove(vAula);
                    Save();
            }
            else
                return;
                //throw new Exception("Se ha producido un error, no se ha encontrado referencia del registro seleccionado. Por Favor comuniquese con un administrador.");
        }

        public void ModificarAula(Aula pAula)
        {
            var vAula = db.Aulas.SingleOrDefault(aula => aula.ID == pAula.ID);
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
        #endregion

        #region Ajax
            /*Obtener horario segun id del aula*/
            [Route("Aula/Schedules/{idAula:int}")]
        public ActionResult getScheduleAula(int idAula)
        {
            var periodo_actual = int.Parse(Request.Cookies["Periodo"].Value);
            if (HttpContext.Request.IsAjaxRequest())
            {
                var listaPlanes = from aula in db.Aulas
                                  join grupo_aula in db.GrupoAula on aula.ID equals grupo_aula.ClassroomID
                                  join horario in db.Horarios on grupo_aula.ScheduleID equals horario.ID
                                  join sede in db.Sedes on aula.SedeID equals sede.ID
                                  join grupo in db.Grupos on grupo_aula.GroupID equals grupo.ID
                                  join plan_bloque_curso in db.BloquesXPlanesXCursos on grupo.BlockXPlanXCourseID equals plan_bloque_curso.ID
                                  join curso in db.Cursos on plan_bloque_curso.CourseID equals curso.ID
                                  join periodo in db.Periodos on grupo.PeriodID equals periodo.ID
                                  where (aula.ID == idAula) && (periodo.ID == periodo_actual) && (horario.StartHour != "700" && horario.StartHour != "900")
                                  
                                  select new { curso.Name, grupo.Number, horario.StartHour, horario.EndHour,Day=horario.Day== "Lunes"?1:
                                                                                                                horario.Day=="Martes"?2:
                                                                                                                horario.Day=="Miércoles"?3:
                                                                                                                horario.Day == "Jueves" ? 4 :
                                                                                                                horario.Day=="Viernes"?5:
                                                                                                                horario.Day=="Sábado"?6:
                                                                                                                0};
                //listaPlanes.Where(p => p.Day == "lunes").OrderBy().ToList();
                listaPlanes = listaPlanes.OrderBy(c => c.StartHour.Length).ThenBy(c => c.StartHour).ThenBy(c=>c.Day);
                /*Es necesario remover elementos de la lista que los horarios no son correctos*/
               
                var json = JsonConvert.SerializeObject(listaPlanes);
                return Content(json);
            }
            return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

        }
        #endregion
    }
}
