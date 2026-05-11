using Dominio.Entidades;
using IRepositorios;

namespace Repositorios
{
    public class UsuarioRepositorio : IUsuarioRepositorio
    {
        private List<Usuario> _usuarios = new List<Usuario>();

        public void Agregar(Usuario usuario)
        {
            _usuarios.Add(usuario);
        }

        public List<Usuario> ObtenerTodos()
        {
            return _usuarios;
        }

        public Usuario ObtenerPorId(int id)
        {
            return _usuarios.FirstOrDefault(u => u.Id == id);
        }

        public void Actualizar(Usuario usuario)
        {
            var indice = _usuarios.FindIndex(u => u.Id == usuario.Id);
            ValidarUsuarioExistente(indice);
            _usuarios[indice] = usuario;
        }

        public void Eliminar(int id)
        {
            var usuario = ObtenerPorId(id);
            ValidarUsuarioNoNulo(usuario);
            _usuarios.Remove(usuario);
        }

        private void ValidarUsuarioExistente(int indice)
        {
            if (indice == -1)
                throw new Exception("Usuario no encontrado");
        }

        private void ValidarUsuarioNoNulo(Usuario usuario)
        {
            if (usuario == null)
                throw new Exception("Usuario no encontrado");
        }
    }
}