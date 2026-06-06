using Dominio.Entidades;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Repositorios.Configs
{
    public class IncidenciaConfig : IEntityTypeConfiguration<Incidencia>
    {
        public void Configure(EntityTypeBuilder<Incidencia> builder)
        {
            builder.HasKey(i => i.Id);
            builder.Property(i => i.Id).ValueGeneratedOnAdd();

            builder.Property(i => i.Tipo)
                   .HasConversion<string>()
                   .HasMaxLength(30)
                   .IsRequired();

            builder.Property(i => i.Cantidad)
                   .HasField("_cantidad")
                   .UsePropertyAccessMode(PropertyAccessMode.Field)
                   .IsRequired();

            builder.HasOne(i => i.Equipo)
                   .WithMany()
                   .HasForeignKey("EquipoNombre")
                   .IsRequired()
                   .OnDelete(DeleteBehavior.Restrict);

            builder.Navigation(i => i.Equipo)
                   .HasField("_equipo")
                   .UsePropertyAccessMode(PropertyAccessMode.Field);

            builder.HasOne<Partido>()
                   .WithMany(p => p.Incidencias)
                   .HasForeignKey("PartidoId")
                   .IsRequired()
                   .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
