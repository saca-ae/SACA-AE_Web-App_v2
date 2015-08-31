using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace SACAAE.Models
{
    public class Professor
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string Link { get; set; }
        [ForeignKey("State")]
        public int? StateID { get; set; }
        public string Tel1 { get; set; }
        public string Tel2 { get; set; }
        public string Email { get; set; }

        public virtual State State { get; set; }
        public virtual ICollection<CommissionXProfessor> CommissionsXProfessors { get; set; }
        public virtual ICollection<PlazaXProfessor> PlazasXProfessors { get; set; }
        public virtual ICollection<Group> Groups { get; set; }
        public virtual ICollection<ProjectXProfessor> ProjectsXProfessors { get; set; }
    }
}