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
        public int PeriodID { get; set; }
        [ForeignKey("BloqueXPlanXCurso")]
        public int BlockXPlanXCourseID { get; set; }
        [ForeignKey("Profesor")]
        public int? ProfessorID { get; set; }
        public int? Capacity { get; set; }

        public virtual BloqueXPlanXCurso BloqueXPlanXCurso { get; set; }
        public virtual Periodo Periodo { get; set; }
        public virtual Profesor Profesor { get; set; }
        public virtual ICollection<GrupoAula> GrupoAulas { get; set; }
    }
}