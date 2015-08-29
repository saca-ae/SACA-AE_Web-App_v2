using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SACAAE.Models
{
    public class AcademicBlock
    {
        public int ID { get; set; }
        public string Description { get; set; }
        public int Level { get; set; }

        public virtual ICollection<AcademicBlockXStudyPlan> AcademicBlocksXStudyPlans { get; set; }
    }
}