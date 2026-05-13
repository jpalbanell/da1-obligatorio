using Dominio.Entidades;
using IServicios;

namespace Servicios
{
    public class SimulacionServicio : ISimulacionServicio
    {
        private readonly IPartidoServicio _partidoServicio;
        private readonly IAuditoriaServicio _auditoriaServicio;
        private readonly ISesionServicio _sesionServicio;
        private const double RankingMaximo = 2500.0;
        private const int MaxGolesBase = 5;
        private const int MinGolesMaximos = 1;

        public SimulacionServicio(IPartidoServicio partidoServicio,
            IAuditoriaServicio auditoriaServicio,
            ISesionServicio sesionServicio)
        {
            _partidoServicio = partidoServicio;
            _auditoriaServicio = auditoriaServicio;
            _sesionServicio = sesionServicio;
        }

        public void SimularPartido(int partidoId, int semillaSimulation)
        {
            _sesionServicio.ValidarRol(Rol.Editor);
            var partido = _partidoServicio.ObtenerPartido(partidoId);
            ValidarPartidoExistente(partido);
            ValidarEquipos(partido);

            var random = new Random(semillaSimulation + partidoId);
            partido.GolesLocal = GenerarGoles(partido.EquipoLocal.RankingFifa, random);
            partido.GolesVisitante = GenerarGoles(partido.EquipoVisitante.RankingFifa, random);
            partido.TieneResultado = true;
            AsignarVencedor(partido, random);
            ActualizarPosiciones(partido);
            _partidoServicio.PropagarResultado(partido);
            _partidoServicio.ActualizarPartido(partido);
            _auditoriaServicio.Registrar(
                $"Simulación de partido: {partidoId} con SemillaSimulation: {semillaSimulation}",
                _sesionServicio.ObtenerUsuarioActual());
        }

        public void SimularFase(FaseTorneo fase, int semillaSimulation)
        {
            _sesionServicio.ValidarRol(Rol.Editor);
            var partidos = _partidoServicio.ObtenerTodos()
                .Where(p => p.Fase == fase && !p.TieneResultado)
                .ToList();
            
            BloquearFaseAnterior(fase);

            foreach (var partido in partidos)
                SimularPartidoDeFase(partido, semillaSimulation);

            _auditoriaServicio.Registrar(
                $"Simulación de fase: {fase} con SemillaSimulation: {semillaSimulation}",
                _sesionServicio.ObtenerUsuarioActual());
        }

        private void SimularPartidoDeFase(Partido partido, int semillaSimulation)
        {
            ValidarEquipos(partido);
            var random = new Random(semillaSimulation + partido.Id);
            partido.GolesLocal = GenerarGoles(partido.EquipoLocal.RankingFifa, random);
            partido.GolesVisitante = GenerarGoles(partido.EquipoVisitante.RankingFifa, random);
            partido.TieneResultado = true;
            AsignarVencedor(partido, random);
            ActualizarPosiciones(partido);
            _partidoServicio.PropagarResultado(partido);
            _partidoServicio.ActualizarPartido(partido);
        }
        
        private void ValidarEquipos(Partido partido)
        {
            if (partido.EquipoLocal == null || partido.EquipoVisitante == null)
                throw new Exception("No se puede simular el partido: aún no hay equipos asignados. Simulá primero las fases anteriores.");
        }

        private int GenerarGoles(int rankingFifa, Random random)
        {
            double fuerza = rankingFifa / RankingMaximo;
            int maxGoles = Math.Max(MinGolesMaximos, (int)(fuerza * MaxGolesBase));
            return random.Next(0, maxGoles + 1);
        }

        private void AsignarVencedor(Partido partido, Random random)
        {
            if (partido.GolesLocal > partido.GolesVisitante)
                partido.Vencedor = partido.EquipoLocal;
            else if (partido.GolesVisitante > partido.GolesLocal)
                partido.Vencedor = partido.EquipoVisitante;
            else if (partido.Fase != FaseTorneo.FaseGrupos)
                partido.Vencedor = random.Next(2) == 0
                    ? partido.EquipoLocal
                    : partido.EquipoVisitante;
        }

        private void ValidarPartidoExistente(Partido partido)
        {
            if (partido == null)
                throw new Exception("Partido no encontrado");
        }
        
        private void ActualizarPosiciones(Partido partido)
        {
            if (partido.Grupo == null) return;

            var posLocal = partido.Grupo.ListaPosiciones
                .FirstOrDefault(p => p.Equipo.Nombre == partido.EquipoLocal.Nombre);

            var posVisitante = partido.Grupo.ListaPosiciones
                .FirstOrDefault(p => p.Equipo.Nombre == partido.EquipoVisitante.Nombre);

            if (posLocal == null || posVisitante == null) return;

            posLocal.AplicarResultado(partido.GolesLocal, partido.GolesVisitante);
            posVisitante.AplicarResultado(partido.GolesVisitante, partido.GolesLocal);
        }
        
        private void BloquearFaseAnterior(FaseTorneo faseActual)
        {
            if (faseActual == FaseTorneo.FaseGrupos) return;
            var faseAnterior = faseActual - 1;
            var partidos = _partidoServicio.ObtenerTodos()
                .Where(p => p.Fase == faseAnterior)
                .ToList();
            foreach (var partido in partidos)
                partido.EstaBloqueado = true;
        }
    }
}