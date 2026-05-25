using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using seguimiento_de_tareas_de_proyectos_MVC.Models;

namespace seguimiento_de_tareas_de_proyectos_MVC.Controllers
{
    public class HomeController : Controller
    {
        private SeguimientoContext db = new SeguimientoContext();

        // GET: Home/Index
        public ActionResult Index()
        {
            if (Session["UsuarioID"] == null)
            {
                return RedirectToAction("Login", "Auth");
            }

            var totalTareas = db.Tareas.Count();
            double porcentajeProgreso = 0;

            if (totalTareas > 0)
            {
                var tareasTerminadas = db.Tareas.Count(t => t.Estado.NombreEstado == "Finalizado");
                porcentajeProgreso = Math.Round(((double)tareasTerminadas / totalTareas) * 100, 1);
            }

            ViewBag.PorcentajeProgreso = porcentajeProgreso;
            ViewBag.TotalTareasGlobales = totalTareas;


            var proyectoLider = db.Tareas
                .GroupBy(t => t.Proyecto.NombreProyecto)
                .Select(g => new { Nombre = g.Key, Cantidad = g.Count() })
                .OrderByDescending(p => p.Cantidad)
                .FirstOrDefault();

            ViewBag.ProyectoMasActivo = proyectoLider != null ? proyectoLider.Nombre : "Sin Proyectos Activos";
            ViewBag.CantidadProyectoMasActivo = proyectoLider != null ? proyectoLider.Cantidad : 0;


            List<RankingUsuarioDTO> listaRanking = (from t in db.Tareas
                                                    join u in db.Usuarios on t.AsignadoAID equals u.UsuarioID
                                                    group t by u.Nombre into grupo
                                                    select new RankingUsuarioDTO
                                                    {
                                                        NombreEmpleado = grupo.Key,
                                                        TotalTareas = grupo.Count()
                                                    })
                               .OrderByDescending(u => u.TotalTareas)
                               .Take(3)
                               .ToList();

            ViewBag.TopUsuariosCarga = listaRanking;

            return View();
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

    // Clase Auxiliar (DTO) para evitar problemas de tipos anónimos en la vista Razor
    public class RankingUsuarioDTO
    {
        public string NombreEmpleado { get; set; }
        public int TotalTareas { get; set; }
    }
}