using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace SACAAE.Models
{
    public class Commission
    {
        public int ID { get; set; }
        public string Name { get; set; }
        [Column(TypeName = "Date")]
        public DateTime Start { get; set; }
        [Column(TypeName = "Date")]
        public DateTime End { get; set; }
        [ForeignKey("State")]
        public int? StateID { get; set; }
        [ForeignKey("EntityType")]
        public int? EntityTypeID { get; set; }

        public virtual State State { get; set; }
        public virtual EntityType EntityType { get; set; }
        public virtual ICollection<CommissionXProfessor> CommissionsXProfessors { get; set; }
    }
}