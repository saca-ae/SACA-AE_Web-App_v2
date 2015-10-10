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
using Newtonsoft.Json;

namespace SACAAE.Controllers
{
    public class ProfesorController : Controller
    {
        private SACAAEContext db = new SACAAEContext();

        // GET: /Professor/
        public ActionResult Index()
        {
            var Professors = db.Professors.ToList();
            var viewModel = new List<ProfesorViewModel>();

            Professors.ForEach(p => viewModel.Add(
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

        // GET: /Professor/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Professor profesor = db.Professors.Find(id);
            if (profesor == null)
            {
                return HttpNotFound();
            }
            return View(profesor);
        }

        // GET: /Professor/Create
        public ActionResult Create()
        {
            ViewBag.StateID = new SelectList(db.States, "ID", "Name");
            return View();
        }

        // POST: /Professor/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include="ID,Name,Link,StateID,Tel1,Tel2,Email")] Professor profesor)
        {
            if (ModelState.IsValid)
            {
                db.Professors.Add(profesor);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.StateID = new SelectList(db.States, "ID", "Name", profesor.StateID);
            return View(profesor);
        }

        // GET: /Professor/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Professor profesor = db.Professors.Find(id);
            if (profesor == null)
            {
                return HttpNotFound();
            }
            ViewBag.StateID = new SelectList(db.States, "ID", "Name", profesor.StateID);
            return View(profesor);
        }

        // POST: /Professor/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include="ID,Name,Link,StateID,Tel1,Tel2,Email")] Professor profesor)
        {
            if (ModelState.IsValid)
            {
                db.Entry(profesor).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.StateID = new SelectList(db.States, "ID", "Name", profesor.StateID);
            return View(profesor);
        }

        // GET: /Professor/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Professor profesor = db.Professors.Find(id);
            if (profesor == null)
            {
                return HttpNotFound();
            }
            return View(profesor);
        }

        // POST: /Professor/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Professor profesor = db.Professors.Find(id);
            db.Professors.Remove(profesor);
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

        // GET: Professor/Schedule/5
        public ActionResult Schedule(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Professor profesor = db.Professors.Find(id);
            if (profesor == null)
            {
                return HttpNotFound();
            }
            return View(profesor);
        }

        #region Ajax
        /*Obtener horario segun id del aula*/
        [Route("Professor/Schedules/{idProfesor:int}")]
        public ActionResult getScheduleProfesor(int idProfesor)
        {
            var periodo_actual = int.Parse(Request.Cookies["Periodo"].Value);
            if (HttpContext.Request.IsAjaxRequest())
            {
                var listaPlanes = from profesor in db.Professors
                                  join grupo in db.Groups on profesor.ID equals grupo.ProfessorID
                                  join plan_bloque_curso in db.BlocksXPlansXCourses on grupo.BlockXPlanXCourseID equals plan_bloque_curso.ID
                                  join curso in db.Courses on plan_bloque_curso.CourseID equals curso.ID
                                  join grupo_aula in db.GroupClassrooms on grupo.ID equals grupo_aula.GroupID
                                 join horario in db.Schedules on grupo_aula.ScheduleID equals horario.ID
                                  join period in db.Periods on grupo.PeriodID equals period.ID
                                 where (profesor.ID==idProfesor) && (horario.StartHour != "700" && horario.StartHour != "900")&&(period.ID==periodo_actual)
                                select new{
                                    curso.Name,
                                    grupo.Number,
                                    horario.StartHour,
                                    horario.EndHour,
                                    horario.Day
                                  };
                //listaPlanes.Where(p => p.Day == "lunes").OrderBy().ToList();
                listaPlanes = listaPlanes.OrderBy(c => c.StartHour).ThenBy(c => c.StartHour).ThenBy(c => c.Day);
                /*Es necesario remover elementos de la lista que los horarios no son correctos*/

                var json = JsonConvert.SerializeObject(listaPlanes);
                return Content(json);
            }
            return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

        }
        #endregion
    }
}
