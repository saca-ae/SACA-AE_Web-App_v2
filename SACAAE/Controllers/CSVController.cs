using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SACAAE.Controllers
{
    public class CSVController : Controller
    {
        // GET: CSV
        public ActionResult CargarCSV()
        {
            return View();
        }
    }
}