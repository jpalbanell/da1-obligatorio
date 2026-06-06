using Dominio.Entidades;
using IRepositorios;
using Microsoft.EntityFrameworkCore;

namespace Repositorios
{
    public class FixtureRepositorio : IFixtureRepositorio
    {
        private readonly SqlContext _context;

        public FixtureRepositorio(SqlContext context)
        {
            _context = context;
        }

        public void Guardar(Fixture fixture)
        {
            var existente = _context.Fixtures
                .Include(f => f.Equipos)
                .Include(f => f.Estadios)
                .FirstOrDefault(); 

            if (existente == null)
            {
                _context.Fixtures.Add(fixture);
            }
            else
            {
                existente.SemillaFixture = fixture.SemillaFixture;
                existente.FechaInicioTorneo = fixture.FechaInicioTorneo;
                existente.MaxPartidosPorDia = fixture.MaxPartidosPorDia;
                existente.SeparacionEntreFechas = fixture.SeparacionEntreFechas;
                existente.EstaGenerado = fixture.EstaGenerado;
                existente.CrucesGenerados = fixture.CrucesGenerados;

                existente.Equipos.Clear();
                foreach (var equipo in fixture.Equipos)
                    existente.Equipos.Add(equipo);

                existente.Estadios.Clear();
                foreach (var estadio in fixture.Estadios)
                    existente.Estadios.Add(estadio);
            }

            _context.SaveChanges();
        }

        public Fixture Obtener()
        {
            return _context.Fixtures
                .Include(f => f.Equipos)
                .Include(f => f.Estadios)
                .FirstOrDefault();
        }
    }
}
