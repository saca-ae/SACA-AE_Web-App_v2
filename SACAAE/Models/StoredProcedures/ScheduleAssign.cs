using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SACAAE.Models.StoredProcedures
{
    public class ScheduleAssign
    {
        public int ID { get; set; }
        public string Day { get; set; }
        public string StartHour { get; set; }
        public string EndHour { get; set; }
    }
}