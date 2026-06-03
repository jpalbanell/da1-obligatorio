using Dominio.Entidades;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.ChangeTracking;

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
            builder.Property(u => u.Roles)
                   .HasConversion(
                       roles => string.Join(',', roles.Select(r => r.ToString())),
                       csv => string.IsNullOrEmpty(csv)
                           ? new List<Rol>()
                           : csv.Split(new char[] { ',' }).Select(v => (Rol)Enum.Parse(typeof(Rol), v)).ToList())
                   .HasColumnName("Roles")
                   .HasMaxLength(100)
                   .IsRequired()
                   .Metadata.SetValueComparer(new ValueComparer<List<Rol>>(
                       (a, b) => a.SequenceEqual(b),
                       roles => roles.Aggregate(0, (hash, r) => HashCode.Combine(hash, r.GetHashCode())),
                       roles => roles.ToList()));
        }
    }
}
