using Dominio.Entidades;
using IRepositorios;

namespace Repositorios
{
    public class PartidoRepositorio : IPartidoRepositorio
    {
        private readonly SqlContext _context;

        public PartidoRepositorio(SqlContext context)
        {
            _context = context;
        }

        public void Agregar(Partido partido)
        {
            _context.Partidos.Add(partido);
            _context.SaveChanges();
        }

        public Partido ObtenerPorId(int id)
        {
            return _context.Partidos.FirstOrDefault(p => p.Id == id);
        }

        public List<Partido> ObtenerTodos()
        {
            return _context.Partidos.ToList();
        }

        public void Actualizar(Partido partido)
        {
            var existente = _context.Partidos.FirstOrDefault(p => p.Id == partido.Id);
            if (existente == null)
                throw new KeyNotFoundException("Partido no encontrado");

            // Copia todos los escalares (incluidos los de backing field como Codigo, Fecha, Goles)
            // usando el mismo PropertyAccessMode configurado en PartidoConfig.
            _context.Entry(existente).CurrentValues.SetValues(partido);

            // Las navegaciones se actualizan via Reference para manejar valores null de forma segura
            // (algunos setters del dominio lanzan ArgumentException si reciben null).
            _context.Entry(existente).Reference(p => p.EquipoLocal).CurrentValue = partido.EquipoLocal;
            _context.Entry(existente).Reference(p => p.EquipoVisitante).CurrentValue = partido.EquipoVisitante;
            _context.Entry(existente).Reference(p => p.Estadio).CurrentValue = partido.Estadio;
            _context.Entry(existente).Reference(p => p.Grupo).CurrentValue = partido.Grupo;
            _context.Entry(existente).Reference(p => p.Vencedor).CurrentValue = partido.Vencedor;
            existente.OrigenLocal = partido.OrigenLocal;
            existente.OrigenVisitante = partido.OrigenVisitante;

            _context.SaveChanges();
        }
    }
}
