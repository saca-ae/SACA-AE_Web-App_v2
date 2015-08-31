using CsvHelper;
using CsvHelper.Configuration;
using SACAAE.Data_Access;
using SACAAE.Models;
using SACAAE.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;

namespace SACAAE.Controllers
{
    public class CSVController : Controller
    {
        private SACAAEContext gvDatabase = new SACAAEContext();

        // GET: CSV
        public ActionResult CargarCSV()
        {
            IEnumerable<GroupAssignmentCSVViewModel> vGroups2 = Enumerable.Empty<GroupAssignmentCSVViewModel>();
            return View(vGroups2.ToList());
        }

        /// <summary>
        ///  Controller method to validate the csv document before upload
        /// </summary>
        /// <author> Cristian Araya Fuentes </author> 
        /// <returns></returns>
        public ActionResult UploadCsvFile()
        {
            List <GroupAssignmentCSVViewModel> vGroupsList = new List<GroupAssignmentCSVViewModel>();
            IEnumerable<DataCSV> vGroups ;
            int vPeriod = int.Parse(Request.Cookies["Periodo"].Value);

            var attachedFile = System.Web.HttpContext.Current.Request.Files["CsvDoc"];
            if (attachedFile == null || attachedFile.ContentLength <= 0) return View(vGroupsList);
            var csvReader = new StreamReader(attachedFile.InputStream);
            
            using (csvReader)
            { 
                var vReader = new CsvReader(csvReader);
                vGroups = vReader.GetRecords<DataCSV>();
                int vGroupNumber = 0;

                foreach (DataCSV group in vGroups)
                {
                    String vState = "",vDetails = ""; // Variables to show the assignation process

                    // Validate that GroupNumber field is a number 
                    try { vGroupNumber = Convert.ToInt32(group.Grupo); }
                    catch { }

                    // Validations of each field in the file
                    int vIDGroup = getIDGroup(vPeriod, group.Sede, vGroupNumber, group.Nombre);
                    int vIdProfessor = getIDProfessor(group.Profesor);
                    int vIdSchedule = getIDSchedule(group.Dia, group.HoraInicio, group.HoraFin);
                    int vValidateProfessorSchedule = getProfessorValidationSchedule(vPeriod, vIdProfessor, vIdSchedule);
                    int vIdClassroom = getIDClassroom(group.Aula, group.Sede);
                    int vValidateClassroomSchedule = getClassroomValidationSchedule(vPeriod, vIdClassroom, vIdSchedule);

                    // If there some error in validation, writes on vDetails a little about the error ocurred.
                    if (vIDGroup == 0) { vDetails = " - Informacion de Grupo incorrecta";}
                    if (vIdProfessor == 0) { vDetails += " - Profesor no existe"; }
                    if (vIdSchedule == 0) { vDetails += " - Horario Inválido (Dia, Hora Inicio, Hora Final)";}
                    if (vValidateProfessorSchedule == 0){ vDetails += " - Conflicto de Horario del Profesor";}
                    if (vIdClassroom == 0) { vDetails += " - Aula Incorrecta";}
                    if (vValidateClassroomSchedule == 0) { vDetails += " - Conflicto de Horario del Aula";}

                    if (vIDGroup != 0) // Group is ok
                    {
                        // Creates GroupClassroom objet to save it in the database
                        GroupClassroom vGroupClassroom = new GroupClassroom();

                        if (vIdClassroom != 0 && (vValidateClassroomSchedule == 0)) //Classroom ok but Conflict with schedule
                        { vGroupClassroom.ScheduleID = vIdSchedule; }

                        vGroupClassroom.GroupID = vIDGroup;
                        vGroupClassroom.ClassroomID = vIdClassroom;

                        if (vIdClassroom != 0 || vIdSchedule != 0 ) // There is a classroom or schedule to assign
                        { AddGroupClassroom(vGroupClassroom); }
                        
                        if (vIdProfessor != 0 && (vValidateProfessorSchedule != 0)) //Professor ok
                        {
                            EditGroup(vIDGroup, vIdProfessor);
                        }

                    }

                    // Create de Object of ViewModel to show in the view.
                    GroupAssignmentCSVViewModel vGA = new GroupAssignmentCSVViewModel();
                    vGA.Grupo = Convert.ToInt32(group.Grupo);
                    vGA.Profesor = group.Profesor;
                    vGA.Curso = group.Nombre;
                    vGA.Dia = group.Dia;
                    vGA.HInicio = group.HoraInicio;
                    vGA.HFinal = group.HoraFin;
                    vGA.EstadoAsignacion = vState + "";
                    vGA.Aula = group.Aula;
                    vGA.DetalleAsignacion = vDetails;
                    vGroupsList.Add(vGA);      
                }    
            }

            int cont = vGroupsList.Count;
            return View(vGroupsList);               
        }

