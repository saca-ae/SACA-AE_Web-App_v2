using SACAAE.Data_Access;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SACAAE.Models
{
    public class Modality
    {
        public int ID { get; set; }
        public string Name { get; set; }

        public virtual ICollection<StudyPlan> StudyPlans { get; set; }
    }
}