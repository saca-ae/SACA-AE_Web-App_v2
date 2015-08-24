using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace SACAAE.Models
{
    public class GrupoAula
    {
        public int ID { get; set; }
        [ForeignKey("Grupo")]
        public int GroupID { get; set; }
        [ForeignKey("Aula")]
        public int? ClassroomID { get; set; }
        [ForeignKey("Horario")]
        public int? ScheduleID { get; set; }

        public virtual Grupo Grupo { get; set; }
        public virtual Aula Aula { get; set; }
        public virtual Horario Horario { get; set; }
    }
}