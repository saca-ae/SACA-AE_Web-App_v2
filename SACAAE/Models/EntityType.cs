using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SACAAE.Models
{
    public class EntityType
    {
        public int ID { get; set; }
        public string Name { get; set; }

        public virtual ICollection<Commission> Commissions { get; set; }
        public virtual ICollection<StudyPlan> StudyPlans { get; set; }
        public virtual ICollection<Project> Projects { get; set; }
    }
}