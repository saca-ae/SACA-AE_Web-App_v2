using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SACAAE.WebService_Models
{
    public class PeriodWSModel
    {
        public int ID { get; set; }
        public int Year { get; set; }
        public int Number { get; set; }
        public string Name { get; set; }
    }

    public class BasicInfoWSModel
    {
        public int ID { get; set; }
        public string Name { get; set; }
    }

    public class ProjectCommissionWSModel
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string StartHour { get; set; }
        public string EndHour { get; set; }
        public string Day { get; set; }
    }

    public class GroupWSModel
    {
        public int ID { get; set; }
        public string StartHour { get; set; }
        public string EndHour { get; set; }
        public string Day { get; set; }
        public string Code { get; set; }
    }
}