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

namespace SACAAE.Controllers
{
    [Authorize]
    public class PlazasController : Controller
    {
        private SACAAEContext db = new SACAAEContext();
        private const string TempDataMessageKey = "MessageError";
        private const string TempDataMessageKeySuccess = "MessageSuccess";

        // GET: Plaza
        public ActionResult Index()
        {
            return View(db.Plazas.ToList());
        }

        // GET: Plaza/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Plaza plaza = db.Plazas.Find(id);
            if (plaza == null)
            {
                return HttpNotFound();
            }
            var PPList = plaza.PlazasXProfesores.ToList();
            var professors = new List<PlazaAllocateProfessor>();
            PPList.ForEach(p => professors.Add(new PlazaAllocateProfessor()
            {
                ID = p.ProfessorID,
                Name = p.Profesor.Name,
                Allocate = p.PercentHours
            }));

            var viewModel = new PlazaDetailViewModel()
            {
                ID = plaza.ID,
                Code = plaza.Code,
                PlazaType = plaza.PlazaType,
                TimeType = plaza.TimeType,
                TotalHours = plaza.TotalHours.GetValueOrDefault(),
                EffectiveTime = plaza.EffectiveTime.GetValueOrDefault(),
                TotalAllocate = sumAllocate(professors),
                Professors = professors
            };

            return View(viewModel);
        }

        // GET: Plaza/Create
        public ActionResult Create()
        {
            var viewModel = new PlazaCreateViewModel()
            {
                PlazaTypeList = new SelectList(new List<string>() { "Interna", "Externa" }),
                TimeTypeList = new SelectList(new List<string>() { "Completo", "Parcial" })
            };

            return View(viewModel);
        }

        // POST: Plaza/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(PlazaCreateViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                var plaza = new Plaza()
                {
                    Code = viewModel.Code,
                    PlazaType = viewModel.PlazaType,
                    TimeType = viewModel.TimeType,
                    TotalHours = viewModel.TotalHours,
                    EffectiveTime = viewModel.EffectiveTime
                };

                db.Plazas.Add(plaza);
                db.SaveChanges();

                TempData[TempDataMessageKeySuccess] = "Plaza creada exitosamente";
                return RedirectToAction("Index");
            }

            return View(viewModel);
        }

        // GET: Plaza/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Plaza plaza = db.Plazas.Find(id);
            if (plaza == null)
            {
                return HttpNotFound();
            }
            var viewModel = new PlazaEditViewModel()
            {
                ID = plaza.ID,
                Code = plaza.Code,
                PlazaType = plaza.PlazaType,
                PlazaTypeList = new SelectList(new List<string>() { "Interna", "Externa" }),
                TimeType = plaza.TimeType,
                TimeTypeList = new SelectList(new List<string>() { "Completo", "Parcial" }),
                TotalHours = plaza.TotalHours.GetValueOrDefault(),
                EffectiveTime = plaza.EffectiveTime.GetValueOrDefault()
            };

            return View(viewModel);
        }

        // POST: Plaza/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(PlazaEditViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                var plaza = db.Plazas.Find(viewModel.ID);
                plaza.Code = viewModel.Code;
                plaza.PlazaType = viewModel.PlazaType;
                plaza.TimeType = viewModel.TimeType;
                plaza.TotalHours = viewModel.TotalHours;
                plaza.EffectiveTime = viewModel.EffectiveTime;

                db.Entry(plaza).State = EntityState.Modified;
                db.SaveChanges();

                TempData[TempDataMessageKeySuccess] = "Plaza editada exitosamente";
                return RedirectToAction("Index");
            }
            return View(viewModel);
        }

        // POST: Plaza/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Plaza plaza = db.Plazas.Find(id);
            db.Plazas.Remove(plaza);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        // GET: Plaza/Allocate/5
        public ActionResult Allocate(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Plaza plaza = db.Plazas.Find(id);
            if (plaza == null)
            {
                return HttpNotFound();
            }
            var PPList = plaza.PlazasXProfesores.ToList();
            var professors = new List<PlazaAllocateProfessor>();
            PPList.ForEach(p => professors.Add(new PlazaAllocateProfessor()
            {
                ID = p.ProfessorID,
                Name = p.Profesor.Name,
                Allocate = p.PercentHours
            }));

            var viewModel = new PlazaAllocateViewModel()
            {
                ID = plaza.ID,
                TotalAllocate = sumAllocate(professors),
                Professors = professors
            };

            return View(viewModel);
        }

        // POST: Plaza/Allocate/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Allocate(PlazaAllocateViewModel viewModel)
        {
            var plaza = db.Plazas.Find(viewModel.ID);
            var newProfe = viewModel.Professors.Last();
            plaza.PlazasXProfesores.Add(new PlazaXProfesor()
            {
                ProfessorID = newProfe.ID,
                PercentHours = newProfe.Allocate
            });
            db.SaveChanges();
            
            return RedirectToAction("Allocate", new { id = viewModel.ID });
        }

        // POST: Plaza/EditAllocate/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditAllocate(PlazaAllocateViewModel viewModel)
        {
            var plazaID = viewModel.ID;
            var profeID = viewModel.Professors[0].ID;
            var PxP = db.PlazasXProfesores.Where(p => p.PlazaID == plazaID && p.ProfessorID == profeID).Single();
            PxP.PercentHours = viewModel.Professors[0].Allocate;

            db.Entry(PxP).State = EntityState.Modified;
            db.SaveChanges();

            return RedirectToAction("Allocate", new { id = viewModel.ID });
        }

        // POST: Plaza/DeleteAllocate/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteAllocate(PlazaAllocateViewModel viewModel)
        {
            var plazaID = viewModel.ID;
            var profeID = viewModel.Professors[0].ID;
            var PxP = db.PlazasXProfesores.Where(p => p.PlazaID == plazaID && p.ProfessorID == profeID).Single();

            db.PlazasXProfesores.Remove(PxP);
            db.SaveChanges();

            return RedirectToAction("Allocate", new { id = viewModel.ID });
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Ajax Post
        [Route("Plazas/Professors/List/{plaza:int}")]
        public ActionResult getProfessors(int plaza)
        {
            if (HttpContext.Request.IsAjaxRequest())
            {
                var plazaProfes = db.Plazas.Find(plaza).PlazasXProfesores.Select(p => p.Profesor).ToList();
                var listaProfes = db.Profesores.ToList();
                var json = listaProfes.Except(plazaProfes).Select(p => new { p.ID, p.Name });

                return Json(json, JsonRequestBehavior.AllowGet);
            }
            return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        }
        #endregion
        
        #region Helpers
        private int sumAllocate(List<PlazaAllocateProfessor> PPList)
        {
            var result = 0;
            for (var i = 0; i < PPList.Count; i++)
            {
                result += PPList[i].Allocate;
            }
            return result;
        }
        #endregion 
    }
}
