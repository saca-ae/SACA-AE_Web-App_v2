using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace SACAAE.Models
{
    public class Profesor
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string Link { get; set; }
        [ForeignKey("Estado")]
        public int? State { get; set; }
        public string Tel1 { get; set; }
        public string Tel2 { get; set; }
        public string Email { get; set; }

        public virtual Estado Estado { get; set; }
        public virtual ICollection<ComisionXProfesor> ComisionesXProfesores { get; set; }
        public virtual ICollection<PlazaXProfesor> PlazasXProfesores { get; set; }
        public virtual ICollection<ProfesorXGrupo> ProfesoresXCursos { get; set; }
        public virtual ICollection<ProyectoXProfesor> ProyectosXProfesores { get; set; }
    }
}