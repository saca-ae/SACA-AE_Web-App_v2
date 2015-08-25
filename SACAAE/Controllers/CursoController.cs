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
using SACAAE.Models.ViewModels;
using Newtonsoft.Json;

namespace SACAAE.Controllers
{
    public class CursoController : Controller
    {
        private SACAAEContext db = new SACAAEContext();

        // GET: Curso
        public ActionResult Index()
        {
            var cursos = db.Cursos.ToList();
            var viewModel = new List<CursoIndexViewModel>();

            cursos.ForEach(p => viewModel.Add(
                new CursoIndexViewModel()
                {
                    ID = p.ID,
                    Name = p.Name,
                    TheoreticalHours = p.TheoreticalHours,
                    PracticeHours = p.PracticeHours.GetValueOrDefault(),
                    Block = p.Block,
                    Code = p.Code,
                    Credits = p.Credits.GetValueOrDefault(),
                    External = p.External
                }));

            return View(viewModel);
        }

        // GET: Curso/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Curso curso = db.Cursos.Find(id);
            if (curso == null)
            {
                return HttpNotFound();
            }

            return View(curso);
        }

        // GET: Curso/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Curso/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,Name,Code,TheoreticalHours,Block,External,PracticeHours,Credits")] Curso curso)
        {
            if (ModelState.IsValid)
            {
                db.Cursos.Add(curso);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(curso);
        }

        // GET: Curso/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Curso curso = db.Cursos.Find(id);
            if (curso == null)
            {
                return HttpNotFound();
            }
            return View(curso);
        }

        // POST: Curso/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,Name,Code,TheoreticalHours,Block,External,PracticeHours,Credits")] Curso curso)
        {
            if (ModelState.IsValid)
            {
                db.Entry(curso).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(curso);
        }

        // GET: Curso/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Curso curso = db.Cursos.Find(id);
            if (curso == null)
            {
                return HttpNotFound();
            }
            return View(curso);
        }

        // POST: Curso/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Curso curso = db.Cursos.Find(id);
            db.Cursos.Remove(curso);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
        /*-----------------------------------------------------------------*/
        /*
         * Esteban Segura Benavides
         * Metodo que llama a la vista para asignar un profesor a un curso definido seleccionado de 'Ver Detalle Curso'
         * */
        
        
        // GET: Curso/AsignarProfesoraCurso/{id:int}
        public ActionResult AsignarProfesoraCurso(int id)
        {
            if (Request.UrlReferrer != null)
            {
                ViewBag.returnUrl = Request.UrlReferrer.ToString();
            }
            else
            {
                ViewBag.returnUrl = null;
            }


            /* Se obtiene la lista de profesores */
            ViewBag.Profesores = new SelectList(db.Profesores, "ID", "Name");
            Curso curso = db.Cursos.Find(id);
            return View(curso);
        }
        /*-------------------------------------------------------------------*/
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        /*----------------------------------------------------------------------------*/
        /* Esteban Segura Benavides Creacion funciones ajax
         * Obtener informacion ajax*/
        #region Ajax
       
        #endregion

    }
}
