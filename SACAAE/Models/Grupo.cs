using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace SACAAE.Models
{
    public class Grupo
    {
        public int ID { get; set; }
        public int Number { get; set; }
        [ForeignKey("Periodo")]
        public int Period { get; set; }
        [ForeignKey("BloqueXPlanXCurso")]
        public int BlockXPlanXCourse { get; set; }

        public virtual BloqueXPlanXCurso BloqueXPlanXCurso { get; set; }
        public virtual Periodo Periodo { get; set; }
        public virtual DetalleGrupo DetalleGrupo { get; set; }
    }
}