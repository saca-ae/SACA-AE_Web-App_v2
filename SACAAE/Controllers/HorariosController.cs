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
using Newtonsoft.Json;
using SACAAE.Models.ViewModels;
using SACAAE.Helpers;

namespace SACAAE.Controllers
{
    public class HorariosController : Controller
    {
        private SACAAEContext db = new SACAAEContext();
        private const string TempDataMessageKeySuccess = "MessageSuccess";
        private const string TempDataMessageKeyError = "MessageError";
        // GET: /Horario/
        public ActionResult Index()
        {
            ViewBag.Modalidades = db.Modalities.ToList();
            ViewBag.Sedes = db.Sedes.ToList();
            ViewBag.Periodos = db.Periods.ToList();
            return View();
        }

        public ActionResult Horarios()
        {
            String PlanDeEstudio;
            String Modalidad;
            String Periodo;

            try
            {
                PlanDeEstudio = Request.Cookies["SelPlanDeEstudio"].Value;
                Modalidad = Request.Cookies["SelModalidad"].Value;
                Periodo = Request.Cookies["PeriodoHorario"].Value;
            }
            catch (Exception e)
            {
                throw new ArgumentException("No se detecto ningun Plan de Estudio" + e.Message);
            }

            int IdPlanDeEstudio = Int32.Parse(PlanDeEstudio);
            int IdPeriodo = Int32.Parse(Periodo);
            List<String> Dias = new List<String>();
            Dias.Add("Lunes");
            Dias.Add("Martes");
            Dias.Add("Miercoles");
            Dias.Add("Jueves");
            Dias.Add("Viernes");
            Dias.Add("Sabado");
            Dias.Add("Domingo");
            ViewBag.Dias = Dias;

            List<String> HorasInicio = new List<String>();

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

            List<String> Horas = new List<String>();
            for (int i = 0; i < 24; i++)
            {
                if (i < 10)
                    Horas.Add("0" + i.ToString());
                else
                    Horas.Add(i.ToString());
            }
            ViewBag.Horas = Horas;

            List<String> Minutos = new List<String>();
            for (int i = 0; i < 60; i += 10)
            {
                if (i < 10)
                    Minutos.Add("0" + i.ToString());
                else
                    Minutos.Add(i.ToString());
            }
            int idSede = Int16.Parse(Request.Cookies["SelSede"].Value);
            ViewBag.Minutos = Minutos;
            ViewBag.Bloques = ListarBloquesXPlan(IdPlanDeEstudio);
            ViewBag.Aulas = ListarAulasXSedeCompleta(idSede);
            int idPlanXSede = IdPlanDeEstudioXSede(idSede, IdPlanDeEstudio);
            return View();
        }


        // POST: Horario/Horarios
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Horarios(NewScheduleViewModel pNewSchedule)
        {
            //*****************************************************************************************************
            //******************************* Se obtienen los datos del formulario ********************************
            //*****************************************************************************************************
            int vGroupID = Convert.ToInt32(pNewSchedule.Group);

            // 1 Find the group in the database
            Group vGroup = db.Groups.Find(vGroupID);

            // 2.1 Remove GroupClassroom fields asociated with the Group
            removeGroupClassroomByGroupID(vGroupID);

            // 2.2 new GroupClassrooms asociated with Group
            //Get the list of schedules and classrooms from the View
            List<NewSchedule> vNewSchedule = pNewSchedule.NewSchedule;

            bool vIsValidateClassroom = true;// = isInternScheduleValid(vNewSchedule);

            foreach (NewSchedule tempSchedule in vNewSchedule)
            {
                Schedule vTempSchedule = existSchedule(tempSchedule.Day, tempSchedule.StartHour, tempSchedule.EndHour);
                int vClassroomID = Convert.ToInt32(tempSchedule.Classroom);

                if (isValidScheduleClassroom(vTempSchedule, vClassroomID, vGroupID))
                {
                    GroupClassroom vNewGroupClassroom = new GroupClassroom();

                    vNewGroupClassroom.ClassroomID = Convert.ToInt32(tempSchedule.Classroom);

                    vNewGroupClassroom.ScheduleID = vTempSchedule.ID;

                    vNewGroupClassroom.GroupID = vGroup.ID;

                    vGroup.GroupsClassroom.Add(vNewGroupClassroom);

                }
                else
                {
                    vIsValidateClassroom = false;
                }
            }
            if (vIsValidateClassroom)
            {

                if (vGroup.ProfessorID != null)
                {
                    var vProfessorID = (int)vGroup.ProfessorID;
                    var vPeriod = Request.Cookies["Periodo"].Value;
                    var vPeriodID = db.Periods.Find(int.Parse(vPeriod)).ID;
                    ScheduleHelper dbHelper = new ScheduleHelper();

                    string validate = dbHelper.validationsEditGroup(vProfessorID, vGroupID, vPeriodID);

                    if (validate.Equals("true"))
                    {
                        db.SaveChanges();

                        TempData[TempDataMessageKeySuccess] = "Cambios guardados satisfactoriamente";
                        return RedirectToAction("Index");
                    }
                    // Exist problems in profesor schedule, so the assign is cancelled and the user recive the information of the problem
                    else if (validate.Equals("falseIsGroupShock"))
                    {
                        TempData[TempDataMessageKeyError] = "Existe choque de horario con el profesor asignado al grupo, no se realizaron los cambios";
                        return RedirectToAction("Index");
                    }
                    else if (validate.Equals("falseIsProjectShock"))
                    {
                        TempData[TempDataMessageKeyError] = "Existe choque de horario con el profesor asignado al grupo, no se realizaron los cambios";
                        return RedirectToAction("Index");
                    }

                    else if (validate.Equals("falseIsCommissionShock"))
                    {
                        TempData[TempDataMessageKeyError] = "Existe choque de horario con el profesor asignado al grupo, no se realizaron los cambios";
                        return RedirectToAction("Index");
                    }
                }
                else
                {
                    db.SaveChanges();

                    TempData[TempDataMessageKeySuccess] = "Cambios guardados satisfactoriamente";
                    return RedirectToAction("Index");
                }


            }
            else
            {
                TempData[TempDataMessageKeyError] = "Existe choque de horario con la asignacion del aula";
                return RedirectToAction("Index");
            }

            return RedirectToAction("Index");

        }