        public void AddGroupClassroom(GroupClassroom pGroupClassroom)
        {
            gvDatabase.GroupClassrooms.Add(pGroupClassroom);
            gvDatabase.SaveChanges();
        }

        public void EditGroup(int pIdGroup, int pIdProfessor)
        {
            Group vGroup = gvDatabase.Groups.Find(pIdGroup);
            vGroup.ProfessorID = pIdProfessor;
            gvDatabase.Entry(vGroup).State = EntityState.Modified;
            gvDatabase.SaveChanges();
        }
        public void ValidateCSV() 
        {
            
        }

        /// <summary>
        /// This function validate schedule conflict between the schedule to be assigned to group 
        /// and the schedule of the professor.
        /// </summary>
        /// <author> Cristian Araya Fuentes </author> 
        /// <param name="pIdPeriodo"></param>
        /// <param name="pIdProfessor"></param>
        /// <param name="pIdSchedule"></param>
        /// <returns> Integer value, 1 is correct validation, 0 incorrect validation </returns>
        public int getProfessorValidationSchedule(int pIdPeriodo, int pIdProfessor, int pIdSchedule)
        {
            List<Schedule> ScheduleList;
            String vDay = ""; 
            int vStartHour = 0, vEndHour = 0; 

            // Get the information of the schedule
            var vConsultSchedule =
                (from Schedule S in gvDatabase.Schedules
                 where S.ID == pIdSchedule
                 select S).FirstOrDefault();

            if (vConsultSchedule != null)
            {
                vDay = vConsultSchedule.Day;
                vStartHour = Convert.ToInt32(vConsultSchedule.StartHour);
                vEndHour = Convert.ToInt32(vConsultSchedule.EndHour);
            }

            // Get all the Schedule of the professor on a given day
            var vConsult =
                (from Professor P in gvDatabase.Professors
                 join Group G in gvDatabase.Groups on P.ID equals G.ProfessorID
                 join GroupClassroom GC in gvDatabase.GroupClassrooms on G.ID equals GC.GroupID
                 join Schedule Sc in gvDatabase.Schedules on GC.ScheduleID equals Sc.ID
                 where G.PeriodID == pIdPeriodo
                 where P.ID == pIdProfessor
                 where Sc.Day == vDay
                 select Sc);

            // If there are schedule of the proffesor in that given day, make a list of them,
            // In other case return correct validation (1)
            if (vConsult != null) ScheduleList = vConsult.ToList<Schedule>();
            else return 1;

            // Check if there schedules conflicts
            foreach (Schedule group in ScheduleList)
            {
                int vStart = Convert.ToInt32(group.StartHour);
                int vEnd = Convert.ToInt32(group.EndHour);

                IEnumerable<int> vHoursSequence = Enumerable.Range(vStart + 1, vEnd);
                if (vHoursSequence.Contains(vStartHour)) return 0;
                if (vHoursSequence.Contains(vEndHour)) return 0;
            }

            // If the schedule doesn't conflict with other, accept it
            return 1;
        }

