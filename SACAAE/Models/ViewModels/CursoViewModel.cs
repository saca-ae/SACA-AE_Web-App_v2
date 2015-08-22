using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SACAAE.Models.ViewModels
{
    public class CursoIndexViewModel
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public int TheoreticalHours { get; set; }
        public int Block { get; set; }
        public bool External { get; set; }
        public int PracticeHours { get; set; }
        public int Credits { get; set; }
    }

    public class CursoCreateViewModel
    {

    }
}