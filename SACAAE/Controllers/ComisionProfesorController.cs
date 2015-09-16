using Newtonsoft.Json;
using SACAAE.Data_Access;
using SACAAE.Models;
using SACAAE.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Data.Entity;
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

            ViewBag.Professors = new SelectList(db.Professors, "ID", "Name"); ;

            if (entidadID == 1)
            {
                var comisiones = db.Commissions.Where(p => p.StateID == 1 && (p.EntityTypeID == 1 ||     //TEC
                                                          p.EntityTypeID == 2 || p.EntityTypeID == 3 || //TEC-VIC TEC-REC
                                                          p.EntityTypeID == 4 || p.EntityTypeID == 10)) //TEC-MIXTO TEC-Académico
                                              .OrderBy(p => p.Name);
                ViewBag.Commissions = new SelectList(comisiones, "ID", "Name");
            }
            else
            {
                var comisiones = db.Commissions.Where(p => p.EntityTypeID == entidadID).OrderBy(p => p.Name);
                ViewBag.Commissions = new SelectList(comisiones, "ID", "Name");
            }

            return View();
        }

        // POST: ComisionProfesor/Asignar
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Asignar(ScheduleComissionViewModel pSchedule)
        {
            var vPeriod = Request.Cookies["Periodo"].Value;
            var vIDPeriod = db.Periods.Find(int.Parse(vPeriod)).ID;

            string vHourCharge = pSchedule.HourCharge;
            string vCommission = pSchedule.Commissions;
            string vProfessor = pSchedule.Professors;
            List<ScheduleComission> vSchedules = pSchedule.ScheduleCommission;

            int totalHourAssign=0;

            //Save Commission Professor
            CommissionXProfessor vCommissionProfessor = new CommissionXProfessor();
            vCommissionProfessor.CommissionID = Convert.ToInt32(vCommission);
            vCommissionProfessor.ProfessorID = Convert.ToInt32(vProfessor);
            vCommissionProfessor.HourAllocatedTypeID = Convert.ToInt32(vHourCharge);
            vCommissionProfessor.PeriodID = vIDPeriod;
            vCommissionProfessor.Schedule = new List<Schedule>();
           
          

            //Calculate the total hour assign
            foreach(ScheduleComission vSchedule in vSchedules )
            {
                Schedule vTempSchedule = existSchedule(vSchedule.Day, vSchedule.StartHour, vSchedule.EndHour);
                if (vTempSchedule!= null)
                {
                    //Get id schedule
                    
                    vTempSchedule.CommissionsXProfessors.Add(vCommissionProfessor);
                    
                }

                /******/
                //var HoraInicio = DateTime.Parse(vDay.StartHour);
                //var HoraFin = DateTime.Parse(vDay.EndHour);

                //var CargaC = Math.Ceiling(HoraFin.Subtract(HoraInicio).TotalHours);
                //if (HoraInicio <= DateTime.Parse("12:00 PM") && HoraFin >= DateTime.Parse("01:00 PM"))
                //{
                //    CargaC = CargaC - 1;
                //}
                /*****/

                int vIntStartHour=0;
                int vIntEndHour=0;

                 switch ( vSchedule.StartHour)
                {
                    case "07:30 am":
                        vIntStartHour = 730;
                        break;
                    case "08:30 am":
                        vIntStartHour = 830;
                        break;
                    case "09:30 am":
                        vIntStartHour = 930;
                        break;
                    case "10:30 am":
                        vIntStartHour = 1030;
                        break;
                    case "11:30 am":
                        vIntStartHour = 1130;
                        break;
                    case "12:30 pm":
                        vIntStartHour = 1230;
                        break;

                    case "01:00 pm":
                        vIntStartHour = 1300;
                        break;
                    case "02:00 pm":
                        vIntStartHour = 1400;
                        break;

                    case "03:00 pm":
                        vIntStartHour = 1500;
                        break;
                    case "04:00 pm":
                        vIntStartHour = 1600;
                        break;

                    case "05:00 pm":
                        vIntStartHour = 1700;
                        break;
                    case "06:00 pm":
                        vIntStartHour = 1800;
                        break;

                    case "07:00 pm":
                        vIntStartHour = 1900;
                        break;
                    case "08:00 pm":
                        vIntStartHour = 2000;
                        break;

                    case "09:00 pm":
                        vIntStartHour = 2100;
                        break;
                }

                switch (vSchedule.EndHour) {
                    case "08:20 am":
                        vIntEndHour = 820;
                        break;
                    case "09:20 am":
                        vIntEndHour = 920;
                        break;
                    case "10:20 am":
                        vIntEndHour = 1020;
                        break;
                    case "11:20 am":
                        vIntEndHour = 1120;
                        break;
                    case "12:20 pm":
                        vIntEndHour = 1220;
                        break;

                    case "01:50 pm":
                        vIntEndHour = 1350;
                        break;
                    case "02:50 pm":
                        vIntEndHour = 1450;
                        break;

                    case "03:50 pm":
                        vIntEndHour = 1550;
                        break;
                    case "04:50 pm":
                        vIntEndHour = 1650;
                        break;

                    case "05:50 pm":
                        vIntEndHour = 1750;
                        break;
                    case "06:50 pm":
                        vIntEndHour = 1850;
                        break;

                    case "07:50 pm":
                        vIntEndHour = 1950;
                        break;
                    case "08:50 pm":
                        vIntEndHour = 2050;
                        break;

                    case "09:50 pm":
                        vIntEndHour = 2150;
                        break;
                }

                int vDiferencia = (vIntEndHour - vIntStartHour);

                if (vIntStartHour < 1300) {
                    vDiferencia = vDiferencia + 10;
                }
                else if (vIntStartHour < 1300 & vIntEndHour > 1300) {
                    vDiferencia = vDiferencia + 110;
                }
                else {
                    vDiferencia = vDiferencia + 50;
                }

                vDiferencia = vDiferencia/100;

                totalHourAssign = totalHourAssign+vDiferencia;
            }

           
            vCommissionProfessor.Hours = totalHourAssign;

            db.CommissionsXProfessors.Add(vCommissionProfessor);

            db.SaveChanges();
            TempData[TempDataMessageKey] = "Profesor asignado correctamente.";

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
            ViewBag.profesores = new SelectList(db.Professors, "ID", "Name");

            return View();
        }

        // POST: ComisionProfesor/Revocar
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Revocar(int sltComisiones)
        {
            var revocado = false;
            var comision = db.CommissionsXProfessors.Find(sltComisiones);

            if (comision != null)
            {
                db.CommissionsXProfessors.Remove(comision);
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
            var vPeriod = Request.Cookies["Periodo"].Value;
            int vIDPeriod = db.Periods.Find(int.Parse(vPeriod)).ID;
            if (HttpContext.Request.IsAjaxRequest())
            {
                var listaComisiones = db.Professors.Find(idProfesor)
                                                   .CommissionsXProfessors
                                                   .Where(p => p.PeriodID == vIDPeriod)
                                                   .Select(p => new { p.ID, p.Commission.Name });
                                                   

                return Json(listaComisiones, JsonRequestBehavior.AllowGet);
            }
            return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        }
        #endregion

        #region Helpers
        private Schedule existSchedule(string pDay, string pStartHour, string pEndHour)
        {
            var vSchedule = db.Schedules.Where(p => p.Day == pDay && p.StartHour == pStartHour && p.EndHour == pEndHour).FirstOrDefault();

            if (vSchedule != null)
            {
                return vSchedule;
            }
            else
            {
                //Create schedule and get id
                Schedule vNewSchedule = new Schedule();
                vNewSchedule.Day = pDay;
                vNewSchedule.StartHour = pStartHour;
                vNewSchedule.EndHour = pEndHour;
                vNewSchedule.CommissionsXProfessors = new List<CommissionXProfessor>();

                db.Schedules.Add(vNewSchedule);
                //db.SaveChanges();

                //vSchedule = db.Schedules.Where(p => p.Day == pDay && p.StartHour == pStartHour && p.EndHour == pEndHour).FirstOrDefault();

                //db.SaveChanges();
                return vNewSchedule;
            }
                //select * from Schedule where Day='Domingo' AND StartHour = '07:30 am' AND EndHour = '09:20 am'
        }
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

        /// <summary>
        ///  Remove a profesor from a group
        /// </summary>
        /// <autor> Esteban Segura Benavides </autor>
        /// <param name="pIDGrupo"> ID of group in database</param>
        /// <returns>Information about the action of remove a profesor from a group</returns>
        [Route("ComisionProfesor/ComisionProfesor/{pIDComisionProfesor:int}/removeProfesor")]
        public ActionResult removeGroup(int pIDComisionProfesor)
        {
            if (HttpContext.Request.IsAjaxRequest())
            {
                var vComisionProfesor = db.CommissionsXProfessors.Find(pIDComisionProfesor);
                if(vComisionProfesor!=null)
                {
                    db.CommissionsXProfessors.Remove(vComisionProfesor);
                    db.SaveChanges();
                    var respuesta = new { respuesta = "success" };
                    var json = JsonConvert.SerializeObject(respuesta);
                    return Content(json);
                }
                else
                {
                    var respuesta_error = new { respuesta = "error" };
                    var json_respuesta_error = JsonConvert.SerializeObject(respuesta_error);
                    return Content(json_respuesta_error);
                }
            }
            var error = new { respuesta = "error" };
            var json_error = JsonConvert.SerializeObject(error);
            return Content(json_error);
        }
        #endregion
    }
}