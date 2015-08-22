using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SACAAE.Models
{
    public class Modalidad
    {
        public int ID { get; set; }
        public string Name { get; set; }

        public virtual ICollection<PlanDeEstudio> PlanesDeEstudio { get; set; }
    }
}