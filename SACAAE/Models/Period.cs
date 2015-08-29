using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace SACAAE.Models
{
    public class Period
    {
        public int ID { get; set; }
        public int Year { get; set; }
        [ForeignKey("Number")]
        public int NumberID { get; set; }
        
        public virtual PeriodNumber Number { get; set; }
        public virtual ICollection<CommissionXProfessor> CommissionsXProfessors { get; set; }
        public virtual ICollection<Group> Groups { get; set; }
        public virtual ICollection<ProjectXProfessor> ProjectsXProfessors { get; set; }
    }
}