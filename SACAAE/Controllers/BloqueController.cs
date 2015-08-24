using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using SACAAE.Data_Access;
using SACAAE.Models;

namespace SACAAE.Controllers
{
    [Authorize]
    public class BloqueController : Controller
    {
        private SACAAEContext db = new SACAAEContext();
        private const string TempDataMessageKey = "MessageError";
        private const string TempDataMessageKeySuccess = "MessageSuccess";

        // GET: Bloque
        public ActionResult Index()
        {
            String entity = Request.Cookies["Entidad"].Value;
            var entityID = getEntityID(entity);
            IQueryable<BloqueAcademico> result;
            if (entity == "TEC")
            {
                result = from bloque in db.BloquesAcademicos
                         join BloquesXPlan in db.BloquesAcademicosXPlanesDeEstudio on bloque.ID equals BloquesXPlan.BlockID
                         join planDeEstudio in db.PlanesDeEstudio on BloquesXPlan.PlanID equals planDeEstudio.ID
                         where planDeEstudio.EntityTypeID == 1 || planDeEstudio.EntityTypeID == 2 ||
                         planDeEstudio.EntityTypeID == 3 || planDeEstudio.EntityTypeID == 4 || planDeEstudio.EntityTypeID == 10
                         select bloque;
            }
            else
            {
                result = from bloque in db.BloquesAcademicos
                         join BloquesXPlan in db.BloquesAcademicosXPlanesDeEstudio on bloque.ID equals BloquesXPlan.BlockID
                         join planDeEstudio in db.PlanesDeEstudio on BloquesXPlan.PlanID equals planDeEstudio.ID
                         where planDeEstudio.EntityTypeID == entityID
                         select bloque;
            }

            return View(result.Distinct().ToList());
        }

        // GET: Bloque/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            BloqueAcademico bloqueAcademico = db.BloquesAcademicos.Find(id);
            if (bloqueAcademico == null)
            {
                return HttpNotFound();
            }
            return View(bloqueAcademico);
        }

        // GET: Bloque/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Bloque/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,Description,Level")] BloqueAcademico bloqueAcademico)
        {
            if (ModelState.IsValid)
            {
                db.BloquesAcademicos.Add(bloqueAcademico);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(bloqueAcademico);
        }

        // GET: Bloque/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            BloqueAcademico bloqueAcademico = db.BloquesAcademicos.Find(id);
            if (bloqueAcademico == null)
            {
                return HttpNotFound();
            }
            return View(bloqueAcademico);
        }

        // POST: Bloque/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,Description,Level")] BloqueAcademico bloqueAcademico)
        {
            if (ModelState.IsValid)
            {
                db.Entry(bloqueAcademico).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(bloqueAcademico);
        }

        // GET: Bloque/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            BloqueAcademico bloqueAcademico = db.BloquesAcademicos.Find(id);
            if (bloqueAcademico == null)
            {
                return HttpNotFound();
            }
            return View(bloqueAcademico);
        }

        // POST: Bloque/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            BloqueAcademico bloqueAcademico = db.BloquesAcademicos.Find(id);
            db.BloquesAcademicos.Remove(bloqueAcademico);
            db.SaveChanges();
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
        private int getEntityID(string entityName)
        {
            TipoEntidad entity;
            switch (entityName)
            {
                case "TEC":
                    entity = db.TipoEntidades.SingleOrDefault(p => p.Name == "TEC");
                    break;
                case "CIADEG":
                    entity = db.TipoEntidades.SingleOrDefault(p => p.Name == "CIADEG");
                    break;
                case "TAE":
                    entity = db.TipoEntidades.SingleOrDefault(p => p.Name == "FUNDA-TAE");
                    break;
                case "MAE":
                    entity = db.TipoEntidades.SingleOrDefault(p => p.Name == "FUNDA-MAE");
                    break;
                case "MDE":
                    entity = db.TipoEntidades.SingleOrDefault(p => p.Name == "FUNDA-MDE");
                    break;
                case "MGP":
                    entity = db.TipoEntidades.SingleOrDefault(p => p.Name == "FUNDA-MGP");
                    break;
                case "DDE":
                    entity = db.TipoEntidades.SingleOrDefault(p => p.Name == "FUNDA-Doctorado");
                    break;
                case "Emprendedores":
                    entity = db.TipoEntidades.SingleOrDefault(p => p.Name == "FUNDA-Emprendedores");
                    break;
                case "CIE":
                    entity = db.TipoEntidades.SingleOrDefault(p => p.Name == "FUNDA-CIE");
                    break;
                case "Actualizacion_Cartago":
                    entity = db.TipoEntidades.SingleOrDefault(p => p.Name == "FUNDA-Actualizacion Cartago");
                    break;
                case "Actualizacion_San_Carlos":
                    entity = db.TipoEntidades.SingleOrDefault(p => p.Name == "FUNDA-Actualizacion San Carlos");
                    break;
                default:
                    entity = db.TipoEntidades.SingleOrDefault(p => p.Name == entityName);
                    break;
            }

            return (entity != null) ? entity.ID : 0;
        }
        #endregion
    }
}
