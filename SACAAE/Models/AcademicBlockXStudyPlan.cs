using SACAAE.Data_Access;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace SACAAE.Models
{
    public class AcademicBlockXStudyPlan
    {
        public int ID { get; set; }
        [ForeignKey("StudyPlan")]
        public int PlanID { get; set; }
        [ForeignKey("AcademicBlock")]
        public int BlockID { get; set; }

        public virtual AcademicBlock AcademicBlock { get; set; }
        public virtual StudyPlan StudyPlan { get; set; }
        public virtual ICollection<BlockXPlanXCourse> BlocksXPlansXCourses { get; set; }
        
    }
}