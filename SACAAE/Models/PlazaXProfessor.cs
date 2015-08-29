using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace SACAAE.Models
{
    public class PlazaXProfessor
    {
        public int ID { get; set; }
        [ForeignKey("Plaza")]
        public int PlazaID { get; set; }
        [ForeignKey("Professor")]
        public int ProfessorID { get; set; }
        public int PercentHours { get; set; }

        public virtual Plaza Plaza { get; set; }
        public virtual Professor Professor { get; set; }
    }
}