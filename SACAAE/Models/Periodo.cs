using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace SACAAE.Models
{
    public class Periodo
    {
        public int ID { get; set; }
        public int Year { get; set; }
        [ForeignKey("Numero")]
        public int NumberID { get; set; }
        
        public virtual NumeroPeriodo Numero { get; set; }
        public virtual ICollection<ComisionXProfesor> ComisionesXProfesores { get; set; }
        public virtual ICollection<Grupo> Grupos { get; set; }
        public virtual ICollection<ProyectoXProfesor> ProyectosXProfesores { get; set; }
    }
}