using Dominio.Entidades;
using IRepositorios;
using Repositorios;
using Microsoft.EntityFrameworkCore;

namespace Tests
{
    [TestClass]
    public class NotificacionRepositorioTests
    {
        private INotificacionRepositorio _repositorio;
        private SqlContext _context;

        [TestInitialize]
        public void Setup()
        {
            var options = new DbContextOptionsBuilder<SqlContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;
            _context = new SqlContext(options);
            _repositorio = new NotificacionRepositorio(_context);
        }

        private Usuario CrearUsuarioValido()
        {
            var usuario = new Usuario();
            usuario.Nombre = "Test";
            usuario.Apellido = "Usuario";
            usuario.Email = "test@test.com";
            usuario.FechaNacimiento = new DateTime(1990, 1, 1);
            usuario.Contrasena = "Password@1";
            return usuario;
        }

        private Notificacion CrearNotificacionValida(Usuario periodista)
        {
            var notificacion = new Notificacion();
            notificacion.Mensaje = "Partido editado";
            notificacion.FechaCreacion = DateTime.Now;
            notificacion.Periodista = periodista;
            return notificacion;
        }

        [TestMethod]
        public void Agregar_NotificacionValida_SeGuardaEnBD()
        {
            var periodista = CrearUsuarioValido();
            _context.Usuarios.Add(periodista);
            _context.SaveChanges();

            var notificacion = CrearNotificacionValida(periodista);
            _repositorio.Agregar(notificacion);

            Assert.AreEqual(1, _context.Notificaciones.Count());
        }

        [TestMethod]
        public void ObtenerPorId_IdExistente_RetornaNotificacion()
        {
            var periodista = CrearUsuarioValido();
            _context.Usuarios.Add(periodista);
            _context.SaveChanges();

            var notificacion = CrearNotificacionValida(periodista);
            _repositorio.Agregar(notificacion);

            var resultado = _repositorio.ObtenerPorId(notificacion.Id);

            Assert.IsNotNull(resultado);
            Assert.AreEqual(notificacion.Mensaje, resultado.Mensaje);
        }

        [TestMethod]
        public void ObtenerNoLeidasPorUsuario_UsuarioConNotificaciones_RetornaSoloNoLeidas()
        {
            var periodista = CrearUsuarioValido();
            _context.Usuarios.Add(periodista);
            _context.SaveChanges();

            var noLeida = CrearNotificacionValida(periodista);
            var leida = CrearNotificacionValida(periodista);
            _repositorio.Agregar(noLeida);
            _repositorio.Agregar(leida);

            leida.MarcarLeida();
            _repositorio.Actualizar(leida);

            var resultado = _repositorio.ObtenerNoLeidasPorUsuario(periodista);

            Assert.AreEqual(1, resultado.Count);
            Assert.IsFalse(resultado[0].Leida);
        }

        [TestMethod]
        public void ObtenerPorUsuario_UsuarioConNotificaciones_RetornaTodas()
        {
            var periodista = CrearUsuarioValido();
            _context.Usuarios.Add(periodista);
            _context.SaveChanges();

            var notificacion1 = CrearNotificacionValida(periodista);
            var notificacion2 = CrearNotificacionValida(periodista);
            _repositorio.Agregar(notificacion1);
            _repositorio.Agregar(notificacion2);

            notificacion2.MarcarLeida();
            _repositorio.Actualizar(notificacion2);

            var resultado = _repositorio.ObtenerPorUsuario(periodista);

            Assert.AreEqual(2, resultado.Count);
        }

        [TestMethod]
        public void Actualizar_NotificacionLeida_PersisteCambio()
        {
            var dbName = Guid.NewGuid().ToString();
            var options = new DbContextOptionsBuilder<SqlContext>()
                .UseInMemoryDatabase(dbName)
                .Options;

            var periodista = CrearUsuarioValido();
            int notificacionId;

            using (var contextEscritura = new SqlContext(options))
            {
                contextEscritura.Usuarios.Add(periodista);
                contextEscritura.SaveChanges();
                var notificacion = CrearNotificacionValida(periodista);
                var repo = new NotificacionRepositorio(contextEscritura);
                repo.Agregar(notificacion);
                notificacionId = notificacion.Id;
                notificacion.MarcarLeida();
                repo.Actualizar(notificacion);
            }

            using (var contextLectura = new SqlContext(options))
            {
                var resultado = new NotificacionRepositorio(contextLectura).ObtenerPorId(notificacionId);
                Assert.IsTrue(resultado.Leida);
            }
        }
    }
}
