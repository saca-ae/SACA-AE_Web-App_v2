using SACAAE.Data_Access;
using SACAAE.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace SACAAE.Controllers
{
    public class ReportesController : Controller
    {
        private SACAAEContext db = new SACAAEContext();
        //private repositorioGrupos repoGrupos = new repositorioGrupos();
        //private RepositorioCursos repoCursos = new RepositorioCursos();
        //private RepositorioHorario repoHorario = new RepositorioHorario();
        //private RepositorioProyecto repoProyecto = new RepositorioProyecto();
        //private RepositorioComision repoComisiones = new RepositorioComision();
        //private RepositorioPeriodos repoPeriodos = new RepositorioPeriodos();
        //private RepositorioBloqueXPlanXCurso repoBloqueXPlanXCurso = new RepositorioBloqueXPlanXCurso();

        // ???
        public byte[] StrToByteArray(string str)
        {
            UTF8Encoding encoding = new UTF8Encoding();
            return encoding.GetBytes(str);
        }
        // ???
        public string ByteArrayToString(byte[] ba)
        {
            StringBuilder hex = new StringBuilder(ba.Length * 2);
            foreach (byte b in ba)
                hex.AppendFormat("{0:x2}", b);
            return hex.ToString();
        }

        public FileResult Download()
        {
            var fi = new FileInfo("myfile.txt");
            byte[] bytes;

            //Dictionary<string, int> profesores_carga_tec = new Dictionary<string, int>();
            //Dictionary<string, int> profesores_carga_fundatec = new Dictionary<string, int>();
            //Dictionary<string, List<string>> profesores_cursos_asociados = new Dictionary<string, List<string>>();
            //List<Profesor> todo_profesores = new List<Profesor>();

            ReporteInfo vReportInfo = new ReporteInfo();

            try
            {
                fi.Delete();
            }
            catch (Exception)
            { }

            using (Stream fs = new MemoryStream())
            {
                StreamWriter sw = new StreamWriter(fs, Encoding.UTF8);
                sw.WriteLine("Tipo;Grupo;Nombre;Profesor;Dia;Hora Inicio;Hora Fin;Sede;Aula;Cupo;Plan de Estudio;Modalidad; Horas Teoricas; Horas Practicas; Externo;Carga Estimada;Entidad");
                var IdPeriodo = int.Parse(Request.Cookies["Periodo"].Value);

                //Cursos
                vReportInfo = setCourses(vReportInfo);
                //Proyectos
                vReportInfo = setProjects(vReportInfo);
                //Comisiones
                vReportInfo = setCommissions(vReportInfo);

                Profesor[] array_profesores = vReportInfo.todo_profesores.ToArray();
                Array.Sort(array_profesores, delegate (Profesor user1, Profesor user2)
                {
                    return user1.Profesor_Nombre.CompareTo(user2.Profesor_Nombre);
                });
                string profe_actual = "";
                foreach (Profesor profe in array_profesores)
                {
                    if (profe_actual.Equals(""))
                    {
                        profe_actual = profe.Profesor_Nombre;
                    }


                    if (profe_actual.Equals(profe.Profesor_Nombre))
                    {
                        sw.WriteLine(profe.toStr());
                    }
                    else
                    {
                        if (vReportInfo.profesores_carga_tec.ContainsKey(profe_actual) && vReportInfo.profesores_carga_fundatec.ContainsKey(profe_actual))
                        {
                            sw.WriteLine("SUBTOTAL CARGA TEC;;;" + profe_actual + ";;;;;;;;;;;;" + vReportInfo.profesores_carga_tec[profe_actual]);
                            sw.WriteLine("SUBTOTAL CARGA FUNDATEC;;;" + profe_actual + ";;;;;;;;;;;;" + vReportInfo.profesores_carga_fundatec[profe_actual]);
                            sw.WriteLine("TOTAL CARGA;;;" + profe_actual + ";;;;;;;;;;;;" + (vReportInfo.profesores_carga_tec[profe_actual] + vReportInfo.profesores_carga_fundatec[profe_actual]));

                        }
                        else if (!vReportInfo.profesores_carga_tec.ContainsKey(profe_actual) && vReportInfo.profesores_carga_fundatec.ContainsKey(profe_actual))
                        {
                            sw.WriteLine("SUBTOTAL CARGA TEC;;;" + profe_actual + ";;;;;;;;;;;;" + "0");
                            sw.WriteLine("SUBTOTAL CARGA FUNDATEC;;;" + profe_actual + ";;;;;;;;;;;;" + vReportInfo.profesores_carga_fundatec[profe_actual]);
                            sw.WriteLine("TOTAL CARGA;;;" + profe_actual + ";;;;;;;;;;;;" + vReportInfo.profesores_carga_fundatec[profe_actual]);

                        }
                        else if (vReportInfo.profesores_carga_tec.ContainsKey(profe_actual) && !vReportInfo.profesores_carga_fundatec.ContainsKey(profe_actual))
                        {
                            sw.WriteLine("SUBTOTAL CARGA TEC;;;" + profe_actual + ";;;;;;;;;;;;" + vReportInfo.profesores_carga_tec[profe_actual]);
                            sw.WriteLine("SUBTOTAL CARGA FUNDATEC;;;" + profe_actual + ";;;;;;;;;;;;" + 0);
                            sw.WriteLine("TOTAL CARGA;;;" + profe_actual + ";;;;;;;;;;;;" + vReportInfo.profesores_carga_tec[profe_actual]);

                        }


                        profe_actual = profe.Profesor_Nombre;
                        sw.WriteLine(profe.toStr());
                    }
                }
                // el ciclo se termina con el ultimo profe, por eso hay q hacer un llamado al final
                if (vReportInfo.profesores_carga_tec.ContainsKey(profe_actual) && vReportInfo.profesores_carga_fundatec.ContainsKey(profe_actual))
                {
                    sw.WriteLine("SUBTOTAL CARGA TEC;;;" + profe_actual + ";;;;;;;;;;;" + vReportInfo.profesores_carga_tec[profe_actual]);
                    sw.WriteLine("SUBTOTAL CARGA FUNDATEC;;;" + profe_actual + ";;;;;;;;;;;" + vReportInfo.profesores_carga_fundatec[profe_actual]);
                    sw.WriteLine("TOTAL CARGA;;;" + profe_actual + ";;;;;;;;;;;" + (vReportInfo.profesores_carga_tec[profe_actual] + vReportInfo.profesores_carga_fundatec[profe_actual]));

                }
                else if (!vReportInfo.profesores_carga_tec.ContainsKey(profe_actual) && vReportInfo.profesores_carga_fundatec.ContainsKey(profe_actual))
                {
                    sw.WriteLine("SUBTOTAL CARGA TEC;;;" + profe_actual + ";;;;;;;;;;;" + "0");
                    sw.WriteLine("SUBTOTAL CARGA FUNDATEC;;;" + profe_actual + ";;;;;;;;;;;" + vReportInfo.profesores_carga_fundatec[profe_actual]);
                    sw.WriteLine("TOTAL CARGA;;;" + profe_actual + ";;;;;;;;;;;" + vReportInfo.profesores_carga_fundatec[profe_actual]);

                }
                else if (vReportInfo.profesores_carga_tec.ContainsKey(profe_actual) && !vReportInfo.profesores_carga_fundatec.ContainsKey(profe_actual))
                {
                    sw.WriteLine("SUBTOTAL CARGA TEC;;;" + profe_actual + ";;;;;;;;;;;" + vReportInfo.profesores_carga_tec[profe_actual]);
                    sw.WriteLine("SUBTOTAL CARGA FUNDATEC;;;" + profe_actual + ";;;;;;;;;;;" + 0);
                    sw.WriteLine("TOTAL CARGA;;;" + profe_actual + ";;;;;;;;;;;" + vReportInfo.profesores_carga_tec[profe_actual]);

                }


                /*
                sw.WriteLine("");
                sw.WriteLine("");
                sw.WriteLine("Profesor;Carga Total;");
                foreach (var entry in profesores)
                {
                    sw.WriteLine(entry.Key + ";" + entry.Value);
                }*/

                sw.Flush();
                fs.Flush();
                fs.Position = 0;
                bytes = new byte[fs.Length];



                fs.Read(bytes, 0, bytes.Length);
                sw.Close();
                sw.Dispose();
            }


            return File(bytes, System.Net.Mime.MediaTypeNames.Application.Octet, "Reporte.csv");
        }

        private ReporteInfo setCourses(ReporteInfo pReportInfo)
        {
            var vPeriodID = int.Parse(Request.Cookies["Periodo"].Value);
            var entidad_temp = "";

            var vGroups = db.Groups.Where(p => p.PeriodID == vPeriodID).OrderBy(p => p.Number).ToList();
            foreach (Group vGroup in vGroups)
            {
                var vDetail = db.GroupClassrooms.Where(p => p.GroupID == vGroup.ID).ToList();

                foreach (var vSchedule in vDetail)
                {

                    string HoraInicio = vSchedule.Schedule.StartHour;
                    string HoraFin = vSchedule.Schedule.EndHour;
                    int Carga = 0;
                    var vCourseInfo = db.BlocksXPlansXCourses.Single(p => p.ID == vGroup.BlockXPlanXCourseID).Course;

                    //if (!CursoInfo.Externo)
                    //{

  //HERE            //Carga = (int)vDetail.ProfesoresXCurso.Horas;    //////////////////////////////////////// Here ////////////////////////////////////////

                    /*Carga = ((CursoInfo.HorasTeoricas * 2) + ((int)CursoInfo.HorasPracticas * 1.75));
                    double CargaCupo = this.CalculoCupo(Convert.ToInt32(Detalle.Cupo), Convert.ToInt32(CursoInfo.HorasTeoricas), Convert.ToInt32(CursoInfo.HorasPracticas));
                    Carga = Carga + CargaCupo;*/
                    //}

                    /*sw.WriteLine("Curso;" +
                                item.Numero + ";" +
                                CursoInfo.Nombre + ";" +
                                Detalle.ProfesoresXCurso.Profesore.Nombre + ";" +
                                Dia.Dia1 + ";" +
                                HoraInicio + ";" +
                                HoraFin + ";" +
                                Detalle.Cupo + ";" +
                                item.BloqueXPlanXCurso.BloqueAcademicoXPlanDeEstudio.PlanesDeEstudio.Nombre + ";" +
                                item.BloqueXPlanXCurso.BloqueAcademicoXPlanDeEstudio.PlanesDeEstudio.Modalidade.Nombre + ";" +
                                item.PlanesDeEstudioXSede.Sede1.Nombre + ";" +
                                item.BloqueXPlanXCurso.Curso.HorasTeoricas + ";" +
                                item.BloqueXPlanXCurso.Curso.HorasPracticas + ";" +
                                item.BloqueXPlanXCurso.Curso.Externo.ToString() + ";" +
                                Carga
                                    );*/
                    entidad_temp = vGroup.BlockXPlanXCourse.AcademicBlockXStudyPlan.StudyPlan.EntityType.Name;
                    pReportInfo.todo_profesores.Add(new Profesor
                    {

                        Tipo = "Carga docente",
                        Grupo = vGroup.Number + "",
                        Nombre = vCourseInfo.Name,
                        Profesor_Nombre = vGroup.Professor.Name,
                        Dia = vSchedule.Schedule.Day,
                        HoraInicio = HoraInicio,
                        HoraFin = HoraFin,
                        Cupo = vGroup.Capacity + "",
                        PlanEstudio = vGroup.BlockXPlanXCourse.AcademicBlockXStudyPlan.StudyPlan.Name,
                        Modalidad = vGroup.BlockXPlanXCourse.AcademicBlockXStudyPlan.StudyPlan.Modality.Name,
                        Sede = vSchedule.Classroom.Sede.Name,
                        HoraTeoricas = vGroup.BlockXPlanXCourse.Course.TheoreticalHours + "",
                        HoraPractica = vGroup.BlockXPlanXCourse.Course.PracticeHours + "",
                        CargaEstimada = Carga + "",
                        Aula = vSchedule.Classroom.Code,
                        Entidad = entidad_temp
                    });
                    List<string> vProfessorEntity;
                    // ES UNA ENTIDAD TEC, POR LO QUE VA EN OTRO TOTAL
                    if (entidad_temp.Equals("TEC") || entidad_temp.Equals("TEC-VIC") || entidad_temp.Equals("TEC-REC") ||
                    entidad_temp.Equals("TEC-MIXTO") || entidad_temp.Equals("TEC-Académico") || entidad_temp.Equals("CIADEG "))
                    {
                        if (entidad_temp.Equals("TEC-REC"))
                        {
                            // Se omite este, dado que son horas voluntarias
                        }
                        else
                        {

                            if (pReportInfo.profesores_carga_tec.ContainsKey(vGroup.Professor.Name))
                            {
                                vProfessorEntity = pReportInfo.profesores_cursos_asociados[vGroup.Professor.Name];   // get professor's courses
                                if (!vProfessorEntity.Contains(vCourseInfo.Name))               // omit duplicate counting
                                {
                                    vProfessorEntity.Add(vCourseInfo.Name);
                                    pReportInfo.profesores_cursos_asociados[vGroup.Professor.Name] = vProfessorEntity;
                                    pReportInfo.profesores_carga_tec[vGroup.Professor.Name] += Carga;
                                }
                            }
                            else
                            {
                                pReportInfo.profesores_carga_tec.Add(vGroup.Professor.Name, Carga);
                                // se pregunta si el curso, comision o proyecto ha sido contabilizado
                                if (pReportInfo.profesores_cursos_asociados.ContainsKey(vGroup.Professor.Name))
                                {
                                    vProfessorEntity = pReportInfo.profesores_cursos_asociados[vGroup.Professor.Name];
                                    vProfessorEntity.Add(vCourseInfo.Name);


                                    pReportInfo.profesores_cursos_asociados[vGroup.Professor.Name] = vProfessorEntity;
                                }
                                else
                                {
                                    pReportInfo.profesores_cursos_asociados.Add(vGroup.Professor.Name, new List<String>() { vCourseInfo.Name });
                                }
                            }
                        }
                    }
                    else
                    {
                        if (pReportInfo.profesores_carga_fundatec.ContainsKey(vGroup.Professor.Name))
                        {
                            vProfessorEntity = pReportInfo.profesores_cursos_asociados[vGroup.Professor.Name];  // get professor's courses
                            if (!vProfessorEntity.Contains(vCourseInfo.Name))                                   // omit duplicate counting
                            {
                                vProfessorEntity.Add(vCourseInfo.Name);
                                pReportInfo.profesores_cursos_asociados[vGroup.Professor.Name] = vProfessorEntity;
                                pReportInfo.profesores_carga_fundatec[vGroup.Professor.Name] += Carga;
                            }
                        }
                        else
                        {
                            pReportInfo.profesores_carga_fundatec.Add(vGroup.Professor.Name, Carga);
                            // se pregunta si el curso, comision o proyecto ha sido contabilizado
                            if (pReportInfo.profesores_cursos_asociados.ContainsKey(vGroup.Professor.Name))
                            {
                                vProfessorEntity = pReportInfo.profesores_cursos_asociados[vGroup.Professor.Name];
                                vProfessorEntity.Add(vCourseInfo.Name);


                                pReportInfo.profesores_cursos_asociados[vGroup.Professor.Name] = vProfessorEntity;
                            }
                            else
                            {
                                pReportInfo.profesores_cursos_asociados.Add(vGroup.Professor.Name, new List<String>() { vCourseInfo.Name });
                            }
                        }
                    }
                }

            }
            return pReportInfo;
        }

        private ReporteInfo setProjects(ReporteInfo pReportInfo)
        {
            var entidad_temp = "";

            var vProjects = db.Projects.ToList();
            foreach (var vProject in vProjects)
            {
                var vProfessors = db.ProjectsXProfessors.Where(p => p.ProjectID == vProject.ID).ToList();
                foreach (var vProfe in vProfessors)
                {
                    var vSchedule = vProfe.Schedule.ToList();
                    foreach (var vDay in vSchedule)
                    {
                        string HoraInicio = vDay.StartHour;
                        string HoraFin = vDay.EndHour;
                        ///////////////////////////////////////////////////////////////////
                        double CargaC = 0;// (double)(Day.Hora_Fin - Day.Hora_Inicio) / 100;
                        //if (Day.Hora_Inicio <= 1200 && Day.Hora_Fin >= 1300)
                        //{
                        //    CargaC = CargaC - 1;
                        //}

                        /*sw.WriteLine("Proyecto;N/A;" +
                                    Proyecto.Nombre + ";" +
                                    Profe.Profesore.Nombre + ";" +
                                    Dia.Dia1 + ";" +
                                    HoraInicio + ";" +
                                    HoraFin + ";N/A;N/A;N/A;N/A;N/A;N/A;N/A;" +
                                    Math.Ceiling(CargaC)
                                    );*/
                        entidad_temp = vProfe.Project.EntityType.Name;
                        pReportInfo.todo_profesores.Add(new Profesor
                        {
                            Tipo = "Carga Investigación Extensión",
                            Grupo = "N/A",
                            Nombre = vProject.Name,
                            Profesor_Nombre = vProfe.Professor.Name,
                            Dia = vDay.Day,
                            HoraInicio = HoraInicio,
                            HoraFin = HoraFin,
                            Cupo = "N/A",
                            PlanEstudio = "N/A",
                            Modalidad = "N/A",
                            Sede = "N/A",
                            HoraTeoricas = "N/A",
                            HoraPractica = "N/A",
                            CargaEstimada = CargaC + "",
                            Aula = "N/A",
                            Entidad = entidad_temp
                        });


                        List<string> entidadesXProfesor;
                        // ES UNA ENTIDAD TEC, POR LO QUE VA EN OTRO TOTAL
                        if (entidad_temp.Equals("TEC") || entidad_temp.Equals("TEC-VIC") || entidad_temp.Equals("TEC-REC") ||
                        entidad_temp.Equals("TEC-MIXTO") || entidad_temp.Equals("TEC-Académico") || entidad_temp.Equals("CIADEG "))
                        {
                            if (entidad_temp.Equals("TEC-REC"))
                            {
                                // Se omite este, dado que son horas voluntarias
                            }
                            else
                            {

                                if (pReportInfo.profesores_carga_tec.ContainsKey(vProfe.Professor.Name))
                                {
                                    entidadesXProfesor = pReportInfo.profesores_cursos_asociados[vProfe.Professor.Name];
                                    if (entidadesXProfesor.Contains(vProject.Name))
                                    {
                                        // quiere decir que el curso ya esta contabilizado, entonces se omite
                                    }
                                    else
                                    {
                                        entidadesXProfesor.Add(vProject.Name);
                                        pReportInfo.profesores_cursos_asociados[vProfe.Professor.Name] = entidadesXProfesor;
                                        pReportInfo.profesores_carga_tec[vProfe.Professor.Name] += Convert.ToInt32(CargaC);
                                    }

                                }
                                else
                                {

                                    pReportInfo.profesores_carga_tec.Add(vProfe.Professor.Name, Convert.ToInt32(CargaC));
                                    // se pregunta si el curso, comision o proyecto ha sido contabilizado
                                    if (pReportInfo.profesores_cursos_asociados.ContainsKey(vProfe.Professor.Name))
                                    {
                                        entidadesXProfesor = pReportInfo.profesores_cursos_asociados[vProfe.Professor.Name];
                                        entidadesXProfesor.Add(vProject.Name);


                                        pReportInfo.profesores_cursos_asociados[vProfe.Professor.Name] = entidadesXProfesor;
                                    }
                                    else
                                    {
                                        pReportInfo.profesores_cursos_asociados.Add(vProfe.Professor.Name, new List<String>() { vProject.Name });
                                    }
                                }
                            }
                        }
                        else
                        {
                            if (pReportInfo.profesores_carga_fundatec.ContainsKey(vProfe.Professor.Name))
                            {
                                entidadesXProfesor = pReportInfo.profesores_cursos_asociados[vProfe.Professor.Name];
                                if (entidadesXProfesor.Contains(vProject.Name))
                                {
                                    // quiere decir que el curso ya esta contabilizado, entonces se omite
                                }
                                else
                                {
                                    entidadesXProfesor.Add(vProject.Name);
                                    pReportInfo.profesores_cursos_asociados[vProfe.Professor.Name] = entidadesXProfesor;
                                    pReportInfo.profesores_carga_fundatec[vProfe.Professor.Name] += Convert.ToInt32(CargaC);
                                }
                            }
                            else
                            {
                                pReportInfo.profesores_carga_fundatec.Add(vProfe.Professor.Name, Convert.ToInt32(CargaC));

                                // se pregunta si el curso, comision o proyecto ha sido contabilizado
                                if (pReportInfo.profesores_cursos_asociados.ContainsKey(vProfe.Professor.Name))
                                {
                                    entidadesXProfesor = pReportInfo.profesores_cursos_asociados[vProfe.Professor.Name];
                                    entidadesXProfesor.Add(vProject.Name);


                                    pReportInfo.profesores_cursos_asociados[vProfe.Professor.Name] = entidadesXProfesor;
                                }
                                else
                                {
                                    pReportInfo.profesores_cursos_asociados.Add(vProfe.Professor.Name, new List<String>() { vProject.Name });
                                }

                            }
                        }
                    }
                }
            }
            return pReportInfo;
        }

        private ReporteInfo setCommissions(ReporteInfo pReportInfo)
        {
            var entidad_temp = "";

            var vCommissions = db.Commissions.ToList();
            foreach (var vCommission in vCommissions)
            {
                var vProfessors = db.CommissionsXProfessors.Where(p => p.CommissionID == vCommission.ID);
                foreach (var vProfe in vProfessors)
                {
                    var vSchedule = vProfe.Schedule.ToList();
                    foreach (var vDay in vSchedule)
                    {
                        string HoraInicio = vDay.StartHour;
                        string HoraFin = vDay.EndHour;
                        ///////////////////////////////////////////////////////////////////
                        double CargaC = 0;// (double)(Day.Hora_Fin - Day.Hora_Inicio) / 100;
                        //if (Day.Hora_Inicio <= 1200 && Day.Hora_Fin >= 1300)
                        //{
                        //    CargaC = CargaC - 1;
                        //}

                        /*sw.WriteLine("Comision;N/A;" +
                                    Comision.Nombre + ";" +
                                    Profe.Profesore.Nombre + ";" +
                                    Dia.Dia1 + ";" +
                                    HoraInicio + ";" +
                                    HoraFin + ";N/A;N/A;N/A;N/A;N/A;N/A;N/A;" +
                                    Math.Ceiling(CargaC)
                                    );*/
                        entidad_temp = vCommission.EntityType.Name;
                        pReportInfo.todo_profesores.Add(new Profesor
                        {
                            Tipo = "Carga Académico Administrativo",
                            Grupo = "N/A",
                            Nombre = vCommission.Name,
                            Profesor_Nombre = vProfe.Professor.Name,
                            Dia = vDay.Day,
                            HoraInicio = HoraInicio,
                            HoraFin = HoraFin,
                            Cupo = "N/A",
                            PlanEstudio = "N/A",
                            Modalidad = "N/A",
                            Sede = "N/A",
                            HoraTeoricas = "N/A",
                            HoraPractica = "N/A",
                            CargaEstimada = CargaC + "",
                            Aula = "N/A",
                            Entidad = entidad_temp
                        });

                        List<String> entidadesXProfesor;
                        // ES UNA ENTIDAD TEC, POR LO QUE VA EN OTRO TOTAL
                        if (entidad_temp.Equals("TEC") || entidad_temp.Equals("TEC-VIC") || entidad_temp.Equals("TEC-REC") ||
                        entidad_temp.Equals("TEC-MIXTO") || entidad_temp.Equals("TEC-Académico") || entidad_temp.Equals("CIADEG "))
                        {
                            if (entidad_temp.Equals("TEC-REC"))
                            {
                                // Se omite este, dado que son horas voluntarias
                            }
                            else
                            {

                                if (pReportInfo.profesores_carga_tec.ContainsKey(vProfe.Professor.Name))
                                {
                                    entidadesXProfesor = pReportInfo.profesores_cursos_asociados[vProfe.Professor.Name];
                                    if (entidadesXProfesor.Contains(vCommission.Name))
                                    {
                                        // quiere decir que el curso ya esta contabilizado, entonces se omite
                                    }
                                    else
                                    {
                                        entidadesXProfesor.Add(vCommission.Name);
                                        pReportInfo.profesores_cursos_asociados[vProfe.Professor.Name] = entidadesXProfesor;
                                        pReportInfo.profesores_carga_tec[vProfe.Professor.Name] += Convert.ToInt32(CargaC);
                                    }

                                }
                                else
                                {

                                    pReportInfo.profesores_carga_tec.Add(vProfe.Professor.Name, Convert.ToInt32(CargaC));
                                    if (pReportInfo.profesores_cursos_asociados.ContainsKey(vProfe.Professor.Name))
                                    {
                                        entidadesXProfesor = pReportInfo.profesores_cursos_asociados[vProfe.Professor.Name];
                                        entidadesXProfesor.Add(vCommission.Name);


                                        pReportInfo.profesores_cursos_asociados[vProfe.Professor.Name] = entidadesXProfesor;
                                    }
                                    else
                                    {
                                        pReportInfo.profesores_cursos_asociados.Add(vProfe.Professor.Name, new List<String>() { vCommission.Name });
                                    }
                                }
                            }
                        }
                        else
                        {
                            if (pReportInfo.profesores_carga_fundatec.ContainsKey(vProfe.Professor.Name))
                            {
                                entidadesXProfesor = pReportInfo.profesores_cursos_asociados[vProfe.Professor.Name];
                                if (entidadesXProfesor.Contains(vCommission.Name))
                                {
                                    // quiere decir que el curso ya esta contabilizado, entonces se omite
                                }
                                else
                                {
                                    entidadesXProfesor.Add(vCommission.Name);
                                    pReportInfo.profesores_cursos_asociados[vProfe.Professor.Name] = entidadesXProfesor;
                                    pReportInfo.profesores_carga_fundatec[vProfe.Professor.Name] += Convert.ToInt32(CargaC);
                                }
                            }
                            else
                            {
                                pReportInfo.profesores_carga_fundatec.Add(vProfe.Professor.Name, Convert.ToInt32(CargaC));
                                // se pregunta si el curso, comision o proyecto ha sido contabilizado
                                if (pReportInfo.profesores_cursos_asociados.ContainsKey(vProfe.Professor.Name))
                                {
                                    entidadesXProfesor = pReportInfo.profesores_cursos_asociados[vProfe.Professor.Name];
                                    entidadesXProfesor.Add(vCommission.Name);


                                    pReportInfo.profesores_cursos_asociados[vProfe.Professor.Name] = entidadesXProfesor;
                                }
                                else
                                {
                                    pReportInfo.profesores_cursos_asociados.Add(vProfe.Professor.Name, new List<String>() { vCommission.Name });
                                }
                            }
                        }
                    }
                }
            }
            return pReportInfo;
        }

        public double CalculoCupo(int Cupo, int HorasTeoria, int HorasPractica)
        {

            double resultado = 0;
            if (HorasPractica == 0)
            {
                if (Cupo < 15)
                {
                    resultado = 2;
                }
                else if (Cupo < 25)
                {
                    resultado = 3;
                }
                else if (Cupo < 35)
                {
                    resultado = 4;
                }
                else if (Cupo < 45)
                {
                    resultado = 5;
                }
                else
                {
                    resultado = 6;
                }

                if (HorasTeoria >= 5)
                {
                    resultado = resultado + 0.75;
                }
            }
            if (HorasTeoria == 0)
            {
                if (Cupo < 15)
                {
                    resultado = 3;
                }
                else if (Cupo < 25)
                {
                    resultado = 4.5;
                }
                else
                {
                    resultado = 6;
                }
            }
            else
            {
                if (Cupo < 15)
                {
                    resultado = 2.5;
                }
                else if (Cupo < 25)
                {
                    resultado = 3.75;
                }
                else if (Cupo < 35)
                {
                    resultado = 5.25;
                }
                else
                {
                    resultado = 6.5;
                }
            }
            return resultado;
        }

        private class ReporteInfo
        {
            public ReporteInfo()
            {
                profesores_carga_tec = new Dictionary<string, int>();
                profesores_carga_fundatec = new Dictionary<string, int>();
                profesores_cursos_asociados = new Dictionary<string, List<string>>();
                todo_profesores = new List<Profesor>();
            }

            public Dictionary<string, int> profesores_carga_tec { get; set; }
            public Dictionary<string, int> profesores_carga_fundatec { get; set; }
            public Dictionary<string, List<string>> profesores_cursos_asociados { get; set; }
            public List<Profesor> todo_profesores { get; set; }
        }

        private class Profesor
        {
            public String Tipo { get; set; }
            public String Grupo { get; set; }
            public String Nombre { get; set; }
            public String Profesor_Nombre { get; set; }
            public String Dia { get; set; }
            public String HoraInicio { get; set; }
            public String HoraFin { get; set; }
            public String Cupo { get; set; }
            public String PlanEstudio { get; set; }
            public String Modalidad { get; set; }
            public String Sede { get; set; }
            public String HoraTeoricas { get; set; }
            public String HoraPractica { get; set; }
            public String CargaEstimada { get; set; }
            public String Aula { get; set; }
            public String Entidad { get; set; }

            public String toStr()
            {
                return Tipo + ";" + Grupo + ";" + Nombre + ";" + Profesor_Nombre + ";" + 
                       Dia + ";" + HoraInicio + ";" + HoraFin + ";" + Sede + ";" + Aula + ";" + 
                       Cupo + ";" + PlanEstudio + ";" + Modalidad + ";" + HoraTeoricas + ";" +
                       HoraPractica + ";" + CargaEstimada + ";" + Entidad + ";";
            }
        }
    }
}