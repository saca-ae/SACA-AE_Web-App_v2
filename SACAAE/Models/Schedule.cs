using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace SACAAE.Models
{
    public class Schedule
    {
        public int ID { get; set; }
        public string Day { get; set; }
        public string StartHour { get; set; }
        public string EndHour { set; get; }

        public virtual ICollection<CommissionXProfessor> CommissionsXProfessors { get; set; }
        public virtual ICollection<ProjectXProfessor> ProjectsXProfessors { get; set; }
        public virtual ICollection<GroupClassroom> GroupsClassroom { get; set; }
    }
}