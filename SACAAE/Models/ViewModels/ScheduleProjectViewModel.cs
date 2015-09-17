using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SACAAE.Models.ViewModels
{
    public class ScheduleProjectViewModel
    {

        public string Professors { get; set; }
        public string Projects { get; set; }
        public string HourCharge { get; set; }
        public List<ScheduleProject> ScheduleProject { get; set; }
    }

    public class ScheduleProject
    {
        public string Day { get; set; }
        public string StartHour { get; set; }
        public string EndHour { get; set; }
    }
}