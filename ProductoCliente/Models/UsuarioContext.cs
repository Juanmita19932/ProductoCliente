using Microsoft.EntityFrameworkCore;
using ProductoCliente.Models;

namespace ProductoCliente.Model
{
    public class UsuarioContext : DbContext
    {
        public UsuarioContext(DbContextOptions<UsuarioContext> options) : base(options)
        {
        }

        // Define las propiedades DbSet para tus entidades
        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<Producto> Productos { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Configura las relaciones y restricciones de tus entidades aquí
            // Puedes utilizar Fluent API para definir las configuraciones
        }
    }
}
