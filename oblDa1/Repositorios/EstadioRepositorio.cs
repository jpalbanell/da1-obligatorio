using Dominio.Entidades;
using IRepositorios;

namespace Repositorios
{
    public class EstadioRepositorio : IEstadioRepositorio
    {
        private readonly SqlContext _context;
        
        public EstadioRepositorio(SqlContext context)
        {
            _context = context;
        }

        public void Agregar(Estadio estadio)
        {
            _context.Estadios.Add(estadio);
            _context.SaveChanges();
        }

        public List<Estadio> ObtenerTodos()
        {
            return _context.Estadios.ToList();
        }

        public Estadio ObtenerPorNombre(string nombre)
        {
            return _context.Estadios.FirstOrDefault(e => e.Nombre == nombre);
        }

        public void Actualizar(Estadio estadio, string nombreOriginal)
        {
            var existente = _context.Estadios.FirstOrDefault(e => e.Nombre == nombreOriginal);
            ValidarEstadioNoNulo(existente);

            if (nombreOriginal == estadio.Nombre)  
            {
                existente.Ciudad = estadio.Ciudad;
                existente.Capacidad = estadio.Capacidad;
                existente.Descripcion = estadio.Descripcion;
            }
            else                                        
            {
                _context.Estadios.Remove(existente);
                _context.Estadios.Add(estadio);
            }

            _context.SaveChanges();
        }

        public void Eliminar(string nombre)
        {
            var estadio = _context.Estadios.FirstOrDefault(e => e.Nombre == nombre);
            ValidarEstadioNoNulo(estadio);
            _context.Estadios.Remove(estadio);
            _context.SaveChanges();

        }

        private void ValidarEstadioNoNulo(Estadio estadio)
        {
            if (estadio == null)
                throw new KeyNotFoundException("Estadio no encontrado");
        }
    }
}