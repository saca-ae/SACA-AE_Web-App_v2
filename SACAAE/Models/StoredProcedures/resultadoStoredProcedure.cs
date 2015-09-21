using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SACAAE.Models.StoredProcedures
{
    public class CourseGroupResult
    {
        public int GroupID { get; set; }
        public int GroupNumber { get; set; }
        public string SedeName { get; set; }
        public string ProfessorName { get; set; }
        public string ClassroomCode { get; set; }
        public string StartHour { get; set; }
        public string EndHour { get; set; }
        public string Day { get; set; }
    }

    public class resultadoStoredProcedure
    {
        public int Result { get; set; }
    }
}