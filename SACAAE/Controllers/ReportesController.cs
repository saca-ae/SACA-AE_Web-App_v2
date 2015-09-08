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

        public byte[] StrToByteArray(string str)
        {
            UTF8Encoding encoding = new UTF8Encoding();
            return encoding.GetBytes(str);
        }
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
            Dictionary<String, int> profesores_carga_tec = new Dictionary<string, int>();
            Dictionary<String, int> profesores_carga_fundatec = new Dictionary<string, int>();
            Dictionary<String, List<String>> profesores_cursos_asociados = new Dictionary<String, List<String>>();
            List<Profesor> todo_profesores = new List<Profesor>();
            try
            {
                fi.Delete();
            }
            catch (Exception e)
            { }
            using (Stream fs = new MemoryStream())
            {
                StreamWriter sw = new StreamWriter(fs, Encoding.UTF8);
                sw.WriteLine("Tipo;Grupo;Nombre;Profesor;Dia;Hora Inicio;Hora Fin;Sede;Aula;Cupo;Plan de Estudio;Modalidad; Horas Teoricas; Horas Practicas; Externo;Carga Estimada;Entidad");
                var IdPeriodo = int.Parse(Request.Cookies["Periodo"].Value);
                //int IdPeriodo = repoPeriodos.IdPeriodo(Periodo);
                String entidad_temp = "";
                //IQueryable<Group> Grupos = repoGrupos.ObtenerTodosGrupos(IdPeriodo);
                //foreach (Group item in Grupos)
                //{
                //    Detalle_Grupo Detalle = repoGrupos.obtenerUnDetalleGrupo(item.ID);
                //    if (Detalle != null)
                //    {
                //        IQueryable<Dia> Dias = repoHorario.ObtenerDias(Detalle.Horario);
                //        foreach (Dia Dia in Dias)
                //        {

                //            string HoraInicio = (Dia.Hora_Inicio / 100).ToString() + ":" + (Dia.Hora_Inicio % 100).ToString();
                //            string HoraFin = (Dia.Hora_Fin / 100).ToString() + ":" + (Dia.Hora_Fin % 100).ToString();
                //            int Carga = 0;
                //            Curso CursoInfo = repoBloqueXPlanXCurso.ListarCursoXID(item.BloqueXPlanXCursoID);
                //            //if (!CursoInfo.Externo)
                //            //{
                //            Carga = (int)Detalle.ProfesoresXCurso.Horas;
                //            /*Carga = ((CursoInfo.HorasTeoricas * 2) + ((int)CursoInfo.HorasPracticas * 1.75));
                //            double CargaCupo = this.CalculoCupo(Convert.ToInt32(Detalle.Cupo), Convert.ToInt32(CursoInfo.HorasTeoricas), Convert.ToInt32(CursoInfo.HorasPracticas));
                //            Carga = Carga + CargaCupo;*/
                //            //}

                //            /*sw.WriteLine("Curso;" +
                //                        item.Numero + ";" +
                //                        CursoInfo.Nombre + ";" +
                //                        Detalle.ProfesoresXCurso.Profesore.Nombre + ";" +
                //                        Dia.Dia1 + ";" +
                //                        HoraInicio + ";" +
                //                        HoraFin + ";" +
                //                        Detalle.Cupo + ";" +
                //                        item.BloqueXPlanXCurso.BloqueAcademicoXPlanDeEstudio.PlanesDeEstudio.Nombre + ";" +
                //                        item.BloqueXPlanXCurso.BloqueAcademicoXPlanDeEstudio.PlanesDeEstudio.Modalidade.Nombre + ";" +
                //                        item.PlanesDeEstudioXSede.Sede1.Nombre + ";" +
                //                        item.BloqueXPlanXCurso.Curso.HorasTeoricas + ";" +
                //                        item.BloqueXPlanXCurso.Curso.HorasPracticas + ";" +
                //                        item.BloqueXPlanXCurso.Curso.Externo.ToString() + ";" +
                //                        Carga
                //                            );*/
                //            entidad_temp = item.BloqueXPlanXCurso.BloqueAcademicoXPlanDeEstudio.PlanesDeEstudio.TipoEntidad.Nombre;
                //            todo_profesores.Add(new Profesor
                //            {

                //                Tipo = "Carga docente",
                //                Grupo = item.Numero + "",
                //                Nombre = CursoInfo.Nombre,
                //                Profesor_Nombre = Detalle.ProfesoresXCurso.Profesore.Nombre,
                //                Dia = Dia.Dia1,
                //                HoraInicio = HoraInicio,
                //                HoraFin = HoraFin,
                //                Cupo = Detalle.Cupo + "",
                //                PlanEstudio = item.BloqueXPlanXCurso.BloqueAcademicoXPlanDeEstudio.PlanesDeEstudio.Nombre,
                //                Modalidad = item.BloqueXPlanXCurso.BloqueAcademicoXPlanDeEstudio.PlanesDeEstudio.Modalidade.Nombre,
                //                Sede = item.PlanesDeEstudioXSede.Sede1.Nombre,
                //                HoraTeoricas = item.BloqueXPlanXCurso.Curso.HorasTeoricas + "",
                //                HoraPractica = item.BloqueXPlanXCurso.Curso.HorasPracticas + "",
                //                Externo = item.BloqueXPlanXCurso.Curso.Externo.ToString(),
                //                CargaEstimada = Carga + "",
                //                Aula = Detalle.Aula,
                //                Entidad = entidad_temp
                //            });
                //            List<String> entidadesXProfesor;
                //            // ES UNA ENTIDAD TEC, POR LO QUE VA EN OTRO TOTAL
                //            if (entidad_temp.Equals("TEC") || entidad_temp.Equals("TEC-VIC") || entidad_temp.Equals("TEC-REC") ||
                //            entidad_temp.Equals("TEC-MIXTO") || entidad_temp.Equals("TEC-Académico") || entidad_temp.Equals("CIADEG "))
                //            {
                //                if (entidad_temp.Equals("TEC-REC"))
                //                {
                //                    // Se omite este, dado que son horas voluntarias
                //                }
                //                else
                //                {

                //                    if (profesores_carga_tec.ContainsKey(Detalle.ProfesoresXCurso.Profesore.Nombre))
                //                    {
                //                        entidadesXProfesor = profesores_cursos_asociados[Detalle.ProfesoresXCurso.Profesore.Nombre];
                //                        if (entidadesXProfesor.Contains(CursoInfo.Nombre))
                //                        {
                //                            // quiere decir que el curso ya esta contabilizado, entonces se omite
                //                        }
                //                        else
                //                        {
                //                            entidadesXProfesor.Add(CursoInfo.Nombre);
                //                            profesores_cursos_asociados[Detalle.ProfesoresXCurso.Profesore.Nombre] = entidadesXProfesor;
                //                            profesores_carga_tec[Detalle.ProfesoresXCurso.Profesore.Nombre] += Carga;
                //                        }

                //                    }
                //                    else
                //                    {

                //                        profesores_carga_tec.Add(Detalle.ProfesoresXCurso.Profesore.Nombre, Carga);
                //                        // se pregunta si el curso, comision o proyecto ha sido contabilizado
                //                        if (profesores_cursos_asociados.ContainsKey(Detalle.ProfesoresXCurso.Profesore.Nombre))
                //                        {
                //                            entidadesXProfesor = profesores_cursos_asociados[Detalle.ProfesoresXCurso.Profesore.Nombre];
                //                            entidadesXProfesor.Add(CursoInfo.Nombre);


                //                            profesores_cursos_asociados[Detalle.ProfesoresXCurso.Profesore.Nombre] = entidadesXProfesor;
                //                        }
                //                        else
                //                        {
                //                            profesores_cursos_asociados.Add(Detalle.ProfesoresXCurso.Profesore.Nombre, new List<String>() { CursoInfo.Nombre });
                //                        }
                //                    }
                //                }
                //            }
                //            else
                //            {
                //                if (profesores_carga_fundatec.ContainsKey(Detalle.ProfesoresXCurso.Profesore.Nombre))
                //                {
                //                    entidadesXProfesor = profesores_cursos_asociados[Detalle.ProfesoresXCurso.Profesore.Nombre];
                //                    if (entidadesXProfesor.Contains(CursoInfo.Nombre))
                //                    {
                //                        // quiere decir que el curso ya esta contabilizado, entonces se omite
                //                    }
                //                    else
                //                    {
                //                        entidadesXProfesor.Add(CursoInfo.Nombre);
                //                        profesores_cursos_asociados[Detalle.ProfesoresXCurso.Profesore.Nombre] = entidadesXProfesor;
                //                        profesores_carga_fundatec[Detalle.ProfesoresXCurso.Profesore.Nombre] += Carga;
                //                    }
                //                }
                //                else
                //                {
                //                    profesores_carga_fundatec.Add(Detalle.ProfesoresXCurso.Profesore.Nombre, Carga);
                //                    // se pregunta si el curso, comision o proyecto ha sido contabilizado
                //                    if (profesores_cursos_asociados.ContainsKey(Detalle.ProfesoresXCurso.Profesore.Nombre))
                //                    {
                //                        entidadesXProfesor = profesores_cursos_asociados[Detalle.ProfesoresXCurso.Profesore.Nombre];
                //                        entidadesXProfesor.Add(CursoInfo.Nombre);


                //                        profesores_cursos_asociados[Detalle.ProfesoresXCurso.Profesore.Nombre] = entidadesXProfesor;
                //                    }
                //                    else
                //                    {
                //                        profesores_cursos_asociados.Add(Detalle.ProfesoresXCurso.Profesore.Nombre, new List<String>() { CursoInfo.Nombre });
                //                    }
                //                }
                //            }
                //        }

                //    }

                //}


                //Proyecto

                //IQueryable<Project> Proyectos = repoProyecto.ObtenerTodosProyectos();
                //foreach (Proyecto Proyecto in Proyectos)
                //{
                //    IQueryable<ProyectosXProfesor> Profesores = repoProyecto.ObtenerProyectoXProfesor(Proyecto.ID);
                //    foreach (ProyectosXProfesor Profe in Profesores)
                //    {
                //        IQueryable<Dia> Dias = repoHorario.ObtenerDias((int)Profe.Horario);
                //        foreach (Dia Dia in Dias)
                //        {
                //            string HoraInicio = (Dia.Hora_Inicio / 100).ToString() + ":" + (Dia.Hora_Inicio % 100).ToString();
                //            string HoraFin = (Dia.Hora_Fin / 100).ToString() + ":" + (Dia.Hora_Fin % 100).ToString();
                //            double CargaC = (double)(Dia.Hora_Fin - Dia.Hora_Inicio) / 100;
                //            if (Dia.Hora_Inicio <= 1200 && Dia.Hora_Fin >= 1300)
                //            {
                //                CargaC = CargaC - 1;
                //            }
                //            /*sw.WriteLine("Proyecto;N/A;" +
                //                        Proyecto.Nombre + ";" +
                //                        Profe.Profesore.Nombre + ";" +
                //                        Dia.Dia1 + ";" +
                //                        HoraInicio + ";" +
                //                        HoraFin + ";N/A;N/A;N/A;N/A;N/A;N/A;N/A;" +
                //                        Math.Ceiling(CargaC)
                //                        );*/
                //            entidad_temp = Profe.Proyecto1.TipoEntidad.Nombre;
                //            todo_profesores.Add(new Profesor
                //            {
                //                Tipo = "Carga Investigación Extensión",
                //                Grupo = "N/A",
                //                Nombre = Proyecto.Nombre,
                //                Profesor_Nombre = Profe.Profesore.Nombre,
                //                Dia = Dia.Dia1,
                //                HoraInicio = HoraInicio,
                //                HoraFin = HoraFin,
                //                Cupo = "N/A",
                //                PlanEstudio = "N/A",
                //                Modalidad = "N/A",
                //                Sede = "N/A",
                //                HoraTeoricas = "N/A",
                //                HoraPractica = "N/A",
                //                Externo = "N/A",
                //                CargaEstimada = CargaC + "",
                //                Aula = "N/A",
                //                Entidad = entidad_temp
                //            });


                //            List<String> entidadesXProfesor;
                //            // ES UNA ENTIDAD TEC, POR LO QUE VA EN OTRO TOTAL
                //            if (entidad_temp.Equals("TEC") || entidad_temp.Equals("TEC-VIC") || entidad_temp.Equals("TEC-REC") ||
                //            entidad_temp.Equals("TEC-MIXTO") || entidad_temp.Equals("TEC-Académico") || entidad_temp.Equals("CIADEG "))
                //            {
                //                if (entidad_temp.Equals("TEC-REC"))
                //                {
                //                    // Se omite este, dado que son horas voluntarias
                //                }
                //                else
                //                {

                //                    if (profesores_carga_tec.ContainsKey(Profe.Profesore.Nombre))
                //                    {
                //                        entidadesXProfesor = profesores_cursos_asociados[Profe.Profesore.Nombre];
                //                        if (entidadesXProfesor.Contains(Proyecto.Nombre))
                //                        {
                //                            // quiere decir que el curso ya esta contabilizado, entonces se omite
                //                        }
                //                        else
                //                        {
                //                            entidadesXProfesor.Add(Proyecto.Nombre);
                //                            profesores_cursos_asociados[Profe.Profesore.Nombre] = entidadesXProfesor;
                //                            profesores_carga_tec[Profe.Profesore.Nombre] += Convert.ToInt32(CargaC);
                //                        }

                //                    }
                //                    else
                //                    {

                //                        profesores_carga_tec.Add(Profe.Profesore.Nombre, Convert.ToInt32(CargaC));
                //                        // se pregunta si el curso, comision o proyecto ha sido contabilizado
                //                        if (profesores_cursos_asociados.ContainsKey(Profe.Profesore.Nombre))
                //                        {
                //                            entidadesXProfesor = profesores_cursos_asociados[Profe.Profesore.Nombre];
                //                            entidadesXProfesor.Add(Proyecto.Nombre);


                //                            profesores_cursos_asociados[Profe.Profesore.Nombre] = entidadesXProfesor;
                //                        }
                //                        else
                //                        {
                //                            profesores_cursos_asociados.Add(Profe.Profesore.Nombre, new List<String>() { Proyecto.Nombre });
                //                        }
                //                    }
                //                }
                //            }
                //            else
                //            {
                //                if (profesores_carga_fundatec.ContainsKey(Profe.Profesore.Nombre))
                //                {
                //                    entidadesXProfesor = profesores_cursos_asociados[Profe.Profesore.Nombre];
                //                    if (entidadesXProfesor.Contains(Proyecto.Nombre))
                //                    {
                //                        // quiere decir que el curso ya esta contabilizado, entonces se omite
                //                    }
                //                    else
                //                    {
                //                        entidadesXProfesor.Add(Proyecto.Nombre);
                //                        profesores_cursos_asociados[Profe.Profesore.Nombre] = entidadesXProfesor;
                //                        profesores_carga_fundatec[Profe.Profesore.Nombre] += Convert.ToInt32(CargaC);
                //                    }
                //                }
                //                else
                //                {
                //                    profesores_carga_fundatec.Add(Profe.Profesore.Nombre, Convert.ToInt32(CargaC));

                //                    // se pregunta si el curso, comision o proyecto ha sido contabilizado
                //                    if (profesores_cursos_asociados.ContainsKey(Profe.Profesore.Nombre))
                //                    {
                //                        entidadesXProfesor = profesores_cursos_asociados[Profe.Profesore.Nombre];
                //                        entidadesXProfesor.Add(Proyecto.Nombre);


                //                        profesores_cursos_asociados[Profe.Profesore.Nombre] = entidadesXProfesor;
                //                    }
                //                    else
                //                    {
                //                        profesores_cursos_asociados.Add(Profe.Profesore.Nombre, new List<String>() { Proyecto.Nombre });
                //                    }

                //                }
                //            }
                //        }
                //    }
                //}


                //Comisiones

                //IQueryable<Comisione> Comisiones = repoComisiones.ObtenerTodasComisiones();
                //foreach (Comisione Comision in Comisiones)
                //{
                //    IQueryable<ComisionesXProfesor> Profesores = repoComisiones.ObtenerComisionesXProfesor(Comision.ID);
                //    foreach (ComisionesXProfesor Profe in Profesores)
                //    {
                //        IQueryable<Dia> Dias = repoHorario.ObtenerDias((int)Profe.Horario);
                //        foreach (Dia Dia in Dias)
                //        {
                //            string HoraInicio = (Dia.Hora_Inicio / 100).ToString() + ":" + (Dia.Hora_Inicio % 100).ToString();
                //            string HoraFin = (Dia.Hora_Fin / 100).ToString() + ":" + (Dia.Hora_Fin % 100).ToString();
                //            double CargaC = (double)(Dia.Hora_Fin - Dia.Hora_Inicio) / 100;
                //            if (Dia.Hora_Inicio <= 1200 && Dia.Hora_Fin >= 1300)
                //            {
                //                CargaC = CargaC - 1;
                //            }
                //            /*sw.WriteLine("Comision;N/A;" +
                //                        Comision.Nombre + ";" +
                //                        Profe.Profesore.Nombre + ";" +
                //                        Dia.Dia1 + ";" +
                //                        HoraInicio + ";" +
                //                        HoraFin + ";N/A;N/A;N/A;N/A;N/A;N/A;N/A;" +
                //                        Math.Ceiling(CargaC)
                //                        );*/
                //            entidad_temp = Comision.TipoEntidad.Nombre;
                //            todo_profesores.Add(new Profesor
                //            {
                //                Tipo = "Carga Académico Administrativo",
                //                Grupo = "N/A",
                //                Nombre = Comision.Nombre,
                //                Profesor_Nombre = Profe.Profesore.Nombre,
                //                Dia = Dia.Dia1,
                //                HoraInicio = HoraInicio,
                //                HoraFin = HoraFin,
                //                Cupo = "N/A",
                //                PlanEstudio = "N/A",
                //                Modalidad = "N/A",
                //                Sede = "N/A",
                //                HoraTeoricas = "N/A",
                //                HoraPractica = "N/A",
                //                Externo = "N/A",
                //                CargaEstimada = CargaC + "",
                //                Aula = "N/A",
                //                Entidad = entidad_temp
                //            });

                //            List<String> entidadesXProfesor;
                //            // ES UNA ENTIDAD TEC, POR LO QUE VA EN OTRO TOTAL
                //            if (entidad_temp.Equals("TEC") || entidad_temp.Equals("TEC-VIC") || entidad_temp.Equals("TEC-REC") ||
                //            entidad_temp.Equals("TEC-MIXTO") || entidad_temp.Equals("TEC-Académico") || entidad_temp.Equals("CIADEG "))
                //            {
                //                if (entidad_temp.Equals("TEC-REC"))
                //                {
                //                    // Se omite este, dado que son horas voluntarias
                //                }
                //                else
                //                {

                //                    if (profesores_carga_tec.ContainsKey(Profe.Profesore.Nombre))
                //                    {
                //                        entidadesXProfesor = profesores_cursos_asociados[Profe.Profesore.Nombre];
                //                        if (entidadesXProfesor.Contains(Comision.Nombre))
                //                        {
                //                            // quiere decir que el curso ya esta contabilizado, entonces se omite
                //                        }
                //                        else
                //                        {
                //                            entidadesXProfesor.Add(Comision.Nombre);
                //                            profesores_cursos_asociados[Profe.Profesore.Nombre] = entidadesXProfesor;
                //                            profesores_carga_tec[Profe.Profesore.Nombre] += Convert.ToInt32(CargaC);
                //                        }

                //                    }
                //                    else
                //                    {

                //                        profesores_carga_tec.Add(Profe.Profesore.Nombre, Convert.ToInt32(CargaC));
                //                        if (profesores_cursos_asociados.ContainsKey(Profe.Profesore.Nombre))
                //                        {
                //                            entidadesXProfesor = profesores_cursos_asociados[Profe.Profesore.Nombre];
                //                            entidadesXProfesor.Add(Comision.Nombre);


                //                            profesores_cursos_asociados[Profe.Profesore.Nombre] = entidadesXProfesor;
                //                        }
                //                        else
                //                        {
                //                            profesores_cursos_asociados.Add(Profe.Profesore.Nombre, new List<String>() { Comision.Nombre });
                //                        }
                //                    }
                //                }
                //            }
                //            else
                //            {
                //                if (profesores_carga_fundatec.ContainsKey(Profe.Profesore.Nombre))
                //                {
                //                    entidadesXProfesor = profesores_cursos_asociados[Profe.Profesore.Nombre];
                //                    if (entidadesXProfesor.Contains(Comision.Nombre))
                //                    {
                //                        // quiere decir que el curso ya esta contabilizado, entonces se omite
                //                    }
                //                    else
                //                    {
                //                        entidadesXProfesor.Add(Comision.Nombre);
                //                        profesores_cursos_asociados[Profe.Profesore.Nombre] = entidadesXProfesor;
                //                        profesores_carga_fundatec[Profe.Profesore.Nombre] += Convert.ToInt32(CargaC);
                //                    }
                //                }
                //                else
                //                {
                //                    profesores_carga_fundatec.Add(Profe.Profesore.Nombre, Convert.ToInt32(CargaC));
                //                    // se pregunta si el curso, comision o proyecto ha sido contabilizado
                //                    if (profesores_cursos_asociados.ContainsKey(Profe.Profesore.Nombre))
                //                    {
                //                        entidadesXProfesor = profesores_cursos_asociados[Profe.Profesore.Nombre];
                //                        entidadesXProfesor.Add(Comision.Nombre);


                //                        profesores_cursos_asociados[Profe.Profesore.Nombre] = entidadesXProfesor;
                //                    }
                //                    else
                //                    {
                //                        profesores_cursos_asociados.Add(Profe.Profesore.Nombre, new List<String>() { Comision.Nombre });
                //                    }
                //                }
                //            }
                //        }
                //    }
                //}

                Profesor[] array_profesores = todo_profesores.ToArray();
                Array.Sort(array_profesores, delegate (Profesor user1, Profesor user2)
                {
                    return user1.Profesor_Nombre.CompareTo(user2.Profesor_Nombre);
                });
                String profe_actual = "";
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
                        if (profesores_carga_tec.ContainsKey(profe_actual) && profesores_carga_fundatec.ContainsKey(profe_actual))
                        {
                            sw.WriteLine("SUBTOTAL CARGA TEC;;;" + profe_actual + ";;;;;;;;;;;;" + profesores_carga_tec[profe_actual]);
                            sw.WriteLine("SUBTOTAL CARGA FUNDATEC;;;" + profe_actual + ";;;;;;;;;;;;" + profesores_carga_fundatec[profe_actual]);
                            sw.WriteLine("TOTAL CARGA;;;" + profe_actual + ";;;;;;;;;;;;" + (profesores_carga_tec[profe_actual] + profesores_carga_fundatec[profe_actual]));

                        }
                        else if (!profesores_carga_tec.ContainsKey(profe_actual) && profesores_carga_fundatec.ContainsKey(profe_actual))
                        {
                            sw.WriteLine("SUBTOTAL CARGA TEC;;;" + profe_actual + ";;;;;;;;;;;;" + "0");
                            sw.WriteLine("SUBTOTAL CARGA FUNDATEC;;;" + profe_actual + ";;;;;;;;;;;;" + profesores_carga_fundatec[profe_actual]);
                            sw.WriteLine("TOTAL CARGA;;;" + profe_actual + ";;;;;;;;;;;;" + profesores_carga_fundatec[profe_actual]);

                        }
                        else if (profesores_carga_tec.ContainsKey(profe_actual) && !profesores_carga_fundatec.ContainsKey(profe_actual))
                        {
                            sw.WriteLine("SUBTOTAL CARGA TEC;;;" + profe_actual + ";;;;;;;;;;;;" + profesores_carga_tec[profe_actual]);
                            sw.WriteLine("SUBTOTAL CARGA FUNDATEC;;;" + profe_actual + ";;;;;;;;;;;;" + 0);
                            sw.WriteLine("TOTAL CARGA;;;" + profe_actual + ";;;;;;;;;;;;" + profesores_carga_tec[profe_actual]);

                        }


                        profe_actual = profe.Profesor_Nombre;
                        sw.WriteLine(profe.toStr());
                    }
                }
                // el ciclo se termina con el ultimo profe, por eso hay q hacer un llamado al final
                if (profesores_carga_tec.ContainsKey(profe_actual) && profesores_carga_fundatec.ContainsKey(profe_actual))
                {
                    sw.WriteLine("SUBTOTAL CARGA TEC;;;" + profe_actual + ";;;;;;;;;;;" + profesores_carga_tec[profe_actual]);
                    sw.WriteLine("SUBTOTAL CARGA FUNDATEC;;;" + profe_actual + ";;;;;;;;;;;" + profesores_carga_fundatec[profe_actual]);
                    sw.WriteLine("TOTAL CARGA;;;" + profe_actual + ";;;;;;;;;;;" + (profesores_carga_tec[profe_actual] + profesores_carga_fundatec[profe_actual]));

                }
                else if (!profesores_carga_tec.ContainsKey(profe_actual) && profesores_carga_fundatec.ContainsKey(profe_actual))
                {
                    sw.WriteLine("SUBTOTAL CARGA TEC;;;" + profe_actual + ";;;;;;;;;;;" + "0");
                    sw.WriteLine("SUBTOTAL CARGA FUNDATEC;;;" + profe_actual + ";;;;;;;;;;;" + profesores_carga_fundatec[profe_actual]);
                    sw.WriteLine("TOTAL CARGA;;;" + profe_actual + ";;;;;;;;;;;" + profesores_carga_fundatec[profe_actual]);

                }
                else if (profesores_carga_tec.ContainsKey(profe_actual) && !profesores_carga_fundatec.ContainsKey(profe_actual))
                {
                    sw.WriteLine("SUBTOTAL CARGA TEC;;;" + profe_actual + ";;;;;;;;;;;" + profesores_carga_tec[profe_actual]);
                    sw.WriteLine("SUBTOTAL CARGA FUNDATEC;;;" + profe_actual + ";;;;;;;;;;;" + 0);
                    sw.WriteLine("TOTAL CARGA;;;" + profe_actual + ";;;;;;;;;;;" + profesores_carga_tec[profe_actual]);

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

        public class Profesor
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
            public String Externo { get; set; }
            public String CargaEstimada { get; set; }
            public String Aula { get; set; }
            public String Entidad { get; set; }

            public String toStr()
            {
                return Tipo + ";" + Grupo + ";" + Nombre + ";" + Profesor_Nombre + ";" + Dia + ";" + HoraInicio + ";" + HoraFin + ";" + Sede + ";" + Aula + ";" + Cupo + ";" + PlanEstudio + ";" + Modalidad + ";" + HoraTeoricas + ";" +
                    HoraPractica + ";" + Externo + ";" + CargaEstimada + ";" + Entidad + ";";
            }
        }
    }
}