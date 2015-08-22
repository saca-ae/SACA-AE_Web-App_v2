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
    public class SedesController : Controller
    {
        private SACAAEContext db = new SACAAEContext();
        private const string TempDataMessageKey = "MessageError";
        private const string TempDataMessageKeySuccess = "MessageSuccess";

        // GET: Sedes
        public ActionResult Index()
        {
            return View(db.Sedes.ToList());
        }

        // GET: Sedes/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Sede sede = db.Sedes.Find(id);
            if (sede == null)
            {
                return HttpNotFound();
            }
            return View(sede);
        }

        // GET: Sedes/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Sedes/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,Name")] Sede sede)
        {
            if (ModelState.IsValid)
            {
                if (db.Sedes.Where(p => p.Name == sede.Name).Count() > 0)
                {
                    TempData[TempDataMessageKey] = "Ya existe una sede con el nombre: " + sede.Name;
                }

                db.Sedes.Add(sede);
                db.SaveChanges();

                TempData[TempDataMessageKeySuccess] = "Sede creada correctamente.";
                return RedirectToAction("Index");
            }

            return View(sede);
        }

        // GET: Sedes/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Sede sede = db.Sedes.Find(id);
            if (sede == null)
            {
                return HttpNotFound();
            }
            return View(sede);
        }

        // POST: Sedes/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,Name")] Sede sede)
        {
            if (ModelState.IsValid)
            {
                db.Entry(sede).State = EntityState.Modified;
                db.SaveChanges();
                TempData[TempDataMessageKeySuccess] = "Sede editada correctamente.";
                return RedirectToAction("Index");
            }
            return View(sede);
        }

        // GET: Sedes/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Sede sede = db.Sedes.Find(id);
            if (sede == null)
            {
                return HttpNotFound();
            }
            return View(sede);
        }

        // POST: Sedes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Sede sede = db.Sedes.Find(id);
            db.Sedes.Remove(sede);
            db.SaveChanges();
            TempData[TempDataMessageKeySuccess] = "La sede ha sido eliminada correctamente.";
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
    }
}
