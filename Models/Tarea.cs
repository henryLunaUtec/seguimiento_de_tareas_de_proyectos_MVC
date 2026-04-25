using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace seguimiento_de_tareas_de_proyectos_MVC.Models
{
    [Table("Tareas")]
    public class Tarea
    {
        [Key]
        public int TareaID { get; set; }

        [Required(ErrorMessage = "El título es obligatorio")]
        [Display(Name = "Título de la Tarea")]
        public string Titulo { get; set; }

        [Display(Name = "Descripción")]
        public string Descripcion { get; set; }

        [Required(ErrorMessage = "La prioridad es obligatoria")]
        public string Prioridad { get; set; }

        [Display(Name = "Fecha de Vencimiento")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime? FechaVencimiento { get; set; }

        [Required(ErrorMessage = "El estado es obligatorio")]
        public int EstadoID { get; set; }

        [ForeignKey("EstadoID")]
        public virtual Estado Estado { get; set; }

        [Required(ErrorMessage = "El proyecto es obligatorio")]
        public int ProyectoID { get; set; }

        [ForeignKey("ProyectoID")]
        public virtual Proyecto Proyecto { get; set; }

        public int? CreadoPorID { get; set; }
        public int? AsignadoAID { get; set; }
    }
}