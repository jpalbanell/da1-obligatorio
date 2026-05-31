using Microsoft.EntityFrameworkCore;
using Dominio.Entidades;

namespace Repositorios
{
    public class SqlContext : DbContext
    {
        public DbSet<Estadio> Estadios { get; set; }

        public SqlContext(DbContextOptions<SqlContext> options) : base(options)
        {
            if (Database.IsSqlServer())
                Database.Migrate();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(SqlContext).Assembly);
        }
    }
}