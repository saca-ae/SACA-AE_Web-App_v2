using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using SACAAE.Models;
using SACAAE.Data_Access;
using SACAAE.Models.ViewModels;
using Newtonsoft.Json;
using System.Globalization;

namespace SACAAE.Controllers
{
    public class ComisionController : Controller
    {
        private SACAAEContext db = new SACAAEContext();
        private const string TempDataMessageKeySuccess = "MessageSuccess";
        private const string TempDataMessageKeyError = "MessageError";
        // GET: /Comision/
        public ActionResult Index()
        {
            String entidad = Request.Cookies["Entidad"].Value;
            if (entidad.Equals("TEC"))
            {
                var model = ObtenerComisionesXEntidad(1);
                return View(model);
            }
            else if (entidad.Equals("CIE"))
            {
                var model = ObtenerComisionesXEntidad(7);
                return View(model);
            }
            else if (entidad.Equals("TAE"))
            {
                var model = ObtenerComisionesXEntidad(5);
                return View(model);
            }
            else if (entidad.Equals("MAE"))
            {
                var model = ObtenerComisionesXEntidad(6);
                return View(model);
            }
            else if (entidad.Equals("DDE"))
            {
                var model = ObtenerComisionesXEntidad(11);
                return View(model);
            }
            else if (entidad.Equals("Emprendedores"))
            {
                var model = ObtenerComisionesXEntidad(12);
                return View(model);
            }
            else if (entidad.Equals("Actualizacion_Cartago"))
            {
                var model = ObtenerComisionesXEntidad(9);
                return View(model);
            }
            else
            {
                var model = ObtenerComisionesXEntidad(8); //Actualización San Carlos
                return View(model);
            }
        }

        // GET: /Comision/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Commission commission = db.Commissions.Find(id);
            //****************
            if (commission == null)
            {
                return HttpNotFound();
            }

            ProfessorAssignViewModel commissionViewModel = new ProfessorAssignViewModel();
            commissionViewModel.ID = commission.ID;
            commissionViewModel.Name = commission.Name;

            var vStartHour = commission.Start.ToString("dd/MM/yyyy");
            commissionViewModel.Start = vStartHour;

            var vEndHour = commission.End.ToString("dd/MM/yyyy");

            commissionViewModel.End = vEndHour;
            
            return View(commissionViewModel);
        }

        // GET: /Comision/Create
        public ActionResult Create()
        {
            ViewBag.EntityTypeID = new SelectList(db.EntityTypes, "ID", "Name");
            ViewBag.StateID = new SelectList(db.States, "ID", "Name");
            return View();
        }

        // POST: /Comision/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include="ID,Name,Start,End,StateID,EntityTypeID")] Commission commission)
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

            if (string.IsNullOrEmpty(commission.Name.Trim()))
                throw new ArgumentException("El nombre de la comisión no es válida. Por favor, inténtelo de nuevo");


            if (ModelState.IsValid)
            {
                commission.EntityTypeID = entidadID;
                commission.StateID = 1;
                db.Commissions.Add(commission);
                db.SaveChanges();
                TempData[TempDataMessageKeySuccess] = "Comisión creada correctamente.";
                return RedirectToAction("Index");
            }

