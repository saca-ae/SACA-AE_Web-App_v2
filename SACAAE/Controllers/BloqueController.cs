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
            return View(db.AcademicBlocks.ToList());
        }

        // GET: Bloque/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AcademicBlock bloqueAcademico = db.AcademicBlocks.Find(id);
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
        public ActionResult Create([Bind(Include = "ID,Description,Level")] AcademicBlock bloqueAcademico)
        {
            if (ModelState.IsValid)
            {
                if (db.AcademicBlocks.Where(p => p.Description == bloqueAcademico.Description).Count() > 0)
                {
                    TempData[TempDataMessageKey] = "Ya existe un bloque académico con el nombre: " + bloqueAcademico.Description;
                }

                db.AcademicBlocks.Add(bloqueAcademico);
                db.SaveChanges();

                TempData[TempDataMessageKeySuccess] = "Bloque académico creada correctamente.";
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
            AcademicBlock bloqueAcademico = db.AcademicBlocks.Find(id);
            if (bloqueAcademico == null)
            {
                return HttpNotFound();
            }
            return View(bloqueAcademico);
        }

        // POST: Bloque/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,Description,Level")] AcademicBlock bloqueAcademico)
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
            AcademicBlock bloqueAcademico = db.AcademicBlocks.Find(id);
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
            AcademicBlock bloqueAcademico = db.AcademicBlocks.Find(id);
            db.AcademicBlocks.Remove(bloqueAcademico);
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
    }
}
