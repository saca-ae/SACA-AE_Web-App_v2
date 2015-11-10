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
            bool isCommissionShock = existShockScheduleCommissioninGroup(vProfessorID, pGroupID, pPeriodID);
            //if exist shock with the schedule, the system doesn't let assign new projects in that schedule
            if (!isCommissionShock)
            {
                //if exist shock with the schedule, the system doesn't let assign new projects in that schedule
                bool isProjectShock = existShockScheduleProjectinGroup(vProfessorID, pGroupID, pPeriodID);
                if (!isProjectShock)
                {
                    //if exist shock with the schedule, the system doesn't let assign new projects in that schedule
                    bool isGroupShock = existShockScheduleGroupinGroup(vProfessorID, pGroupID, pPeriodID);
                    if (!isGroupShock)
                    {
                        return "true";
                    }
                    else
                    {
                        return "falseIsGroupSchock";
                    }
                }
                else
                {
                    return "falseIsProjectSchock";
                }
            }
            else
            {
                return "falseIsCommissionSchock";
            }
        }

        public string validationsEditGroup(int pGroupID, int pProfessorID, int pPeriodID)
        {

            //Check the schedule of the commissions related with the professor
            bool isCommissionShock = existShockScheduleCommissioninGroup(pProfessorID, pGroupID, pPeriodID);
            //if exist shock with the schedule, the system doesn't let assign new projects in that schedule
            if (!isCommissionShock)
            {
                //if exist shock with the schedule, the system doesn't let assign new projects in that schedule
                bool isProjectShock = existShockScheduleProjectinGroup(pProfessorID, pGroupID, pPeriodID);
                if (!isProjectShock)
                {
                    //if exist shock with the schedule, the system doesn't let assign new projects in that schedule
                    bool isGroupShock = existShockScheduleGroupWithoutGroupSelect(pProfessorID, pGroupID, pPeriodID);
                    if (!isGroupShock)
                    {
                        return "true";
                    }
                    else
                    {
                        return "falseIsGroupSchock";
                    }
                }
                else
                {
                    return "falseIsProjectSchock";
                }
            }
            else
            {
                return "falseIsCommissionSchock";
            }
        }

        /// <summary>
        /// <autor>Esteban Segura Benavides</autor>
        /// Check all posibles shocks in all schedules of the professor
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
                bool isCommissionShock = existShockScheduleCommissionwithActualCommission(pProfessorID, pPeriodID, pSchedules);
                //if exist shock with the schedule, the system doesn't let assign new projects in that schedule
                if (!isCommissionShock)
                {
                    //if exist shock with the schedule, the system doesn't let assign new projects in that schedule
                    bool isProjectShock = existShockScheduleProjectwithActualCommission(pProfessorID, pPeriodID, pSchedules);
                    if (!isProjectShock)
                    {
                        //if exist shock with the schedule, the system doesn't let assign new projects in that schedule
                        bool isGroupShock = existShockScheduleGroupwithActualCommission(pProfessorID, pPeriodID, pSchedules);
                        if (!isGroupShock)
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
            else
            {
                return "falseIsProfessorShock";
            }
        }

        public string validationsEditCommisson(int pCommissionID, int pProfessorID, int pPeriodID, List<ScheduleComission> pSchedules)
        {
            bool isCommissionShock = existShockScheduleCommissionWithoutCommissionSelect(pProfessorID, pPeriodID, pSchedules, pCommissionID);
            if (!isCommissionShock)
            {
                //if exist shock with the schedule, the system doesn't let assign new projects in that schedule
                bool isProjectShock = existShockScheduleProjectwithActualCommission(pProfessorID, pPeriodID, pSchedules);
                if (!isProjectShock)
                {
                    //if exist shock with the schedule, the system doesn't let assign new projects in that schedule
                    bool isGroupShock = existShockScheduleGroupwithActualCommission(pProfessorID, pPeriodID, pSchedules);
                    if (!isGroupShock)
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

        public string validationProject(int pProjectID, int pProfessorID,int pPeriodID, List<ScheduleProject> pSchedule)
        {
            bool vIsProfessorAssign = isProfessorAssigntoProject(pProjectID,pProfessorID,pPeriodID);
            if (!vIsProfessorAssign)
            {
                bool isProjectShock = existShockScheduleProjectwithActualProject(pProfessorID, pPeriodID, pSchedule);
                if (!isProjectShock)
                {
                    //if exist shock with the schedule, the system doesn't let assign new projects in that schedule
                    bool isCommissionShock = existShockScheduleCommissionwithActualProject(pProfessorID, pPeriodID, pSchedule);
                    if (!isCommissionShock)
                    {
                        //if exist shock with the schedule, the system doesn't let assign new projects in that schedule
                        bool isGroupShock = existShockScheduleGroupwithProject(pProfessorID, pPeriodID, pSchedule);
                        if (!isGroupShock)
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
                        return "falseIsCommissionShock";
                    }
                }
                else
                {
                    return "falseIsProjectShock";
                }
            }
            else
            {
                return "falseIsProfessorShock";
            }
        }
        public string validationsEditProject(int pProjectID, int pProfessorID, int pPeriodID,List<ScheduleProject> pSchedule)
        {
            bool isProjectShock = existShockScheduleProjectWithoutProject(pProfessorID, pPeriodID,pSchedule,pProjectID);
            if (!isProjectShock)
            {
                //if exist shock with the schedule, the system doesn't let assign new projects in that schedule
                bool isCommissionShock = existShockScheduleCommissionwithActualProject(pProfessorID,pPeriodID, pSchedule);
                if (!isCommissionShock)
                {
                    //if exist shock with the schedule, the system doesn't let assign new projects in that schedule
                    bool isGroupShock = existShockScheduleGroupwithProject(pProfessorID,pPeriodID, pSchedule);
                    if (!isGroupShock)
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
                    return "falseIsCommissionShock";
                }
            }
            else
            {
                return "falseIsProjectShock";
            }
        }
        private bool existShockScheduleGroupWithoutGroupSelect(int pProfessorID, int pGroupID, int pPeriodID)
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
                    if (vActualScheduleCommission.ID != pGroupID)
                    {
                        if (vNewSchedule.Day.Equals(vActualScheduleCommission.Day))
                        {
                            var vActualStartHour = DateTime.Parse(vActualScheduleCommission.StartHour);
                            var vActualEndHour = DateTime.Parse(vActualScheduleCommission.EndHour);
                            var vNewStartHour = DateTime.Parse(vNewSchedule.StartHour);
                            var vNewEndHour = DateTime.Parse(vNewSchedule.EndHour);

                            //Check the range of the schedule
                            if ((vActualStartHour <= vNewStartHour && vNewStartHour <= vActualEndHour) ||
                                (vActualStartHour <= vNewEndHour && vNewEndHour <= vActualEndHour) ||
                                (vNewStartHour <= vActualStartHour && vActualStartHour <= vNewEndHour) ||
                                (vNewStartHour <= vActualEndHour && vActualEndHour <= vNewEndHour))
                            {
                                return true;
                            }

                        }
                    }
                }
            }
            return false;
        }


        private bool existShockScheduleCommissionWithoutCommissionSelect(int pProfessorID,int pPeriodID, List<ScheduleComission> pSchedules, int pCommissionID)
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
                        if (vNewSchedule.Day.Equals(vActualScheduleCommission.Day))
                        {
                            var vActualStartHour = DateTime.Parse(vActualScheduleCommission.StartHour);
                            var vActualEndHour = DateTime.Parse(vActualScheduleCommission.EndHour);
                            var vNewStartHour = DateTime.Parse(vNewSchedule.StartHour);
                            var vNewEndHour = DateTime.Parse(vNewSchedule.EndHour);

                            //Check the range of the schedule
                            if ((vActualStartHour <= vNewStartHour && vNewStartHour <= vActualEndHour) ||
                                (vActualStartHour <= vNewEndHour && vNewEndHour <= vActualEndHour) ||
                                (vNewStartHour <= vActualStartHour && vActualStartHour <= vNewEndHour) ||
                                (vNewStartHour <= vActualEndHour && vActualEndHour <= vNewEndHour))
                            {
                                return true;
                            }
                        }
                    }
                }
            }
            return false;
        }

        public bool existShockScheduleProjectWithoutProject(int pProfessorID,int pPeriodID,List<ScheduleProject> pSchedules, int pProjectID)
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
                        if (vNewSchedule.Day.Equals(vActualScheduleProject.Day))
                        {
                            var vActualStartHour = DateTime.Parse(vActualScheduleProject.StartHour);
                            var vActualEndHour = DateTime.Parse(vActualScheduleProject.EndHour);
                            var vNewStartHour = DateTime.Parse(vNewSchedule.StartHour);
                            var vNewEndHour = DateTime.Parse(vNewSchedule.EndHour);

                            //Check the range of the schedule
                            if ((vActualStartHour <= vNewStartHour && vNewStartHour <= vActualEndHour) ||
                                (vActualStartHour <= vNewEndHour && vNewEndHour <= vActualEndHour) ||
                                (vNewStartHour <= vActualStartHour && vActualStartHour <= vNewEndHour) ||
                                (vNewStartHour <= vActualEndHour && vActualEndHour <= vNewEndHour))
                            {
                                return true;
                            }
                        }

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
        private bool existShockScheduleCommissioninGroup(int pProfessorID, int pGroupID, int pPeriodID)
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
                    if (vNewSchedule.Day.Equals(vActualScheduleCommission.Day))
                    {
                        var vActualStartHour = DateTime.Parse(vActualScheduleCommission.StartHour);
                        var vActualEndHour = DateTime.Parse(vActualScheduleCommission.EndHour);
                        var vNewStartHour = DateTime.Parse(vNewSchedule.StartHour);
                        var vNewEndHour = DateTime.Parse(vNewSchedule.EndHour);

                        //Check the range of the schedule
                        if ((vActualStartHour <= vNewStartHour && vNewStartHour <= vActualEndHour) ||
                            (vActualStartHour <= vNewEndHour && vNewEndHour <= vActualEndHour) ||
                            (vNewStartHour <= vActualStartHour && vActualStartHour <= vNewEndHour) ||
                            (vNewStartHour <= vActualEndHour && vActualEndHour <= vNewEndHour))
                        {
                            return true;
                        }
                    }
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
        private bool existShockScheduleProjectinGroup(int pProfessorID, int pGroupID, int pPeriodID)
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
                    if (vNewSchedule.Day.Equals(vActualScheduleProject.Day))
                    {
                        var vActualStartHour = DateTime.Parse(vActualScheduleProject.StartHour);
                        var vActualEndHour = DateTime.Parse(vActualScheduleProject.EndHour);
                        var vNewStartHour = DateTime.Parse(vNewSchedule.StartHour);
                        var vNewEndHour = DateTime.Parse(vNewSchedule.EndHour);

                        //Check the range of the schedule
                        if ((vActualStartHour <= vNewStartHour && vNewStartHour <= vActualEndHour) ||
                            (vActualStartHour <= vNewEndHour && vNewEndHour <= vActualEndHour) ||
                            (vNewStartHour <= vActualStartHour && vActualStartHour <= vNewEndHour) ||
                            (vNewStartHour <= vActualEndHour && vActualEndHour <= vNewEndHour))
                        {
                            return true;
                        }
                    }
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
        private bool existShockScheduleGroupinGroup(int pProfessorID, int pGroupID, int pPeriodID)
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
                    if (vNewSchedule.Day.Equals(vActualScheduleCommission.Day))
                    {
                        var vActualStartHour = DateTime.Parse(vActualScheduleCommission.StartHour);
                        var vActualEndHour = DateTime.Parse(vActualScheduleCommission.EndHour);
                        var vNewStartHour = DateTime.Parse(vNewSchedule.StartHour);
                        var vNewEndHour = DateTime.Parse(vNewSchedule.EndHour);

                        //Check the range of the schedule
                        if ((vActualStartHour <= vNewStartHour && vNewStartHour <= vActualEndHour) ||
                            (vActualStartHour <= vNewEndHour && vNewEndHour <= vActualEndHour) ||
                            (vNewStartHour <= vActualStartHour && vActualStartHour <= vNewEndHour) ||
                            (vNewStartHour <= vActualEndHour && vActualEndHour <= vNewEndHour))
                        {
                            return true;
                        }

                    }
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
        private bool existShockScheduleCommissionwithActualCommission(int pProfessorID, int pPeriodID, List<ScheduleComission> pSchedules)
        {
            //Get the day, starthour and endhour where professor was assign in commission
            var commission_schedule = db.SP_getProfessorScheduleCommission(pProfessorID, pPeriodID).ToList();

            //Verify each scheedule with the new assign information
            foreach (var vNewSchedule in pSchedules)
            {
                foreach (var vActualScheduleCommission in commission_schedule)
                {
                    bool vIsScheduleCorrect = verifyScheduleCommission(vNewSchedule, vActualScheduleCommission);
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
        private bool existShockScheduleProjectwithActualCommission(int pProfessorID, int pPeriodID, List<ScheduleComission> pSchedules)
        {
            //Get the day, starthour and endhour where professor was assign in commission
            var project_schedule = db.SP_getProfessorScheduleProject(pProfessorID, pPeriodID).ToList();

            //Verify each scheedule with the new assign information
            foreach (var vNewSchedule in pSchedules)
            {
                foreach (var vActualScheduleCommission in project_schedule)
                {
                    bool vIsScheduleCorrect = verifyScheduleCommission(vNewSchedule, vActualScheduleCommission);
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
        private bool existShockScheduleGroupwithActualCommission(int pProfessorID, int pPeriodID, List<ScheduleComission> pSchedules)
        {
            //Get the day, starthour and endhour where professor was assign in commission
            var group_schedule = db.SP_getProfessorScheduleGroup(pProfessorID, pPeriodID).ToList();

            //Verify each scheedule with the new assign information
            foreach (var vNewSchedule in pSchedules)
            {
                foreach (var vActualScheduleCommission in group_schedule)
                {
                    bool vIsScheduleCorrect = verifyScheduleCommission(vNewSchedule, vActualScheduleCommission);
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
        private bool existShockScheduleCommissionwithActualProject(int pProfessorID, int pPeriodID,List<ScheduleProject> pSchedules)
        {
            //Get the day, starthour and endhour where professor was assign in commission
            var commission_schedule = db.SP_getProfessorScheduleCommission(pProfessorID, pPeriodID).ToList();

            //Verify each scheedule with the new assign information
            foreach (var vNewSchedule in pSchedules)
            {
                foreach (var vActualScheduleCommission in commission_schedule)
                {
                    bool vIsScheduleCorrect = verifyScheduleProject(vNewSchedule, vActualScheduleCommission);
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
        public bool existShockScheduleProjectwithActualProject(int pProfessorID,int pPeriodID, List<ScheduleProject> pSchedules)
        {
            //Get the day, starthour and endhour where professor was assign in commission
            var project_schedule = db.SP_getProfessorScheduleProject(pProfessorID, pPeriodID).ToList();

            //Verify each scheedule with the new assign information
            foreach (var vNewSchedule in pSchedules)
            {
                foreach (var vActualScheduleProject in project_schedule)
                {
                    bool vIsScheduleCorrect = verifyScheduleProject(vNewSchedule, vActualScheduleProject);
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
        private bool existShockScheduleGroupwithProject(int pProfessorID,int pPeriodID, List<ScheduleProject> pSchedules)
        {
            //Get the day, starthour and endhour where professor was assign in commission
            var project_schedule = db.SP_getProfessorScheduleGroup(pProfessorID, pPeriodID).ToList();

            //Verify each scheedule with the new assign information
            foreach (var vNewSchedule in pSchedules)
            {
                foreach (var vActualScheduleProject in project_schedule)
                {
                    bool vIsScheduleCorrect= verifyScheduleProject(vNewSchedule, vActualScheduleProject);
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

        private bool verifyScheduleCommission(ScheduleComission pNewSchedule, ScheduleAssign pActualSchedule)
        {

            if (pNewSchedule.Day.Equals(pActualSchedule.Day))
            {
                var vActualStartHour = DateTime.Parse(pActualSchedule.StartHour);
                var vActualEndHour = DateTime.Parse(pActualSchedule.EndHour);
                var vNewStartHour = DateTime.Parse(pNewSchedule.StartHour);
                var vNewEndHour = DateTime.Parse(pNewSchedule.EndHour);

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

        private bool verifyScheduleProject(ScheduleProject pNewSchedule, ScheduleAssign pActualSchedule)
        {
            
            if (pNewSchedule.Day.Equals(pActualSchedule.Day))
            {
                var vActualStartHour = DateTime.Parse(pActualSchedule.StartHour);
                var vActualEndHour = DateTime.Parse(pActualSchedule.EndHour);
                var vNewStartHour = DateTime.Parse(pNewSchedule.StartHour);
                var vNewEndHour = DateTime.Parse(pNewSchedule.EndHour);

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