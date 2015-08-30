using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace SACAAE.Models
{
    public class PeriodNumber
    {
        public int ID { get; set; }
        public int Number { get; set; }
        [ForeignKey("Type")]
        public int TypeID { get; set; }

        public virtual PeriodType Type { get; set; }
    }
}