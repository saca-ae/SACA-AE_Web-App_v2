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
using SACAAE.Models.ViewModels;

namespace SACAAE.Controllers
{
    public class ProfesorController : Controller
    {
        private SACAAEContext db = new SACAAEContext();

        // GET: /Profesor/
        public ActionResult Index()
        {
            var profesores = db.Profesores.ToList();
            var viewModel = new List<ProfesorViewModel>();

            profesores.ForEach(p => viewModel.Add(
                new ProfesorViewModel()
                {
                    ID = p.ID,
                    Name = p.Name,
                    Link = p.Link,
                    Tel1 = p.Tel1,
                    Tel2 = p.Tel2,
                    StateID = p.StateID.GetValueOrDefault(),
                    Email = p.Email 
                }));

            return View(viewModel);
        }

        // GET: /Profesor/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Profesor profesor = db.Profesores.Find(id);
            if (profesor == null)
            {
                return HttpNotFound();
            }
            return View(profesor);
        }

        // GET: /Profesor/Create
        public ActionResult Create()
        {
            ViewBag.StateID = new SelectList(db.Estados, "ID", "Name");
            return View();
        }

        // POST: /Profesor/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include="ID,Name,Link,StateID,Tel1,Tel2,Email")] Profesor profesor)
        {
            if (ModelState.IsValid)
            {
                db.Profesores.Add(profesor);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.StateID = new SelectList(db.Estados, "ID", "Name", profesor.StateID);
            return View(profesor);
        }

        // GET: /Profesor/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Profesor profesor = db.Profesores.Find(id);
            if (profesor == null)
            {
                return HttpNotFound();
            }
            ViewBag.StateID = new SelectList(db.Estados, "ID", "Name", profesor.StateID);
            return View(profesor);
        }

        // POST: /Profesor/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include="ID,Name,Link,StateID,Tel1,Tel2,Email")] Profesor profesor)
        {
            if (ModelState.IsValid)
            {
                db.Entry(profesor).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.StateID = new SelectList(db.Estados, "ID", "Name", profesor.StateID);
            return View(profesor);
        }

        // GET: /Profesor/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Profesor profesor = db.Profesores.Find(id);
            if (profesor == null)
            {
                return HttpNotFound();
            }
            return View(profesor);
        }

        // POST: /Profesor/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Profesor profesor = db.Profesores.Find(id);
            db.Profesores.Remove(profesor);
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
