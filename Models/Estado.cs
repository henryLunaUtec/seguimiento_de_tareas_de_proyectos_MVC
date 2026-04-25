using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace seguimiento_de_tareas_de_proyectos_MVC.Models
{
    [Table("Estados")] 
    public class Estado
    {
        [Key]
        public int EstadoID { get; set; }
        public string NombreEstado { get; set; }
    }
}