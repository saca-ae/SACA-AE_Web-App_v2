using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace SACAAE.Models
{
    public class Classroom
    {
        public int ID { get; set; }
        [ForeignKey("Sede")]
        public int SedeID { get; set; }
        public string Code { get; set; }
        public int Capacity { get; set; }
        public bool Active { get; set; }     

        public virtual Sede Sede { get; set; }
        public virtual ICollection<GroupClassroom> GroupsClassroom { get; set; }
    }
}