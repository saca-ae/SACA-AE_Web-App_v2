using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SACAAE.Models.ViewModels
{
    public class GroupAssignmentCSVViewModel
    {
        public GroupAssignmentCSVViewModel(){ }
        public int Grupo { get; set; }
        public string Curso { get; set; }
        public string Profesor { get; set; }
        public string Dia { get; set; }
        public string HInicio { get; set; }
        public string HFinal { get; set; }
        public string Aula { get; set; }
        public string EstadoAsignacion { get; set; }
        public string DetalleAsignacion { get; set; }
    }
}