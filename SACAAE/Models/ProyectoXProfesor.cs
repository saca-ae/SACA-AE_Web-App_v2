using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace SACAAE.Models
{
    public class ProyectoXProfesor
    {
        public int ID { get; set; }
        [ForeignKey("Proyecto")]
        public int ProjectID { get; set; }
        [ForeignKey("Profesor")]
        public int ProfessorID { get; set; }
        [ForeignKey("TipoAsignacionProfesor")]
        public int? AssignProfessorTypeID { get; set; }
        [ForeignKey("Periodo")]
        public int PeriodID { get; set; }
        public int? Hours { get; set; }

        public virtual Periodo Periodo { get; set; }
        public virtual Proyecto Proyecto { get; set; }
        public virtual Profesor Profesor { get; set; }
        public virtual TipoAsignacionProfesor TipoAsignacionProfesor { get; set; }
        public virtual ICollection<Horario> Horario { get; set; }
    }
}