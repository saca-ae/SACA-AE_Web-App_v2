using SACAAE.Data_Access;
using SACAAE.Helpers;
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
        private LoadAcademicHelper LAHelper = new LoadAcademicHelper();
        SACAAE.Helpers.LoadAcademicHelper.ReporteInfo vReportInfo = new SACAAE.Helpers.LoadAcademicHelper.ReporteInfo();

        /// <author>Adonis Mora Angulo</author>
        /// <summary>
        /// Download the general report on a csv file
        /// </summary>
        /// <returns></returns>
        public FileResult Download()
        {
            var fi = new FileInfo("myfile.txt");
            byte[] bytes;
            SACAAE.Helpers.LoadAcademicHelper.ReporteInfo vReportInfo = new SACAAE.Helpers.LoadAcademicHelper.ReporteInfo();
            int vPeriodID = int.Parse(Request.Cookies["Periodo"].Value);
            try
            {
                fi.Delete();
            }
            catch (Exception)
            { }

            using (Stream fs = new MemoryStream())
            {
                StreamWriter sw = new StreamWriter(fs, Encoding.UTF8);
                sw.WriteLine("Tipo;Grupo;Nombre;Profesor;Dia;Hora Inicio;Hora Fin;Sede;Aula;Plan de Estudio;Carga Estimada;Tipo Hora;Entidad");
                var IdPeriodo = int.Parse(Request.Cookies["Periodo"].Value);

                vReportInfo = LAHelper.setCourses(vReportInfo, vPeriodID);
                vReportInfo = LAHelper.setProjects(vReportInfo, vPeriodID);
                vReportInfo = LAHelper.setCommissions(vReportInfo, vPeriodID);

                SACAAE.Helpers.LoadAcademicHelper.Profesor[] array_profesores = vReportInfo.todo_profesores.ToArray();
                Array.Sort(array_profesores)
                    
                    /*
                    , delegate(SACAAE.Helpers.LoadAcademicHelper.Profesor user1, 
                                                      SACAAE.Helpers.LoadAcademicHelper.Profesor user2)
                {
                    return user1.Profesor_Nombre.CompareTo(user2.Profesor_Nombre);
                })*/
                     ;


                string profe_actual = "";
                double vCargaTec = 0, vCargaReconocimiento = 0, vCargaRecargo = 0;

                foreach (SACAAE.Helpers.LoadAcademicHelper.Profesor profe in array_profesores)
                {
                    vCargaTec = 0; vCargaReconocimiento = 0; vCargaRecargo = 0;
                    if (profe_actual.Equals("")) profe_actual = profe.Profesor_Nombre;

                    if (profe_actual.Equals(profe.Profesor_Nombre)) sw.WriteLine(profe.toStr());

                    else
                    {
                        if (vReportInfo.profesores_carga_tec.ContainsKey(profe_actual)) 
                        {
                            vCargaTec = vReportInfo.profesores_carga_tec[profe_actual];
                            sw.WriteLine("SUBTOTAL CARGA TEC;;;" + profe_actual + ";;;;;;;;" + vCargaTec);
                        }
                            
                        if (vReportInfo.profesores_carga_reconocimiento.ContainsKey(profe_actual)) 
                        {
                            vCargaReconocimiento = vReportInfo.profesores_carga_reconocimiento[profe_actual];
                            sw.WriteLine("SUBTOTAL CARGA Reconocimiento;;;" + profe_actual + ";;;;;;;;" + vCargaReconocimiento);
                        }

                        if (vReportInfo.profesores_carga_recargo.ContainsKey(profe_actual))
                        {
                            vCargaRecargo = vReportInfo.profesores_carga_recargo[profe_actual];
                            sw.WriteLine("SUBTOTAL CARGA Recargo;;;" + profe_actual + ";;;;;;;;" + vCargaRecargo);
                        }
                        sw.WriteLine("TOTAL CARGA;;;" + profe_actual + ";;;;;;;;" + (vCargaRecargo + vCargaReconocimiento + vCargaTec));
                        sw.WriteLine();

                        profe_actual = profe.Profesor_Nombre;
                        sw.WriteLine(profe.toStr());
                    }

                        
                    
                }

                // last professor at loop's end
                if (vReportInfo.profesores_carga_tec.ContainsKey(profe_actual))
                {
                    vCargaTec = vReportInfo.profesores_carga_tec[profe_actual];
                    sw.WriteLine("SUBTOTAL CARGA TEC;;;" + profe_actual + ";;;;;;;;" + vCargaTec);
                }

                if (vReportInfo.profesores_carga_reconocimiento.ContainsKey(profe_actual))
                {
                    vCargaReconocimiento = vReportInfo.profesores_carga_reconocimiento[profe_actual];
                    sw.WriteLine("SUBTOTAL CARGA Reconocimiento;;;" + profe_actual + ";;;;;;;;" + vCargaReconocimiento);
                }

                if (vReportInfo.profesores_carga_recargo.ContainsKey(profe_actual))
                {
                    vCargaRecargo = vReportInfo.profesores_carga_recargo[profe_actual];
                    sw.WriteLine("SUBTOTAL CARGA Recargo;;;" + profe_actual + ";;;;;;;;" + vCargaRecargo);
                }
                sw.WriteLine("TOTAL CARGA;;;" + profe_actual + ";;;;;;;;" + (vCargaRecargo + vCargaReconocimiento + vCargaTec));
                sw.WriteLine();

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
    }
}