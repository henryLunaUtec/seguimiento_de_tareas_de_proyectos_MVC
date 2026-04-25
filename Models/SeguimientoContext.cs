using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;

namespace seguimiento_de_tareas_de_proyectos_MVC.Models

{
    public class SeguimientoContext : DbContext
    {
        public SeguimientoContext() : base("name=CadenaConexion")
        {
        }

        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<Tarea> Tareas { get; set; }
        public DbSet<Proyecto> Proyectos { get; set; }
        public DbSet<Rol> Roles { get; set; }

        public DbSet<Estado> Estados { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
        }
    }
}