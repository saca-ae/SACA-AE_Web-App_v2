using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace SACAAE.Models
{
    public class BloqueAcademicoXPlanDeEstudio
    {
        public int ID { get; set; }
        [ForeignKey("PlanDeEstudio")]
        public int PlanID { get; set; }
        [ForeignKey("BloqueAcademico")]
        public int BlockID { get; set; }

        public virtual BloqueAcademico BloqueAcademico { get; set; }
        public virtual PlanDeEstudio PlanDeEstudio { get; set; }
        public virtual ICollection<BloqueXPlanXCurso> BloquesXPlanesXCursos { get; set; }
    }
}