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
    }
}
