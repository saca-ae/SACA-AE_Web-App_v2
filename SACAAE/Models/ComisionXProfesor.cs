using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace SACAAE.Models
{
    public class ComisionXProfesor
    {
        public int ID { get; set; }
        [ForeignKey("Comision")]
        public int Commission { get; set; }
        [ForeignKey("Profesor")]
        public int Professor { get; set; }
        [ForeignKey("Periodo")]
        public int Period { get; set; }
        public int? Hours { get; set; }

        public virtual Comision Comision { get; set; }
        public virtual Periodo Periodo { get; set; }
        public virtual Profesor Profesor { get; set; }
        public virtual ICollection<Horario> Horario { get; set; }
    }
}