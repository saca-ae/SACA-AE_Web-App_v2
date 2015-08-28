using SACAAE.Data_Access;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace SACAAE.Models
{
    public class Periodo
    {
        private SACAAEContext database = new SACAAEContext();

        public int ID { get; set; }
        public int Year { get; set; }
        [ForeignKey("Numero")]
        public int NumberID { get; set; }
        
        public virtual NumeroPeriodo Numero { get; set; }
        public virtual ICollection<ComisionXProfesor> ComisionesXProfesores { get; set; }
        public virtual ICollection<Grupo> Grupos { get; set; }
        public virtual ICollection<ProyectoXProfesor> ProyectosXProfesores { get; set; }

        public class NuevoPeriodo
        {
            public int Number { get; set; }
            public int Year { get; set; }
        }

        public Periodo GetNextPeriod(String pPeriodType)
        {
            int vNumber = 0, vYear = 0;
            NuevoPeriodo vLastPeriod =
                (from TipoPeriodo TP in database.TiposPeriodo
                 from NumeroPeriodo N in database.PeriodoAño
                 from Periodo P in database.Periodos
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

            Periodo vPeriod = new Periodo();
            vPeriod.NumberID = vNumber;
            vPeriod.Year = vYear;

            return vPeriod;
        }

        public int getIDPeriodNumber(int pPeriodNumber, String pPeriodType) 
        {
            return (from NumeroPeriodo in database.PeriodoAño
                    join TipoPeriodo in database.TiposPeriodo on NumeroPeriodo.TypeID equals TipoPeriodo.ID
                    where TipoPeriodo.Name == pPeriodType
                    where NumeroPeriodo.Number == pPeriodNumber
                    select NumeroPeriodo).FirstOrDefault().ID;
        }

        public int getIDPeriod(int pPeriodYear, int pPeriodNumberID)
        {
            return (from Periodo P in database.Periodos
                    where P.NumberID == pPeriodNumberID
                    where P.Year == pPeriodYear
                    select P).FirstOrDefault().ID;
        }

        public Periodo AddNewSemester() 
        {
            Periodo vPeriod = GetNextPeriod("Semestre");
            AddPeriod(vPeriod);
            return vPeriod;
        }

        public void AddPeriod(Periodo pPeriod)
        {
            database.Periodos.Add(pPeriod);
            database.SaveChanges();
        }

    }
}