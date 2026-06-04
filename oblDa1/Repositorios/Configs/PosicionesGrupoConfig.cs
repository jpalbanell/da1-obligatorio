using Dominio.Entidades;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Repositorios.Configs
{
    public class PosicionesGrupoConfig : IEntityTypeConfiguration<PosicionesGrupo>
    {
        public void Configure(EntityTypeBuilder<PosicionesGrupo> builder)
        {
            builder.HasKey(p => p.Id);
            builder.Property(p => p.Id).ValueGeneratedOnAdd();

            builder.Ignore(p => p.EtiquetaGrupo);

            builder.Property(p => p.Puntos)
                   .HasField("_puntos")
                   .UsePropertyAccessMode(PropertyAccessMode.Field);

            builder.Property(p => p.GolesFavor)
                   .HasField("_golesFavor")
                   .UsePropertyAccessMode(PropertyAccessMode.Field);

            builder.Property(p => p.GolesContra)
                   .HasField("_golesContra")
                   .UsePropertyAccessMode(PropertyAccessMode.Field);

            // PosicionFinal valida 1–4 pero el valor por defecto en BD es 0;
            // field access evita que el setter lance excepción al materializar filas no finalizadas.
            builder.Property(p => p.PosicionFinal)
                   .HasField("_posicionFinal")
                   .UsePropertyAccessMode(PropertyAccessMode.Field);

            builder.HasOne(p => p.Equipo)
                   .WithMany()
                   .IsRequired()
                   .OnDelete(DeleteBehavior.Restrict);

            builder.Navigation(p => p.Equipo)
                   .HasField("_equipo")
                   .UsePropertyAccessMode(PropertyAccessMode.Field);

            builder.HasOne(p => p.Grupo)
                   .WithMany(g => g.ListaPosiciones)
                   .IsRequired()
                   .OnDelete(DeleteBehavior.Restrict);

            builder.Navigation(p => p.Grupo)
                   .HasField("_grupo")
                   .UsePropertyAccessMode(PropertyAccessMode.Field);
        }
    }
}
