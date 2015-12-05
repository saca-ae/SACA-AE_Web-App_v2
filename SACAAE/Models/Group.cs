using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace SACAAE.Models
{
    public class Group
    {
        public int ID { get; set; }
        public int Number { get; set; }
        [ForeignKey("Period")]
        public int PeriodID { get; set; }
        [ForeignKey("BlockXPlanXCourse")]
        public int BlockXPlanXCourseID { get; set; }
        [ForeignKey("Professor")]
        public int? ProfessorID { get; set; }
        [ForeignKey("HourAllocatedType")]
        public int? HourAllocatedTypeID { get; set; }
        public int? Capacity { get; set; }
        public int EstimatedHour { get; set; }
        public virtual Period Period { get; set; }
        public virtual Professor Professor { get; set; }
        public virtual HourAllocatedType HourAllocatedType { get; set; }
        public virtual BlockXPlanXCourse BlockXPlanXCourse { get; set; }
        public virtual ICollection<GroupClassroom> GroupsClassroom { get; set; }
    }
}