        /// <summary>
        /// This function validate schedule conflict between the schedule to be assigned to group 
        /// and the schedule of the classroom.
        /// </summary>
        /// <author> Cristian Araya Fuentes </author> 
        /// <param name="pIdPeriodo"></param>
        /// <param name="pIdClassroom"></param>
        /// <param name="pIdSchedule"></param>
        /// <returns> Integer value, 1 is correct validation, 0 incorrect validation </returns>
        public int getClassroomValidationSchedule(int pIdPeriodo, int pIdClassroom, int pIdSchedule)
        {
            List<Schedule> ScheduleList;
            String vDay = "";
            int vStartHour = 0, vEndHour = 0;

            // Get the information of the schedule
            var vConsultSchedule =
                (from Schedule S in gvDatabase.Schedules
                 where S.ID == pIdSchedule
                 select S).FirstOrDefault();

            if (vConsultSchedule != null)
            {
                vDay = vConsultSchedule.Day;
                vStartHour = Convert.ToInt32(vConsultSchedule.StartHour);
                vEndHour = Convert.ToInt32(vConsultSchedule.EndHour);
            }

            // Get all the Schedule of the classroom on a given day
            var vConsult =
                (from Classroom C in gvDatabase.Classrooms
                 join GroupClassroom GC in gvDatabase.GroupClassrooms on C.ID equals GC.ClassroomID
                 join Group G in gvDatabase.Groups on GC.GroupID equals G.ID
                 join Schedule Sc in gvDatabase.Schedules on GC.ScheduleID equals Sc.ID
                 where G.PeriodID == pIdPeriodo
                 where C.ID == pIdClassroom
                 where Sc.Day == vDay
                 select Sc);

            // If there are schedule of the classroom in that given day, make a list of them.
            // In other case return correct validation (1)
            if (vConsult != null) ScheduleList = vConsult.ToList<Schedule>();
            else return 1;

            // Check if there schedules conflicts
            foreach (Schedule group in ScheduleList)
            {
                int vStart = Convert.ToInt32(group.StartHour);
                int vEnd = Convert.ToInt32(group.EndHour);

                IEnumerable<int> vHoursSequence = Enumerable.Range(vStart + 1, vEnd);
                if (vHoursSequence.Contains(vStartHour)) return 0;
                if (vHoursSequence.Contains(vEndHour)) return 0;
            }

            // If the schedule doesn't conflict with other, accept it
            return 1;
        }

        public int getIDClassroom(String pClassroom, String pSede)
        {
            int? vID = null;

            var vConsult =
                (from Classroom C in gvDatabase.Classrooms
                 join Sede S in gvDatabase.Sedes on C.SedeID equals S.ID
                 where C.Code == pClassroom
                 where S.Name == pSede
                 select C).FirstOrDefault();

            if (vConsult != null) vID = vConsult.ID;

            if (vID == null) return 0;
            return Convert.ToInt32(vID);
        }

        public int getIDSchedule(String pDay, String pStartHour, String pEndHour)
        {
            int? vID = null;
            var vConsult = 
                (from Schedule S in gvDatabase.Schedules
                  where S.Day == pDay
                  where S.StartHour == pStartHour.Replace(":", "")
                  where S.EndHour == pEndHour.Replace(":", "")
                  select S).FirstOrDefault();

            if (vConsult != null) vID = vConsult.ID;

            if (vID == null) return 0;
            return Convert.ToInt32(vID);
        }

        public int getIDProfessor(String pProfessorName)
        {
            int? vID = null;
            
            var vConsult = 
                (from Professor P in gvDatabase.Professors
                 where P.Name == pProfessorName
                 select P).FirstOrDefault();

            if (vConsult != null) vID = vConsult.ID;
            
            if (vID == null) return 0;
            return Convert.ToInt32(vID);
            
        }

        public int getIDGroup(int pPeriodNumber, String pSede, int pGroupNumber, String pCourseName)
        {
            int? vID = null;
            var vConsult = 
                (from Group G in gvDatabase.Groups
                 join BlockXPlanXCourse BPC in gvDatabase.BlocksXPlansXCourses on G.BlockXPlanXCourseID equals BPC.ID
                 join Sede S in gvDatabase.Sedes on BPC.SedeID equals S.ID
                 join Course C in gvDatabase.Courses on BPC.CourseID equals C.ID
                 where G.PeriodID == pPeriodNumber
                 where S.Name == pSede
                 where G.Number == pGroupNumber
                 where C.Name == pCourseName
                 select G).FirstOrDefault();

            if (vConsult != null) vID = vConsult.ID;
            if (vID == null) return 0;
            return Convert.ToInt32(vID);
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
                }

