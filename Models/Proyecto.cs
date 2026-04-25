using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace seguimiento_de_tareas_de_proyectos_MVC.Models
{
    [Table("Proyectos")]
    public class Proyecto
    {
        [Key]
        public int ProyectoID { get; set; }
        public string NombreProyecto { get; set; }
        public string Descripcion { get; set; }
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [Display(Name = "Fecha de Creación")]
        public DateTime? FechaCreacion { get; set; }
    }
}