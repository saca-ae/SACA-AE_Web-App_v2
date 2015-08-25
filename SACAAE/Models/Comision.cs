using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace SACAAE.Models
{
    public class Comision
    {
        public int ID { get; set; }
        public string Name { get; set; }
        [Column(TypeName = "Date")]
        public DateTime Start { get; set; }
        [Column(TypeName = "Date")]
        public DateTime End { get; set; }
        [ForeignKey("Estado")]
        public int? StateID { get; set; }
        [ForeignKey("TipoEntidad")]
        public int? EntityTypeID { get; set; }

        public virtual Estado Estado { get; set; }
        public virtual TipoEntidad TipoEntidad { get; set; }
        public virtual ICollection<ComisionXProfesor> ComisionesXProfesores { get; set; }
    }
}