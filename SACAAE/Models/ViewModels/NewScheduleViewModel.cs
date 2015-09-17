using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SACAAE.Models.ViewModels
{
    public class NewScheduleViewModel
    {
        public string PeriodID{get; set;}
        public string Sede { get; set; }
        public string Modality { get; set; }
        public string Plan { get; set;}

        public string Block { get; set; }
        public string Course { get; set; }
        public string Group { get; set; }
        public List<NewSchedule> NewSchedule { get; set; }

    }

    public class NewSchedule
    {
         public string Day { get; set; }
        public string StartHour { get; set; }
        public string EndHour { get; set; }
        public string Classroom{get; set;}
    }
}