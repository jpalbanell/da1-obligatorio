using Dominio.Entidades;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Repositorios.Configs
{
    public class UsuarioConfig : IEntityTypeConfiguration<Usuario>
    {
        public void Configure(EntityTypeBuilder<Usuario> builder)
        {
            builder.HasKey(u => u.Id);
            builder.Property(u => u.Id).ValueGeneratedOnAdd();
            builder.Property(u => u.Nombre).IsRequired().HasMaxLength(100);
            builder.Property(u => u.Apellido).IsRequired().HasMaxLength(100);
            builder.Property(u => u.Email).IsRequired().HasMaxLength(200);
            builder.Property(u => u.FechaNacimiento).IsRequired();
            builder.Property(u => u.Contrasena)
                   .HasField("_contrasena")
                   .UsePropertyAccessMode(PropertyAccessMode.Field)
                   .HasColumnName("Contrasena")
                   .IsRequired();
            builder.Ignore(u => u.Roles);
        }
    }
}
