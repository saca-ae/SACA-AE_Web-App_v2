using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SACAAE.WebService_Models
{
    public class NameWSModel
    {
        public string Name { get; set; }
    }

    public class PeriodInformationWSModel
    {
        public int Level { get; set; }
        public int Course { get; set; }
        public int Number { get; set; }
        public int Day { get; set; }
        public int StartHour { get; set; }
        public int EndHour { get; set; }
        public int Professor { get; set; }
    }

}