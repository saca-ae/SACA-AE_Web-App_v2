using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SACAAE.Models
{
    public class Plaza
    {
        public int ID { get; set; }
        public string Code { get; set; }
        public string PlazaType { get; set; }
        public string TimeType { get; set; }
        public int? TotalHours { get; set; }
        public int? EffectiveTime { get; set; }

        public virtual ICollection<PlazaXProfessor> PlazasXProfessors { get; set; }
    }
}