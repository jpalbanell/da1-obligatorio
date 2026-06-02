using Dominio.Entidades;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Repositorios.Configs
{
    public class EquipoConfig : IEntityTypeConfiguration<Equipo>
    {
        public void Configure(EntityTypeBuilder<Equipo> builder)
        {
            builder.HasKey(e => e.Nombre);
            builder.Property(e => e.Nombre).HasMaxLength(60);
            builder.Property(e => e.Confederacion)
                .HasConversion<string>()
                .HasMaxLength(20);
            builder.Property(e => e.RankingFifa);
        }
    }
}