                vWriter.NextRecord();
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
                from Group G in gvDatabase.Groups
                join BlockXPlanXCourse BPC in gvDatabase.BlocksXPlansXCourses on G.BlockXPlanXCourseID equals BPC.ID
                join AcademicBlockXStudyPlan BP in gvDatabase.AcademicBlocksXStudyPlans on BPC.BlockXPlanID equals BP.ID
                join StudyPlan PE in gvDatabase.StudyPlans on BP.PlanID equals PE.ID
                join Course C in gvDatabase.Courses on BPC.CourseID equals C.ID
                join Modality MO in gvDatabase.Modalities on PE.ModeID equals MO.ID
                join Sede SE in gvDatabase.Sedes on BPC.SedeID equals SE.ID

                // Left Outer Join
                join Professor PF in gvDatabase.Professors on G.ProfessorID equals PF.ID
                into aPF
                from bPF in aPF.DefaultIfEmpty()

                join GroupClassroom GA in gvDatabase.GroupClassrooms on G.ID equals GA.GroupID 
                into aGA from bGA in aGA.DefaultIfEmpty()

                join Schedule H in gvDatabase.Schedules on bGA.ScheduleID equals H.ID
                into aH from bH in aH.DefaultIfEmpty()

                join Classroom AU in gvDatabase.Classrooms on bGA.ClassroomID equals AU.ID
                into aAU from bAU in aAU.DefaultIfEmpty()
                 
                where (G.PeriodID == pIDPeriod && PE.EntityTypeID == pIDEntity)
                select new DataCSV
                {
                    Grupo = G.Number.ToString(),
                    Nombre = C.Name,
                    Profesor = bPF.Name,
                    Dia = bH.Day,
                    HoraInicio = bH.StartHour,
                    HoraFin = bH.EndHour,
                    Sede = SE.Name,
                    Aula = bAU.Code,
                    PlandeEstudio = PE.Name,
                    Modalidad = MO.Name
                };

            return vGroupesCSV;
        }

        #region Helpers
        private int getEntityID(string entityName)
        {
            EntityType entity;
            switch (entityName)
            {
                case "TEC":
                    entity = gvDatabase.EntityTypes.SingleOrDefault(p => p.Name == "TEC");
                    break;
                case "CIADEG":
                    entity = gvDatabase.EntityTypes.SingleOrDefault(p => p.Name == "CIADEG");
                    break;
                case "TAE":
                    entity = gvDatabase.EntityTypes.SingleOrDefault(p => p.Name == "FUNDA-TAE");
                    break;
                case "MAE":
                    entity = gvDatabase.EntityTypes.SingleOrDefault(p => p.Name == "FUNDA-MAE");
                    break;
                case "MDE":
                    entity = gvDatabase.EntityTypes.SingleOrDefault(p => p.Name == "FUNDA-MDE");
                    break;
                case "MGP":
                    entity = gvDatabase.EntityTypes.SingleOrDefault(p => p.Name == "FUNDA-MGP");
                    break;
                case "DDE":
                    entity = gvDatabase.EntityTypes.SingleOrDefault(p => p.Name == "FUNDA-Doctorado");
                    break;
                case "Emprendedores":
                    entity = gvDatabase.EntityTypes.SingleOrDefault(p => p.Name == "FUNDA-Emprendedores");
                    break;
                case "CIE":
                    entity = gvDatabase.EntityTypes.SingleOrDefault(p => p.Name == "FUNDA-CIE");
                    break;
                case "Actualizacion_Cartago":
                    entity = gvDatabase.EntityTypes.SingleOrDefault(p => p.Name == "FUNDA-Actualizacion Cartago");
                    break;
                case "Actualizacion_San_Carlos":
                    entity = gvDatabase.EntityTypes.SingleOrDefault(p => p.Name == "FUNDA-Actualizacion San Carlos");
                    break;
                default:
                    entity = gvDatabase.EntityTypes.SingleOrDefault(p => p.Name == entityName);
                    break;
            }

            return (entity != null) ? entity.ID : 0;
        }
        #endregion
    }
}