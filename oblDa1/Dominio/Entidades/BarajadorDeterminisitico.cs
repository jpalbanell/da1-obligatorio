namespace Dominio
{
    public static class BarajadorDeterministico
    {
        public static void Barajar<T>(List<T> lista, Random random)
        {
            for (int i = lista.Count - 1; i > 0; i--)
            {
                int j = random.Next(0, i + 1);
                (lista[i], lista[j]) = (lista[j], lista[i]);
            }
        }
    }
}