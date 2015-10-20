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
using SACAAE.WebService_Models;


namespace SACAAE.Controllers
{
    public class ProfesorController : Controller
    {
        private SACAAEContext db = new SACAAEContext();
        private const string TempDataMessageKeySuccess = "MessageSuccess";
        private const string TempDataMessageKeyError = "MessageError";
        // GET: /Professor/
        public ActionResult Index()
        {
            var Professors = db.Professors.ToList();
            var viewModel = new List<ProfesorViewModel>();

            ReporteInfo vReportInfo = new ReporteInfo();
            //Cursos
            vReportInfo = setCourses(vReportInfo);
            //Proyectos
            vReportInfo = setProjects(vReportInfo);
            //Comisiones
            vReportInfo = setCommissions(vReportInfo);

            Profesor[] array_profesores = vReportInfo.todo_profesores.ToArray();
            string profe_actual = "";

            Array.Sort(array_profesores, delegate(Profesor user1, Profesor user2)
            {
                return user1.Profesor_Nombre.CompareTo(user2.Profesor_Nombre);
            });

            List<ProfesorViewModel> vProfList = new List<ProfesorViewModel>();
            foreach (Profesor profe in array_profesores)
            {
                int vCargaTEC = 0;
                int vCargaFundaTEC = 0;

                if (profe_actual.Equals(""))
                {
                    profe_actual = profe.Profesor_Nombre;
                }

                if (profe_actual.Equals(profe.Profesor_Nombre))
                {
                    
                }
                else
                {
                    if (vReportInfo.profesores_carga_tec.ContainsKey(profe_actual) && vReportInfo.profesores_carga_fundatec.ContainsKey(profe_actual))
                    {
                        vCargaTEC = vReportInfo.profesores_carga_tec[profe_actual];
                        vCargaFundaTEC = vReportInfo.profesores_carga_fundatec[profe_actual];
                    }
                    else if (!vReportInfo.profesores_carga_tec.ContainsKey(profe_actual) && vReportInfo.profesores_carga_fundatec.ContainsKey(profe_actual))
                    {
                        vCargaFundaTEC =  vReportInfo.profesores_carga_fundatec[profe_actual];

                    }
                    else if (vReportInfo.profesores_carga_tec.ContainsKey(profe_actual) && !vReportInfo.profesores_carga_fundatec.ContainsKey(profe_actual))
                    {
                        vCargaTEC = vReportInfo.profesores_carga_tec[profe_actual];
                    }

                    vProfList.Add(
                    new ProfesorViewModel()
                    {
                        ID = 0,
                        Name = profe.Profesor_Nombre,
                        Link = "",
                        Tel1 = "",
                        Tel2 = "",
                        StateID = 1 ,
                        Email = "" , 
                        TECHours = vCargaTEC,
                        FundaTECHours = vCargaFundaTEC
                    });
                    profe_actual = profe.Profesor_Nombre;
                }
            }

            for (int vCont = 0; vCont < Professors.Count(); vCont++) 
            {
                Professor vProf = Professors.ElementAt(vCont);
                int vTecHours = 0, vFundaTECHours = 0;
                if ((vProfList.Find(item => item.Name == vProf.Name)) != null)
                {
                    vTecHours = vProfList.Find(item => item.Name == vProf.Name).TECHours;
                }
                if ((vProfList.Find(item => item.Name == vProf.Name)) != null)
                {
                    vFundaTECHours = vProfList.Find(item => item.Name == vProf.Name).FundaTECHours;
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
                        TECHours = vTecHours,
                        FundaTECHours = vFundaTECHours,
                        TotalHours = vFundaTECHours + vTecHours
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


        #region Helpers

        /// <author>Adonis Mora Angulo</author>
        /// <summary>
        /// Set all the data from professors and courses on the helper class
        /// </summary>
        /// <param name="pReportInfo">Helper class to save all data</param>
        /// <returns></returns>
        private ReporteInfo setCourses(ReporteInfo pReportInfo)
        {
            var vPeriodID = int.Parse(Request.Cookies["Periodo"].Value);
            var entidad_temp = "";

            var vGroups = db.Groups.Where(p => p.PeriodID == vPeriodID).OrderBy(p => p.Number).ToList();
            foreach (Group vGroup in vGroups)
            {
                var vDetail = db.GroupClassrooms.Where(p => p.GroupID == vGroup.ID).ToList();
                var vCargaEstimada = (int)Math.Floor(10.0 / vDetail.Count);

                foreach (var vSchedule in vDetail)
                {

                    string HoraInicio = vSchedule.Schedule.StartHour;
                    string HoraFin = vSchedule.Schedule.EndHour;
                    int Carga = vCargaEstimada;
                    var vCourseInfo = db.BlocksXPlansXCourses.Single(p => p.ID == vGroup.BlockXPlanXCourseID).Course;

                    entidad_temp = vGroup.BlockXPlanXCourse.AcademicBlockXStudyPlan.StudyPlan.EntityType.Name;
                    pReportInfo.todo_profesores.Add(new Profesor
                    {

                        Tipo = "Carga docente",
                        Grupo = vGroup.Number + "",
                        Nombre = vCourseInfo.Name,
                        Profesor_Nombre = (vGroup.Professor != null) ? vGroup.Professor.Name : "No asignado",
                        Dia = vSchedule.Schedule.Day,
                        HoraInicio = HoraInicio,
                        HoraFin = HoraFin,
                        Cupo = (vGroup.Capacity ?? vSchedule.Classroom.Capacity) + "",
                        PlanEstudio = vGroup.BlockXPlanXCourse.AcademicBlockXStudyPlan.StudyPlan.Name,
                        Modalidad = vGroup.BlockXPlanXCourse.AcademicBlockXStudyPlan.StudyPlan.Modality.Name,
                        Sede = vGroup.BlockXPlanXCourse.Sede.Name,
                        HoraTeoricas = vGroup.BlockXPlanXCourse.Course.TheoreticalHours + "",
                        HoraPractica = vGroup.BlockXPlanXCourse.Course.PracticeHours + "",
                        CargaEstimada = Carga + "",
                        Aula = vSchedule.Classroom.Code,
                        Entidad = entidad_temp
                    });
                    List<string> vProfessorEntity;
                    // All TEC entities
                    if (entidad_temp.Equals("TEC") || entidad_temp.Equals("TEC-VIC") || entidad_temp.Equals("TEC-REC") ||
                    entidad_temp.Equals("TEC-MIXTO") || entidad_temp.Equals("TEC-Académico") || entidad_temp.Equals("CIADEG "))
                    {
                        if (!entidad_temp.Equals("TEC-REC")) // Omit volunteer hours
                        {
                            if (vGroup.Professor == null)
                            {
                            }
                            else if (pReportInfo.profesores_carga_tec.ContainsKey(vGroup.Professor.Name))
                            {
                                vProfessorEntity = pReportInfo.profesores_cursos_asociados[vGroup.Professor.Name];  // get professor's courses
                                if (!vProfessorEntity.Contains(vCourseInfo.Name))                                   // omit duplicate counting
                                {
                                    vProfessorEntity.Add(vCourseInfo.Name);
                                    pReportInfo.profesores_cursos_asociados[vGroup.Professor.Name] = vProfessorEntity;
                                    pReportInfo.profesores_carga_tec[vGroup.Professor.Name] += Carga;
                                }
                            }
                            else
                            {
                                pReportInfo.profesores_carga_tec.Add(vGroup.Professor.Name, Carga);
                                if (pReportInfo.profesores_cursos_asociados.ContainsKey(vGroup.Professor.Name))     // ask if the course, project or commission has been counted
                                {
                                    vProfessorEntity = pReportInfo.profesores_cursos_asociados[vGroup.Professor.Name];
                                    vProfessorEntity.Add(vCourseInfo.Name);


                                    pReportInfo.profesores_cursos_asociados[vGroup.Professor.Name] = vProfessorEntity;
                                }
                                else
                                {
                                    pReportInfo.profesores_cursos_asociados.Add(vGroup.Professor.Name, new List<string>() { vCourseInfo.Name });
                                }
                            }
                        }
                    }
                    else
                    {
                        if (vGroup.Professor == null)
                        {
                        }
                        else if (pReportInfo.profesores_carga_fundatec.ContainsKey(vGroup.Professor.Name))
                        {
                            vProfessorEntity = pReportInfo.profesores_cursos_asociados[vGroup.Professor.Name];  // get professor's courses
                            if (!vProfessorEntity.Contains(vCourseInfo.Name))                                   // omit duplicate counting
                            {
                                vProfessorEntity.Add(vCourseInfo.Name);
                                pReportInfo.profesores_cursos_asociados[vGroup.Professor.Name] = vProfessorEntity;
                                pReportInfo.profesores_carga_fundatec[vGroup.Professor.Name] += Carga;
                            }
                        }
                        else
                        {
                            pReportInfo.profesores_carga_fundatec.Add(vGroup.Professor.Name, Carga);
                            if (pReportInfo.profesores_cursos_asociados.ContainsKey(vGroup.Professor.Name))     // ask if the course, project or commission has been counted
                            {
                                vProfessorEntity = pReportInfo.profesores_cursos_asociados[vGroup.Professor.Name];
                                vProfessorEntity.Add(vCourseInfo.Name);


                                pReportInfo.profesores_cursos_asociados[vGroup.Professor.Name] = vProfessorEntity;
                            }
                            else
                            {
                                pReportInfo.profesores_cursos_asociados.Add(vGroup.Professor.Name, new List<string>() { vCourseInfo.Name });
                            }
                        }
                    }
                }

            }
            return pReportInfo;
        }

        /// <author>Adonis Mora Angulo</author>
        /// <summary>
        /// Set all the data from professors and projects on the helper class
        /// </summary>
        /// <param name="pReportInfo">Helper class to save all data</param>
        /// <returns></returns>
        private ReporteInfo setProjects(ReporteInfo pReportInfo)
        {
            var entidad_temp = "";

            var vProjects = db.Projects.ToList();
            foreach (var vProject in vProjects)
            {
                var vProfessors = db.ProjectsXProfessors.Where(p => p.ProjectID == vProject.ID).ToList();
                foreach (var vProfe in vProfessors)
                {
                    var vSchedule = vProfe.Schedule.ToList();
                    foreach (var vDay in vSchedule)
                    {
                        var HoraInicio = DateTime.Parse(vDay.StartHour);
                        var HoraFin = DateTime.Parse(vDay.EndHour);

                        var CargaC = Math.Ceiling(HoraFin.Subtract(HoraInicio).TotalHours);
                        if (HoraInicio <= DateTime.Parse("12:00 PM") && HoraFin >= DateTime.Parse("01:00 PM"))
                        {
                            CargaC = CargaC - 1;
                        }

                        entidad_temp = vProfe.Project.EntityType.Name;
                        pReportInfo.todo_profesores.Add(new Profesor
                        {
                            Tipo = "Carga Investigación Extensión",
                            Grupo = "N/A",
                            Nombre = vProject.Name,
                            Profesor_Nombre = vProfe.Professor.Name,
                            Dia = vDay.Day,
                            HoraInicio = HoraInicio.ToShortTimeString(),
                            HoraFin = HoraFin.ToShortTimeString(),
                            Cupo = "N/A",
                            PlanEstudio = "N/A",
                            Modalidad = "N/A",
                            Sede = "N/A",
                            HoraTeoricas = "N/A",
                            HoraPractica = "N/A",
                            CargaEstimada = CargaC + "",
                            Aula = "N/A",
                            Entidad = entidad_temp
                        });


                        List<string> entidadesXProfesor;
                        // All TEC entities
                        if (entidad_temp.Equals("TEC") || entidad_temp.Equals("TEC-VIC") || entidad_temp.Equals("TEC-REC") ||
                        entidad_temp.Equals("TEC-MIXTO") || entidad_temp.Equals("TEC-Académico") || entidad_temp.Equals("CIADEG "))
                        {
                            if (!entidad_temp.Equals("TEC-REC")) // Omit volunteer hours
                            {
                                if (pReportInfo.profesores_carga_tec.ContainsKey(vProfe.Professor.Name))
                                {
                                    entidadesXProfesor = pReportInfo.profesores_cursos_asociados[vProfe.Professor.Name];    // get professor's courses
                                    if (!entidadesXProfesor.Contains(vProject.Name))                                        // omit duplicate counting
                                    {
                                        entidadesXProfesor.Add(vProject.Name);
                                        pReportInfo.profesores_cursos_asociados[vProfe.Professor.Name] = entidadesXProfesor;
                                        pReportInfo.profesores_carga_tec[vProfe.Professor.Name] += Convert.ToInt32(CargaC);
                                    }

                                }
                                else
                                {

                                    pReportInfo.profesores_carga_tec.Add(vProfe.Professor.Name, Convert.ToInt32(CargaC));
                                    if (pReportInfo.profesores_cursos_asociados.ContainsKey(vProfe.Professor.Name))         // ask if the course, project or commission has been counted
                                    {
                                        entidadesXProfesor = pReportInfo.profesores_cursos_asociados[vProfe.Professor.Name];
                                        entidadesXProfesor.Add(vProject.Name);


                                        pReportInfo.profesores_cursos_asociados[vProfe.Professor.Name] = entidadesXProfesor;
                                    }
                                    else
                                    {
                                        pReportInfo.profesores_cursos_asociados.Add(vProfe.Professor.Name, new List<string>() { vProject.Name });
                                    }
                                }
                            }
                        }
                        else
                        {
                            if (pReportInfo.profesores_carga_fundatec.ContainsKey(vProfe.Professor.Name))
                            {
                                entidadesXProfesor = pReportInfo.profesores_cursos_asociados[vProfe.Professor.Name];
                                if (!entidadesXProfesor.Contains(vProject.Name))                                        // omit duplicate counting
                                {
                                    entidadesXProfesor.Add(vProject.Name);
                                    pReportInfo.profesores_cursos_asociados[vProfe.Professor.Name] = entidadesXProfesor;
                                    pReportInfo.profesores_carga_fundatec[vProfe.Professor.Name] += Convert.ToInt32(CargaC);
                                }
                            }
                            else
                            {
                                pReportInfo.profesores_carga_fundatec.Add(vProfe.Professor.Name, Convert.ToInt32(CargaC));
                                if (pReportInfo.profesores_cursos_asociados.ContainsKey(vProfe.Professor.Name))         // ask if the course, project or commission has been counted
                                {
                                    entidadesXProfesor = pReportInfo.profesores_cursos_asociados[vProfe.Professor.Name];
                                    entidadesXProfesor.Add(vProject.Name);


                                    pReportInfo.profesores_cursos_asociados[vProfe.Professor.Name] = entidadesXProfesor;
                                }
                                else
                                {
                                    pReportInfo.profesores_cursos_asociados.Add(vProfe.Professor.Name, new List<string>() { vProject.Name });
                                }

                            }
                        }
                    }
                }
            }
            return pReportInfo;
        }

        /// <author>Adonis Mora Angulo</author>
        /// <summary>
        /// Set all the data from professors and commisions on the helper class
        /// </summary>
        /// <param name="pReportInfo">Helper class to save all data</param>
        /// <returns></returns>
        private ReporteInfo setCommissions(ReporteInfo pReportInfo)
        {
            var entidad_temp = "";

            var vCommissions = db.Commissions.ToList();
            foreach (var vCommission in vCommissions)
            {
                var vProfessors = db.CommissionsXProfessors.Where(p => p.CommissionID == vCommission.ID).ToList();
                foreach (var vProfe in vProfessors)
                {
                    var vSchedule = vProfe.Schedule.ToList();
                    foreach (var vDay in vSchedule)
                    {
                        var HoraInicio = DateTime.Parse(vDay.StartHour);
                        var HoraFin = DateTime.Parse(vDay.EndHour);

                        var CargaC = Math.Ceiling(HoraFin.Subtract(HoraInicio).TotalHours);
                        if (HoraInicio <= DateTime.Parse("12:00 PM") && HoraFin >= DateTime.Parse("01:00 PM"))
                        {
                            CargaC = CargaC - 1;
                        }

                        entidad_temp = vCommission.EntityType.Name;
                        pReportInfo.todo_profesores.Add(new Profesor
                        {
                            Tipo = "Carga Académico Administrativo",
                            Grupo = "N/A",
                            Nombre = vCommission.Name,
                            Profesor_Nombre = vProfe.Professor.Name,
                            Dia = vDay.Day,
                            HoraInicio = HoraInicio.ToShortTimeString(),
                            HoraFin = HoraFin.ToShortTimeString(),
                            Cupo = "N/A",
                            PlanEstudio = "N/A",
                            Modalidad = "N/A",
                            Sede = "N/A",
                            HoraTeoricas = "N/A",
                            HoraPractica = "N/A",
                            CargaEstimada = CargaC + "",
                            Aula = "N/A",
                            Entidad = entidad_temp
                        });

                        List<string> entidadesXProfesor;
                        // All TEC entities
                        if (entidad_temp.Equals("TEC") || entidad_temp.Equals("TEC-VIC") || entidad_temp.Equals("TEC-REC") ||
                        entidad_temp.Equals("TEC-MIXTO") || entidad_temp.Equals("TEC-Académico") || entidad_temp.Equals("CIADEG "))
                        {
                            if (!entidad_temp.Equals("TEC-REC"))     // Omit volunteer hours
                            {
                                if (pReportInfo.profesores_carga_tec.ContainsKey(vProfe.Professor.Name))
                                {
                                    entidadesXProfesor = pReportInfo.profesores_cursos_asociados[vProfe.Professor.Name];
                                    if (!entidadesXProfesor.Contains(vCommission.Name))                                     // omit duplicate counting
                                    {
                                        entidadesXProfesor.Add(vCommission.Name);
                                        pReportInfo.profesores_cursos_asociados[vProfe.Professor.Name] = entidadesXProfesor;
                                        pReportInfo.profesores_carga_tec[vProfe.Professor.Name] += Convert.ToInt32(CargaC);
                                    }

                                }
                                else
                                {

                                    pReportInfo.profesores_carga_tec.Add(vProfe.Professor.Name, Convert.ToInt32(CargaC));
                                    if (pReportInfo.profesores_cursos_asociados.ContainsKey(vProfe.Professor.Name))         // ask if the course, project or commission has been counted
                                    {
                                        entidadesXProfesor = pReportInfo.profesores_cursos_asociados[vProfe.Professor.Name];
                                        entidadesXProfesor.Add(vCommission.Name);


                                        pReportInfo.profesores_cursos_asociados[vProfe.Professor.Name] = entidadesXProfesor;
                                    }
                                    else
                                    {
                                        pReportInfo.profesores_cursos_asociados.Add(vProfe.Professor.Name, new List<string>() { vCommission.Name });
                                    }
                                }
                            }
                        }
                        else
                        {
                            if (pReportInfo.profesores_carga_fundatec.ContainsKey(vProfe.Professor.Name))
                            {
                                entidadesXProfesor = pReportInfo.profesores_cursos_asociados[vProfe.Professor.Name];
                                if (!entidadesXProfesor.Contains(vCommission.Name))                                          // omit duplicate counting
                                {
                                    entidadesXProfesor.Add(vCommission.Name);
                                    pReportInfo.profesores_cursos_asociados[vProfe.Professor.Name] = entidadesXProfesor;
                                    pReportInfo.profesores_carga_fundatec[vProfe.Professor.Name] += Convert.ToInt32(CargaC);
                                }
                            }
                            else
                            {
                                pReportInfo.profesores_carga_fundatec.Add(vProfe.Professor.Name, Convert.ToInt32(CargaC));
                                if (pReportInfo.profesores_cursos_asociados.ContainsKey(vProfe.Professor.Name))             // ask if the course, project or commission has been counted
                                {
                                    entidadesXProfesor = pReportInfo.profesores_cursos_asociados[vProfe.Professor.Name];
                                    entidadesXProfesor.Add(vCommission.Name);


                                    pReportInfo.profesores_cursos_asociados[vProfe.Professor.Name] = entidadesXProfesor;
                                }
                                else
                                {
                                    pReportInfo.profesores_cursos_asociados.Add(vProfe.Professor.Name, new List<string>() { vCommission.Name });
                                }
                            }
                        }
                    }
                }
            }
            return pReportInfo;
        }

        /// <author>Adonis Mora Angulo</author>
        /// <summary>
        /// Helper class to save all courses, projects and commissions data
        /// </summary>
        private class ReporteInfo
        {
            public ReporteInfo()
            {
                profesores_carga_tec = new Dictionary<string, int>();
                profesores_carga_fundatec = new Dictionary<string, int>();
                profesores_cursos_asociados = new Dictionary<string, List<string>>();
                todo_profesores = new List<Profesor>();
            }

            public Dictionary<string, int> profesores_carga_tec { get; set; }
            public Dictionary<string, int> profesores_carga_fundatec { get; set; }
            public Dictionary<string, List<string>> profesores_cursos_asociados { get; set; }
            public List<Profesor> todo_profesores { get; set; }
        }

        /// <author>Adonis Mora Angulo</author>
        /// <summary>
        /// Helper class to write each line on the csv file
        /// </summary>
        private class Profesor
        {
            public string Tipo { get; set; }
            public string Grupo { get; set; }
            public string Nombre { get; set; }
            public string Profesor_Nombre { get; set; }
            public string Dia { get; set; }
            public string HoraInicio { get; set; }
            public string HoraFin { get; set; }
            public string Cupo { get; set; }
            public string PlanEstudio { get; set; }
            public string Modalidad { get; set; }
            public string Sede { get; set; }
            public string HoraTeoricas { get; set; }
            public string HoraPractica { get; set; }
            public string CargaEstimada { get; set; }
            public string Aula { get; set; }
            public string Entidad { get; set; }

            public string toStr()
            {
                return Tipo + ";" + Grupo + ";" + Nombre + ";" + Profesor_Nombre + ";" +
                       Dia + ";" + HoraInicio + ";" + HoraFin + ";" + Sede + ";" + Aula + ";" +
                       Cupo + ";" + PlanEstudio + ";" + Modalidad + ";" + HoraTeoricas + ";" +
                       HoraPractica + ";" + CargaEstimada + ";" + Entidad + ";";
            }
        }

        #endregion

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
                    case "Miercoles": index = 2; break;
                    case "Jueves":  index = 3; break;
                    case "Viernes": index = 4; break;
                    case "Sabado":  index = 5; break;
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
