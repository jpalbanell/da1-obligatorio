using Dominio.Entidades;

namespace IServicios
{
    public interface ITorneoServicio
    {
        void AgregarEquipo(Equipo equipo);
        List<Equipo> ObtenerTodos();
        void EditarEquipo(Equipo equipo, string nombreOriginal);
        Equipo ObtenerPorNombre(string nombre);
        
        void EliminarEquipo(string nombre);
        void CompletarEquiposAutomaticamente(int semillaCompletar);
        
        void AgregarEstadio(Estadio estadio);
        void ModificarEstadio(Estadio estadio, string nombreOriginal);
        void EliminarEstadio(string nombre);
        Estadio ObtenerEstadio(string nombre);
        List<Estadio> ObtenerTodosEstadios();
        ResultadoImportacion ImportarEquipos(string contenidoCsv);
        
        void GenerarFixture(Fixture fixture);
        List<Grupo> ObtenerGrupos();
        Fixture ObtenerFixture();
        void GenerarCruces(int semillaCrucesFase);
        void EditarPartido(int partidoId, DateTime fecha, string nombreEstadio, bool cargarResultado, int golesLocal, int golesVisitante, List<Incidencia>? incidencias = null);
        void SimularPartido(int partidoId, int semillaSimulation);
        void SimularFase(FaseTorneo fase, int semillaSimulation);
        Partido ObtenerPartido(int id);
        List<Partido> ObtenerTodosPartidos();
        List<Partido> ObtenerPartidosPorFase(FaseTorneo fase);
        List<Partido> ObtenerPartidosPorFecha(DateTime fecha);
        List<Partido> ObtenerPartidosPorGrupo(string etiquetaGrupo);
        List<Partido> ObtenerPartidosPorEstadio(string nombreEstadio);
        List<string> ObtenerMotoresDisponibles();
    }
}