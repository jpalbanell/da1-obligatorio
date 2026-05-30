using Dominio;

namespace Tests
{
    [TestClass]
    public class BarajadorDeterministicoTests
    {
        [TestMethod]
        public void Barajar_MismaSemilla_ProduceMismoResultado()
        {
            var lista1 = new List<int> { 1, 2, 3, 4, 5 };
            var lista2 = new List<int> { 1, 2, 3, 4, 5 };

            BarajadorDeterministico.Barajar(lista1, new Random(42));
            BarajadorDeterministico.Barajar(lista2, new Random(42));

            CollectionAssert.AreEqual(lista1, lista2);
            CollectionAssert.AreNotEqual(lista1, new List<int> { 1, 2, 3, 4, 5 });
        }
    }
}