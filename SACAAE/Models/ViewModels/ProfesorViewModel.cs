using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SACAAE.Models.ViewModels
{
    public class ProfesorViewModel
    {
        public int ID { get; set; }
        public String Name { get; set; }
        public String Link { get; set; }
        public int StateID { get; set; }
        public String Tel1 { get; set; }
        public String Tel2 { get; set; }
        public String Email { get; set; }
        public double LoadAcademic { get; set; }
        public double TECHours { get; set; }
        public double ReconocimientoHours { get; set; }
        public double RecargoHours { get; set; }
        public double TotalHours { get; set; }
    }
}