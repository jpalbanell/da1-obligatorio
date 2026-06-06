namespace Servicios.Exportacion
{
    public abstract class Exportador
    {
        public abstract byte[] Exportar(TablaExportable tabla);
    }
}