using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SACAAE.Models.ViewModels
{
    public class PlazaCreateViewModel
    {
        public string Code { get; set; }
        public string PlazaType { get; set; }
        public SelectList PlazaTypeList { get; set; }
        public string TimeType { get; set; }
        public SelectList TimeTypeList { get; set; }
        public int TotalHours { get; set; }
        public int EffectiveTime { get; set; }
    }

    public class PlazaEditViewModel
    {
        public int ID { get; set; }
        public string Code { get; set; }
        public string PlazaType { get; set; }
        public SelectList PlazaTypeList { get; set; }
        public string TimeType { get; set; }
        public SelectList TimeTypeList { get; set; }
        public int TotalHours { get; set; }
        public int EffectiveTime { get; set; }
    }

    public class PlazaDetailViewModel
    {
        public int ID { get; set; }
        public string Code { get; set; }
        public string PlazaType { get; set; }
        public string TimeType { get; set; }
        public int TotalHours { get; set; }
        public int EffectiveTime { get; set; }
        public int TotalAllocate { get; set; }
        public List<PlazaAllocateProfessor> Professors { get; set; }
    }

    public class PlazaAllocateViewModel
    {
        public int ID { get; set; }
        public int TotalAllocate { get; set; }
        public List<PlazaAllocateProfessor> Professors { get; set; }
    }

    public class PlazaAllocateProfessor
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public int Allocate { get; set; }
    }

}