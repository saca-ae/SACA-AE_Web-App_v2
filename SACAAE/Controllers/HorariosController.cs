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

namespace SACAAE.Controllers
{
    public class HorariosController : Controller
    {
        private SACAAEContext db = new SACAAEContext();

        // GET: /Horario/
        public ActionResult Index()
        {
            ViewBag.Modalidades = db.Modalities.ToList();
            ViewBag.Sedes = db.Sedes.ToList();
            ViewBag.Periodos = db.Periods.ToList();
            return View();
        }

        // POST /Horario/
        /*[HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Index()
        {
            return RedirectToAction("Horarios");
        }*/

        
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
        public ActionResult Horarios(NewScheduleViewModel vNewSchedule)
        {
            //*****************************************************************************************************
            //******************************* Se obtienen los datos del formulario ********************************
            //*****************************************************************************************************
            /*int Cantidad;
            int PlanDeEstudio;
            try
            {
                 Cantidad = Convert.ToInt32(Request.Cookies["Cantidad"].Value);
            }
            catch (Exception e)
            {
                Cantidad = 0;
            }

            try
           {
                PlanDeEstudio = Int32.Parse(Request.Cookies["SelPlanDeEstudio"].Value);
            }
            catch (Exception e)
            {
                throw new ArgumentException("No se detecto ningun Grupo o Plan de Estudio" + e.Message);
            }
            //*****************************************************************************************************

            //En caso de que el horario este vacio se asume que se desea borrar por lo que se limpian los dias y se termina, este return evita que falle el programa cuando no hay cookies
            if (Cantidad == 0) 
            { 
                return RedirectToAction("Horarios"); 
             }
            //Eliminar Horarios Viejos
            //for (int i = 1; i <= Cantidad; i++)
            //{
            //   String Detalles= Request.Cookies["Cookie" + i].Value;
            //   string[] Partes = Detalles.Split('|');
            //   int Grupo = Int32.Parse(Partes[5]);
            //    int IdHorario = IdHorarioCurso(Grupo);
            //    if(IdHorario!=0)
            //    {
            //        Horario.EliminarDias(IdHorario);
            //    }
            //}

            //Guardar Datos
            for (int i = 1; i <= Cantidad; i++)
            {
                String Detalles = Request.Cookies["Cookie" + i].Value;//Obtiene los datos de la cookie
                string[] Partes = Detalles.Split('|');
                String Curso = Partes[0];
                String Dia = Partes[1];
                String HoraInicio = Partes[2];
                String HoraFin = Partes[3];
                String Bloque = Partes[4];
                int Grupo = Int32.Parse(Partes[5]);
                String Aula = Partes[6];
                if (Curso != "d")
                {
                    //Curso segun el ID y el Plan de Estudio
                    int IdCurso = IdCursos(Curso, PlanDeEstudio);

                    //Se obtiene el id del horario del grupo
                    int IdHorario = IdHorarioCurso(Grupo);

                    //Si existe Que hace aqui????
                    //if (IdHorario != 0)
                    //{
                    //    Horario.AgregarDia(Dia, IdHorario, Convert.ToInt32(HoraInicio), Convert.ToInt32(HoraFin));
                    //}

                    //En caso de que no exista se crea el nuevo horario y se retorna el id
                    //else
                    //{
                    if (IdHorario == null)
                    {
                        //Se agrega el horario nuevo
                        IdHorario = NuevoHorario(Dia, HoraInicio, HoraFin);
                    }
                    //Horario.AgregarDia(Dia, HorarioNuevo, Convert.ToInt32(HoraInicio), Convert.ToInt32(HoraFin));

                    //Se retorna el id del Aula de acuerdo al codigo Porque no se busca por id y se valida si es valido o no???
                    int vIDAula = idAula(Aula);

                    //Se obtiene el cupo del aula INNECESARIO
                    //int cupo = repoAulas.ObtenerAula(idAula).Espacio;

                    //Se almacena la relacion
                    GroupClassroom vNewGroupClassroom = new GroupClassroom();
                    vNewGroupClassroom.ClassroomID = vIDAula;
                    vNewGroupClassroom.ScheduleID = IdHorario;
                    vNewGroupClassroom.GroupID = Grupo;
                    //GroupClassroom(GroupID,ClassroomID, ScheduleID)
                    //Cursos.GuardarDetallesCurso(Grupo, HorarioNuevo, Aula, 5, cupo);
                    // }
                    //}

                }
            }
            Response.Cookies.Clear();
            TempData["Message"] = "Cambios guardados satisfactoriamente";*/
            return RedirectToAction("Horarios");
        }
            
        /*public ActionResult ObtenerHorarios(int plan, int periodo)
        {
            int idSede = Int16.Parse(Request.Cookies["SelSede"].Value);
            int idPlanXSede = repoPlanes.IdPlanDeEstudioXSede(idSede, plan);
            IQueryable listaHorarios = Horario.obtenerInfo(idPlanXSede, periodo);
                var json = JsonConvert.SerializeObject(listaHorarios);

                return Content(json);
        }*/

