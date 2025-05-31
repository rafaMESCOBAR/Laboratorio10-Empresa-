using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Laboratorio10.Models;

namespace Laboratorio10.Controllers
{
    public class CoursesController : Controller
    {
        private UniversityDBEntities db = new UniversityDBEntities();

        // GET: Courses - MODIFICADO ✅
        public ActionResult Index()
        {
            var cursosActivos = db.Course
                                  .Where(c => !c.IsDeleted)
                                  .ToList();
            return View(cursosActivos);
        }

        // GET: Courses/Details/5 - MODIFICADO ✅
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Course course = db.Course.Find(id);
            // MODIFICADO: Verificar que no esté eliminado
            if (course == null || course.IsDeleted)
            {
                return HttpNotFound();
            }
            return View(course);
        }

        // GET: Courses/Create - SIN CAMBIOS
        public ActionResult Create()
        {
            return View();
        }

        // POST: Courses/Create - MODIFICADO ✅
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "CourseID,Title,Credits")] Course course)
        {
            if (ModelState.IsValid)
            {
                course.IsDeleted = false;  // ← AGREGADO: Marcar como activo
                db.Course.Add(course);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(course);
        }

        // GET: Courses/Edit/5 - MODIFICADO ✅
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Course course = db.Course.Find(id);
            // MODIFICADO: Verificar que no esté eliminado
            if (course == null || course.IsDeleted)
            {
                return HttpNotFound();
            }
            return View(course);
        }

        // POST: Courses/Edit/5 - MODIFICADO ✅
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "CourseID,Title,Credits,IsDeleted")] Course course)
        {
            if (ModelState.IsValid)
            {
                db.Entry(course).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(course);
        }

        // GET: Courses/Delete/5 - MODIFICADO ✅
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Course course = db.Course.Find(id);
            // MODIFICADO: Verificar que no esté eliminado
            if (course == null || course.IsDeleted)
            {
                return HttpNotFound();
            }
            return View(course);
        }

        // POST: Courses/Delete/5 - COMPLETAMENTE REESCRITO ✅
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Course course = db.Course.Find(id);
            course.IsDeleted = true;
            db.Entry(course).State = EntityState.Modified;
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