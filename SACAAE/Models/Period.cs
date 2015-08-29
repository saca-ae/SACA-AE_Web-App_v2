using SACAAE.Data_Access;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace SACAAE.Models
{
    public class Period
    {
        private SACAAEContext gvDatabase = new SACAAEContext();

        public int ID { get; set; }
        public int Year { get; set; }
        [ForeignKey("Number")]
        public int NumberID { get; set; }
        
        public virtual PeriodNumber Number { get; set; }
        public virtual ICollection<CommissionXProfessor> CommissionsXProfessors { get; set; }
        public virtual ICollection<Group> Groups { get; set; }
        public virtual ICollection<ProjectXProfessor> ProjectsXProfessors { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <author> Cristian Araya Fuentes </author> 
        /// <param name=pPeriodType> Name of the period type</param>
        /// <returns></returns>
        public Period GetNextPeriod(String pPeriodType)
        {
            int vNumber = 0, vYear = 0;
            NuevoPeriodo vLastPeriod =
                (from PeriodType TP in gvDatabase.PeriodTypes
                 from PeriodNumber N in gvDatabase.PeriodNumbers
                 from Period P in gvDatabase.Periods
                 where TP.Name == pPeriodType
                 where N.ID == P.NumberID
                 where TP.ID == N.TypeID
                 orderby P.Year descending, N.Number descending
                 select new NuevoPeriodo { Number = N.Number, Year = P.Year }).FirstOrDefault();

            vNumber = vLastPeriod.Number;
            vYear = vLastPeriod.Year;

            if ((vNumber != 0) && (vYear != 0)) 
            {
                if (pPeriodType == "Semestre") 
                {
                    if (vNumber == 2) { vNumber = 1; vYear += 1; }
                    else { vNumber = 2; }
                }
            }

            Period vPeriod = new Period();
            vPeriod.NumberID = vNumber;
            vPeriod.Year = vYear;

            return vPeriod;
        }

        public int getIDPeriodNumber(int pPeriodNumber, String pPeriodType) 
        {
            return (from NumeroPeriodo in gvDatabase.PeriodNumbers
                    join TipoPeriodo in gvDatabase.PeriodTypes on NumeroPeriodo.TypeID equals TipoPeriodo.ID
                    where TipoPeriodo.Name == pPeriodType
                    where NumeroPeriodo.Number == pPeriodNumber
                    select NumeroPeriodo).FirstOrDefault().ID;
        }

        public int getIDPeriod(int pPeriodYear, int pPeriodNumberID)
        {
            return (from Period P in gvDatabase.Periods
                    where P.NumberID == pPeriodNumberID
                    where P.Year == pPeriodYear
                    select P).FirstOrDefault().ID;
        }

        public Period AddNewSemester() 
        {
            Period vPeriod = GetNextPeriod("Semestre");
            AddPeriod(vPeriod);
            return vPeriod;
        }

        public void AddPeriod(Period pPeriod)
        {
            gvDatabase.Periods.Add(pPeriod);
            gvDatabase.SaveChanges();
        }

        public class NuevoPeriodo
        {
            public int Number { get; set; }
            public int Year { get; set; }
        }

    }
}