       /* public ActionResult ExisteHorario(string dia,int HoraInicio, int HoraFin, string aula, int grupo, int periodo)
        {
            int res = ExisteHorarioHelper(dia, HoraInicio, HoraFin, aula, grupo, periodo);
            return Json(res, JsonRequestBehavior.AllowGet);
        }*/
        

        public ActionResult Resultado()
        {
            return View();
        }

        #region Ajax
        [Route("Horarios/Plan/{pPlanID:int}/Bloques")]
        public ActionResult ListarBloquesXPlan(int pPlanID)
        {
            var ListaBloquexPlan =  from Bloques in db.AcademicBlocks
                   join BloquesXPlan in db.AcademicBlocksXStudyPlans on Bloques.ID equals BloquesXPlan.BlockID
                   where BloquesXPlan.PlanID == pPlanID
                   select new{Bloques.ID, Bloques.Description};
            var json = JsonConvert.SerializeObject(ListaBloquexPlan);
            return Content(json);
        }
        #endregion
        #region Helpers
        
        [Route("Horarios/Sedes/{pSedeID:int}/Aulas")]
        public ActionResult ListarAulasXSedeCompleta(int pSedeID)
        {
            var ListaAulasXSede =  from Aulas in db.Classrooms
                   where Aulas.SedeID == pSedeID
                   select new{Aulas.ID, Aulas.Code};

            var json = JsonConvert.SerializeObject(ListaAulasXSede);
            return Content(json);
        }


        public int IdPlanDeEstudioXSede(int sede, int plan)
        {
            return (from planXSede in db.StudyPlansXSedes
                    where planXSede.SedeID == sede && planXSede.StudyPlanID == plan
                    select planXSede).FirstOrDefault().ID;
        }
       /* public int ExisteHorarioHelper(string dia, int HoraInicio, int HoraFin, string aula, int grupo, int periodo)
        {
            var vDetalleGrupo = from Horario in db.Schedules
                                join Grupos in db.Groups on DetalleGrupo.Grupo equals Grupos.ID
                                where Dia.Dia1 == dia && (Dia.Hora_Inicio <= HoraInicio && Dia.Hora_Fin >= HoraFin) && Grupos.Periodo == periodo && DetalleGrupo.Aula == aula || DetalleGrupo.Grupo == grupo
                                select DetalleGrupo;
            if (vDetalleGrupo.Any())
                return 1;
            else
                return 0;
        }*/
        public int IdCursos(string CursoBuscado, int PlanDeEstudioCurso)
        {

            IQueryable<Course> Resultado =
                from Curso in db.Courses
                join BloqueXPlanXCursos in db.BlocksXPlansXCourses on Curso.ID equals BloqueXPlanXCursos.CourseID
                join BloquesXPlan in db.AcademicBlocksXStudyPlans on BloqueXPlanXCursos.BlockXPlanID equals BloquesXPlan.ID
                where (Curso.Name == CursoBuscado && BloquesXPlan.PlanID == PlanDeEstudioCurso)
                select Curso;

            try
            {
                return Resultado.FirstOrDefault().ID;
            }
            catch (Exception e)
            {
                return -1;
            }
        }

        public int IdHorarioCurso(int grupo)
        {
            IQueryable<GroupClassroom> Resultado =
                from Detalle_Grupos in db.Groups
                 join Grupo_Classroom in db.GroupClassrooms on Detalle_Grupos.ID equals Grupo_Classroom.GroupID
                where Grupo_Classroom.GroupID == grupo
                select Grupo_Classroom;
            GroupClassroom res = Resultado.FirstOrDefault();
            if (res == null)
                return 0;
            return res.Schedule.ID;
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
                vNewSchedule.CommissionsXProfessors = new List<CommissionXProfessor>();

                db.Schedules.Add(vNewSchedule);
                //db.SaveChanges();

                //vSchedule = db.Schedules.Where(p => p.Day == pDay && p.StartHour == pStartHour && p.EndHour == pEndHour).FirstOrDefault();

                //db.SaveChanges();
                return vNewSchedule;
            }
            //select * from Schedule where Day='Domingo' AND StartHour = '07:30 am' AND EndHour = '09:20 am'
        }

        public int NuevoHorario(string pDay, string pStartHour, string pEndHour)
        {
            //Create schedule and get id
            Schedule vNewSchedule = new Schedule();
            vNewSchedule.Day = pDay;
            vNewSchedule.StartHour = pStartHour;
            vNewSchedule.EndHour = pEndHour;

            db.Schedules.Add(vNewSchedule);
            db.SaveChanges();

            return 1;
        }

        
        public int idAula(string pCodigoAula)
        {
            return (from Aulas in db.Classrooms
                    where Aulas.Code == pCodigoAula
                    select Aulas).FirstOrDefault().ID;
        }
        #endregion
    }

    

}
