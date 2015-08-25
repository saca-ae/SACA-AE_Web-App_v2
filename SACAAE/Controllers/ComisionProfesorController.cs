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
    public class ComisionProfesorController : Controller
    {
        private SACAAEContext db = new SACAAEContext();
        private const string TempDataMessageKey = "Message";

        // GET: ComisionProfesor/Asignar
        public ActionResult Asignar()
        {

            List<String> HorasInicio = new List<String>();
            List<String> HorasFin = new List<String>();
            for (int i = 7; i < 23; i++)
            {
                HorasInicio.Add(i.ToString() + ":00");
                HorasFin.Add(i.ToString() + ":00");
            }
            ViewBag.HorasInicio = HorasInicio;
            ViewBag.HorasFin = HorasFin;


            if (Request.UrlReferrer != null)
            {
                ViewBag.returnUrl = Request.UrlReferrer.ToString();
            }
            else
            {
                ViewBag.returnUrl = null;
            }

            var entidad = Request.Cookies["Entidad"].Value;
            var entidadID = getEntityID(entidad);

            ViewBag.profesores = new SelectList(db.Profesores, "ID", "Name"); ;

            if (entidadID == 1)
            {
                var comisiones = db.Comisiones.Where(p => p.StateID == 1 && (p.EntityTypeID == 1 ||     //TEC
                                                          p.EntityTypeID == 2 || p.EntityTypeID == 3 || //TEC-VIC TEC-REC
                                                          p.EntityTypeID == 4 || p.EntityTypeID == 10)) //TEC-MIXTO TEC-Académico
                                              .OrderBy(p => p.Name);
                ViewBag.comisiones = new SelectList(comisiones, "ID", "Name");
            }
            else
            {
                var comisiones = db.Comisiones.Where(p => p.EntityTypeID == entidadID).OrderBy(p => p.Name);
                ViewBag.comisiones = new SelectList(comisiones, "ID", "Name");
            }

            return View();
        }

        // POST: ComisionProfesor/Asignar
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Asignar(String profesor, String comision)
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
            var IdPeriodo = db.Periodos.Find(int.Parse(periodo)).ID;

            for (int i = 1; i < Cantidad; i++)
            {
                String Detalles = Request.Cookies["DiaSeleccionadoCookie" + i].Value;//Obtiene los datos de la cookie
                string[] Partes = Detalles.Split('|');

                String Dia = Partes[0];
                String HoraInicio = Partes[1];
                String HoraFin = Partes[2];


                if (Dia != "d")
                {
                    //var creado = repositoriocomisionesprofesor.CrearComisionProfesor(profesor, comision, Dia, HoraInicio, HoraFin, IdPeriodo);
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

        // GET: ComisionProfesor/Revocar
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

        // POST: ComisionProfesor/Revocar
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Revocar(int sltComisiones)
        {
            var revocado = false;
            var comision = db.ComisionesXProfesores.Find(sltComisiones);

            if (comision != null)
            {
                db.ComisionesXProfesores.Remove(comision);
                db.SaveChanges();
                revocado = true;
            }

            if (revocado)
            {
                TempData[TempDataMessageKey] = "Profesor revocado de comisión correctamente.";
            }
            else
            {
                TempData[TempDataMessageKey] = "Ocurrió un error al revocar el profesor.";
            }

            return RedirectToAction("Revocar");
        }

        #region Ajax POST
        [Route("ComisionProfesor/Profesor/Comisiones/{idProfesor:int}")]
        public ActionResult ObtenerComisionesXProfesor(int idProfesor)
        {
            if (HttpContext.Request.IsAjaxRequest())
            {
                var listaComisiones = db.Profesores.Find(idProfesor)
                                                   .ComisionesXProfesores
                                                   .Select(p => new { p.Comision.ID, p.Comision.Name });

                return Json(listaComisiones, JsonRequestBehavior.AllowGet);
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