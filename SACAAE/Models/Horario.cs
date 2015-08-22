using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace SACAAE.Models
{
    public class Horario
    {
        public int ID { get; set; }
        public string Day { get; set; }
        public string StartHour { get; set; }
        public string EndHour { set; get; }

        public virtual ICollection<ComisionXProfesor> ComisionesXProfesores { get; set; }
        public virtual ICollection<ProyectoXProfesor> ProyectosXProfesores { get; set; }
        public virtual ICollection<GrupoAula> GruposAulas { get; set; }
    }
}