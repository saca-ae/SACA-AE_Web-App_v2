using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SACAAE.Models.ViewModels
{
    public class AlertViewModel
    {
        public List<Commission> Commissions { get; set; }
        public List<Project> Projects { get; set; }
    }
}