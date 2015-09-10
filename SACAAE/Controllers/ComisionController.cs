using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using SACAAE.Models;
using SACAAE.Data_Access;

namespace SACAAE.Controllers
{
    public class ComisionController : Controller
    {
        private SACAAEContext db = new SACAAEContext();
        private const string TempDataMessageKey = "Message";
        // GET: /Comision/
        public ActionResult Index()
        {
            String entidad = Request.Cookies["Entidad"].Value;
            if (entidad.Equals("TEC"))
            {
                var model = ObtenerComisionesXEntidad(1);
                return View(model);
            }
            else if (entidad.Equals("CIE"))
            {
                var model = ObtenerComisionesXEntidad(7);
                return View(model);
            }
            else if (entidad.Equals("TAE"))
            {
                var model = ObtenerComisionesXEntidad(5);
                return View(model);
            }
            else if (entidad.Equals("MAE"))
            {
                var model = ObtenerComisionesXEntidad(6);
                return View(model);
            }
            else if (entidad.Equals("DDE"))
            {
                var model = ObtenerComisionesXEntidad(11);
                return View(model);
            }
            else if (entidad.Equals("Emprendedores"))
            {
                var model = ObtenerComisionesXEntidad(12);
                return View(model);
            }
            else if (entidad.Equals("Actualizacion_Cartago"))
            {
                var model = ObtenerComisionesXEntidad(9);
                return View(model);
            }
            else
            {
                var model = ObtenerComisionesXEntidad(8); //Actualización San Carlos
                return View(model);
            }
        }

        // GET: /Comision/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Commission commission = db.Commissions.Find(id);
            if (commission == null)
            {
                return HttpNotFound();
            }
            return View(commission);
        }

        // GET: /Comision/Create
        public ActionResult Create()
        {
            ViewBag.EntityTypeID = new SelectList(db.EntityTypes, "ID", "Name");
            ViewBag.StateID = new SelectList(db.States, "ID", "Name");
            return View();
        }

        // POST: /Comision/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include="ID,Name,Start,End,StateID,EntityTypeID")] Commission commission)
        {
            String entidad = Request.Cookies["Entidad"].Value;
            int entidadID;

            if (entidad.Equals("TEC")) { entidadID = 1; }
            else if (entidad.Equals("CIE")) { entidadID = 7; }
            else if (entidad.Equals("TAE")) { entidadID = 5; }
            else if (entidad.Equals("MAE")) { entidadID = 6; }
            else if (entidad.Equals("DDE")) { entidadID = 11; }
            else if (entidad.Equals("Emprendedores")) { entidadID = 12; }
            else if (entidad.Equals("Actualizacion_Cartago")) { entidadID = 9; }
            else { entidadID = 8; }

            if (string.IsNullOrEmpty(commission.Name.Trim()))
                throw new ArgumentException("El nombre de la comisión no es válida. Por favor, inténtelo de nuevo");


            if (ModelState.IsValid)
            {
                commission.EntityTypeID = entidadID;
                commission.StateID = 1;
                db.Commissions.Add(commission);
                db.SaveChanges();
                TempData[TempDataMessageKey] = "Comisión creada correctamente.";
                return RedirectToAction("Index");
            }

            ViewBag.EntityTypeID = new SelectList(db.EntityTypes, "ID", "Name", commission.EntityTypeID);
            ViewBag.StateID = new SelectList(db.States, "ID", "Name", commission.StateID);
            return View(commission);
        }

        // GET: /Comision/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Commission commission = db.Commissions.Find(id);
            if (commission == null)
            {
                return HttpNotFound();
            }
            ViewBag.EntityTypeID = new SelectList(db.EntityTypes, "ID", "Name", commission.EntityTypeID);
            ViewBag.StateID = new SelectList(db.States, "ID", "Name", commission.StateID);
            return View(commission);
        }

        // POST: /Comision/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include="ID,Name,Start,End,StateID,EntityTypeID")] Commission commission)
        {

            if (!ExisteComision(commission))
            {
                AgregarComision(commission);
            }

            if (ModelState.IsValid)
            {
                var temp = db.Commissions.Find(commission.ID);

                if (temp != null)
                {
                    db.Entry(temp).Property(c => c.Name).CurrentValue = commission.Name;
                    db.Entry(temp).Property(c => c.Start).CurrentValue = commission.Start;
                    db.Entry(temp).Property(c => c.End).CurrentValue = commission.End;
                }
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(commission);
        }

        // GET: /Comision/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Commission commission = db.Commissions.Find(id);
            if (commission == null)
            {
                return HttpNotFound();
            }
            return View(commission);
        }

        // POST: /Comision/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Commission commission = db.Commissions.Find(id);
            if (commission != null)
            {
                db.Entry(commission).Property(c => c.StateID).CurrentValue = 2;
            }
            db.SaveChanges();
            TempData[TempDataMessageKey] = "Comisión eliminada correctamente.";
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Helpers
        public IQueryable<Commission> ObtenerComisionesXEntidad(int entidad)
        {
            if (entidad == 1)
            {
                return from comision in db.Commissions
                       orderby comision.Name
                       where comision.StateID == 1 && comision.EntityType.ID == 1 || //TEC
                       comision.EntityType.ID == 2 || comision.EntityType.ID == 3 || //TEC-VIC TEC-REC
                       comision.EntityType.ID == 4 || comision.EntityType.ID == 10 //TEC-MIXTO TEC-Académico
                       select comision;
            }
            else
            {
                return from comision in db.Commissions
                       orderby comision.Name
                       where comision.StateID == 1 && comision.EntityType.ID == entidad
                       select comision;

            }

        }

        public bool ExisteComision(Commission comision)
        {
            if (comision == null)
                return false;
            return (db.Commissions.SingleOrDefault(c => c.ID == comision.ID ||
                c.Name == comision.Name) != null);
        }

        private void AgregarComision(Commission  comision)
        {
            if (ExisteComision(comision))
                throw new ArgumentException("Comision ya existe");
            db.Commissions.Add(comision);
        }
        #endregion
    }
}
