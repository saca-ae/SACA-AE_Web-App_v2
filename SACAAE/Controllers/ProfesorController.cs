﻿using System;
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
using SACAAE.WebService_Models;
using SACAAE.Helpers;


namespace SACAAE.Controllers
{
    public class ProfesorController : Controller
    {
        private SACAAEContext db = new SACAAEContext();
        private LoadAcademicHelper LAHelper = new LoadAcademicHelper();
        private const string TempDataMessageKeySuccess = "MessageSuccess";
        private const string TempDataMessageKeyError = "MessageError";
        SACAAE.Helpers.LoadAcademicHelper.ReporteInfo vReportInfo = new SACAAE.Helpers.LoadAcademicHelper.ReporteInfo();
        // GET: /Professor/
        public ActionResult Index()
        {
            var Professors = db.Professors.ToList();
            int vPeriod = int.Parse(Request.Cookies["Periodo"].Value);
            var viewModel = new List<ProfesorViewModel>();

            SACAAE.Helpers.LoadAcademicHelper.ReporteInfo vReportInfo = new SACAAE.Helpers.LoadAcademicHelper.ReporteInfo();
            vReportInfo = LAHelper.setCourses(vReportInfo, vPeriod);
            vReportInfo = LAHelper.setProjects(vReportInfo, vPeriod);
            vReportInfo = LAHelper.setCommissions(vReportInfo, vPeriod);

            SACAAE.Helpers.LoadAcademicHelper.Profesor[] array_profesores = 
                vReportInfo.todo_profesores.OrderBy(c => c.Profesor_Nombre).ToArray();
            
            string profe_actual = "";

            List<ProfesorViewModel> vProfList = new List<ProfesorViewModel>();
            foreach (SACAAE.Helpers.LoadAcademicHelper.Profesor profe in array_profesores)
            {
                double vCargaTEC = 0, vReconocimientoHours = 0, vRecargoHours = 0;

                if (profe_actual.Equals(""))
                {
                    profe_actual = profe.Profesor_Nombre;
                }

                if (!profe_actual.Equals(profe.Profesor_Nombre))
                {
                    if (vReportInfo.profesores_carga_tec.ContainsKey(profe_actual))                 
                        vCargaTEC = vReportInfo.profesores_carga_tec[profe_actual];

                    if (vReportInfo.profesores_carga_reconocimiento.ContainsKey(profe_actual))
                        vReconocimientoHours = vReportInfo.profesores_carga_reconocimiento[profe_actual];

                    if (vReportInfo.profesores_carga_recargo.ContainsKey(profe_actual))
                        vRecargoHours = vReportInfo.profesores_carga_recargo[profe_actual];

                    vProfList.Add(
                    new ProfesorViewModel()
                    {
                        ID = 0,
                        Name = profe_actual,
                        Link = "",
                        Tel1 = "",
                        Tel2 = "",
                        StateID = 1 ,
                        Email = "" ,
                        LoadAcademic = vCargaTEC * 100 / 40,
                        TECHours = vCargaTEC,
                        ReconocimientoHours = vReconocimientoHours,
                        RecargoHours = vRecargoHours,
                        TotalHours = vCargaTEC + vReconocimientoHours + vRecargoHours
                    });
                    profe_actual = profe.Profesor_Nombre;
                }
            }

            for (int vCont = 0; vCont < Professors.Count(); vCont++) 
            {
                Professor vProf = Professors.ElementAt(vCont);
                double vTecHours = 0, vReconocimientoHoursAux = 0, vRecargoHoursAux = 0, vAcademicLoad = 0;
                if ((vProfList.Find(item => item.Name == vProf.Name)) != null)
                {
                    vTecHours = vProfList.Find(item => item.Name == vProf.Name).TECHours;
                    vReconocimientoHoursAux = vProfList.Find(item => item.Name == vProf.Name).ReconocimientoHours;
                    vRecargoHoursAux = vProfList.Find(item => item.Name == vProf.Name).RecargoHours;
                    vAcademicLoad = vProfList.Find(item => item.Name == vProf.Name).LoadAcademic;
                }

                viewModel.Add(
                    new ProfesorViewModel()
                    {
                        ID = vProf.ID,
                        Name = vProf.Name,
                        Link = vProf.Link,
                        Tel1 = vProf.Tel1,
                        Tel2 = vProf.Tel2,
                        StateID = vProf.StateID.GetValueOrDefault(),
                        Email = vProf.Email,
                        LoadAcademic = vAcademicLoad,
                        TECHours = vTecHours,
                        ReconocimientoHours = vReconocimientoHoursAux,
                        RecargoHours = vRecargoHoursAux,
                        TotalHours = vReconocimientoHoursAux + vTecHours + vRecargoHoursAux
                    });
            } 
              
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
                TempData[TempDataMessageKeySuccess] = "Profesor editado correctamente";
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

            var actualPeriod = int.Parse(Request.Cookies["Periodo"].Value);
            var data = new ScheduleDataList();
            var courses = db.SP_getAllCoursesPerProf(actualPeriod, profesor.Name).ToList();
            var commissions = db.SP_getAllCommissionsPerProf(actualPeriod, profesor.Name).ToList();
            var projects = db.SP_getAllProjectsPerProf(actualPeriod, profesor.Name).ToList();

            courses.ForEach(p => data.add(
                p.Day,
                (p.Name + "\nGrupo: " + p.Number),
                DateTime.Parse(p.StartHour),
                DateTime.Parse(p.EndHour),
                "Curso")
            );

            commissions.ForEach(p => data.add(
                p.Day,
                p.Name,
                DateTime.Parse(p.StartHour),
                DateTime.Parse(p.EndHour),
                "Comisión")
            );

            projects.ForEach(p => data.add(
                p.Day,
                p.Name,
                DateTime.Parse(p.StartHour),
                DateTime.Parse(p.EndHour),
                "Proyecto")
            );

            var viewModel = new ScheduleProfessorViewModel()
            {
                Name = profesor.Name,
                ScheduleData = data.getData()
            };

            return View(viewModel);
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

        #region Helpers
        private class ScheduleDataList
        {
            private List<List<ScheduleData>> data;

            public ScheduleDataList()
            {
                this.data = new List<List<ScheduleData>>();
                for(int i=0; i<6; i++)
                {
                    this.data.Add(new List<ScheduleData>());
                }
            }

            public void add(string day, string name, DateTime startHour, DateTime endHour, string type)
            {
                var index = 0;
                switch (day)
                {
                    case "Lunes":   index = 0; break;
                    case "Martes":  index = 1; break;
                    case "Miércoles": index = 2; break;
                    case "Jueves":  index = 3; break;
                    case "Viernes": index = 4; break;
                    case "Sábado": index = 5; break;
                }

                this.data[index].Add(new ScheduleData()
                {
                    Name = name,
                    Type = type,
                    Difference = getDifference(startHour, endHour),
                    StartBlock = getHourBlock(startHour)
                });
            }

            public List<List<ScheduleData>> getData()
            {
                return this.data;
            }

            private int getDifference(DateTime startHour, DateTime endHour)
            {
                return (int)Math.Ceiling(endHour.Subtract(startHour).TotalHours);
            }

            private int getHourBlock(DateTime time)
            {
                var hh = 7;
                var mm = 00;
                var tt = "am";

                for (int i = 0; i < 16; i++)
                {
                    var start = DateTime.Parse(hh + ":" + mm + tt);
                    var end = DateTime.Parse((hh + 1) + ":" + mm + tt);

                    if (betweenDates(start, end, time)) return i;

                    hh++;
                    if (hh == 12) tt = "pm";
                }
                return 0;
            }

            private bool betweenDates(DateTime beginDate, DateTime endDate, DateTime date)
            {
                return (beginDate <= date && date < endDate);
            }
        }
        #endregion
    }
}
