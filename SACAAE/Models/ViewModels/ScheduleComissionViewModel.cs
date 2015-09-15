using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SACAAE.Models.ViewModels
{
    public class ScheduleComissionViewModel
    {
        public string Professors { get; set; }
        public string Commissions { get; set; }
        public string HourCharge { get; set; }
        public List<ScheduleComission> ScheduleCommission { get; set; }
    }

    public class ScheduleComission
    {
        public string Day { get; set; }
        public string StartHour { get; set; }
        public string EndHour { get; set; }
    }
}