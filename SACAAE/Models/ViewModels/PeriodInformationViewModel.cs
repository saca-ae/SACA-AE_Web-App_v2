using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SACAAE.Models.ViewModels
{
    public class PeriodInformationViewModel
    {
        public int Level { get; set; }
        public string Name { get; set; }
        public int Number { get; set; }
        public string Day { get; set; }
        public string HInicio { get; set; }
        public string HFin { get; set; }
        public string Professor { get; set; }
    }
}