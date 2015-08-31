using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace SACAAE.Models
{
    public class StudyPlanXSede
    {
        public int ID { get; set; }
        [ForeignKey("Sede")]
        public int SedeID { get; set; }
        [ForeignKey("StudyPlan")]
        public int StudyPlanID { get; set; }

        public virtual StudyPlan StudyPlan { get; set; }
        public virtual Sede Sede { get; set; }
    }
}