using Dominio.Entidades;
using Servicios.Exportacion;

namespace Tests.TestsServicios
{
    [TestClass]
    public class ConversorAuditoriaTests
    {
        [TestMethod]
        public void ATabla_ConUnLog_GeneraEncabezadosYUnaFila()
        {
            var usuario = new Usuario
            {
                Nombre = "Admin", Apellido = "Test", Email = "admin@test.com",
                FechaNacimiento = new DateTime(1990, 1, 1), Contrasena = "Password@1"
            };
            var log = new LogAuditoria
            {
                Timestamp = new DateTime(2026, 6, 5, 14, 30, 0),
                Accion = "Creacion de usuario",
                Usuario = usuario
            };
            var logs = new List<LogAuditoria> { log };

            var tabla = ConversorAuditoria.ATabla(logs);

            CollectionAssert.AreEqual(
                new List<string> { "Fecha", "Accion", "Usuario" },
                tabla.Encabezados);
            Assert.AreEqual(1, tabla.Filas.Count);
            CollectionAssert.AreEqual(
                new List<string> { "2026-06-05 14:30:00", "Creacion de usuario", "admin@test.com" },
                tabla.Filas[0]);
        }
    }
}