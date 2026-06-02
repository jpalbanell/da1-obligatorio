using Microsoft.EntityFrameworkCore;
using Dominio.Entidades;

namespace Repositorios
{
    public class SqlContext : DbContext
    {
        public DbSet<Estadio> Estadios { get; set; }
        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<Equipo> Equipos { get; set; }

        public SqlContext(DbContextOptions<SqlContext> options) : base(options)
        {
        }
        
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(SqlContext).Assembly);
        }
    }
}