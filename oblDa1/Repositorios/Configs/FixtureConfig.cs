using Dominio.Entidades;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Repositorios.Configs
{
    public class FixtureConfig : IEntityTypeConfiguration<Fixture>
    {
        public void Configure(EntityTypeBuilder<Fixture> builder)
        {
            builder.HasKey(f => f.Id);
            builder.Property(f => f.Id).ValueGeneratedOnAdd();

            builder.Property(f => f.SemillaFixture);
            builder.Property(f => f.FechaInicioTorneo);
            builder.Property(f => f.MaxPartidosPorDia);
            builder.Property(f => f.SeparacionEntreFechas);
            builder.Property(f => f.EstaGenerado);
            builder.Property(f => f.CrucesGenerados);

            builder.HasMany(f => f.Equipos)
                   .WithMany()
                   .UsingEntity(
                       "FixtureEquipos",
                       l => l.HasOne(typeof(Equipo)).WithMany()
                             .HasForeignKey("EquipoNombre").HasPrincipalKey("Nombre")
                             .OnDelete(DeleteBehavior.Restrict),
                       r => r.HasOne(typeof(Fixture)).WithMany()
                             .HasForeignKey("FixtureId").HasPrincipalKey("Id")
                             .OnDelete(DeleteBehavior.Cascade),
                       j => j.HasKey("FixtureId", "EquipoNombre")
                   );

            builder.HasMany(f => f.Estadios)
                   .WithMany()
                   .UsingEntity(
                       "FixtureEstadios",
                       l => l.HasOne(typeof(Estadio)).WithMany()
                             .HasForeignKey("EstadioNombre").HasPrincipalKey("Nombre")
                             .OnDelete(DeleteBehavior.Restrict),
                       r => r.HasOne(typeof(Fixture)).WithMany()
                             .HasForeignKey("FixtureId").HasPrincipalKey("Id")
                             .OnDelete(DeleteBehavior.Cascade),
                       j => j.HasKey("FixtureId", "EstadioNombre")
                   );

            builder.Navigation(f => f.Equipos)
                   .HasField("_equipos")
                   .UsePropertyAccessMode(PropertyAccessMode.Field);

            builder.Navigation(f => f.Estadios)
                   .HasField("_estadios")
                   .UsePropertyAccessMode(PropertyAccessMode.Field);
        }
    }
}
