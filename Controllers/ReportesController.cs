using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Linq;
using System.Web.Mvc;
using seguimiento_de_tareas_de_proyectos_MVC.Models;

namespace seguimiento_de_tareas_de_proyectos_MVC.Controllers
{
    public class ReportesController : Controller
    {
        private SeguimientoContext db = new SeguimientoContext();

        // GET: Reportes
        public ActionResult Index()
        {
            if (Session["UsuarioID"] == null)
            {
                return RedirectToAction("Login", "Auth");
            }

            int idUsuarioLogueado = Convert.ToInt32(Session["UsuarioID"]);

            List<TareaReporteDTO> misTareasAdo = new List<TareaReporteDTO>();

            string cadenaConexion = ConfigurationManager.ConnectionStrings["CadenaConexion"].ConnectionString;

            using (SqlConnection conexion = new SqlConnection(cadenaConexion))
            {
                using (SqlCommand comando = new SqlCommand("sp_ListarTareasPorUsuario", conexion))
                {
                    comando.CommandType = CommandType.StoredProcedure;

                    comando.Parameters.AddWithValue("@UsuarioID", idUsuarioLogueado);

                    conexion.Open();
                    using (SqlDataReader lector = comando.ExecuteReader())
                    {
                        while (lector.Read())
                        {
                            var tarea = new TareaReporteDTO
                            {
                                TareaID = Convert.ToInt32(lector["TareaID"]),
                                Titulo = lector["Titulo"].ToString(),
                                Descripcion = lector["Descripcion"].ToString(),
                                NombreEstado = lector["NombreEstado"].ToString(),
                                NombreProyecto = lector["NombreProyecto"].ToString(),
                                Prioridad = lector["Prioridad"].ToString(),
                                FechaVencimiento = lector["FechaVencimiento"] != DBNull.Value ? Convert.ToDateTime(lector["FechaVencimiento"]) : (DateTime?)null
                            };
                            misTareasAdo.Add(tarea);
                        }
                    }
                }
            }

            ViewBag.MisTareasAsignadas = misTareasAdo;

           
            ViewBag.TareasPorEstado = db.Tareas
                .GroupBy(t => t.Estado.NombreEstado)
                .Select(g => new { Estado = g.Key, Total = g.Count() })
                .ToDictionary(x => x.Estado ?? "Pendiente", x => x.Total);

            var topCriticas = db.Tareas
                .Where(t => t.Prioridad == "Alta")
                .OrderBy(t => t.FechaVencimiento)
                .Take(5)
                .ToList();

            return View(topCriticas);
        }
    }

    // Clase Modelo Temporal (DTO) para almacenar las columnas planas devueltas por el SP en ADO.NET
    public class TareaReporteDTO
    {
        public int TareaID { get; set; }
        public string Titulo { get; set; }
        public string Descripcion { get; set; }
        public string NombreEstado { get; set; }
        public string NombreProyecto { get; set; }
        public string Prioridad { get; set; }
        public DateTime? FechaVencimiento { get; set; }
    }
}