using Dominio.Entidades;
using Repositorios;

namespace Servicios
{
    public class AutenticacionServicio : IAutenticacionServicio
    {
        private readonly IUsuarioRepositorio _repositorio;

        public AutenticacionServicio(IUsuarioRepositorio repositorio)
        {
            _repositorio = repositorio;
        }

        public Usuario Login(string email, string contrasena)
        {
            var usuario = _repositorio.ObtenerTodos()
                .FirstOrDefault(u => u.Email == email);
            if (usuario == null)
                throw new Exception("Credenciales inválidas.");
            if (!usuario.VerificarContrasena(contrasena))
                throw new Exception("Credenciales inválidas.");
            return usuario;
        }
        
        public void CambiarContrasena(int id, string nuevaContrasena)
        {
            var usuario = _repositorio.ObtenerPorId(id);
            if (usuario == null)
                throw new Exception("Usuario no encontrado.");
            usuario.Contrasena = nuevaContrasena;
            _repositorio.Actualizar(usuario);
        }
    }
}