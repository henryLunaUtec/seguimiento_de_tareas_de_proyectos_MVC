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
        [Required(ErrorMessage = "El nombre del proyecto es obligatorio para el registro")]
        public string NombreProyecto { get; set; }
        [Required(ErrorMessage = "La descripcion del proyecto es obligatoria para el registro")]
        public string Descripcion { get; set; }
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [Display(Name = "Fecha de Creación")]
        [Required(ErrorMessage = "La Fecha es requerida")]
        public DateTime? FechaCreacion { get; set; }
    }
}