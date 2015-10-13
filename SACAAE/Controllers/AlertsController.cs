using SACAAE.Data_Access;
using SACAAE.Models;
using SACAAE.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace SACAAE.Controllers
{
    public class AlertsController : Controller
    {
        private SACAAEContext db = new SACAAEContext();                     // Database context

        // GET: Alerts
        public ActionResult Index()
        {
            var today = DateTime.Now;
            today.AddDays(1);

            var viewModel = new AlertViewModel()
            {
                Commissions = db.Commissions.Where(p => p.End < today && p.State.Name == "En proceso").ToList(),
                Projects = db.Projects.Where(p => p.End < today && p.State.Name == "En proceso").ToList()
            };

            if ((viewModel.Commissions.Count + viewModel.Projects.Count) == 0)
            {
                Request.Cookies["alerts"].Value = "false";
                Response.Cookies["alerts"].Value = "false";
            }

            return View(viewModel);
        }

        // POST: Alerts/ResolveCommissions
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ResolveCommissions(AlertViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                for (int i = 0; i < viewModel.Commissions.Count; i++)
                {
                    var commission = db.Commissions.Find(viewModel.Commissions[i].ID);
                    db.Entry(commission).Property(c => c.StateID).CurrentValue = 2;
                    db.SaveChanges();

                    if (commission.CommissionsXProfessors.Count > 0)
                    {
                        var commissionProfessors = commission.CommissionsXProfessors.ToList();
                        foreach (var commissionProfessor in commissionProfessors)
                        {
                            db.CommissionsXProfessors.Remove(commissionProfessor);
                            db.SaveChanges();
                        }
                    }
                }
            }
            return RedirectToAction("Index");
        }

        // POST: Alerts/ResolveProjects
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ResolveProjects(AlertViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                for (int i = 0; i < viewModel.Projects.Count; i++)
                {
                    var project = db.Projects.Find(viewModel.Projects[i].ID);
                    db.Entry(project).Property(c => c.StateID).CurrentValue = 2;
                    db.SaveChanges();

                    if (project.ProjectsXProfessors.Count > 0)
                    {
                        var projectProfessors = project.ProjectsXProfessors.ToList();
                        foreach (var projectProfessor in projectProfessors)
                        {
                            db.ProjectsXProfessors.Remove(projectProfessor);
                            db.SaveChanges();
                        }
                    }
                }
            }
            return RedirectToAction("Index");
        }

        #region Ajax Post
        /// <author>Adonis Mora Angulo</author>
        /// <summary>
        /// Ajax post method for commissions and projects expired
        /// </summary>
        /// <param></param>
        /// <returns></returns>
        [Route("Alerts/Expired")]
        public ActionResult thereExpired()
        {
            if (HttpContext.Request.IsAjaxRequest())
            {
                var today = DateTime.Now;
                today.AddDays(1);
                var count = 0;
                count += db.Commissions.Where(p => p.End < today && p.State.Name == "En proceso").ToList().Count;
                count += db.Projects.Where(p => p.End < today && p.State.Name == "En proceso").ToList().Count;

                return Json((count > 0), JsonRequestBehavior.AllowGet);
            }
            return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        }
        #endregion
    }
}