using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace SACAAE.Models
{
    public class PlanDeEstudioXSede
    {
        public int ID { get; set; }
        [ForeignKey("Sede")]
        public int SedeID { get; set; }
        [ForeignKey("PlanDeEstudio")]
        public int StudyPlanID { get; set; }

        public virtual PlanDeEstudio PlanDeEstudio { get; set; }
        public virtual Sede Sede { get; set; }
    }
}