using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SACAAE.Models
{
    public class Sede
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public int GroupEnum { get; set; }

        public virtual ICollection<Classroom> Classrooms { get; set; }
        public virtual ICollection<StudyPlanXSede> StudyPlansXSedes { get; set; }
    }
}