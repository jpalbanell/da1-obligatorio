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
                existente.NombreMotorSimulacion = fixture.NombreMotorSimulacion;

                var equipoNombres = fixture.Equipos.Select(e => e.Nombre).ToList();
                var estadioNombres = fixture.Estadios.Select(e => e.Nombre).ToList();

                existente.Equipos.Clear();
                foreach (var nombre in equipoNombres)
                {
                    var equipo = _context.Equipos.Find(nombre);
                    if (equipo != null) existente.Equipos.Add(equipo);
                }

                existente.Estadios.Clear();
                foreach (var nombre in estadioNombres)
                {
                    var estadio = _context.Estadios.Find(nombre);
                    if (estadio != null) existente.Estadios.Add(estadio);
                }
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