            ViewBag.EntityTypeID = new SelectList(db.EntityTypes, "ID", "Name", commission.EntityTypeID);
            ViewBag.StateID = new SelectList(db.States, "ID", "Name", commission.StateID);
            return View(commission);
        }

        // GET: /Comision/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Commission commission = db.Commissions.Find(id);
            if (commission == null)
            {
                return HttpNotFound();
            }
            ViewBag.EntityTypeID = new SelectList(db.EntityTypes, "ID", "Name", commission.EntityTypeID);
            ViewBag.StateID = new SelectList(db.States, "ID", "Name", commission.StateID);
            return View(commission);
        }

        // POST: /Comision/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include="ID,Name,Start,End,StateID,EntityTypeID")] Commission commission)
        {

            if (!ExisteComision(commission))
            {
                AgregarComision(commission);
            }

            if (ModelState.IsValid)
            {
                var temp = db.Commissions.Find(commission.ID);

                if (temp != null)
                {
                    db.Entry(temp).Property(c => c.Name).CurrentValue = commission.Name;
                    db.Entry(temp).Property(c => c.Start).CurrentValue = commission.Start;
                    db.Entry(temp).Property(c => c.End).CurrentValue = commission.End;
                }
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(commission);
        }

        // GET: /Comision/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Commission commission = db.Commissions.Find(id);
            if (commission == null)
            {
                return HttpNotFound();
            }
            return View(commission);
        }

        //GET: /Comision/AsignarProfesorComision/5
        public ActionResult AsignarProfesorComision(int? id)
        
        {
            Commission commission = db.Commissions.Find(id);
            ScheduleComissionViewModel commissionViewModel = new ScheduleComissionViewModel();
            commissionViewModel.Commissions = commission.Name;

            ViewBag.Professors = new SelectList(db.Professors, "ID", "Name"); ;
            return View(commissionViewModel);
        }

        // POST: ComisionProfesor/AsignarProfesorComision
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AsignarProfesorComision(ScheduleComissionViewModel pSchedule)
        {
            var vPeriod = Request.Cookies["Periodo"].Value;
            var vIDPeriod = db.Periods.Find(int.Parse(vPeriod)).ID;

            var vHourCharge = Convert.ToInt32(pSchedule.HourCharge);
            var vCommissionID = Convert.ToInt32(pSchedule.Commissions);
            var vProfessorID = Convert.ToInt32(pSchedule.Professors);

            List<ScheduleComission> vSchedules = pSchedule.ScheduleCommission;
            string validate = validations(vCommissionID,vProfessorID,vSchedules);
            if(validate.Equals("true"))
            {
                int totalHourAssign = 0;

                //Save Commission Professor
                CommissionXProfessor vCommissionProfessor = new CommissionXProfessor();
                vCommissionProfessor.CommissionID = Convert.ToInt32(vCommissionID);
                vCommissionProfessor.ProfessorID = Convert.ToInt32(vProfessorID);
                if (vHourCharge.Equals("1"))
                {
                    vCommissionProfessor.HourAllocatedTypeID = Convert.ToInt32(vHourCharge);
                }
                vCommissionProfessor.PeriodID = vIDPeriod;
                vCommissionProfessor.Schedule = new List<Schedule>();

                //Calculate the total hour assign
                foreach (ScheduleComission vSchedule in vSchedules)
                {
                    Schedule vTempSchedule = existSchedule(vSchedule.Day, vSchedule.StartHour, vSchedule.EndHour);
                    if (vTempSchedule != null)
                    {
                        //Get id schedule

                        vTempSchedule.CommissionsXProfessors.Add(vCommissionProfessor);

                    }

                    //Convert StartHour to DateTime
                    var vStartHour = DateTime.Parse(vSchedule.StartHour);
                    var vEndHour = DateTime.Parse(vSchedule.EndHour);

                    var CargaC = Math.Ceiling(vEndHour.Subtract(vStartHour).TotalHours);

                    int vDiferencia = Convert.ToInt32(CargaC);


                    totalHourAssign = totalHourAssign + vDiferencia;
                }


                vCommissionProfessor.Hours = totalHourAssign;

                db.CommissionsXProfessors.Add(vCommissionProfessor);

                db.SaveChanges();
                TempData[TempDataMessageKeySuccess] = "Profesor asignado correctamente.";

                return RedirectToAction("Details", new { id = vCommissionID });
            }
            else if(validate.Equals("falseIsGroupSchock"))
            {
                TempData[TempDataMessageKeyError] = "Existe choque de horario con grupos, no se asigno al profesor al comisión";
                return RedirectToAction("AsignarProfesorComision");
            }
            else if(validate.Equals("falseIsProjectSchock"))
            {
                TempData[TempDataMessageKeyError] = "Existe choque de horario con proyectos, no se asigno al profesor al comisión";
                return RedirectToAction("AsignarProfesorComision");
            }
                
            else if(validate.Equals("falseIsCommissionSchock"))
            {
                TempData[TempDataMessageKeyError] = "Existe choque de horario con comisiones, no se asigno al profesor al comisión";
                return RedirectToAction("AsignarProfesorComision");
            }
            else if (validate.Equals("falseIsProfessorSchock"))
            {
                TempData[TempDataMessageKeyError] = "El profesor ya esta asignado a esta comision, no se permite asignar dos veces a un profesor a una comisión ";
                return RedirectToAction("AsignarProfesorComision");
            }
            return View();
        }

        // POST: /Comision/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Commission commission = db.Commissions.Find(id);
            if (commission != null)
            {
                //db.Entry(commission).Property(c => c.StateID).CurrentValue = 2;
                db.Commissions.Remove(commission);
            }
            db.SaveChanges();
            TempData[TempDataMessageKeySuccess] = "Comisión eliminada correctamente.";
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        //GET: Comision /DetalleAsignacion/5
        public ActionResult DetalleAsignacion(int id)
        {
            CommissionXProfessor comision_profesor = db.CommissionsXProfessors.Find(id);
            return View(comision_profesor);
        }

        //GET: Comision / EditarAsignacion/5
        public ActionResult EditarAsignacion(int id)
        {
            if (Request.UrlReferrer != null)
            {
                ViewBag.returnUrl = Request.UrlReferrer.ToString();
            }
            else
            {
                ViewBag.returnUrl = null;
            }

            var commission_profesor = db.CommissionsXProfessors.Find(id);
            var commissionID = commission_profesor.CommissionID;
            Commission commission = db.Commissions.Find(commissionID);
            ScheduleComissionViewModel commissionViewModel = new ScheduleComissionViewModel();
            commissionViewModel.Commissions = commission.Name;

            ViewBag.Professors = new SelectList(db.Professors, "ID", "Name"); 

            return View(commissionViewModel);
        }

        // POST: /Comision/EditarAsignacion/5
        [HttpPost, ActionName("EditarAsignacion")]
        [ValidateAntiForgeryToken]
        public ActionResult EditarAsignacion(ScheduleComissionViewModel pSchedule)
        {

            var vPeriod = Request.Cookies["Periodo"].Value;
            var vIDPeriod = db.Periods.Find(int.Parse(vPeriod)).ID;

            var vCommissionProfessorID = Convert.ToInt32(pSchedule.Commissions);
            var vHourCharge = Convert.ToInt32(pSchedule.HourCharge);
            var vProfessorID = Convert.ToInt32(pSchedule.Professors);
            
            if (ModelState.IsValid)
            {
                CommissionXProfessor vCommissionProfessor = db.CommissionsXProfessors.Find(vCommissionProfessorID);
                var vCommissionID = vCommissionProfessor.CommissionID;
                List<ScheduleComission> vSchedules = pSchedule.ScheduleCommission;
                string validate = validationsEdit(vCommissionID, vProfessorID, vSchedules);
                if (validate.Equals("true"))
                {
                    vCommissionProfessor.ProfessorID = vProfessorID;
                    if (vHourCharge.Equals("1"))
                    {
                        vCommissionProfessor.HourAllocatedTypeID = Convert.ToInt32(vHourCharge);
                    }
                    else
                    {
                        vCommissionProfessor.HourAllocatedTypeID = null;
                    }
                    var totalHourAssign = 0;
                    vCommissionProfessor.Schedule.Clear();
                    //Calculate the total hour assign
                    foreach (ScheduleComission vSchedule in vSchedules)
                    {
                        Schedule vTempSchedule = existSchedule(vSchedule.Day, vSchedule.StartHour, vSchedule.EndHour);


                        if (vTempSchedule != null)
                        {
                            //Get id schedule

                            vTempSchedule.CommissionsXProfessors.Add(vCommissionProfessor);
                            vCommissionProfessor.Schedule.Add(vTempSchedule);
                        }


                        //Convert StartHour to DateTime
                        var vStartHour = DateTime.Parse(vSchedule.StartHour);
                        var vEndHour = DateTime.Parse(vSchedule.EndHour);

                        var CargaC = Math.Ceiling(vEndHour.Subtract(vStartHour).TotalHours);

                        int vDiferencia = Convert.ToInt32(CargaC);


                        totalHourAssign = totalHourAssign + vDiferencia;
                    }


                    vCommissionProfessor.Hours = totalHourAssign;

                    db.SaveChanges();
                    TempData[TempDataMessageKeySuccess] = "Profesor asignado correctamente.";
                    return RedirectToAction("Details", new { id = vCommissionProfessor.CommissionID });
                }
                else if (validate.Equals("falseIsGroupSchock"))
                {
                    TempData[TempDataMessageKeyError] = "Existe choque de horario con grupos, no se asigno al profesor al comisión";
                    /* Get the list of professor related with commission */
                    if (Request.UrlReferrer != null)
                    {
                        ViewBag.returnUrl = Request.UrlReferrer.ToString();
                    }
                    else
                    {
                        ViewBag.returnUrl = null;
                    }

                    var commission_profesor = db.CommissionsXProfessors.Find(vCommissionProfessorID);
                    var commissionID = commission_profesor.CommissionID;
                    Commission commission = db.Commissions.Find(commissionID);
                    ScheduleComissionViewModel commissionViewModel = new ScheduleComissionViewModel();
                    commissionViewModel.Commissions = commission.Name;

                    ViewBag.Professors = new SelectList(db.Professors, "ID", "Name");

                    return View(commissionViewModel);
                }
                else if (validate.Equals("falseIsProjectSchock"))
                {
                    TempData[TempDataMessageKeyError] = "Existe choque de horario con proyectos, no se asigno al profesor al comisión";
                    if (Request.UrlReferrer != null)
                    {
                        ViewBag.returnUrl = Request.UrlReferrer.ToString();
                    }
                    else
                    {
                        ViewBag.returnUrl = null;
                    }

                    var commission_profesor = db.CommissionsXProfessors.Find(vCommissionProfessorID);
                    var commissionID = commission_profesor.CommissionID;
                    Commission commission = db.Commissions.Find(commissionID);
                    ScheduleComissionViewModel commissionViewModel = new ScheduleComissionViewModel();
                    commissionViewModel.Commissions = commission.Name;

                    ViewBag.Professors = new SelectList(db.Professors, "ID", "Name");

                    return View(commissionViewModel);
                }

                
                
                
            }
            return View();
    }

        
            
        
        #region Helpers
        public IQueryable<Commission> ObtenerComisionesXEntidad(int entidad)
        {
            if (entidad == 1)
            {
                return from comision in db.Commissions
                       orderby comision.Name
                       where comision.EntityType.ID == 1 || //TEC
                       comision.EntityType.ID == 2 || comision.EntityType.ID == 3 || //TEC-VIC TEC-REC
                       comision.EntityType.ID == 4 || comision.EntityType.ID == 10 //TEC-MIXTO TEC-Académico
                       select comision;
            }
            else
            {
                return from comision in db.Commissions
                       orderby comision.Name
                       where comision.EntityType.ID == entidad
                       select comision;

            }

        }

        public bool ExisteComision(Commission comision)
        {
            if (comision == null)
                return false;
            return (db.Commissions.SingleOrDefault(c => c.ID == comision.ID ||
                c.Name == comision.Name) != null);
        }

        private void AgregarComision(Commission  comision)
        {
            if (ExisteComision(comision))
                throw new ArgumentException("Comision ya existe");
            db.Commissions.Add(comision);
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
            var commission_schedule = db.SP_getProfessorScheduleProject(pProfessorID, vPeriodID).ToList();

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
            var commission_schedule = db.SP_getProfessorScheduleGroup(pProfessorID, vPeriodID).ToList();

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

        public string validations(int vCommissionID, int vProfessorID, List<ScheduleComission> pSchedules)
        {
            bool vIsProfessorAssign = isProfessorAssign(Convert.ToInt32(vCommissionID), Convert.ToInt32(vProfessorID));
            if (!vIsProfessorAssign)
            {
                //Check the schedule of the commissions related with the professor
                bool isCommissionShock = existShockScheduleCommission(Convert.ToInt32(vProfessorID), pSchedules);
                //if exist shock with the schedule, the system doesn't let assign new projects in that schedule
                if (!isCommissionShock)
                {
                    //if exist shock with the schedule, the system doesn't let assign new projects in that schedule
                    bool isProjectShock = existShockScheduleProject(Convert.ToInt32(vProfessorID), pSchedules);
                    if (!isProjectShock)
                    {
                        //if exist shock with the schedule, the system doesn't let assign new projects in that schedule
                        bool isGroupShock = existShockScheduleGroup(Convert.ToInt32(vProfessorID), pSchedules);
                        if (!isGroupShock)
                        {
                            return "true";
                        }
                        else
                        {
                            return "falseIsGroupSchock";
                        }
                    }
                    else
                    {
                        return "falseIsProjectSchock";
                    }
                }
                else
                {
                    return "falseIsCommissionSchock";
                }
            }
            else
            {
                return "falseIsProfessorSchock";
            }
        }

        public string validationsEdit(int vCommissionID, int vProfessorID, List<ScheduleComission> pSchedules)
        {
            
            //if exist shock with the schedule, the system doesn't let assign new projects in that schedule
            bool isProjectShock = existShockScheduleProject(Convert.ToInt32(vProfessorID), pSchedules);
            if (!isProjectShock)
            {
                //if exist shock with the schedule, the system doesn't let assign new projects in that schedule
                bool isGroupShock = existShockScheduleGroup(Convert.ToInt32(vProfessorID), pSchedules);
                if (!isGroupShock)
                {
                    return "true";
                }
                else
                {
                    return "falseIsGroupSchock";
                }
            }
            else
            {
                return "falseIsProjectSchock";
            }
            
        }
            
        
        #endregion

        #region ajax
        /// <summary>
        /// Esteban Segura Benavides
        /// Remove  professor from commission according to id commission and id professor
        /// </summary>
        /// <param name="pCommissionID"></param>
        /// <param name="pProfessorID"></param>
        /// <returns></returns>
        [Route("Commission/{pCommissionID:int}/Professor/{pProfessorID:int}/removeProfesor")]
        public ActionResult removeProfessorsCommission(int pCommissionID,int pProfessorID)
        {
            if (HttpContext.Request.IsAjaxRequest())
            {
                var vPeriod = Request.Cookies["Periodo"].Value;
                var vIDPeriod = db.Periods.Find(int.Parse(vPeriod)).ID;

                remove_from_db(vIDPeriod, pCommissionID, pProfessorID);
                /*var deleteComissionProfesor = (from commissionProfesor in db.CommissionsXProfessors
                                              where commissionProfesor.ProfessorID == pProfessorID &&
                                                    commissionProfesor.CommissionID == pCommissionID &&
                                                    commissionProfesor.PeriodID == vIDPeriod
                                              select commissionProfesor);

                foreach (var commissionProfessor in deleteComissionProfesor)
                {
                    db.CommissionsXProfessors.Remove(commissionProfessor);
                }
                db.SaveChanges();*/
                var respuesta = new { respuesta = "success" };
                var json = JsonConvert.SerializeObject(respuesta);
                return Content(json);
            }
            var respuesta_error = new { respuesta = "error" };
            var json_error = JsonConvert.SerializeObject(respuesta_error);
            return Content(json_error);
        }


        public void remove_from_db(int pIDPeriod, int pCommissionID, int pProfessorID)
        {
            var deleteComissionProfesor = (from commissionProfesor in db.CommissionsXProfessors
                                           where commissionProfesor.ProfessorID == pProfessorID &&
                                                 commissionProfesor.CommissionID == pCommissionID &&
                                                 commissionProfesor.PeriodID == pIDPeriod
                                           select commissionProfesor);

            foreach (var commissionProfessor in deleteComissionProfesor)
            {
                db.CommissionsXProfessors.Remove(commissionProfessor);
            }
            db.SaveChanges();
        }
        /// <summary>
        /// Esteban Segura Benavides
        /// Get professor information incommission 
        /// </summary>
        /// <param name="pCommissionProfesorID"></param>
        /// <returns>Professor information</returns>
        [Route("Comission/{pCommissionProfesorID:int}/Profesor")]
        public ActionResult getProfessorAssign(int pCommissionProfesorID)
        {
            var vPeriod = Request.Cookies["Periodo"].Value;
            var vIDPeriod = db.Periods.Find(int.Parse(vPeriod)).ID;

            var getAssign = (from commission_profesor in db.CommissionsXProfessors
                             join professor in db.Professors on commission_profesor.ProfessorID equals professor.ID
                             join period in db.Periods on commission_profesor.PeriodID equals period.ID
                             where commission_profesor.ID == pCommissionProfesorID & period.ID == vIDPeriod
                             select new { professorID = professor.ID, professorName = professor.Name, commission_profesor.HourAllocatedTypeID}).ToList();

            var json = JsonConvert.SerializeObject(getAssign);
            return Content(json);

        }

        /// <summary>
        /// Get all professor related with a determinate commission according a id commission
        /// </summary>
        /// <param name="pCommissionID"></param>
        /// <returns>Information about a professor assign</returns>
        [Route("Commission/{pCommissionID:int}/Professors")]
        public ActionResult getProfesorsCommission(int pCommissionID)
        {
            var vPeriod = Request.Cookies["Periodo"].Value;
            var vIDPeriod = db.Periods.Find(int.Parse(vPeriod)).ID;

            var getAssign = (from commission_profesor in db.CommissionsXProfessors
                             join professor in db.Professors on commission_profesor.ProfessorID equals professor.ID
                             join period in db.Periods on commission_profesor.PeriodID equals period.ID
                             where commission_profesor.CommissionID == pCommissionID & period.ID == vIDPeriod
                             select new { commission_profesor.Hours, commissionProfessorID = commission_profesor.ID,professorID = professor.ID, professor.Name, commission_profesor.HourAllocatedTypeID}).ToList();

            var json = JsonConvert.SerializeObject(getAssign);
            return Content(json);
        }

        /// <summary>
        /// Get the schedulle of a professor in a determinate commission
        /// </summary>
        /// <param name="pCommissionProfessorID"></param>
        /// <returns>Day, StartHour and EndHour in a schedule in commission</returns>
        [Route("Commission/getScheduleProfesor/{pCommissionProfessorID:int}")]
        public ActionResult getScheduleProfesor(int pCommissionProfessorID)
        {
            var json = "";

            var schedule_professor = db.SP_getScheduleCommissionProfessor(pCommissionProfessorID).ToList();

            json = JsonConvert.SerializeObject(schedule_professor);
            return Content(json);
            
        }

        /// <summary>
        /// Check if a schedule with pDay, pStartHour and pEndHour exist, if exist return the value include id from database
        /// else create and return values from database
        /// </summary>
        /// <param name="pDay"></param>
        /// <param name="pStartHour"></param>
        /// <param name="pEndHour"></param>
        /// <returns></returns>
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
        #endregion
    }
}
