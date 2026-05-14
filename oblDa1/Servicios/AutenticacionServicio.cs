using Dominio.Entidades;
using IRepositorios;
using IServicios;

namespace Servicios
{
    public class AutenticacionServicio : IAutenticacionServicio
    {
        private readonly IUsuarioRepositorio _repositorio;
        private readonly ISesionServicio _sesionServicio;
        private readonly IAuditoriaServicio _auditoriaServicio;
        
        private const string ContrasenaDefault = "Password@1";

        public AutenticacionServicio(
            IUsuarioRepositorio repositorio,
            IAuditoriaServicio auditoriaServicio,
            ISesionServicio sesionServicio)
        {
            _repositorio = repositorio;
            _auditoriaServicio = auditoriaServicio;
            _sesionServicio = sesionServicio;
        }

        public Usuario Login(string email, string contrasena)
        {
            var usuario = ObtenerUsuarioPorEmail(email);
            ValidarContrasena(usuario, contrasena);
            _sesionServicio.IniciarSesion(usuario);
            _auditoriaServicio.Registrar($"Login exitoso: {usuario.Email}", usuario);
            return usuario;
        }
        
        public void CambiarContrasena(int id, string nuevaContrasena)
        {
            var usuarioEnSesion = _sesionServicio.ObtenerUsuarioActual();
            if (usuarioEnSesion == null)
                throw new InvalidOperationException("No hay sesión activa.");
            if (usuarioEnSesion.Id != id)
                throw new UnauthorizedAccessException("No podés cambiar la contraseña de otro usuario.");
            
            var usuario = ObtenerUsuarioExistente(id);
            usuario.Contrasena = nuevaContrasena;
            _repositorio.Actualizar(usuario);
            _auditoriaServicio.Registrar($"Cambio de contraseña: {usuario.Email}",_sesionServicio.ObtenerUsuarioActual());
        }

        public void ReiniciarContrasena(int id)
        {
            _sesionServicio.ValidarRol(Rol.Administrador);
            var usuarioActual = _sesionServicio.ObtenerUsuarioActual();
            var usuario = ObtenerUsuarioExistente(id);
            if (usuarioActual.Id == usuario.Id)
                throw new UnauthorizedAccessException("No podés reiniciar tu propia contraseña.");
            usuario.Contrasena = GenerarContrasenaDefault();
            _repositorio.Actualizar(usuario);
            _auditoriaServicio.Registrar($"Reinicio de contraseña: {usuario.Email}", _sesionServicio.ObtenerUsuarioActual());
        }
        
        private string GenerarContrasenaDefault()
        {
            return ContrasenaDefault;
        }
        
        private Usuario ObtenerUsuarioExistente(int id)
        {
            var usuario = _repositorio.ObtenerPorId(id);
            if (usuario == null)
                throw new KeyNotFoundException("Usuario no encontrado.");
            return usuario;
        }
        
        private Usuario ObtenerUsuarioPorEmail(string email)
        {
            var usuario = _repositorio.ObtenerTodos()
                .FirstOrDefault(u => u.Email.Equals(email, StringComparison.OrdinalIgnoreCase));
            if (usuario == null)
                throw new UnauthorizedAccessException("Credenciales inválidas.");
            return usuario;
        }
        
        private void ValidarContrasena(Usuario usuario, string contrasena)
        {
            if (!usuario.VerificarContrasena(contrasena))
                throw new UnauthorizedAccessException("Credenciales inválidas.");
        }
    }
}