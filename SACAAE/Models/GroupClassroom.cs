using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace SACAAE.Models
{
    public class GroupClassroom
    {
        public int ID { get; set; }
        [ForeignKey("Group")]
        public int GroupID { get; set; }
        [ForeignKey("Classroom")]
        public int? ClassroomID { get; set; }
        [ForeignKey("Schedule")]
        public int? ScheduleID { get; set; }

        public virtual Group Group { get; set; }
        public virtual Classroom Classroom { get; set; }
        public virtual Schedule Schedule { get; set; }
    }
}