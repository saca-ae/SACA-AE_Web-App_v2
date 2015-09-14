using SACAAE.Data_Access;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace SACAAE.Models
{
    public class BlockXPlanXCourse
    {
        public int ID { get; set; }
        [ForeignKey("AcademicBlockXStudyPlan")]
        public int BlockXPlanID { get; set; }
        [ForeignKey("Course")]
        public int CourseID { get; set; }
        [ForeignKey("Sede")]
        public int? SedeID { get; set; }
        public int GroupsPerPeriods { get; set; }
        
        public virtual Course Course { get; set; }
        public virtual Sede Sede { get; set; }
        public virtual AcademicBlockXStudyPlan AcademicBlockXStudyPlan { get; set; }
        public virtual ICollection<Group> Groups { get; set; }

    }
}