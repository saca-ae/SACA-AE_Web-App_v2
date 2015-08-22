using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace SACAAE.Models
{
    public class DetalleGrupo
    {
        public int ID { get; set; }
        [ForeignKey("ProfesorXCurso")]
        public int? Professor { get; set; }
        public int? Capacity { get; set; }

        [Required]
        public virtual Grupo Grupo { get; set; }
        public virtual ProfesorXCurso ProfesorXCurso { get; set; }
        public virtual ICollection<GrupoAula> GrupoAulas { get; set; }
    }
}