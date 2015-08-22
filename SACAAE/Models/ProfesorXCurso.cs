using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace SACAAE.Models
{
    public class ProfesorXCurso
    {
        public int ID { get; set; }
        [ForeignKey("Profesor")]
        public int Professor { get; set; }
        public int? Hours { get; set; }

        public virtual Profesor Profesor { get; set; }
        public virtual ICollection<DetalleGrupo> DetallesDelGrupo { get; set; }
    }
}