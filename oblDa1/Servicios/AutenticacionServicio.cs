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
            var usuario = ObtenerUsuarioPorEmail(email);
            ValidarContrasena(usuario, contrasena);
            return usuario;
        }
        
        public void CambiarContrasena(int id, string nuevaContrasena)
        {
            var usuario = ObtenerUsuarioExistente(id);
            usuario.Contrasena = nuevaContrasena;
            _repositorio.Actualizar(usuario);
        }

        public void ReiniciarContrasena(int id)
        {
            var usuario = ObtenerUsuarioExistente(id);
            usuario.Contrasena = "Password@1";
            _repositorio.Actualizar(usuario);
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