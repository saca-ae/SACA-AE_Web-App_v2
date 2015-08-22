using SACAAE.Data_Access;
using SACAAE.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace SACAAE.Controllers
{
    [Authorize]
    public class ProyectoProfesorController : Controller
    {
        private SACAAEContext db = new SACAAEContext();
        private const string TempDataMessageKey = "Message";

        // GET: ProyectoProfesor/Asignar
        public ActionResult Asignar()
        {
            /*EN CASO DE UTILIZAR HORARIO TEC
            
            List<String> HorasInicio = new List<String>();
            List<String> HorasFin = new List<String>();
            
            for (int i = 7; i < 22; i++)
            {
                if (i < 13)
                    HorasInicio.Add(i.ToString() + ":30");
                else
                    HorasInicio.Add(i.ToString() + ":00");
            }
            ViewBag.HorasInicio = HorasInicio;

            List<String> HorasFin = new List<String>();
            for (int i = 8; i < 22; i++)
            {
                if (i < 13)
                    HorasFin.Add(i.ToString() + ":20");
                else
                    HorasFin.Add(i.ToString() + ":50");
            }
            ViewBag.HorasFin = HorasFin;
            */

            List<String> HorasInicio = new List<String>();
            List<String> HorasFin = new List<String>();
            for (int i = 7; i < 23; i++)
            {
                HorasInicio.Add(i.ToString() + ":00");
                HorasFin.Add(i.ToString() + ":00");
            }
            ViewBag.HorasInicio = HorasInicio;
            ViewBag.HorasFin = HorasFin;

            String entidad = Request.Cookies["Entidad"].Value;
            var entidadID = getEntityID(entidad);

            ViewBag.profesores = new SelectList(db.Profesores, "ID", "Name"); ;

            if (entidadID == 1)
            {
                var proyectos = db.Proyectos.Where(p => p.State == 1 && (p.EntityType == 1 ||     //TEC
                                                         p.EntityType == 2 || p.EntityType == 3 || //TEC-VIC TEC-REC
                                                         p.EntityType == 4 || p.EntityType == 10)) //TEC-MIXTO TEC-Académico
                                             .OrderBy(p => p.Name);
                ViewBag.proyectos = new SelectList(proyectos, "ID", "Name");
            }
            else
            {
                var proyectos = db.Proyectos.Where(p => p.EntityType == entidadID).OrderBy(p => p.Name);
                ViewBag.proyectos = new SelectList(proyectos, "ID", "Name");
            }
            return View();
        }

        // POST: ProyectoProfesor/Asignar
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Asignar(String sltProfesor, String sltProyecto)
        {

            int Cantidad;
            try
            {
                Cantidad = Convert.ToInt32(Request.Cookies["Cantidad"].Value);
                Cantidad++;
            }
            catch (Exception e)
            {
                Cantidad = 0;
            }

            var periodo = Request.Cookies["Periodo"].Value;
            var IdPeriodo = db.Periodos.Where(p => p.Name == periodo).FirstOrDefault().ID;

            for (int i = 1; i < Cantidad; i++)
            {
                String Detalles = Request.Cookies["DiaSeleccionadoCookie" + i].Value;//Obtiene los datos de la cookie
                string[] Partes = Detalles.Split('|');

                String Dia = Partes[0];
                String HoraInicio = Partes[1];
                String HoraFin = Partes[2];

                if (Dia != "d")
                {
                    //var creado = repoProfesProyectos.CrearProyectoProfesor(sltProfesor, sltProyecto, Dia, HoraInicio, HoraFin, IdPeriodo);

                    //if (creado)
                    //{
                    //    TempData[TempDataMessageKey] = "Profesor asignado correctamente.";
                    //}
                    //else
                    //{
                    //    TempData[TempDataMessageKey] = "Ocurrió un error al asignar el profesor.";
                    //}
                }


            }
            return RedirectToAction("Asignar");
        }

        // GET: ProyectoProfesor/Revocar
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
            ViewBag.profesores = new SelectList(db.Profesores, "ID", "Name");

            return View();
        }

        // POST: ProyectoProfesor/Revocar
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Revocar(int sltProyectos)
        {
            var revocado = false;

            var proyecto = db.ProyectosXProfesores.Find(sltProyectos);

            if (proyecto != null)
            {
                db.ProyectosXProfesores.Remove(proyecto);
                db.SaveChanges();
                revocado = true;
            }

            if (revocado)
            {
                TempData[TempDataMessageKey] = "Profesor revocado de proyecto correctamente.";
            }
            else
            {
                TempData[TempDataMessageKey] = "Ocurrió un error al revocar el profesor.";
            }

            return RedirectToAction("Revocar");
        }

        #region Ajax Post
        [Route("ProyectoProfesor/Profesor/Proyecto/{idProfesor:int}")]
        public ActionResult ObtenerProyectosXProfesor(int idProfesor)
        {
            if (HttpContext.Request.IsAjaxRequest())
            {
                var listaProyectos = db.Profesores.Find(idProfesor)
                                                  .ProyectosXProfesores
                                                  .Select(p => new { p.Proyecto.ID, p.Proyecto.Name });

                return Json(listaProyectos, JsonRequestBehavior.AllowGet);
            }
            return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        }
        #endregion

        #region Helpers
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
        #endregion
    }
}