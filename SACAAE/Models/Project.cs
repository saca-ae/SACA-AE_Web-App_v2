using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace SACAAE.Models
{
    public class Project
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public DateTime? Start { get; set; }
        public DateTime? End { get; set; }
        [ForeignKey("State")]
        public int? StateID { get; set; }
        public string Link { get; set; }
        [ForeignKey("EntityType")]
        public int? EntityTypeID { get; set; }

        public virtual State State { get; set; }
        public virtual EntityType EntityType { get; set; }
        public virtual ICollection<ProjectXProfessor> ProjectsXProfessors { get; set; }
    }
}