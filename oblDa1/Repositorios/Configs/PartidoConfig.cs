using Dominio.Entidades;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Repositorios.Configs
{
    public class PartidoConfig : IEntityTypeConfiguration<Partido>
    {
        public void Configure(EntityTypeBuilder<Partido> builder)
        {
            builder.HasKey(p => p.Id);
            builder.Property(p => p.Id).ValueGeneratedOnAdd();

            // Field access para Codigo y Fecha: sus setters lanzan excepción con valores
            // vacíos/default, pero EF necesita poder escribir el campo durante materialización.
            builder.Property(p => p.Codigo)
                   .HasField("_codigo")
                   .UsePropertyAccessMode(PropertyAccessMode.Field)
                   .IsRequired()
                   .HasMaxLength(20);

            builder.Property(p => p.Fecha)
                   .HasField("_fecha")
                   .UsePropertyAccessMode(PropertyAccessMode.Field)
                   .IsRequired();

            builder.Property(p => p.Fase)
                   .HasConversion<string>()
                   .HasMaxLength(30);

            builder.Property(p => p.GolesLocal)
                   .HasField("_golesLocal")
                   .UsePropertyAccessMode(PropertyAccessMode.Field);

            builder.Property(p => p.GolesVisitante)
                   .HasField("_golesVisitante")
                   .UsePropertyAccessMode(PropertyAccessMode.Field);

            // --- Relaciones a Equipo (tres FKs distintas; se nombran explícitamente para evitar ambigüedad) ---

            builder.HasOne(p => p.EquipoLocal)
                   .WithMany()
                   .HasForeignKey("EquipoLocalNombre")
                   .IsRequired(false)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(p => p.EquipoVisitante)
                   .WithMany()
                   .HasForeignKey("EquipoVisitanteNombre")
                   .IsRequired(false)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(p => p.Vencedor)
                   .WithMany()
                   .HasForeignKey("VencedorNombre")
                   .IsRequired(false)
                   .OnDelete(DeleteBehavior.Restrict);

            // --- Relación a Estadio (Estadio.Partidos está ignorado en EstadioConfig) ---

            builder.HasOne(p => p.Estadio)
                   .WithMany()
                   .HasForeignKey("EstadioNombre")
                   .IsRequired(false)
                   .OnDelete(DeleteBehavior.Restrict);

            // --- Relación a Grupo (nullable: los partidos de fase eliminatoria no tienen grupo) ---

            builder.HasOne(p => p.Grupo)
                   .WithMany(g => g.ListaPartidos)
                   .IsRequired(false)
                   .OnDelete(DeleteBehavior.Restrict);

            // --- Auto-referencias (dos FKs; se nombran explícitamente para evitar ambigüedad) ---

            builder.HasOne(p => p.OrigenLocal)
                   .WithMany()
                   .HasForeignKey("OrigenLocalId")
                   .IsRequired(false)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(p => p.OrigenVisitante)
                   .WithMany()
                   .HasForeignKey("OrigenVisitanteId")
                   .IsRequired(false)
                   .OnDelete(DeleteBehavior.Restrict);

            // --- Modos de acceso a campo para navegaciones con backing fields ---

            builder.Navigation(p => p.EquipoLocal)
                   .HasField("_equipoLocal")
                   .UsePropertyAccessMode(PropertyAccessMode.Field);

            builder.Navigation(p => p.EquipoVisitante)
                   .HasField("_equipoVisitante")
                   .UsePropertyAccessMode(PropertyAccessMode.Field);

            // Estadio y Grupo tienen setters que lanzan en null; field access permite que EF
            // escriba null al materializar partidos sin estadio asignado o de fase eliminatoria.
            builder.Navigation(p => p.Estadio)
                   .HasField("_estadio")
                   .UsePropertyAccessMode(PropertyAccessMode.Field);

            builder.Navigation(p => p.Grupo)
                   .HasField("_grupo")
                   .UsePropertyAccessMode(PropertyAccessMode.Field);

            builder.Navigation(p => p.Vencedor)
                   .HasField("_vencedor")
                   .UsePropertyAccessMode(PropertyAccessMode.Field);
        }
    }
}
