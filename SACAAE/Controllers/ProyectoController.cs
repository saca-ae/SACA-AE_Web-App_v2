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
    public class ProyectoController : Controller
    {
        private SACAAEContext db = new SACAAEContext();
        private const string TempDataMessageKey = "Message";
        // GET: /Proyecto/
        public ActionResult Index()
        {
            String entidad = Request.Cookies["Entidad"].Value;

            if (entidad.Equals("TEC"))
            {
                var model = ObtenerProyectoXEntidad(1);
                return View(model);
            }
            else if (entidad.Equals("CIE"))
            {
                var model = ObtenerProyectoXEntidad(7);
                return View(model);
            }
            else if (entidad.Equals("TAE"))
            {
                var model = ObtenerProyectoXEntidad(5);
                return View(model);
            }
            else if (entidad.Equals("MAE"))
            {
                var model = ObtenerProyectoXEntidad(6);
                return View(model);
            }
            else if (entidad.Equals("DDE"))
            {
                var model = ObtenerProyectoXEntidad(11);
                return View(model);
            }
            else if (entidad.Equals("Emprendedores"))
            {
                var model = ObtenerProyectoXEntidad(12);
                return View(model);
            }
            else if (entidad.Equals("Actualizacion_Cartago"))
            {
                var model = ObtenerProyectoXEntidad(9);
                return View(model);
            }
            else
            {
                var model = ObtenerProyectoXEntidad(8); //Actualización San Carlos
                return View(model);
            }
        }

        // GET: /Proyecto/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Project project = db.Projects.Find(id);
            if (project == null)
            {
                return HttpNotFound();
            }
            return View(project);
        }

        // GET: /Proyecto/Create
        public ActionResult Create()
        {

            var model = new Project();
            ViewBag.EntityTypeID = new SelectList(db.EntityTypes, "ID", "Name");
            ViewBag.StateID = new SelectList(db.States, "ID", "Name");
            return View(model);
        }

        // POST: /Proyecto/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include="ID,Name,Start,End,StateID,Link,EntityTypeID")] Project project)
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


            if (ModelState.IsValid)
            {
                Project proyectoNuevo = new Project()
                {
                    Name = project.Name,
                    Start = project.Start,
                    End = project.End,
                    EntityTypeID = entidadID,
                    Link = project.Link,
                    StateID = 1
                };
                db.Projects.Add(proyectoNuevo);
                db.SaveChanges();
                TempData[TempDataMessageKey] = "Proyecto creado correctamente.";
                return RedirectToAction("Index");
            }
            
            return RedirectToAction("Create");
        }

        // GET: /Proyecto/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Project project = db.Projects.Find(id);
            if (project == null)
            {
                return HttpNotFound();
            }
            ViewBag.EntityTypeID = new SelectList(db.EntityTypes, "ID", "Name", project.EntityTypeID);
            ViewBag.StateID = new SelectList(db.States, "ID", "Name", project.StateID);
            return View(project);
        }

        // POST: /Proyecto/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,Name,Start,End,StateID,Link,EntityTypeID")] Project proyecto)
        {
            if (ModelState.IsValid)
            {
                if (!ExisteProyecto(proyecto))
                    db.Projects.Add(proyecto);

                var temp = db.Projects.Find(proyecto.ID);

                if (temp != null)
                {
                    db.Entry(temp).Property(p => p.Name).CurrentValue = proyecto.Name;
                    db.Entry(temp).Property(p => p.Start).CurrentValue = proyecto.Start;
                    db.Entry(temp).Property(p => p.End).CurrentValue = proyecto.End;
                }
                db.SaveChanges();
                TempData[TempDataMessageKey] = "Proyecto editado correctamente.";
                
                return RedirectToAction("Index");
            }
            
            return RedirectToAction("Edit");
        }

        // GET: /Proyecto/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Project project = db.Projects.Find(id);
            if (project == null)
            {
                return HttpNotFound();
            }
            return View(project);
        }

        // POST: /Proyecto/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Project proyecto = db.Projects.Find(id);
            if (!ExisteProyecto(proyecto))
                throw new ArgumentException("Proyecto no existe");

            var temp = db.Projects.Find(proyecto.ID);
            if (temp != null)
            {
                db.Entry(temp).Property(p => p.StateID).CurrentValue = 2;
            }
            TempData[TempDataMessageKey] = "Proyecto eliminado correctamente.";
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
        public IQueryable<Project> ObtenerProyectoXEntidad(int entidad)
        {
            if (entidad == 1)
            {
                return from proyecto in db.Projects
                       orderby proyecto.Name
                       where proyecto.StateID == 1 && proyecto.EntityTypeID == 1 || //TEC
                       proyecto.EntityTypeID == 2 || proyecto.EntityTypeID== 3 || //TEC-VIC TEC-REC
                       proyecto.EntityTypeID == 4 || proyecto.EntityTypeID == 10 //TEC-MIXTO TEC Acádemico
                       select proyecto;
            }
            else
            {
                return from proyecto in db.Projects
                       orderby proyecto.Name
                       where proyecto.StateID== 1 && proyecto.EntityTypeID == entidad
                       select proyecto;
            }
        }

        public bool ExisteProyecto(Project proyecto)
        {
            if (proyecto == null)
                return false;
            return (db.Projects.SingleOrDefault(p => p.ID == proyecto.ID ||
                p.Name == proyecto.Name) != null);
        }
        #endregion
    }
}
