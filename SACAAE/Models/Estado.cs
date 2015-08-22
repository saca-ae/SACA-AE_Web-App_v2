using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SACAAE.Models
{
    public class Estado
    {
        public int ID { get; set; }
        public string Name { get; set; }

        public virtual ICollection<Comision> Comisiones { get; set; }
        public virtual ICollection<Proyecto> Proyectos { get; set; }
        public virtual ICollection<Profesor> Profesores { get; set; }
    }
}