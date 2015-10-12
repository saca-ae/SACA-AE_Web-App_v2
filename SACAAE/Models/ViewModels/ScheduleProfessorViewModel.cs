using SACAAE.WebService_Models;
using System.Collections.Generic;

namespace SACAAE.Models.ViewModels
{
    public class ScheduleProfessorViewModel
    {
        public string Name { get; set; }
        public List<List<ScheduleData>> ScheduleData { get; set; }
    }

    public class ScheduleData
    {
        public string Name { get; set; }
        public string Type { get; set; }
        public int StartBlock { get; set; }
        public int Difference { get; set; }
    }
}