        public ActionResult Resultado()
        {
            return View();
        }

        #region Ajax

        [Route("Horarios/Plan/{pPlanID:int}/Bloques")]
        public ActionResult ListarBloquesXPlan(int pPlanID)
        {
            var ListaBloquexPlan = from Bloques in db.AcademicBlocks
                                   join BloquesXPlan in db.AcademicBlocksXStudyPlans on Bloques.ID equals BloquesXPlan.BlockID
                                   where BloquesXPlan.PlanID == pPlanID
                                   select new { Bloques.ID, Bloques.Description };
            var json = JsonConvert.SerializeObject(ListaBloquexPlan);
            return Content(json);
        }

        [Route("Horarios/Sedes/{pSedeID:int}/Aulas")]
        public ActionResult ListarAulasXSedeCompleta(int pSedeID)
        {
            var ListaAulasXSede = from Aulas in db.Classrooms
                                  where Aulas.SedeID == pSedeID
                                  select new { Aulas.ID, Aulas.Code };

            var json = JsonConvert.SerializeObject(ListaAulasXSede);
            return Content(json);
        }

        [Route("Horarios/Grupo/{pGroupID:int}")]
        public ActionResult getScheduleGroup(int pGroupID)
        {
            var vScheduleGroup = (from gc in db.GroupClassrooms
                                  join s in db.Schedules on gc.ScheduleID equals s.ID
                                  join c in db.Classrooms on gc.ClassroomID equals c.ID
                                  where gc.GroupID == pGroupID
                                  select new { gc.ID, s.Day, s.StartHour, s.EndHour, ClassroomID = c.ID, c.Code }).ToList();

            var json = JsonConvert.SerializeObject(vScheduleGroup);
            return Content(json);


        }
        #endregion
        #region Helpers

        public bool removeGroupClassroomByGroupID(int pGroupID)
        {
            var list = (from gc in db.GroupClassrooms
                        where gc.GroupID == pGroupID
                        select gc).ToList();

            foreach (var element in list)
            {
                db.GroupClassrooms.Remove(element);

            }
            return true;
        }
        public int IdPlanDeEstudioXSede(int sede, int plan)
        {
            return (from planXSede in db.StudyPlansXSedes
                    where planXSede.SedeID == sede && planXSede.StudyPlanID == plan
                    select planXSede).FirstOrDefault().ID;
        }


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
                vNewSchedule.GroupsClassroom = new List<GroupClassroom>();

                db.Schedules.Add(vNewSchedule);

                return vNewSchedule;
            }
        }

        public bool isValidScheduleClassroom(Schedule pNewSchedule, int pClassroomID, int pGroupID)
        {
            var vPeriod = Request.Cookies["Periodo"].Value;
            var vPeriodID = db.Periods.Find(int.Parse(vPeriod)).ID;

            var vSchedule = (from schedule in db.Schedules
                             join groupclass in db.GroupClassrooms on schedule.ID equals groupclass.ScheduleID
                             join g in db.Groups on groupclass.GroupID equals g.ID
                             where (groupclass.GroupID != pGroupID && groupclass.ClassroomID == pClassroomID &&
                                 schedule.Day == pNewSchedule.Day && g.PeriodID == vPeriodID)
                             select schedule).ToList();

            foreach (Schedule tempSchedule in vSchedule)
            {
                DateTime vTempScheduleStartHour = DateTime.Parse(tempSchedule.StartHour);
                DateTime vTempScheduleEndHour = DateTime.Parse(tempSchedule.EndHour);
                DateTime vNewScheduleStartHour = DateTime.Parse(pNewSchedule.StartHour);
                DateTime vNewScheduleEndHour = DateTime.Parse(pNewSchedule.EndHour);

                if ((vTempScheduleStartHour <= vNewScheduleStartHour && vNewScheduleStartHour <= vTempScheduleEndHour) ||
                    (vTempScheduleStartHour <= vNewScheduleEndHour && vNewScheduleEndHour <= vTempScheduleEndHour) ||
                    (vNewScheduleStartHour <= vTempScheduleStartHour && vTempScheduleStartHour <= vNewScheduleEndHour) ||
                    (vNewScheduleStartHour <= vTempScheduleEndHour && vTempScheduleEndHour <= vNewScheduleEndHour))
                {
                    return false;
                }
            }

            return true;
        }

        #endregion
    }



}
