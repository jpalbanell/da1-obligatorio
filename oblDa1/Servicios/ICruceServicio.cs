using Dominio.Entidades;

namespace Servicios
{
    public interface ICruceServicio
    {
        void GenerarCruces(int semillaCrucesFase, Usuario usuario);
        List<PosicionesGrupo> CalcularPosicionesGrupo(Grupo grupo);
        List<PosicionesGrupo> ObtenerClasificados();
        (List<PosicionesGrupo> primeros, List<PosicionesGrupo> segundos, List<PosicionesGrupo> mejoresTerceros) SeleccionarClasificados();
        List<(PosicionesGrupo local, PosicionesGrupo visitante, string codigo)> GenerarEmparejamientos(int semilla);
    }
}