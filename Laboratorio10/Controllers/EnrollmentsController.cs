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
    public class EnrollmentsController : Controller
    {
        private UniversityDBEntities db = new UniversityDBEntities();

        // GET: Enrollments - MODIFICADO ✅
        public ActionResult Index()
        {
            var enrollment = db.Enrollment
                .Include(e => e.Course)
                .Include(e => e.Student)
                .Where(e => !e.IsDeleted);  // ← AGREGADO: Filtrar no eliminados
            return View(enrollment.ToList());
        }

        // GET: Enrollments/Details/5 - MODIFICADO ✅
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Enrollment enrollment = db.Enrollment.Find(id);
            // MODIFICADO: Verificar que no esté eliminado
            if (enrollment == null || enrollment.IsDeleted)
            {
                return HttpNotFound();
            }
            return View(enrollment);
        }

        // GET: Enrollments/Create - MODIFICADO ✅
        public ActionResult Create()
        {
            // MODIFICADO: Filtrar solo cursos y estudiantes activos
            ViewBag.CourseID = new SelectList(db.Course.Where(c => !c.IsDeleted), "CourseID", "Title");
            ViewBag.StudentID = new SelectList(db.Student.Where(s => !s.IsDeleted), "StudentID", "LastName");
            return View();
        }

        // POST: Enrollments/Create - MODIFICADO ✅
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "EnrollmentID,Grade,CourseID,StudentID")] Enrollment enrollment)
        {
            if (ModelState.IsValid)
            {
                enrollment.IsDeleted = false;  // ← AGREGADO: Marcar como activo
                db.Enrollment.Add(enrollment);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            // MODIFICADO: Filtrar solo cursos y estudiantes activos
            ViewBag.CourseID = new SelectList(db.Course.Where(c => !c.IsDeleted), "CourseID", "Title", enrollment.CourseID);
            ViewBag.StudentID = new SelectList(db.Student.Where(s => !s.IsDeleted), "StudentID", "LastName", enrollment.StudentID);
            return View(enrollment);
        }

        // GET: Enrollments/Edit/5 - MODIFICADO ✅
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Enrollment enrollment = db.Enrollment.Find(id);
            // MODIFICADO: Verificar que no esté eliminado
            if (enrollment == null || enrollment.IsDeleted)
            {
                return HttpNotFound();
            }
            // MODIFICADO: Filtrar solo cursos y estudiantes activos
            ViewBag.CourseID = new SelectList(db.Course.Where(c => !c.IsDeleted), "CourseID", "Title", enrollment.CourseID);
            ViewBag.StudentID = new SelectList(db.Student.Where(s => !s.IsDeleted), "StudentID", "LastName", enrollment.StudentID);
            return View(enrollment);
        }

        // POST: Enrollments/Edit/5 - MODIFICADO ✅
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "EnrollmentID,Grade,CourseID,StudentID,IsDeleted")] Enrollment enrollment)
        {
            if (ModelState.IsValid)
            {
                db.Entry(enrollment).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            // MODIFICADO: Filtrar solo cursos y estudiantes activos
            ViewBag.CourseID = new SelectList(db.Course.Where(c => !c.IsDeleted), "CourseID", "Title", enrollment.CourseID);
            ViewBag.StudentID = new SelectList(db.Student.Where(s => !s.IsDeleted), "StudentID", "LastName", enrollment.StudentID);
            return View(enrollment);
        }

        // GET: Enrollments/Delete/5 - MODIFICADO ✅
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Enrollment enrollment = db.Enrollment.Find(id);
            // MODIFICADO: Verificar que no esté eliminado
            if (enrollment == null || enrollment.IsDeleted)
            {
                return HttpNotFound();
            }
            return View(enrollment);
        }

        // POST: Enrollments/Delete/5 - COMPLETAMENTE REESCRITO ✅
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Enrollment enrollment = db.Enrollment.Find(id);
            // CAMBIO PRINCIPAL: Soft delete en lugar de eliminación física
            enrollment.IsDeleted = true;
            db.Entry(enrollment).State = EntityState.Modified;
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