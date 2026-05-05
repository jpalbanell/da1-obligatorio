using Dominio.Entidades;
using Repositorios;

namespace Servicios
{
    public class AutenticacionServicio : IAutenticacionServicio
    {
        private readonly IUsuarioRepositorio _repositorio;
        private readonly ISesionServicio _sesionServicio;
        private readonly IAuditoriaServicio _auditoriaServicio;

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
            var usuario = ObtenerUsuarioExistente(id);
            usuario.Contrasena = nuevaContrasena;
            _repositorio.Actualizar(usuario);
            _auditoriaServicio.Registrar($"Cambio de contraseña: {usuario.Email}",_sesionServicio.ObtenerUsuarioActual());
        }

        public void ReiniciarContrasena(int id)
        {
            var usuario = ObtenerUsuarioExistente(id);
            usuario.Contrasena = "Password@1";
            _repositorio.Actualizar(usuario);
            _auditoriaServicio.Registrar($"Reinicio de contraseña: {usuario.Email}", _sesionServicio.ObtenerUsuarioActual());
        }
        
        private Usuario ObtenerUsuarioExistente(int id)
        {
            var usuario = _repositorio.ObtenerPorId(id);
            if (usuario == null)
                throw new Exception("Usuario no encontrado.");
            return usuario;
        }
        
        private Usuario ObtenerUsuarioPorEmail(string email)
        {
            var usuario = _repositorio.ObtenerTodos()
                .FirstOrDefault(u => u.Email == email);
            if (usuario == null)
                throw new Exception("Credenciales inválidas.");
            return usuario;
        }
        
        private void ValidarContrasena(Usuario usuario, string contrasena)
        {
            if (!usuario.VerificarContrasena(contrasena))
                throw new Exception("Credenciales inválidas.");
        }
    }
}