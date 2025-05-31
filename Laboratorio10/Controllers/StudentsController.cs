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
    public class StudentsController : Controller
    {
        private UniversityDBEntities db = new UniversityDBEntities();

        // GET: Students - MODIFICADO ✅
        public ActionResult Index()
        {
            var estudiantesActivos = db.Student
                                      .Where(s => !s.IsDeleted)
                                      .ToList();
            return View(estudiantesActivos);
        }

        // GET: Students/Details/5 - MODIFICADO ✅
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Student student = db.Student.Find(id);
            // MODIFICADO: Verificar que no esté eliminado
            if (student == null || student.IsDeleted)
            {
                return HttpNotFound();
            }
            return View(student);
        }

        // GET: Students/Create - SIN CAMBIOS
        public ActionResult Create()
        {
            return View();
        }

        // POST: Students/Create - MODIFICADO ✅
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "StudentID,LastName,FirstName,Email,EnrollmentDate")] Student student)
        {
            if (ModelState.IsValid)
            {
                student.IsDeleted = false;  // ← AGREGADO: Marcar como activo
                db.Student.Add(student);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(student);
        }

        // GET: Students/Edit/5 - MODIFICADO ✅
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Student student = db.Student.Find(id);
            // MODIFICADO: Verificar que no esté eliminado
            if (student == null || student.IsDeleted)
            {
                return HttpNotFound();
            }
            return View(student);
        }

        // POST: Students/Edit/5 - MODIFICADO ✅
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "StudentID,LastName,FirstName,Email,EnrollmentDate,IsDeleted")] Student student)
        {
            if (ModelState.IsValid)
            {
                db.Entry(student).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(student);
        }

        // GET: Students/Delete/5 - MODIFICADO ✅
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Student student = db.Student.Find(id);
            // MODIFICADO: Verificar que no esté eliminado
            if (student == null || student.IsDeleted)
            {
                return HttpNotFound();
            }
            return View(student);
        }

        // POST: Students/Delete/5 - COMPLETAMENTE REESCRITO ✅
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Student student = db.Student.Find(id);
            student.IsDeleted = true;
            db.Entry(student).State = EntityState.Modified;
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