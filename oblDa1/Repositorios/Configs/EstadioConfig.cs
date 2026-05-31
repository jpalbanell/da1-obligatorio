using Dominio.Entidades;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Repositorios.Configs
{
    public class EstadioConfig : IEntityTypeConfiguration<Estadio>
    {
        public void Configure(EntityTypeBuilder<Estadio> builder)
        {
            builder.HasKey(e => e.Nombre);
            builder.Property(e => e.Nombre).HasMaxLength(80);
            builder.Property(e => e.Ciudad).IsRequired().HasMaxLength(60);
            builder.Property(e => e.Descripcion).HasMaxLength(400);
            builder.Ignore(e => e.Partidos);
        }
    }
}