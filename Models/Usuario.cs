using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Core.Common.CommandTrees.ExpressionBuilder;
using System.Linq;
using System.Web;

namespace seguimiento_de_tareas_de_proyectos_MVC.Models
{
    [Table("Usuarios")]
    public class Usuario
    {
        [Key]
        public int UsuarioID { get; set; }
        [Required(ErrorMessage = "El nombre es obligatorio para el registro")]
        public string Nombre { get; set; }
        [Required(ErrorMessage = "El correo es obligatorio para el registro")]
        public string Correo { get; set; }
        [Required(ErrorMessage = "La contraseña es obligatorio para el registro")]
        public string Password { get; set; }
        public int RolID { get; set; }

        [ForeignKey("RolID")]
        public virtual Rol Rol { get; set; }
    }
}