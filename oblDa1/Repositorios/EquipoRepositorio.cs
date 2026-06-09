using Dominio.Entidades;
using IRepositorios;
using Microsoft.EntityFrameworkCore; // ExecuteSqlRaw

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

            existente.Confederacion = equipo.Confederacion;
            existente.RankingFifa = equipo.RankingFifa;
            existente.Bandera = equipo.Bandera;

            if (nombreOriginal != equipo.Nombre)
            {
                _context.Database.ExecuteSqlRaw(
                    "UPDATE FixtureEquipos SET EquipoNombre = {0} WHERE EquipoNombre = {1}",
                    equipo.Nombre, nombreOriginal);
                _context.Database.ExecuteSqlRaw(
                    "UPDATE Partidos SET EquipoLocalNombre = {0} WHERE EquipoLocalNombre = {1}",
                    equipo.Nombre, nombreOriginal);
                _context.Database.ExecuteSqlRaw(
                    "UPDATE Partidos SET EquipoVisitanteNombre = {0} WHERE EquipoVisitanteNombre = {1}",
                    equipo.Nombre, nombreOriginal);
                _context.Database.ExecuteSqlRaw(
                    "UPDATE Partidos SET VencedorNombre = {0} WHERE VencedorNombre = {1}",
                    equipo.Nombre, nombreOriginal);
                _context.Database.ExecuteSqlRaw(
                    "UPDATE Incidencias SET EquipoNombre = {0} WHERE EquipoNombre = {1}",
                    equipo.Nombre, nombreOriginal);
                existente.Nombre = equipo.Nombre;
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
