using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace seguimiento_de_tareas_de_proyectos_MVC.Models
{
    [Table("Roles")]
    public class Rol
    {
        [Key]
        public int RolID { get; set; }
        public string NombreRol { get; set; }
    }
}