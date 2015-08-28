using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace SACAAE.Models
{
    public class BloqueXPlanXCurso
    {
        public int ID { get; set; }
        [ForeignKey("BloqueAcademicoXPlanDeEstudio")]
        public int BlockXPlanID { get; set; }
        [ForeignKey("Curso")]
        public int CourseID { get; set; }
        [ForeignKey("Sede")]
        public int? SedeID { get; set; }
        public int GroupsPerPeriods { get; set; }
        
        public virtual Curso Curso { get; set; }
        public virtual Sede Sede { get; set; }
        public virtual BloqueAcademicoXPlanDeEstudio BloqueAcademicoXPlanDeEstudio { get; set; }
        public virtual ICollection<Grupo> Grupos { get; set; }
    }
}