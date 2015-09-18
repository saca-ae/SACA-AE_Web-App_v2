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

        /// <summary>
        /// Returns the view
        /// </summary>
        /// <author> Cristian Araya Fuentes </author> 
        /// <returns> ActionResult with the view </returns>
        public ActionResult CargarCSV()
        {
            return View();
        }

        /// <summary>
        ///  Controller method to validate the csv document before upload
        /// </summary>
        /// <author> Cristian Araya Fuentes </author> 
        /// <returns> Json with the results of assigned groups </returns>
        [HttpPost]
        public ActionResult UploadCsvFile()
        {
            gvDatabase.Configuration.AutoDetectChangesEnabled = false;
            gvDatabase.Configuration.ValidateOnSaveEnabled = false;

            List <GroupAssignmentCSVViewModel> vGroupsList = new List<GroupAssignmentCSVViewModel>();
            IEnumerable<DataCSV> vGroups ;
            int vPeriod = int.Parse(Request.Cookies["Periodo"].Value);

            var vAttachedFile = System.Web.HttpContext.Current.Request.Files["CsvDoc"];
            if (vAttachedFile == null || vAttachedFile.ContentLength <= 0) return Json(null);
            var vCsvReader = new StreamReader(vAttachedFile.InputStream);

            using (vCsvReader)
            {
                var vReader = new CsvReader(vCsvReader);
                vGroups = vReader.GetRecords<DataCSV>();
                int vGroupNumber = 0;

                foreach (DataCSV group in vGroups)
                {
                    String vState = "",vDetails = ""; // Variables to show the assignation process

                    // Validate that GroupNumber field is a number 
                    try {vGroupNumber = Convert.ToInt32(group.Grupo);}
                    catch { }

                    // Validations of each field in the file
                    int vIDGroup = getIDGroup(vPeriod, group.Sede, vGroupNumber, group.Nombre);
                    int vIdProfessor = getIDProfessor(group.Profesor);
                    int vIdSchedule = getIDSchedule(group.Dia, group.HoraInicio, group.HoraFin);
                    int vIdClassroom = getIDClassroom(group.Aula, group.Sede);
                    int vIdBlock = getIDBlock(vIDGroup);

                    int vValidateProfessorSchedule = getProfessorValidationSchedule(vPeriod, vIdProfessor, vIdSchedule);
                    int vValidateClassroomSchedule = getClassroomValidationSchedule(vPeriod, vIdClassroom, vIdSchedule);
                    int vValidateBlockSchedule = getBlockValidationSchedule(vIdBlock, vIdSchedule, vPeriod);

                    // If there some error in validation, writes on vDetails a little about the error ocurred.
                    if (vIDGroup == 0) { vDetails = " - Informacion de Grupo incorrecta";}
                    if (vIdProfessor == 0) { vDetails += " - Profesor no existe"; }
                    if (vIdSchedule == 0) { vDetails += " - Horario Inválido";}
                    if (vValidateProfessorSchedule == 0){ vDetails += " - Conflicto de Horario del Profesor";}
                    if (vIdClassroom == 0) { vDetails += " - Aula Incorrecta";}
                    if (vValidateClassroomSchedule == 0) { vDetails += " - Conflicto de Horario del Aula";}

                    if (vIDGroup != 0) // Group is ok
                    {
                        // Creates GroupClassroom objet to save it in the database
                        GroupClassroom vGroupClassroom = new GroupClassroom();  
                        vGroupClassroom.ScheduleID = vIdSchedule;
                        vGroupClassroom.GroupID = vIDGroup;
                        vGroupClassroom.ClassroomID = vIdClassroom;

                        if (vIdSchedule != 0 && vIdClassroom != 0 && vValidateClassroomSchedule != 0) // There is a classroom and schedule to assign
                        {
                            // Validates Schedule COnflict in Block -> if (vValidateBlockSchedule != 0)

                            vState = "Incompleto";
                            AddGroupClassroom(vGroupClassroom);
                            if (vIdProfessor != 0 && (vValidateProfessorSchedule != 0)) //Professor ok
                            {
                                EditGroup(vIDGroup, vIdProfessor);
                                vState = "Completo";
                            }
                        }
                        else
                        {
                            vState = "Error";
                        }
                    }
                    else
                    {
                        vState = "Error";
                        vDetails = "Grupo no existe - Compruebe que no ha modificado la información por defecto de la plantilla";
                    }

                    // Creates the Object of ViewModel to show in the view.
                    GroupAssignmentCSVViewModel vGA = new GroupAssignmentCSVViewModel();
                    vGA.Grupo = Convert.ToInt32(group.Grupo);
                    vGA.Profesor = group.Profesor;
                    vGA.Curso = group.Nombre;
                    vGA.Dia = group.Dia;
                    vGA.HInicio = group.HoraInicio;
                    vGA.HFinal = group.HoraFin;
                    vGA.EstadoAsignacion = vState;
                    vGA.Aula = group.Aula;
                    vGA.DetalleAsignacion = vDetails;
                    vGroupsList.Add(vGA);      
                }    
            }

            return Json(vGroupsList, JsonRequestBehavior.AllowGet);             
        }

        /// <summary>
        /// Downloads the CSV template for assignments.
        /// </summary>
        /// <author> Cristian Araya Fuentes </author>
        /// <returns> Csv file </returns>
        public FileResult DownloadCSVTemplate()
        {
            String vTemplateFileName = "plantillaAsignacion.csv";
            byte[] vFileSize;
            int vIDEntity = getEntityID(Request.Cookies["Entidad"].Value);
            int vIDPeriod = Int32.Parse(Request.Cookies["Periodo"].Value);

            IEnumerable<DataCSV> vGroups = getGroupesPeriod(vIDPeriod, vIDEntity);
                
            using (Stream memoryStream = new MemoryStream())
            {
                StreamWriter streamWriter = new StreamWriter(memoryStream, Encoding.UTF8);

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

        /// <summary>
        /// This function validates the schedule conflict between the schedule of all groups of the same block
        /// </summary>
        /// <author> Cristian Araya Fuentes </author> 
        /// <param name="pIdBlock"></param>
        /// <param name="pIdSchedule"></param>
        /// <param name="pIdPeriodo"></param>
        /// <returns> Integer value, 1 is correct validation, 0 incorrect validation </returns>
        public int getBlockValidationSchedule(int pIdBlock, int pIdSchedule, int pIdPeriodo)
        {
            List<Schedule> ScheduleList;
            String vDay = "";
            int vStartHour = 0, vEndHour = 0;

            // Gets the information of the schedule
            var vConsultSchedule =
                (from Schedule S in gvDatabase.Schedules
                 where S.ID == pIdSchedule
                 select S).FirstOrDefault();

            if (vConsultSchedule != null)
            {
                vDay = vConsultSchedule.Day;
                vStartHour = getIntHour(vConsultSchedule.StartHour);
                vEndHour = getIntHour(vConsultSchedule.EndHour);
            }

            // Gets all the Schedule of the classroom on a given day
            var vConsult =
                (from Group G in gvDatabase.Groups
                 join BlockXPlanXCourse BPC in gvDatabase.BlocksXPlansXCourses on G.BlockXPlanXCourseID equals BPC.ID
                 join AcademicBlockXStudyPlan BP in gvDatabase.AcademicBlocksXStudyPlans on BPC.BlockXPlanID equals BP.ID
                 join GroupClassroom GC in gvDatabase.GroupClassrooms on G.ID equals GC.GroupID
                 join Schedule Sc in gvDatabase.Schedules on GC.ScheduleID equals Sc.ID
                 where G.PeriodID == pIdPeriodo
                 where BP.ID == pIdBlock
                 where Sc.Day == vDay
                 select Sc);

            // If there are schedule of the classroom in that given day, make a list of them.
            // In other case return correct validation (1)
            if (vConsult != null) ScheduleList = vConsult.ToList<Schedule>();
            else return 1;

            // Checks if there schedules conflicts.
            foreach (Schedule group in ScheduleList)
            {
                int vStart = getIntHour(group.StartHour);
                int vEnd = getIntHour(group.EndHour);

                IEnumerable<int> vHoursSequence = Enumerable.Range(vStart + 1, vEnd - vStart);
                if (vHoursSequence.Contains(vStartHour)) return 0;
                if (vHoursSequence.Contains(vEndHour)) return 0;
            }

            // If the schedule doesn't conflict with other, accepts it
            return 1;
        }

        /// <summary>
        /// This function validates schedule conflict between the schedule to be assigned to group 
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

            // Gets the information of the schedule
            var vConsultSchedule =
                (from Schedule S in gvDatabase.Schedules
                 where S.ID == pIdSchedule
                 select S).FirstOrDefault();

            if (vConsultSchedule != null)
            {
                vDay = vConsultSchedule.Day;
                vStartHour = getIntHour(vConsultSchedule.StartHour);
                vEndHour = getIntHour(vConsultSchedule.EndHour);
            }

            // Gets all the Schedule of the classroom on a given day
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

            // Checks if there schedules conflicts
            foreach (Schedule group in ScheduleList)
            {
                int vStart = getIntHour(group.StartHour);
                int vEnd = getIntHour(group.EndHour);

                IEnumerable<int> vHoursSequence = Enumerable.Range(vStart + 1, vEnd - vStart);
                if (vHoursSequence.Contains(vStartHour)) return 0;
                if (vHoursSequence.Contains(vEndHour)) return 0;
            }

            // If the schedule doesn't conflict with other, accepts it
            return 1;
        }

        /// <summary>
        /// This function validates schedule conflict between the schedule to be assigned to group 
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

            // Gets the information of the schedule
            var vConsultSchedule =
                (from Schedule S in gvDatabase.Schedules
                 where S.ID == pIdSchedule
                 select S).FirstOrDefault();

            if (vConsultSchedule != null)
            {
                vDay = vConsultSchedule.Day;
                vStartHour = getIntHour(vConsultSchedule.StartHour);
                vEndHour = getIntHour(vConsultSchedule.EndHour);
            }

            // Gets all the Schedule of the professor on a given day
            var vConsult =
                (from Professor P in gvDatabase.Professors
                 join Group G in gvDatabase.Groups on P.ID equals G.ProfessorID
                 join GroupClassroom GC in gvDatabase.GroupClassrooms on G.ID equals GC.GroupID
                 join Schedule Sc in gvDatabase.Schedules on GC.ScheduleID equals Sc.ID
                 where G.PeriodID == pIdPeriodo
                 where P.ID == pIdProfessor
                 where Sc.Day == vDay
                 select Sc);

            // If there are schedule of the professor in that given day, make a list of them,
            // In other case return correct validation (1)
            if (vConsult != null) ScheduleList = vConsult.ToList<Schedule>();
            else return 1;

            // Checks if there are schedules conflicts
            foreach (Schedule group in ScheduleList)
            {
                int vStart = getIntHour(group.StartHour);
                int vEnd = getIntHour(group.EndHour);

                IEnumerable<int> vHoursSequence = Enumerable.Range(vStart + 1, vEnd - vStart);
                if (vHoursSequence.Contains(vStartHour)) return 0;
                if (vHoursSequence.Contains(vEndHour)) return 0;
            }

            // If the schedule doesn't conflict with other, accepts it
            return 1;
        }

        /// <summary>
        /// Gets the id of one group's block
        /// </summary>
        /// <author> Cristian Araya Fuentes </author> 
        /// <param name="pIdGroup"></param>
        /// <returns> Id of academic block, if doesn't exist returns 0  </returns>
        public int getIDBlock(int pIdGroup)
        {
            int? vID = null;

            var vConsult =
                (from Group G in gvDatabase.Groups
                 join BlockXPlanXCourse BPC in gvDatabase.BlocksXPlansXCourses on G.BlockXPlanXCourseID equals BPC.ID
                 join AcademicBlockXStudyPlan BP in gvDatabase.AcademicBlocksXStudyPlans on BPC.BlockXPlanID equals BP.ID
                 where G.ID == pIdGroup
                 select BP).FirstOrDefault();

            if (vConsult != null) vID = vConsult.ID;

            if (vID == null) return 0;
            return Convert.ToInt32(vID);
        }

        /// <summary>
        /// Gets the id of a classroom.
        /// </summary>
        /// <author> Cristian Araya Fuentes </author> 
        /// <param name="pClassroom"></param>
        /// <param name="pSede"></param>
        /// <returns> Id of classroom, if doesn't exist returns 0  </returns>
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

        /// <summary>
        /// Gets the id of a group.
        /// </summary>
        /// <author> Cristian Araya Fuentes </author> 
        /// <param name="pPeriodNumber"></param>
        /// <param name="pSede"></param>
        /// <param name="pGroupNumber"></param>
        /// <param name="pCourseName"></param>
        /// <returns> Id of group, if doesn't exist returns 0 </returns>
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

        /// <summary>
        /// Gets the id of a professor.
        /// </summary>
        /// <author> Cristian Araya Fuentes </author> 
        /// <param name="pProfessorName"></param>
        /// <returns> Id of professor, if doesn't exist returns 0  </returns>
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

        /// <summary>
        /// Gets the id of a schedule.
        /// </summary>
        /// <author> Cristian Araya Fuentes </author> 
        /// <param name="pDay"></param>
        /// <param name="pStartHour"></param>
        /// <param name="pEndHour"></param>
        /// <returns> Id of schedule, if doesn't exist returns 0  </returns>
        public int getIDSchedule(String pDay, String pStartHour, String pEndHour)
        {
            int? vID = null;

            var vResult = gvDatabase.SPGetIDSchedule(pDay, pStartHour, pEndHour).ToList();

            if (vResult != null) vID = vResult.ElementAt(0).Result ;

            if (vID == null) return 0;
            return Convert.ToInt32(vID);
        }

        /// <summary>
        /// This function queries the groups that are given in a period and in a particular entity.
        /// </summary>
        /// <author> Cristian Araya Fuentes </author>
        /// <param name="pIDPeriod"> ID of period </param>
        /// <param name="pIDEntity"> ID of entity </param>
        /// <returns> A object of type "IEnumerable<DataCSV>" </returns>
        private IEnumerable<DataCSV> getGroupesPeriod2(int pIDPeriod, int pIDEntity)
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
                into aPF from bPF in aPF.DefaultIfEmpty()

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

        /// <summary>
        /// This function queries the groups that are given in a period and in a particular entity.
        /// </summary>
        /// <author> Cristian Araya Fuentes </author>
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
                into aGA
                from bGA in aGA.DefaultIfEmpty()

                join Schedule H in gvDatabase.Schedules on bGA.ScheduleID equals H.ID
                into aH
                from bH in aH.DefaultIfEmpty()

                join Classroom AU in gvDatabase.Classrooms on bGA.ClassroomID equals AU.ID
                into aAU
                from bAU in aAU.DefaultIfEmpty()

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

        private int getIntHour(string pHour)
        {
            switch (pHour)
                {
                    case "07:30 am": return 730;
                    case "08:30 am": return 830;
                    case "09:30 am": return 930;
                    case "10:30 am": return 1030;
                    case "11:30 am": return 1130;
                    case "12:30 pm": return 1230;
                    case "01:00 pm": return 1300;
                    case "02:00 pm": return 1400;
                    case "03:00 pm": return 1500;
                    case "04:00 pm": return 1600;
                    case "05:00 pm": return 1700;
                    case "06:00 pm": return 1800;
                    case "07:00 pm": return 1900;
                    case "08:00 pm": return 2000;
                    case "09:00 pm": return 2100;
                    case "08:20 am": return 820;
                    case "09:20 am": return 920;
                    case "10:20 am": return 1020;
                    case "11:20 am": return 1120;
                    case "12:20 pm": return 1220;
                    case "01:50 pm": return 1350;
                    case "02:50 pm": return 1450;
                    case "03:50 pm": return 1550;
                    case "04:50 pm": return 1650;
                    case "05:50 pm": return 1750;
                    case "06:50 pm": return 1850;
                    case "07:50 pm": return 1950;
                    case "08:50 pm": return 2050; 
                    case "09:50 pm": return 2150;
                }
            return 0;
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