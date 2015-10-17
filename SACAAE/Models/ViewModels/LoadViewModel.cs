using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SACAAE.Models.ViewModels
{
    public class LoadViewModel
    {
        public String Name { get; set; }
        public bool Course { get; set; }
        public bool Comission { get; set; }
        public bool Project { get; set; }
        public bool Selected { get; set; }
    }
}