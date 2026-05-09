using Dominio.Entidades;

namespace Web.DTOs
{
    public class UsuarioResponse
    {
        public int Id { get; set; }
        public string Nombre { get; set; } = "";
        public string Apellido { get; set; } = "";
        public string Email { get; set; } = "";
        public string FechaNacimiento { get; set; } = "";
        public List<string> Roles { get; set; } = new();

        public static UsuarioResponse FromEntity(Usuario usuario) => new()
        {
            Id = usuario.Id,
            Nombre = usuario.Nombre,
            Apellido = usuario.Apellido,
            Email = usuario.Email,
            FechaNacimiento = usuario.FechaNacimiento.ToString("MM-dd-yyyy"),
            Roles = usuario.Roles.Select(r => r.ToString()).ToList()
        };
    }
}