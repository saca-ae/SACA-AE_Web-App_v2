using SACAAE.Data_Access;
using SACAAE.Models.StoredProcedures;
using SACAAE.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SACAAE.Helpers
{
    public class ScheduleHelper
    {
        private SACAAEContext db = new SACAAEContext();

        /// <summary>
        /// <autor>Esteban Segura Benavides</autor>
        /// Check all posibles shocks in all schedules of the professor
        /// </summary>
        /// <param name="vProfessorID"></param>
        /// <param name="pGroupID"></param>
        /// <returns></returns>
        public string validations(int vProfessorID, int pGroupID, int pPeriodID)
        {

            //Check the schedule of the commissions related with the professor
            bool vIsScheduleConflictCommission = existScheduleConflictCommissioninGroup(vProfessorID, pGroupID, pPeriodID);
            //if exist shock with the schedule, the system doesn't let assign new projects in that schedule
            bool vIsScheduleConflictProject = existScheduleConflictProjectinGroup(vProfessorID, pGroupID, pPeriodID);
            //if exist shock with the schedule, the system doesn't let assign new projects in that schedule
            bool vIsScheduleConflictGroup = existScheduleConflictGroupinGroup(vProfessorID, pGroupID, pPeriodID);

            return isScheduleConflict(vIsScheduleConflictCommission, vIsScheduleConflictProject, vIsScheduleConflictGroup);
                    
        }

        public string validationsEditGroup(int pGroupID, int pProfessorID, int pPeriodID)
        {

            //Check the schedule of the commissions related with the professor
            bool vIsScheduleConflictCommission = existScheduleConflictCommissioninGroup(pProfessorID, pGroupID, pPeriodID);           
            //if exist shock with the schedule, the system doesn't let assign new projects in that schedule
            bool vIsScheduleConflictProject = existScheduleConflictProjectinGroup(pProfessorID, pGroupID, pPeriodID);
            //if exist shock with the schedule, the system doesn't let assign new projects in that schedule
            bool vIsScheduleConflictGroup = existScheduleConflictGroupWithoutGroupSelect(pProfessorID, pGroupID, pPeriodID);

            return isScheduleConflict(vIsScheduleConflictCommission, vIsScheduleConflictProject, vIsScheduleConflictGroup);
        }

        /// <summary>
        /// <autor>Esteban Segura Benavides</autor>
        /// Check all posibles schedule conflicts in all schedules of the professor
        /// </summary>
        /// <param name="pProfessorID"></param>
        /// <param name="pGroupID"></param>
        /// <returns></returns>
        public string validationsCommission(int vCommissionID, int pProfessorID, int pPeriodID, List<ScheduleComission> pSchedules)
        {
            bool vIsProfessorAssign = isProfessorAssigntoCommission(vCommissionID, pProfessorID, pPeriodID);
            if (!vIsProfessorAssign)
            {
                //Check the schedule of the commissions related with the professor
                bool vIsScheduleConflictCommission = existScheduleConflictCommissionwithActualCommission(pProfessorID, pPeriodID, pSchedules);
                //if exist shock with the schedule, the system doesn't let assign new projects in that schedule
                bool vIsScheduleConflictProject = existScheduleConflictProjectwithActualCommission(pProfessorID, pPeriodID, pSchedules);
                //if exist shock with the schedule, the system doesn't let assign new projects in that schedule
                bool vIsScheduleConflictGroup = existScheduleConflictGroupwithActualCommission(pProfessorID, pPeriodID, pSchedules);

                return isScheduleConflict(vIsScheduleConflictCommission, vIsScheduleConflictProject, vIsScheduleConflictGroup);
            }
            else
            {
                return "falseIsProfessorShock";
            }
        }

        public string validationsEditCommisson(int pCommissionID, int pProfessorID, int pPeriodID, List<ScheduleComission> pSchedules)
        {
            //Check the schedule of the commissions related with the professor
            bool vIsScheduleConflictCommission = existScheduleConflictCommissionWithoutCommissionSelect(pProfessorID, pPeriodID, pSchedules, pCommissionID);
            //if exist shock with the schedule, the system doesn't let assign new projects in that schedule
            bool vIsScheduleConflictProject = existScheduleConflictProjectwithActualCommission(pProfessorID, pPeriodID, pSchedules);
            //if exist shock with the schedule, the system doesn't let assign new projects in that schedule
            bool vIsScheduleConflictGroup = existScheduleConflictGroupwithActualCommission(pProfessorID, pPeriodID, pSchedules);

            return isScheduleConflict(vIsScheduleConflictCommission, vIsScheduleConflictProject, vIsScheduleConflictGroup);
        }

        public string validationProject(int pProjectID, int pProfessorID,int pPeriodID, List<ScheduleProject> pSchedule)
        {
            bool vIsProfessorAssign = isProfessorAssigntoProject(pProjectID,pProfessorID,pPeriodID);
            if (!vIsProfessorAssign)
            {
                bool vIsScheduleConflictProject = existScheduleConflictProjectwithActualProject(pProfessorID, pPeriodID, pSchedule);           
                //if exist shock with the schedule, the system doesn't let assign new projects in that schedule
                bool vIsScheduleConflictCommission = existScheduleConflictCommissionwithActualProject(pProfessorID, pPeriodID, pSchedule);
                //if exist shock with the schedule, the system doesn't let assign new projects in that schedule
                bool vIsScheduleConflictGroup = existScheduleConflictGroupwithProject(pProfessorID, pPeriodID, pSchedule);

                return isScheduleConflict(vIsScheduleConflictCommission, vIsScheduleConflictProject, vIsScheduleConflictGroup);
            }
            else
            {
                return "falseIsProfessorShock";
            }
        }
        public string validationsEditProject(int pProjectID, int pProfessorID, int pPeriodID,List<ScheduleProject> pSchedule)
        {
            bool vIsScheduleConflictProject = existScheduleConflictProjectWithoutProject(pProfessorID, pPeriodID, pSchedule, pProjectID);
           
            //if exist shock with the schedule, the system doesn't let assign new projects in that schedule
            bool vIsScheduleConflictCommission = existScheduleConflictCommissionwithActualProject(pProfessorID, pPeriodID, pSchedule);
            
            //if exist shock with the schedule, the system doesn't let assign new projects in that schedule
            bool vIsScheduleConflictGroup = existScheduleConflictGroupwithProject(pProfessorID, pPeriodID, pSchedule);

            return isScheduleConflict(vIsScheduleConflictCommission, vIsScheduleConflictProject, vIsScheduleConflictGroup);
        }

        public string isScheduleConflict(bool pIsScheduleConflictCommission, bool pIsScheduleConflictProject, bool pIsScheduleConflictGroup)
        {
            if (!pIsScheduleConflictCommission)
            {
                if (!pIsScheduleConflictProject)
                {
                    if (!pIsScheduleConflictGroup)
                    {
                        return "true";
                    }
                    else
                    {
                        return "falseIsGroupShock";
                    }
                }
                else
                {
                    return "falseIsProjectShock";
                }
            }
            else
            {
                return "falseIsCommissionShock";
            }
        }

        private bool existScheduleConflictGroupWithoutGroupSelect(int pProfessorID, int pGroupID, int pPeriodID)
        {
            /*Get Group from database accordin to pGroupID*/
            var vListScheduleGroup = (from grupo in db.Groups
                                      join grupo_aula in db.GroupClassrooms on grupo.ID equals grupo_aula.GroupID
                                      join horario in db.Schedules on grupo_aula.ScheduleID equals horario.ID
                                      where (grupo.ID == pGroupID)
                                      select new { horario.StartHour, horario.EndHour, horario.Day }).ToList();


            //Get the day, starthour and endhour where professor was assign in commission
            var group_schedule = db.SP_getProfessorScheduleGroup(pProfessorID, pPeriodID).ToList();

            //Verify each scheedule with the new assign information
            foreach (var vNewSchedule in vListScheduleGroup)
            {
                foreach (var vActualScheduleGroup in group_schedule)
                {
                    if (vActualScheduleGroup.ID != pGroupID)
                    {
                        bool vIsScheduleCorrect = verifyRange(vNewSchedule.Day, vActualScheduleGroup.Day, vActualScheduleGroup.StartHour,
                                                                       vActualScheduleGroup.EndHour, vNewSchedule.StartHour, vNewSchedule.EndHour);
                        if (vIsScheduleCorrect)
                            return vIsScheduleCorrect;
                    }
                }
            }
            return false;
        }


        private bool existScheduleConflictCommissionWithoutCommissionSelect(int pProfessorID,int pPeriodID, List<ScheduleComission> pSchedules, int pCommissionID)
        {
            //Get the day, starthour and endhour where professor was assign in commission
            var commission_schedule = db.SP_getProfessorScheduleCommission(pProfessorID, pPeriodID).ToList();

            //Verify each scheedule with the new assign information
            foreach (var vNewSchedule in pSchedules)
            {
                foreach (var vActualScheduleCommission in commission_schedule)
                {
                    if (vActualScheduleCommission.ID != pCommissionID)
                    {
                        bool vIsScheduleCorrect = verifyRange(vNewSchedule.Day, vActualScheduleCommission.Day, vActualScheduleCommission.StartHour,
                                                                       vActualScheduleCommission.EndHour, vNewSchedule.StartHour, vNewSchedule.EndHour);
                        if (vIsScheduleCorrect)
                            return vIsScheduleCorrect;
                    }
                }
            }
            return false;
        }

        public bool existScheduleConflictProjectWithoutProject(int pProfessorID,int pPeriodID,List<ScheduleProject> pSchedules, int pProjectID)
        {
            //Get the day, starthour and endhour where professor was assign in commission
            var project_schedule = db.SP_getProfessorScheduleProject(pProfessorID, pPeriodID).ToList();

            //Verify each scheedule with the new assign information
            foreach (var vNewSchedule in pSchedules)
            {
                foreach (var vActualScheduleProject in project_schedule)
                {
                    if (vActualScheduleProject.ID != pProjectID)
                    {
                        bool vIsScheduleCorrect = verifyRange(vNewSchedule.Day, vActualScheduleProject.Day, vActualScheduleProject.StartHour,
                                                                       vActualScheduleProject.EndHour, vNewSchedule.StartHour, vNewSchedule.EndHour);
                        if (vIsScheduleCorrect)
                            return vIsScheduleCorrect;

                    }
                    else
                    {
                        return false;
                    }
                }
            }
            return false;
        }

        #region Group
        /// <summary>
        /// Esteban Segura Benavides
        /// Check posibles conflicts with the new project schedule and the all commission schedule related with determinated professor
        /// </summary>
        /// <param name="pProfessorID"></param>
        /// <param name="pSchedules"></param>
        /// <returns>true if found any problem with the schedules</returns>
        private bool existScheduleConflictCommissioninGroup(int pProfessorID, int pGroupID, int pPeriodID)
        {
            /*Get Group from database accordin to idGrupo*/
            var vListScheduleGroup = (from grupo in db.Groups
                                      join grupo_aula in db.GroupClassrooms on grupo.ID equals grupo_aula.GroupID
                                      join horario in db.Schedules on grupo_aula.ScheduleID equals horario.ID
                                      where (grupo.ID == pGroupID)
                                      select new { horario.StartHour, horario.EndHour, horario.Day }).ToList();

            //Get the day, starthour and endhour where professor was assign in commission
            var commission_schedule = db.SP_getProfessorScheduleCommission(pProfessorID, pPeriodID).ToList();

            //Verify each scheedule with the new assign information
            foreach (var vNewSchedule in vListScheduleGroup)
            {
                foreach (var vActualScheduleCommission in commission_schedule)
                {
                    bool vIsScheduleCorrect = verifyRange(vNewSchedule.Day, vActualScheduleCommission.Day, vActualScheduleCommission.StartHour,
                                                                        vActualScheduleCommission.EndHour, vNewSchedule.StartHour, vNewSchedule.EndHour);
                    if (vIsScheduleCorrect)
                        return vIsScheduleCorrect;
                }
            }
            return false;
        }

        /// <summary>
        /// Esteban Segura Benavides
        /// Check posibles conflicts with the new project schedule and the all project schedule related with determinated professor
        /// </summary>
        /// <param name="pProfessorID"></param>
        /// <param name="pGroupID"></param>
        /// <returns>true if found any problem with the schedules</returns>
        private bool existScheduleConflictProjectinGroup(int pProfessorID, int pGroupID, int pPeriodID)
        {
            /*Get Group from database accordin to pGroupID*/
            var vListScheduleGroup = (from grupo in db.Groups
                                      join grupo_aula in db.GroupClassrooms on grupo.ID equals grupo_aula.GroupID
                                      join horario in db.Schedules on grupo_aula.ScheduleID equals horario.ID
                                      where (grupo.ID == pGroupID)
                                      select new { horario.StartHour, horario.EndHour, horario.Day }).ToList();


            //Get the day, starthour and endhour where professor was assign in commission
            var project_schedule = db.SP_getProfessorScheduleProject(pProfessorID, pPeriodID).ToList();

            //Verify each scheedule with the new assign information
            foreach (var vNewSchedule in vListScheduleGroup)
            {
                foreach (var vActualScheduleProject in project_schedule)
                {
                    bool vIsScheduleCorrect = verifyRange(vNewSchedule.Day, vActualScheduleProject.Day, vActualScheduleProject.StartHour,
                                                                        vActualScheduleProject.EndHour, vNewSchedule.StartHour, vNewSchedule.EndHour);
                    if (vIsScheduleCorrect)
                        return vIsScheduleCorrect;
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
        private bool existScheduleConflictGroupinGroup(int pProfessorID, int pGroupID, int pPeriodID)
        {
            /*Get Group from database accordin to pGroupID*/
            var vListScheduleGroup = (from grupo in db.Groups
                                      join grupo_aula in db.GroupClassrooms on grupo.ID equals grupo_aula.GroupID
                                      join horario in db.Schedules on grupo_aula.ScheduleID equals horario.ID
                                      where (grupo.ID == pGroupID)
                                      select new { horario.StartHour, horario.EndHour, horario.Day }).ToList();


            //Get the day, starthour and endhour where professor was assign in commission
            var group_schedule = db.SP_getProfessorScheduleGroup(pProfessorID, pPeriodID).ToList();

            //Verify each scheedule with the new assign information
            foreach (var vNewSchedule in vListScheduleGroup)
            {
                foreach (var vActualScheduleCommission in group_schedule)
                {
                    bool vIsScheduleCorrect = verifyRange(vNewSchedule.Day, vActualScheduleCommission.Day, vActualScheduleCommission.StartHour,
                                                                        vActualScheduleCommission.EndHour, vNewSchedule.StartHour, vNewSchedule.EndHour);
                    if (vIsScheduleCorrect)
                        return vIsScheduleCorrect;
                }
            }
            return false;
        }

#endregion

        #region Commission
        /// <summary>
        /// Check if a professor is already assig in a project
        /// </summary>
        /// <param name="pCommissionID"></param>
        /// <param name="pProfessorID"></param>
        /// <returns>if professor is already assign return true el return false</returns>
        private bool isProfessorAssigntoCommission(int pCommissionID, int pProfessorID, int pPeriodID )
        {
            var getAssign = (from commission_profesor in db.CommissionsXProfessors
                             join professor in db.Professors on commission_profesor.ProfessorID equals professor.ID
                             join period in db.Periods on commission_profesor.PeriodID equals period.ID
                             where commission_profesor.CommissionID == pCommissionID & period.ID == pPeriodID
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

        /// <summary>
        /// Esteban Segura Benavides
        /// Check posibles conflicts with the new project schedule and the all commission schedule related with determinated professor
        /// </summary>
        /// <param name="pProfessorID"></param>
        /// <param name="pSchedules"></param>
        /// <returns>true if found any problem with the schedules</returns>
        private bool existScheduleConflictCommissionwithActualCommission(int pProfessorID, int pPeriodID, List<ScheduleComission> pSchedules)
        {
            //Get the day, starthour and endhour where professor was assign in commission
            var commission_schedule = db.SP_getProfessorScheduleCommission(pProfessorID, pPeriodID).ToList();

            //Verify each scheedule with the new assign information
            foreach (var vNewSchedule in pSchedules)
            {
                foreach (var vActualScheduleCommission in commission_schedule)
                {
                    bool vIsScheduleCorrect = verifyRange(vNewSchedule.Day, vActualScheduleCommission.Day,vActualScheduleCommission.StartHour,
                                                                        vActualScheduleCommission.EndHour,vNewSchedule.StartHour, vNewSchedule.EndHour);
                    if (vIsScheduleCorrect)
                        return vIsScheduleCorrect;
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
        private bool existScheduleConflictProjectwithActualCommission(int pProfessorID, int pPeriodID, List<ScheduleComission> pSchedules)
        {
            //Get the day, starthour and endhour where professor was assign in commission
            var project_schedule = db.SP_getProfessorScheduleProject(pProfessorID, pPeriodID).ToList();

            //Verify each scheedule with the new assign information
            foreach (var vNewSchedule in pSchedules)
            {
                foreach (var vActualScheduleCommission in project_schedule)
                {
                    bool vIsScheduleCorrect = verifyRange(vNewSchedule.Day, vActualScheduleCommission.Day,vActualScheduleCommission.StartHour,
                                                                        vActualScheduleCommission.EndHour,vNewSchedule.StartHour,vNewSchedule.EndHour);
                    if (vIsScheduleCorrect)
                        return vIsScheduleCorrect;
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
        private bool existScheduleConflictGroupwithActualCommission(int pProfessorID, int pPeriodID, List<ScheduleComission> pSchedules)
        {
            //Get the day, starthour and endhour where professor was assign in commission
            var group_schedule = db.SP_getProfessorScheduleGroup(pProfessorID, pPeriodID).ToList();

            //Verify each scheedule with the new assign information
            foreach (var vNewSchedule in pSchedules)
            {
                foreach (var vActualScheduleCommission in group_schedule)
                {
                    bool vIsScheduleCorrect = verifyRange(vNewSchedule.Day, vActualScheduleCommission.Day,vActualScheduleCommission.StartHour,
                                                            vActualScheduleCommission.EndHour, vNewSchedule.StartHour,vNewSchedule.EndHour);
                    if (vIsScheduleCorrect)
                        return vIsScheduleCorrect;
                }
            }
            return false;
        }

       

#endregion

        #region Project
        /// <summary>
        /// <autor>Esteban Segura Benavides</autor>
        /// Check posibles conflicts with the new project schedule and the all commission schedule related with determinated professor
        /// </summary>
        /// <param name="pProfessorID"></param>
        /// <param name="pSchedules"></param>
        /// <returns>true if found any problem with the schedules</returns>
        private bool existScheduleConflictCommissionwithActualProject(int pProfessorID, int pPeriodID,List<ScheduleProject> pSchedules)
        {
            //Get the day, starthour and endhour where professor was assign in commission
            var commission_schedule = db.SP_getProfessorScheduleCommission(pProfessorID, pPeriodID).ToList();

            //Verify each scheedule with the new assign information
            foreach (var vNewSchedule in pSchedules)
            {
                foreach (var vActualScheduleCommission in commission_schedule)
                {
                    bool vIsScheduleCorrect = verifyRange(vNewSchedule.Day, vActualScheduleCommission.Day, vActualScheduleCommission.StartHour,
                                                          vActualScheduleCommission.EndHour, vNewSchedule.StartHour,vNewSchedule.EndHour);
                    if (vIsScheduleCorrect)
                        return vIsScheduleCorrect;
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
        public bool existScheduleConflictProjectwithActualProject(int pProfessorID,int pPeriodID, List<ScheduleProject> pSchedules)
        {
            //Get the day, starthour and endhour where professor was assign in commission
            var project_schedule = db.SP_getProfessorScheduleProject(pProfessorID, pPeriodID).ToList();

            //Verify each scheedule with the new assign information
            foreach (var vNewSchedule in pSchedules)
            {
                foreach (var vActualScheduleProject in project_schedule)
                {
                    bool vIsScheduleCorrect = verifyRange(vNewSchedule.Day, vActualScheduleProject.Day, vActualScheduleProject.StartHour,
                                                         vActualScheduleProject.EndHour,vNewSchedule.StartHour, vNewSchedule.EndHour);
                    if (vIsScheduleCorrect)
                        return vIsScheduleCorrect;
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
        private bool existScheduleConflictGroupwithProject(int pProfessorID,int pPeriodID, List<ScheduleProject> pSchedules)
        {
            //Get the day, starthour and endhour where professor was assign in commission
            var project_schedule = db.SP_getProfessorScheduleGroup(pProfessorID, pPeriodID).ToList();

            //Verify each scheedule with the new assign information
            foreach (var vNewSchedule in pSchedules)
            {
                foreach (var vActualScheduleProject in project_schedule)
                {
                    bool vIsScheduleCorrect= verifyRange(vNewSchedule.Day, vActualScheduleProject.Day, vActualScheduleProject.StartHour,
                                                                    vActualScheduleProject.EndHour,vNewSchedule.StartHour,vNewSchedule.EndHour);
                    if(vIsScheduleCorrect)
                        return vIsScheduleCorrect;
                }
            }
            return false;
        }

    

        /// <summary>
        /// Check if a professor is already assig in a project
        /// </summary>
        /// <param name="pProjectID"></param>
        /// <param name="pProfessorID"></param>
        /// <returns>if professor is already assign return true el return false</returns>
        private bool isProfessorAssigntoProject(int pProjectID, int pProfessorID, int pPeriodID)
        {
            var getAssign = (from project_profesor in db.ProjectsXProfessors
                             join professor in db.Professors on project_profesor.ProfessorID equals professor.ID
                             join period in db.Periods on project_profesor.PeriodID equals period.ID
                             where project_profesor.ProjectID == pProjectID & period.ID == pPeriodID
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
        #endregion

        private bool verifyRange(string pNewScheduleDay,string pActualScheduleDay, string pActualStartHour, string pActualEndHour, 
                                string pNewStartHour, string pNewEndHour)
        {
            if (pNewScheduleDay.Equals(pActualScheduleDay))
            {
                DateTime vActualStartHour = DateTime.Parse(pActualStartHour);
                DateTime vActualEndHour = DateTime.Parse(pActualEndHour);
                DateTime vNewStartHour = DateTime.Parse(pNewStartHour);
                DateTime vNewEndHour = DateTime.Parse(pNewEndHour);

                //Check the range of the schedule
                if ((vActualStartHour <= vNewStartHour && vNewStartHour <= vActualEndHour) ||
                    (vActualStartHour <= vNewEndHour && vNewEndHour <= vActualEndHour) ||
                    (vNewStartHour <= vActualStartHour && vActualStartHour <= vNewEndHour) ||
                    (vNewStartHour <= vActualEndHour && vActualEndHour <= vNewEndHour))
                {
                    return true;
                }
            }
            return false;
        }
    }
}