using Dominio.Entidades;

namespace Tests
{
    [TestClass]
    public class ConfederacionExtensionesTests
    {
        [TestMethod]
        public void CupoMaximo_UEFA_Retorna16()
        {
            Assert.AreEqual(16, Confederacion.UEFA.CupoMaximo());
        }

        [TestMethod]
        public void CupoMaximo_CONMEBOL_Retorna7()
        {
            Assert.AreEqual(7, Confederacion.CONMEBOL.CupoMaximo());
        }

        [TestMethod]
        public void CupoMaximo_CONCACAF_Retorna7()
        {
            Assert.AreEqual(7, Confederacion.CONCACAF.CupoMaximo());
        }

        [TestMethod]
        public void CupoMaximo_CAF_Retorna9()
        {
            Assert.AreEqual(9, Confederacion.CAF.CupoMaximo());
        }

        [TestMethod]
        public void CupoMaximo_AFC_Retorna8()
        {
            Assert.AreEqual(8, Confederacion.AFC.CupoMaximo());
        }

        [TestMethod]
        public void CupoMaximo_OFC_Retorna1()
        {
            Assert.AreEqual(1, Confederacion.OFC.CupoMaximo());
        }

        [TestMethod]
        public void CupoMaximo_TodasLasConfederaciones_RetornaValorPositivo()
        {
            foreach (Confederacion conf in Enum.GetValues(typeof(Confederacion)))
                Assert.IsTrue(conf.CupoMaximo() > 0);
        }
    }
}