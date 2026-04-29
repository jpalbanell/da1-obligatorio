using Dominio.Entidades;
using Repositorios;

namespace Servicios
{
    public class UsuarioServicio : IUsuarioServicio
    {
        private readonly IUsuarioRepositorio _repositorio;
        private readonly IAuditoriaServicio _auditoriaServicio;
        private readonly ISesionServicio _sesionServicio;
        private int _proximoId = 1;

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
            ValidarEmailUnico(usuario.Email);
            usuario.Id = _proximoId++;
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
            _repositorio.Actualizar(usuario);
        }

        public void EliminarUsuario(int id)
        {
            _repositorio.Eliminar(id);
        }
        
        private void ValidarEmailUnico(string email)
        {
            var existente = _repositorio.ObtenerTodos()
                .Any(u => u.Email.Equals(email, StringComparison.OrdinalIgnoreCase));
            if (existente)
                throw new Exception("Ya existe un usuario con ese email.");
        }
        
    }
}