using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace SACAAE.Models
{
    public class NumeroPeriodo
    {
        public int ID { get; set; }
        public int Number { get; set; }
        [ForeignKey("Tipo")]
        public int TypeID { get; set; }

        public virtual TipoPeriodo Tipo { get; set; }
    }
}