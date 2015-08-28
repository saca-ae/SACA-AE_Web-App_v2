using CsvHelper;
using CsvHelper.Configuration;
using SACAAE.Data_Access;
using SACAAE.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace SACAAE.Controllers
{
    public class CSVController : Controller
    {
        private SACAAEContext database = new SACAAEContext();

        // GET: CSV
        public ActionResult CargarCSV()
        {
            return View();
        }

        public void SaveCSV() 
        {

        }

        public FileResult DownloadCSVTemplate()
        {
            String vTemplateFileName = "plantillaAsignacion.csv";
            byte[] vFileSize;
            int vIDEntity = getEntityID(Request.Cookies["Entidad"].Value);
            int vIDPeriod = Int32.Parse(Request.Cookies["Periodo"].Value);

            IEnumerable<DataCSV> vGroups = getGroupesPeriod(vIDPeriod, vIDEntity);

            using (Stream memoryStream = new MemoryStream())
            {
                StreamWriter streamWriter = new StreamWriter (memoryStream,Encoding.UTF8);

                var vWriter = new CsvWriter(streamWriter);

                //Write the CSV's file header
                vWriter.WriteHeader(typeof(DataCSV));

                //Write each group
                foreach (DataCSV group in vGroups)
                {
                    vWriter.WriteRecord(group);
                    vWriter.NextRecord();
                }
                 
                streamWriter.Flush();
                memoryStream.Flush();
                memoryStream.Position = 0;
                vFileSize = new byte[memoryStream.Length];
                memoryStream.Read(vFileSize, 0, vFileSize.Length);
                streamWriter.Close();
                streamWriter.Dispose();
            }

            return File(vFileSize, System.Net.Mime.MediaTypeNames.Application.Octet, vTemplateFileName);
        }


        /// <summary>
        /// This function queries the groups that are given in a period and in a particular entity.
        /// </summary>
        /// <param name="pIDPeriod"> ID of period </param>
        /// <param name="pIDEntity"> ID of entity </param>
        /// <returns> A object of type "IEnumerable<DataCSV>" </returns>
        private IEnumerable<DataCSV> getGroupesPeriod(int pIDPeriod, int pIDEntity)
        {
            // Queries all the tables needed for the complete information of groups. 
            // Complete Information of groups: Group's Number, Course Name, Professor, Day, Start Hour, End Hour, Headquarter, Classroom, Study Plan and Modality
            IEnumerable<DataCSV> vGroupesCSV =
                from Grupo G in database.Grupos
                join BloqueXPlanXCurso BPC in database.BloquesXPlanesXCursos on G.ID equals BPC.BlockXPlanID
                join BloqueAcademicoXPlanDeEstudio BP in database.BloquesAcademicosXPlanesDeEstudio on BPC.BlockXPlanID equals BP.ID
                join PlanDeEstudio PE in database.PlanesDeEstudio on BP.PlanID equals PE.ID
                join Curso C in database.Cursos on BPC.CourseID equals C.ID
                join GrupoAula GA in database.GrupoAula on G.ID equals GA.GroupID
                join Horario H in database.Horarios on GA.ScheduleID equals H.ID
                join Profesor PF in database.Profesores on G.ProfessorID equals PF.ID
                join Periodo PRD in database.Periodos on G.PeriodID equals PRD.ID
                join Sede SE in database.Sedes on BPC.SedeID equals SE.ID
                join Aula AU in database.Aulas on GA.ClassroomID equals AU.ID
                join Modalidad MO in database.Modalidades on PE.ModeID equals MO.ID
                where G.PeriodID == pIDPeriod && PE.EntityTypeID == pIDEntity
                select new DataCSV
                {
                    Grupo = G.Number.ToString(),
                    Nombre = C.Name,
                    Profesor = PF.Name,
                    Dia = H.Day,
                    HoraInicio = H.StartHour,
                    HoraFin = H.EndHour,
                    Sede = SE.Name,
                    Aula = AU.Code,
                    PlandeEstudio = PE.Name,
                    Modalidad = MO.Name
                };

            return vGroupesCSV;
        }

        #region Helpers
        private int getEntityID(string entityName)
        {
            TipoEntidad entity;
            switch (entityName)
            {
                case "TEC":
                    entity = database.TipoEntidades.SingleOrDefault(p => p.Name == "TEC");
                    break;
                case "CIADEG":
                    entity = database.TipoEntidades.SingleOrDefault(p => p.Name == "CIADEG");
                    break;
                case "TAE":
                    entity = database.TipoEntidades.SingleOrDefault(p => p.Name == "FUNDA-TAE");
                    break;
                case "MAE":
                    entity = database.TipoEntidades.SingleOrDefault(p => p.Name == "FUNDA-MAE");
                    break;
                case "MDE":
                    entity = database.TipoEntidades.SingleOrDefault(p => p.Name == "FUNDA-MDE");
                    break;
                case "MGP":
                    entity = database.TipoEntidades.SingleOrDefault(p => p.Name == "FUNDA-MGP");
                    break;
                case "DDE":
                    entity = database.TipoEntidades.SingleOrDefault(p => p.Name == "FUNDA-Doctorado");
                    break;
                case "Emprendedores":
                    entity = database.TipoEntidades.SingleOrDefault(p => p.Name == "FUNDA-Emprendedores");
                    break;
                case "CIE":
                    entity = database.TipoEntidades.SingleOrDefault(p => p.Name == "FUNDA-CIE");
                    break;
                case "Actualizacion_Cartago":
                    entity = database.TipoEntidades.SingleOrDefault(p => p.Name == "FUNDA-Actualizacion Cartago");
                    break;
                case "Actualizacion_San_Carlos":
                    entity = database.TipoEntidades.SingleOrDefault(p => p.Name == "FUNDA-Actualizacion San Carlos");
                    break;
                default:
                    entity = database.TipoEntidades.SingleOrDefault(p => p.Name == entityName);
                    break;
            }

            return (entity != null) ? entity.ID : 0;
        }
        #endregion
    }
}