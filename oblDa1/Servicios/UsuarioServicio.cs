using Dominio.Entidades;
using IRepositorios;
using IServicios;

namespace Servicios
{
    public class UsuarioServicio : IUsuarioServicio
    {
        private readonly IUsuarioRepositorio _repositorio;
        private readonly IAuditoriaServicio _auditoriaServicio;
        private readonly ISesionServicio _sesionServicio;

        public UsuarioServicio(IUsuarioRepositorio repositorio,
            IAuditoriaServicio auditoriaServicio,
            ISesionServicio sesionServicio)
        {
            _repositorio = repositorio;
            _auditoriaServicio = auditoriaServicio;
            _sesionServicio = sesionServicio;
        }

        public void AgregarUsuario(Usuario usuario)
        {
            _sesionServicio.ValidarRol(Rol.Administrador);
            ValidarEmailUnico(usuario.Email);
            var usuarios = _repositorio.ObtenerTodos();
            usuario.Id = usuarios.Any() ? usuarios.Max(u => u.Id) + 1 : 1;
            _repositorio.Agregar(usuario);
            _auditoriaServicio.Registrar($"Alta de usuario: {usuario.Nombre}", _sesionServicio.ObtenerUsuarioActual());
        }

        public Usuario ObtenerUsuario(int id)
        {
            return _repositorio.ObtenerPorId(id);
        }

        public List<Usuario> ObtenerTodos()
        {
            return _repositorio.ObtenerTodos();
        }

        public void ModificarUsuario(Usuario usuario)
        {
            _sesionServicio.ValidarRol(Rol.Administrador);
            var original = _repositorio.ObtenerPorId(usuario.Id);
            ValidarUsuarioExiste(usuario.Id);
            ValidarEmailUnicoEnEdicion(usuario);
            usuario.AsignarContrasenaCifrada(original.ObtenerContrasenaCifrada());
            _repositorio.Actualizar(usuario);
            _auditoriaServicio.Registrar($"Edición de usuario: {usuario.Nombre}", _sesionServicio.ObtenerUsuarioActual());
        }

        private void ValidarUsuarioExiste(int id)
        {
            if (_repositorio.ObtenerPorId(id) == null)
                throw new KeyNotFoundException("Usuario no encontrado.");
        }

        private void ValidarEmailUnicoEnEdicion(Usuario usuario)
        {
            var existente = _repositorio.ObtenerTodos()
                .FirstOrDefault(u => u.Email.Equals(usuario.Email, StringComparison.OrdinalIgnoreCase));
            if (existente != null && existente.Id != usuario.Id)
                throw new InvalidOperationException("Ya existe un usuario con ese email.");
        }

        public void EliminarUsuario(int id)
        {
            _sesionServicio.ValidarRol(Rol.Administrador);
            ValidarUsuarioExiste(id);
            _repositorio.Eliminar(id);
            _auditoriaServicio.Registrar($"Eliminación de usuario: {id}", _sesionServicio.ObtenerUsuarioActual());
        }
        
        private void ValidarEmailUnico(string email)
        {
            var existente = _repositorio.ObtenerTodos()
                .Any(u => u.Email.Equals(email, StringComparison.OrdinalIgnoreCase));
            if (existente)
                throw new InvalidOperationException("Ya existe un usuario con ese email.");
        }
        
        public Usuario Login(string email, string contrasena)
        {
            var usuario = _repositorio.ObtenerTodos()
                .FirstOrDefault(u => u.Email.Equals(email, StringComparison.OrdinalIgnoreCase));
            if (usuario == null)
                throw new UnauthorizedAccessException("Credenciales inválidas.");
            if (!usuario.VerificarContrasena(contrasena))
                throw new UnauthorizedAccessException("Credenciales inválidas.");
            _sesionServicio.IniciarSesion(usuario);
            return usuario;
        }
    }
}