using System;

namespace SACAAE.Models
{
    /// <summary>
    /// This class is used to handle CSV files.
    /// </summary>
    class DataCSV
    {

        public DataCSV() { }

        public DataCSV(String pGroup, String pName, String pProfessor, String pDay, String pStartHour,
                       String pEndHour, String pHeadQuarter, String pClassroom, String pStudyPlan, String pModality)
        {
            this.Grupo = pGroup;
            this.Nombre = pName;
            this.Profesor = pProfessor;
            this.Dia = pDay;
            this.HoraInicio = pStartHour;
            this.HoraFin = pEndHour;
            this.Sede = pHeadQuarter;
            this.Aula = pClassroom;
            this.PlandeEstudio = pStudyPlan;
            this.Modalidad = pModality;
        }
        public String Grupo { get; set; }
        public String Nombre { get; set; }
        public String Profesor { get; set; }
        public String Dia { get; set; }
        public String HoraInicio { get; set; }
        public String HoraFin { get; set; }
        public String Sede { get; set; }
        public String Aula { get; set; }
        public String PlandeEstudio { get; set; }
        public String Modalidad { get; set; }

    }
}