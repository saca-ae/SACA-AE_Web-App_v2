using Newtonsoft.Json;
using SACAAE.Data_Access;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace SACAAE.Controllers
{
    public class PeriodoController : Controller
    {
        private SACAAEContext db = new SACAAEContext();

        // GET: Periodo
        public ActionResult Index()
        {
            return View();
        }
    }
}