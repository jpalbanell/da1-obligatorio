using Dominio.Entidades;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Repositorios.Configs
{
    public class NotificacionConfig : IEntityTypeConfiguration<Notificacion>
    {
        public void Configure(EntityTypeBuilder<Notificacion> builder)
        {
            builder.HasKey(n => n.Id);
            builder.Property(n => n.Id).ValueGeneratedOnAdd();
            builder.Property(n => n.Mensaje)
                   .HasField("_mensaje")
                   .UsePropertyAccessMode(PropertyAccessMode.Field)
                   .IsRequired()
                   .HasMaxLength(500);
            builder.Property(n => n.FechaCreacion)
                   .HasField("_fechaCreacion")
                   .UsePropertyAccessMode(PropertyAccessMode.Field)
                   .IsRequired();
            builder.Property(n => n.Leida)
                   .IsRequired();
            builder.HasOne(n => n.Periodista)
                   .WithMany()
                   .IsRequired();
            builder.Navigation(n => n.Periodista)
                   .HasField("_periodista")
                   .UsePropertyAccessMode(PropertyAccessMode.Field);
        }
    }
}
