using SACAAE.Data_Access;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Web;
using System.Web.Mvc;

namespace SACAAE.Controllers
{
    public class ReporteProfeCursoPlanController : Controller
    {
        private SACAAEContext db = new SACAAEContext();

        [Authorize]
        public ActionResult Index()
        {
            var periodos = db.Periods.Select(p => new
            {
                ID = p.ID,
                Name = (p.Year + " - " + p.Number.Type.Name + " " + p.Number.Number)
            }).ToList();

            ViewBag.Sedes = new SelectList(db.Sedes, "ID", "Name");
            ViewBag.Modalidades = new SelectList(db.Modalities, "ID", "Name");
            ViewBag.Periodos = new SelectList(periodos, "ID", "Name");

            return View();
        }

        // No se usa LOL
        [Authorize]
        public FileResult Download()
        {
            var fi = new FileInfo("myfile.txt");
            byte[] bytes;
            try
            {
                fi.Delete();
            }
            catch (Exception)
            { }
            using (Stream fs = new MemoryStream())
            {
                StreamWriter sw = new StreamWriter(fs);
                sw.WriteLine("Codigo;Nombre;Grupo;Curso Externo;Dia;Hora Inicio;Hora Fin;Cupo;Profesor;Creditos");
                string Periodo = Request.Cookies["Periodo"].Value;
                int idPeriodo = Int16.Parse(Periodo);
                string Plan = Request.Cookies["Plan"].Value;
                int idPlan = Int16.Parse(Periodo);
                PropertyInfo[] properties = obtenerProfeCursoPorPlan(idPlan, idPeriodo).GetType().GetProperties();
                foreach (PropertyInfo item in properties)
                {

                    //                    string HoraInicio = (Dia.Hora_Inicio / 100).ToString() + ":" + (Dia.Hora_Inicio % 100).ToString();
                    //                    string HoraFin = (Dia.Hora_Fin / 100).ToString() + ":" + (Dia.Hora_Fin % 100).ToString();
                    //                    double Carga = 0;
                    //                    if (!item.Curso1.Externo)
                    //                    {
                    //                        Carga = ((item.Curso1.HorasTeoricas * 2) + ((int)item.Curso1.HorasPracticas * 1.75));
                    //                        double CargaCupo = this.CalculoCupo(Convert.ToInt32(Detalle.Cupo), Convert.ToInt32(item.Curso1.HorasTeoricas), Convert.ToInt32(item.Curso1.HorasPracticas));
                    //                        Carga = Carga + CargaCupo;
                    //                    }
                    sw.WriteLine(item.GetValue("Code", null) + ";" +
                                item.GetValue("Name", null) + ";" +
                                item.GetValue("Number", null) + ";" +
                                "si" + ";" +
                                item.GetValue("Classroom", null) + ";" +
                                item.GetValue("Capacity", null) + ";" +
                                item.GetValue("Professor", null));
                }
                //                                item.Grupo1.PlanesDeEstudioXSede.PlanesDeEstudio.Nombre + ";" +
                //                                item.Grupo1.PlanesDeEstudioXSede.PlanesDeEstudio.Modalidade.Nombre + ";" +
                //                                item.Grupo1.PlanesDeEstudioXSede.Sede1.Nombre + ";" +
                //                                item.Curso1.HorasTeoricas + ";" +
                //                                item.Curso1.HorasPracticas + ";" +
                //                                item.Curso1.Externo.ToString() + ";" +
                //                                Carga
                //                                 );
                //                }
                //            }
                //        }

                //        //Proyecto

                //        IQueryable<Proyecto> Proyectos = repoProyecto.ObtenerTodosProyectos();
                //        foreach (Proyecto Proyecto in Proyectos)
                //        {
                //            IQueryable<ProyectosXProfesor> Profesores = repoProyecto.ObtenerProyectoXProfesor(Proyecto.ID);
                //            foreach (ProyectosXProfesor Profe in Profesores)
                //            {
                //                IQueryable<Dia> Dias = repoHorario.ObtenerDias(Profe.Horario);
                //                foreach (Dia Dia in Dias)
                //                {
                //                    string HoraInicio = (Dia.Hora_Inicio / 100).ToString() + ":" + (Dia.Hora_Inicio % 100).ToString();
                //                    string HoraFin = (Dia.Hora_Fin / 100).ToString() + ":" + (Dia.Hora_Fin % 100).ToString();

                //                    sw.WriteLine("Proyecto;N/A;" +
                //                                Proyecto.Nombre + ";" +
                //                                Profe.Profesores.Nombre + ";" +
                //                                Dia.Dia1 + ";" +
                //                                HoraInicio + ";" +
                //                                HoraFin + ";N/A;N/A;N/A;N/A;N/A;N/A;N/A;" +
                //                                (((Dia.Hora_Fin - Dia.Hora_Inicio) / 100) * 3)
                //                                );
                //                }
                //            }
                //        }


                //        //Comisiones

                //        IQueryable<Comisione> Comisiones = repoComisiones.ObtenerTodasComisiones();
                //        foreach (Comisione Comision in Comisiones)
                //        {
                //            IQueryable<ComisionesXProfesor> Profesores = repoComisiones.ObtenerComisionesXProfesor(Comision.ID);
                //            foreach (ComisionesXProfesor Profe in Profesores)
                //            {
                //                IQueryable<Dia> Dias = repoHorario.ObtenerDias(Profe.Horario);
                //                foreach (Dia Dia in Dias)
                //                {
                //                    string HoraInicio = (Dia.Hora_Inicio / 100).ToString() + ":" + (Dia.Hora_Inicio % 100).ToString();
                //                    string HoraFin = (Dia.Hora_Fin / 100).ToString() + ":" + (Dia.Hora_Fin % 100).ToString();

                //                    sw.WriteLine("Comision;N/A;" +
                //                                Comision.Nombre + ";" +
                //                                Profe.Profesores.Nombre + ";" +
                //                                Dia.Dia1 + ";" +
                //                                HoraInicio + ";" +
                //                                HoraFin + ";N/A;N/A;N/A;N/A;N/A;N/A;N/A;" +
                //                                ((Dia.Hora_Fin - Dia.Hora_Inicio) / 100)
                //                                );
                //                }
                //            }
                //        }

                sw.Flush();
                fs.Flush();
                fs.Position = 0;
                bytes = new byte[fs.Length];
                fs.Read(bytes, 0, bytes.Length);
                sw.Close();
                sw.Dispose();

            }
            return File(bytes, System.Net.Mime.MediaTypeNames.Application.Octet, "Reporte.csv");
            //}
        }

