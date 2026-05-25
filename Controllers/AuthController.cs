using System;
using System.Linq;
using System.Web.Mvc;
using seguimiento_de_tareas_de_proyectos_MVC.Models;

namespace seguimiento_de_tareas_de_proyectos_MVC.Controllers
{
    public class AuthController : Controller
    {
        private SeguimientoContext db = new SeguimientoContext();

        public ActionResult Login()
        {
            if (Session["UsuarioID"] != null)
            {
                return RedirectToAction("Index", "Home");
            }
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(string correo, string password)
        {
            var usuario = db.Usuarios.FirstOrDefault(u => u.Correo == correo && u.Password == password);

            if (usuario != null)
            {
                Session["UsuarioID"] = usuario.UsuarioID;
                Session["NombreUsuario"] = usuario.Nombre;
                Session["RolUsuario"] = usuario.Rol?.NombreRol ?? "Usuario";

                return RedirectToAction("Index", "Home");
            }

            ViewBag.Error = "El correo electrónico o la contraseña son incorrectos.";
            return View();
        }

        public ActionResult Logout()
        {
            Session.Clear();
            Session.Abandon();
            return RedirectToAction("Login");
        }
    }
}