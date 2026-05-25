using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using seguimiento_de_tareas_de_proyectos_MVC.Models;

namespace seguimiento_de_tareas_de_proyectos_MVC.Controllers
{
    public class TareasController : Controller
    {
        private SeguimientoContext db = new SeguimientoContext();

        // GET: Tareas
        // Modificado con LINQ para recibir filtros desde la interfaz de usuario
        public ActionResult Index(int? proyectoId, string buscarTitulo) 
        {
            if (Session["UsuarioID"] == null) return RedirectToAction("Login", "Auth");

            ViewBag.proyectoId = new SelectList(db.Proyectos, "ProyectoID", "NombreProyecto", proyectoId);
            ViewBag.BusquedaActual = buscarTitulo;

            var query = db.Tareas.Include(t => t.Estado).Include(t => t.Proyecto).AsQueryable();

            if (proyectoId.HasValue) query = query.Where(t => t.ProyectoID == proyectoId.Value);
            if (!string.IsNullOrEmpty(buscarTitulo)) query = query.Where(t => t.Titulo.Contains(buscarTitulo));
            return View(query.ToList());
        }

        // GET: Tareas/Details/5
        public ActionResult Details(int? id)
        {
            if (Session["UsuarioID"] == null) return RedirectToAction("Login", "Auth");

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Tarea tarea = db.Tareas.Find(id);
            if (tarea == null)
            {
                return HttpNotFound();
            }
            return View(tarea);
        }

        // GET: Tareas/Create
        public ActionResult Create()
        {
            if (Session["UsuarioID"] == null) return RedirectToAction("Login", "Auth");

            ViewBag.EstadoID = new SelectList(db.Estados, "EstadoID", "NombreEstado");
            ViewBag.ProyectoID = new SelectList(db.Proyectos, "ProyectoID", "NombreProyecto");
            return View();
        }

        // POST: Tareas/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "TareaID,Titulo,Descripcion,Prioridad,FechaVencimiento,ProyectoID,EstadoID,CreadoPorID,AsignadoAID")] Tarea tarea)
        {
            if (ModelState.IsValid)
            {
                db.Tareas.Add(tarea);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.EstadoID = new SelectList(db.Estados, "EstadoID", "NombreEstado", tarea.EstadoID);
            ViewBag.ProyectoID = new SelectList(db.Proyectos, "ProyectoID", "NombreProyecto", tarea.ProyectoID);
            return View(tarea);
        }

        // GET: Tareas/Edit/5
        public ActionResult Edit(int? id)
        {
            if (Session["UsuarioID"] == null) return RedirectToAction("Login", "Auth");

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Tarea tarea = db.Tareas.Find(id);
            if (tarea == null)
            {
                return HttpNotFound();
            }

            ViewBag.EstadoID = new SelectList(db.Estados, "EstadoID", "NombreEstado", tarea.EstadoID);
            ViewBag.ProyectoID = new SelectList(db.Proyectos, "ProyectoID", "NombreProyecto", tarea.ProyectoID);
            return View(tarea);
        }

        // POST: Tareas/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "TareaID,Titulo,Descripcion,Prioridad,FechaVencimiento,ProyectoID,EstadoID,CreadoPorID,AsignadoAID")] Tarea tarea)
        {
            // ESCUDO DE SEGURIDAD
            if (Session["UsuarioID"] == null) return RedirectToAction("Login", "Auth");

            if (ModelState.IsValid)
            {
                // Forzamos al contexto a marcar la entidad como modificada
                db.Entry(tarea).State = EntityState.Modified;

                // Guardamos los cambios de forma síncrona en la base de datos SQL
                db.SaveChanges();

                return RedirectToAction("Index");
            }

            // Si el modelo llega a fallar, volvemos a cargar los combos con LINQ ordenados para no romper la vista
            var proyectosParaCombo = db.Proyectos.OrderBy(p => p.NombreProyecto).ToList();
            var estadosParaCombo = db.Estados.OrderBy(e => e.NombreEstado).ToList();

            ViewBag.EstadoID = new SelectList(estadosParaCombo, "EstadoID", "NombreEstado", tarea.EstadoID);
            ViewBag.ProyectoID = new SelectList(proyectosParaCombo, "ProyectoID", "NombreProyecto", tarea.ProyectoID);

            return View(tarea);
        }
        // GET: Tareas/Delete/5
        public ActionResult Delete(int? id)
        {
            if (Session["UsuarioID"] == null) return RedirectToAction("Login", "Auth");

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Tarea tarea = db.Tareas.Find(id);
            if (tarea == null)
            {
                return HttpNotFound();
            }
            return View(tarea);
        }

        // POST: Tareas/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Tarea tarea = db.Tareas.Find(id);
            db.Tareas.Remove(tarea);
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