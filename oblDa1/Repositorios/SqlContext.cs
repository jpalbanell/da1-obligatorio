using Microsoft.EntityFrameworkCore;
using Dominio.Entidades;

namespace Repositorios
{
    public class SqlContext : DbContext
    {
        public DbSet<Estadio> Estadios { get; set; }
        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<Equipo> Equipos { get; set; }
        public DbSet<LogAuditoria> LogsAuditoria { get; set; }
        public DbSet<Grupo> Grupos { get; set; }
        public DbSet<PosicionesGrupo> PosicionesGrupo { get; set; }
        public DbSet<Partido> Partidos { get; set; }

        public SqlContext(DbContextOptions<SqlContext> options) : base(options)
        {
        }
        
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(SqlContext).Assembly);
        }
    }
}