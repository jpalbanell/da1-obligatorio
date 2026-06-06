using Dominio.Entidades;
using IRepositorios;

namespace Repositorios
{
    public class UsuarioRepositorio : IUsuarioRepositorio
    {
        private readonly SqlContext _context;

        public UsuarioRepositorio(SqlContext context)
        {
            _context = context;
        }

        public void Agregar(Usuario usuario)
        {
            _context.Usuarios.Add(usuario);
            _context.SaveChanges();
        }

        public List<Usuario> ObtenerTodos()
        {
            return _context.Usuarios.ToList();
        }

        public List<Usuario> ObtenerPorRol(Rol rol)
        {
            return _context.Usuarios.Where(u => u.Roles.Contains(rol)).ToList();
        }

        public Usuario ObtenerPorId(int id)
        {
            return _context.Usuarios.FirstOrDefault(u => u.Id == id);
        }

        public void Actualizar(Usuario usuario)
        {
            var existente = _context.Usuarios.FirstOrDefault(u => u.Id == usuario.Id);
            ValidarUsuarioNoNulo(existente);
            existente.Nombre = usuario.Nombre;
            existente.Apellido = usuario.Apellido;
            existente.Email = usuario.Email;
            existente.FechaNacimiento = usuario.FechaNacimiento;
            existente.AsignarContrasenaCifrada(usuario.ObtenerContrasenaCifrada());
            _context.SaveChanges();
        }

        public void Eliminar(int id)
        {
            var usuario = _context.Usuarios.FirstOrDefault(u => u.Id == id);
            ValidarUsuarioNoNulo(usuario);
            _context.Usuarios.Remove(usuario);
            _context.SaveChanges();
        }

        private void ValidarUsuarioNoNulo(Usuario usuario)
        {
            if (usuario == null)
                throw new KeyNotFoundException("Usuario no encontrado");
        }
    }
}
