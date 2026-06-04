using Dominio.Entidades;

namespace Web.DTOs
{
    public class UsuarioRequest
    {
        public string Nombre { get; set; } = "";
        public string Apellido { get; set; } = "";
        public string Email { get; set; } = "";
        public DateTime FechaNacimiento { get; set; } = DateTime.Today;
        public string Contrasena { get; set; } = "";
        public bool EsAdministrador { get; set; } = false;
        public bool EsEditor { get; set; } = false;
        public bool EsPeriodista { get; set; } = false;

        public Usuario ToEntity()
        {
            var usuario = new Usuario();
            usuario.Nombre = Nombre;
            usuario.Apellido = Apellido;
            usuario.Email = Email;
            usuario.FechaNacimiento = FechaNacimiento;
            if (!string.IsNullOrEmpty(Contrasena))
                usuario.Contrasena = Contrasena;
            if (EsAdministrador) usuario.Roles.Add(Rol.Administrador);
            if (EsEditor) usuario.Roles.Add(Rol.Editor);
            if (EsPeriodista) usuario.Roles.Add(Rol.Periodista);
            return usuario;
        }
    }
}