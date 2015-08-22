using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace SACAAE.Models
{
    public class PlanDeEstudio
    {
        public int ID { get; set; }
        public string Name { get; set; }
        [ForeignKey("Modalidad")]
        public int Mode { get; set; }
        [ForeignKey("TipoEntidad")]
        public int? EntityType { get; set; }

        public virtual Modalidad Modalidad { get; set; }
        public virtual TipoEntidad TipoEntidad { get; set; }
        public virtual ICollection<PlanDeEstudioXSede> PlanesDeEstudioXSedes { get; set; }
        public virtual ICollection<BloqueAcademicoXPlanDeEstudio> BloquesAcademicosXPlanesDeEstudio { get; set; }
    }
}