using Newtonsoft.Json;
using SACAAE.Data_Access;
using SACAAE.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace SACAAE.Controllers
{
    public class PeriodoController : Controller
    {
        private SACAAEContext database = new SACAAEContext();
        private Periodo gPeriod = new Periodo();
        // GET: Periodo
        public ActionResult Index()
        {
            gPeriod = gPeriod.AddNewSemester();
            ViewBag.Period = "" + gPeriod.Year + " - " + gPeriod.NumberID + " Semestre";
            ViewBag.IdPeriod = gPeriod.getIDPeriod(gPeriod.Year, gPeriod.NumberID);
            return View();
        }

    }
}