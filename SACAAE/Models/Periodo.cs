using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SACAAE.Models
{
    public class Periodo
    {
        public int ID { get; set; }
        public string Name { get; set; }

        public virtual ICollection<ComisionXProfesor> ComisionesXProfesores { get; set; }
        public virtual ICollection<Grupo> Grupos { get; set; }
        public virtual ICollection<ProyectoXProfesor> ProyectosXProfesores { get; set; }
    }
}