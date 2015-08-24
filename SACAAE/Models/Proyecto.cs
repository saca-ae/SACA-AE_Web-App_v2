using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace SACAAE.Models
{
    public class Proyecto
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public DateTime? Start { get; set; }
        public DateTime? End { get; set; }
        [ForeignKey("Estado")]
        public int? StateID { get; set; }
        public string Link { get; set; }
        [ForeignKey("TipoEntidad")]
        public int? EntityTypeID { get; set; }

        public virtual Estado Estado { get; set; }
        public virtual ICollection<ProyectoXProfesor> ProyectosXProfesores { get; set; }
        public virtual TipoEntidad TipoEntidad { get; set; }
    }
}