using Dominio.Entidades;
using IRepositorios;

namespace Repositorios
{
    public class EquipoRepositorio : IEquipoRepositorio
    {
        private readonly SqlContext _context;

        public EquipoRepositorio(SqlContext context)
        {
            _context = context;
        }

        public void Agregar(Equipo equipo)
        {
            _context.Equipos.Add(equipo);
            _context.SaveChanges();
        }

        public List<Equipo> ObtenerTodos()
        {
            return _context.Equipos.ToList();
        }

        public Equipo ObtenerPorNombre(string nombre)
        {
            return _context.Equipos.FirstOrDefault(e => e.Nombre == nombre);
        }

        public void Actualizar(Equipo equipo, string nombreOriginal)
        {
            var existente = _context.Equipos.FirstOrDefault(e => e.Nombre == nombreOriginal);
            ValidarEquipoNoNulo(existente);

            if (nombreOriginal == equipo.Nombre)
            {
                existente.Confederacion = equipo.Confederacion;
                existente.RankingFifa = equipo.RankingFifa;
            }
            else
            {
                _context.Equipos.Remove(existente);
                _context.Equipos.Add(equipo);
            }

            _context.SaveChanges();
        }

        public void Eliminar(string nombre)
        {
            var equipo = _context.Equipos.FirstOrDefault(e => e.Nombre == nombre);
            ValidarEquipoNoNulo(equipo);
            _context.Equipos.Remove(equipo);
            _context.SaveChanges();
        }

        private void ValidarEquipoNoNulo(Equipo equipo)
        {
            if (equipo == null)
                throw new KeyNotFoundException("Equipo no encontrado");
        }
    }
}