using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace SACAAE.Models
{
    public class StudyPlan
    {
        public int ID { get; set; }
        public string Name { get; set; }
        [ForeignKey("Modality")]
        public int ModeID { get; set; }
        [ForeignKey("EntityType")]
        public int? EntityTypeID { get; set; }

        public virtual Modality Modality { get; set; }
        public virtual EntityType EntityType { get; set; }
        public virtual ICollection<StudyPlanXSede> StudyPlansXSedes { get; set; }
        public virtual ICollection<AcademicBlockXStudyPlan> AcademicBlocksXStudyPlans { get; set; }
    }
}