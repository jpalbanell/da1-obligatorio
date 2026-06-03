using Dominio.Entidades;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Repositorios.Configs
{
    public class LogAuditoriaConfig : IEntityTypeConfiguration<LogAuditoria>
    {
        public void Configure(EntityTypeBuilder<LogAuditoria> builder)
        {
            builder.HasKey(l => l.Id);
            builder.Property(l => l.Id).ValueGeneratedOnAdd();
            builder.Property(l => l.Timestamp)
                   .HasField("_timestamp")
                   .UsePropertyAccessMode(PropertyAccessMode.Field)
                   .IsRequired();
            builder.Property(l => l.Accion)
                   .HasField("_accion")
                   .UsePropertyAccessMode(PropertyAccessMode.Field)
                   .IsRequired()
                   .HasMaxLength(500);
            builder.HasOne(l => l.Usuario)
                   .WithMany()
                   .IsRequired();
            builder.Navigation(l => l.Usuario)
                   .HasField("_usuario")
                   .UsePropertyAccessMode(PropertyAccessMode.Field);
        }
    }
}
