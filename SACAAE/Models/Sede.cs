using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SACAAE.Models
{
    public class Sede
    {
        public int ID { get; set; }
        public string Name { get; set; }

        public virtual ICollection<Aula> Aulas { get; set; }
        public virtual ICollection<PlanDeEstudioXSede> PlanesDeEstudioXSedes { get; set; }
    }
}