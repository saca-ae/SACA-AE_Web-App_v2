using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace SACAAE.Models
{
    public class CommissionXProfessor
    {
        public int ID { get; set; }
        [ForeignKey("Commission")]
        public int CommissionID { get; set; }
        [ForeignKey("Professor")]
        public int ProfessorID { get; set; }
        [ForeignKey("HourAllocatedType")]
        public int? HourAllocatedTypeID { get; set; }
        [ForeignKey("Period")]
        public int PeriodID { get; set; }
        public int? Hours { get; set; }

        public virtual Commission Commission { get; set; }
        public virtual Period Period { get; set; }
        public virtual Professor Professor { get; set; }
        public virtual HourAllocatedType HourAllocatedType { get; set; }
        public virtual ICollection<Schedule> Schedule { get; set; }
    }
}