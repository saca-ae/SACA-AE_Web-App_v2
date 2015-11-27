using Newtonsoft.Json;
using SACAAE.Data_Access;
using SACAAE.Helpers;
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
        private ScheduleHelper dbHelper = new ScheduleHelper();
        private const string TempDataMessageKeySuccess = "MessageSuccess";
        private const string TempDataMessageKeyError = "MessageError";
        // GET: ComisionProfesor/Asignar
        public ActionResult Asignar(int? Profesor)
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

            ViewBag.Professors = new SelectList(db.Professors.OrderBy(p => p.Name), "ID", "Name",Profesor); 
            

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
            var vPeriodID = db.Periods.Find(int.Parse(vPeriod)).ID;

            string vHourCharge = pSchedule.HourCharge;
            int vCommissionID = Convert.ToInt32(pSchedule.Commissions);
            int vProfessorID = Convert.ToInt32(pSchedule.Professors);
            List<ScheduleComission> vSchedules = pSchedule.ScheduleCommission;

            string validate = dbHelper.validationsCommission(vCommissionID, vProfessorID, vPeriodID, vSchedules);

            if(validate.Equals("true"))
            {
                int totalHourAssign=0;

                //Save Commission Professor
                CommissionXProfessor vCommissionProfessor = new CommissionXProfessor();
                vCommissionProfessor.CommissionID = Convert.ToInt32(vCommissionID);
                vCommissionProfessor.ProfessorID = Convert.ToInt32(vProfessorID);
                if (vHourCharge.Equals("1"))
                {
                    vCommissionProfessor.HourAllocatedTypeID = Convert.ToInt32(vHourCharge);
                }
                vCommissionProfessor.PeriodID = vPeriodID;
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

                    //Convert StartHour to DateTime
                    var vStartHour = DateTime.Parse(vSchedule.StartHour);
                    var vEndHour = DateTime.Parse(vSchedule.EndHour);

                    var CargaC = Math.Ceiling(vEndHour.Subtract(vStartHour).TotalHours);
                
                    int vDiferencia = Convert.ToInt32(CargaC);
                

                    totalHourAssign = totalHourAssign+vDiferencia;
                }

           
                vCommissionProfessor.Hours = totalHourAssign;

                db.CommissionsXProfessors.Add(vCommissionProfessor);

                db.SaveChanges();
                TempData[TempDataMessageKeySuccess] = "Profesor asignado correctamente.";

                return RedirectToAction("Asignar");
            }
            else if (validate.Equals("falseIsGroupShock"))
            {
                TempData[TempDataMessageKeyError] = "Existe choque de horario con grupos, no se asigno al profesor a la comisión";
                return RedirectToAction("Asignar");
            }
            else if (validate.Equals("falseIsProjectShock"))
            {
                TempData[TempDataMessageKeyError] = "Existe choque de horario con proyectos, no se asigno al profesor a la comisión";
                return RedirectToAction("Asignar");
            }

            else if (validate.Equals("falseIsCommissionShock"))
            {
                TempData[TempDataMessageKeyError] = "Existe choque de horario con comisiones, no se asigno al profesor a la comisión";
                return RedirectToAction("Asignar");
            }
            else if (validate.Equals("falseIsProfessorShock"))
            {
                TempData[TempDataMessageKeyError] = "El profesor ya esta asignado a esta comision, no se permite asignar dos veces a un profesor a una comisión ";
                return RedirectToAction("Asignar");
            }
            return View();
           
           
        }

        //GET: ComissionProfessor/Editar
        public ActionResult Editar()
        {
            if (Request.UrlReferrer != null)
            {
                ViewBag.returnUrl = Request.UrlReferrer.ToString();
            }
            else
            {
                ViewBag.returnUrl = null;
            }

            /* get List of all commissions */
            ViewBag.Commission = new SelectList(db.Commissions.OrderBy(p => p.Name), "ID", "Name");

            return View();
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
            ViewBag.profesores = new SelectList(db.Professors.OrderBy(p => p.Name), "ID", "Name");

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
                TempData[TempDataMessageKeySuccess] = "Profesor revocado de comisión correctamente.";
            }
            else
            {
                TempData[TempDataMessageKeySuccess] = "Ocurrió un error al revocar el profesor.";
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

        /// <summary>
        /// Esteban Segura Benavides
        /// Check posibles conflicts with the new project schedule and the all commission schedule related with determinated professor
        /// </summary>
        /// <param name="pProfessorID"></param>
        /// <param name="pSchedules"></param>
        /// <returns>true if found any problem with the schedules</returns>
        public bool existShockScheduleCommission(int pProfessorID, List<ScheduleComission> pSchedules)
        {
            var vPeriod = Request.Cookies["Periodo"].Value;
            var vPeriodID = db.Periods.Find(int.Parse(vPeriod)).ID;

            //Get the day, starthour and endhour where professor was assign in commission
            var commission_schedule = db.SP_getProfessorScheduleCommission(pProfessorID, vPeriodID).ToList();

            //Verify each scheedule with the new assign information
            foreach (var vNewSchedule in pSchedules)
            {
                foreach (var vActualScheduleCommission in commission_schedule)
                {
                    if (vNewSchedule.Day.Equals(vActualScheduleCommission.Day))
                    {
                        var vActualStartHour = DateTime.Parse(vActualScheduleCommission.StartHour);
                        var vActualEndHour = DateTime.Parse(vActualScheduleCommission.EndHour);
                        var vNewStartHour = DateTime.Parse(vNewSchedule.StartHour);
                        var vNewEndHour = DateTime.Parse(vNewSchedule.EndHour);

                        //Check the range of the schedule
                        if ((vActualStartHour <= vNewStartHour && vNewStartHour <= vActualEndHour) ||
                            (vActualStartHour <= vNewEndHour && vNewEndHour <= vActualEndHour) ||
                            (vNewStartHour <= vActualStartHour && vActualStartHour <= vNewEndHour) ||
                            (vNewStartHour <= vActualEndHour && vActualEndHour <= vNewEndHour))
                        {
                            return true;
                        }
                    }
                }
            }
            return false;
        }

        /// <summary>
        /// Esteban Segura Benavides
        /// Check posibles conflicts with the new project schedule and the all project schedule related with determinated professor
        /// </summary>
        /// <param name="pProfessorID"></param>
        /// <param name="pSchedules"></param>
        /// <returns>true if found any problem with the schedules</returns>
        public bool existShockScheduleProject(int pProfessorID, List<ScheduleComission> pSchedules)
        {
            var vPeriod = Request.Cookies["Periodo"].Value;
            var vPeriodID = db.Periods.Find(int.Parse(vPeriod)).ID;

            //Get the day, starthour and endhour where professor was assign in commission
            var project_schedule = db.SP_getProfessorScheduleProject(pProfessorID, vPeriodID).ToList();

            //Verify each scheedule with the new assign information
            foreach (var vNewSchedule in pSchedules)
            {
                foreach (var vActualScheduleProject in project_schedule)
                {
                    if (vNewSchedule.Day.Equals(vActualScheduleProject.Day))
                    {
                        var vActualStartHour = DateTime.Parse(vActualScheduleProject.StartHour);
                        var vActualEndHour = DateTime.Parse(vActualScheduleProject.EndHour);
                        var vNewStartHour = DateTime.Parse(vNewSchedule.StartHour);
                        var vNewEndHour = DateTime.Parse(vNewSchedule.EndHour);

                        //Check the range of the schedule
                        if ((vActualStartHour <= vNewStartHour && vNewStartHour <= vActualEndHour) ||
                            (vActualStartHour <= vNewEndHour && vNewEndHour <= vActualEndHour) ||
                            (vNewStartHour <= vActualStartHour && vActualStartHour <= vNewEndHour) ||
                            (vNewStartHour <= vActualEndHour && vActualEndHour <= vNewEndHour))
                        {
                            return true;
                        }
                    }
                }
            }
            return false;
        }

        /// <summary>
        /// Esteban Segura Benavides
        /// Check posibles conflicts with the new project schedule and the all group schedule related with determinated professor
        /// </summary>
        /// <param name="pProfessorID"></param>
        /// <param name="pSchedules"></param>
        /// <returns>true if found any problem with the schedules</returns>
        public bool existShockScheduleGroup(int pProfessorID, List<ScheduleComission> pSchedules)
        {
            var vPeriod = Request.Cookies["Periodo"].Value;
            var vPeriodID = db.Periods.Find(int.Parse(vPeriod)).ID;

            //Get the day, starthour and endhour where professor was assign in commission
            var project_schedule = db.SP_getProfessorScheduleGroup(pProfessorID, vPeriodID).ToList();

            //Verify each scheedule with the new assign information
            foreach (var vNewSchedule in pSchedules)
            {
                foreach (var vActualScheduleProject in project_schedule)
                {
                    if (vNewSchedule.Day.Equals(vActualScheduleProject.Day))
                    {
                        var vActualStartHour = DateTime.Parse(vActualScheduleProject.StartHour);
                        var vActualEndHour = DateTime.Parse(vActualScheduleProject.EndHour);
                        var vNewStartHour = DateTime.Parse(vNewSchedule.StartHour);
                        var vNewEndHour = DateTime.Parse(vNewSchedule.EndHour);

                        //Check the range of the schedule
                        if ((vActualStartHour <= vNewStartHour && vNewStartHour <= vActualEndHour) ||
                            (vActualStartHour <= vNewEndHour && vNewEndHour <= vActualEndHour) ||
                            (vNewStartHour <= vActualStartHour && vActualStartHour <= vNewEndHour) ||
                            (vNewStartHour <= vActualEndHour && vActualEndHour <= vNewEndHour))
                        {
                            return true;
                        }

                    }
                }
            }
            return false;
        }

        /// <summary>
        /// Check if a professor is already assig in a project
        /// </summary>
        /// <param name="pCommissionID"></param>
        /// <param name="pProfessorID"></param>
        /// <returns>if professor is already assign return true el return false</returns>
        public bool isProfessorAssign(int pCommissionID, int pProfessorID)
        {
            var vPeriod = Request.Cookies["Periodo"].Value;
            var vIDPeriod = db.Periods.Find(int.Parse(vPeriod)).ID;

            var getAssign = (from commission_profesor in db.CommissionsXProfessors
                             join professor in db.Professors on commission_profesor.ProfessorID equals professor.ID
                             join period in db.Periods on commission_profesor.PeriodID equals period.ID
                             where commission_profesor.CommissionID == pCommissionID & period.ID == vIDPeriod
                             select new { professorID = professor.ID }).ToList();
            foreach (var professor in getAssign)
            {
                if (pProfessorID == professor.professorID)
                {
                    return true;
                }
            }
            return false;
        }
        #endregion
    }
}