        #region Ajax Post
        [Route("ReporteProfeCursoPlan/Plan/{pPlan:int}/Periodo/{pPeriod:int}")]
        public ActionResult ObtenerCursosProfesoresPlan(int pPlan, int pPeriod)
        {
            if (HttpContext.Request.IsAjaxRequest())
            {
                var vInfo = obtenerProfeCursoPorPlan(pPlan, pPeriod);
                return Json(vInfo, JsonRequestBehavior.AllowGet);
            }
            return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        }
        #endregion

        #region Helpers
        private IQueryable obtenerProfeCursoPorPlan(int pPlan, int pPeriod)
        {
            return (from Curso in db.Courses
                   join BloqueXPlanXCursos in db.BlocksXPlansXCourses on Curso.ID equals BloqueXPlanXCursos.CourseID
                   join BloquesXPlan in db.AcademicBlocksXStudyPlans on BloqueXPlanXCursos.BlockXPlanID equals BloquesXPlan.ID
                   join grupo in db.Groups on BloqueXPlanXCursos.ID equals grupo.BlockXPlanXCourseID
                   join grupoAula in db.GroupClassrooms on grupo.ID equals grupoAula.GroupID
                   join aula in db.Classrooms on grupoAula.ClassroomID equals aula.ID
                   where BloquesXPlan.PlanID == pPlan && grupo.PeriodID == pPeriod
                   select new { Code = Curso.Code,
                                Name = Curso.Name,
                                External = Curso.External,
                                GrupoID = grupo.ID,
                                Number = grupo.Number,
                                Classroom = aula.Code,
                                Capacity = grupo.Capacity,
                                Professor = grupo.Professor.Name,
                                Credits = Curso.Credits })
                .Distinct();
        }
        #endregion
    }
}