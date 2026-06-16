namespace Dominio.Entidades
{
    public static class ConfederacionExtensiones
    {
        private static readonly Dictionary<Confederacion, int> Cupos = new()
        {
            { Confederacion.UEFA, 16 },
            { Confederacion.CONMEBOL, 7 },
            { Confederacion.CONCACAF, 7 },
            { Confederacion.CAF, 9 },
            { Confederacion.AFC, 8 },
            { Confederacion.OFC, 1 }
        };

        public static int CupoMaximo(this Confederacion confederacion)
        {
            if (Cupos.TryGetValue(confederacion, out var cupo))
                return cupo;
            throw new ArgumentException($"Confederación inválida: {confederacion}");
        }
    }
}