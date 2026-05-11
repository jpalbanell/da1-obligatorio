using Dominio.Entidades;
using Repositorios;
using Servicios;

namespace Tests
{
    [TestClass]
    public class EstadioServicioTests
    {
        private IEstadioServicio _servicio;
        private IEstadioRepositorio _repositorio;
        private IAuditoriaServicio _auditoriaServicio;
        private IAuditoriaRepositorio _auditoriaRepositorio;
        private ISesionServicio _sesionServicio;

        [TestInitialize]
        public void Setup()
        {
            _repositorio = new EstadioRepositorio();
            _auditoriaRepositorio = new AuditoriaRepositorio();
            _auditoriaServicio = new AuditoriaServicio(_auditoriaRepositorio);
            _sesionServicio = new SesionServicio();
            _servicio = new EstadioServicio(_repositorio, _auditoriaServicio, _sesionServicio);

            var usuario = new Usuario();
            usuario.Nombre = "Santiago";
            usuario.Apellido = "Garcia";
            usuario.Email = "santiago@ejemplo.com";
            usuario.FechaNacimiento = new DateTime(1990, 5, 15);
            usuario.Contrasena = "Abcdef1@";
            usuario.Roles.Add(Rol.Administrador);
            _sesionServicio.IniciarSesion(usuario);
        }

        private Estadio CrearEstadioValido(string nombre, string ciudad, int capacidad)
        {
            var estadio = new Estadio();
            estadio.Nombre = nombre;
            estadio.Ciudad = ciudad;
            estadio.Capacidad = capacidad;
            return estadio;
        }

        [TestMethod]
        public void AgregarEstadio_ConDatosValidos_DeberiaAgregarlo()
        {
            var estadio = CrearEstadioValido("Centenario", "Montevideo", 60000);

            _servicio.AgregarEstadio(estadio);
            var resultado = _servicio.ObtenerTodos();

            Assert.AreEqual(1, resultado.Count);
        }

        [TestMethod]
        [ExpectedException(typeof(Exception))]
        public void AgregarEstadio_ConNombreDuplicado_DeberiaLanzarExcepcion()
        {
            var estadio1 = CrearEstadioValido("Centenario", "Montevideo", 60000);
            var estadio2 = CrearEstadioValido("Centenario", "Buenos Aires", 50000);

            _servicio.AgregarEstadio(estadio1);
            _servicio.AgregarEstadio(estadio2);
        }

        [TestMethod]
        public void ObtenerEstadio_ConNombreExistente_DeberiaRetornarlo()
        {
            var estadio = CrearEstadioValido("Centenario", "Montevideo", 60000);
            _servicio.AgregarEstadio(estadio);

            var resultado = _servicio.ObtenerEstadio("Centenario");

            Assert.AreEqual("Centenario", resultado.Nombre);
        }

        [TestMethod]
        public void ObtenerEstadio_ConNombreInexistente_DeberiaRetornarNull()
        {
            var resultado = _servicio.ObtenerEstadio("NoExiste");
            Assert.IsNull(resultado);
        }

        [TestMethod]
        public void ObtenerTodos_ConVariosEstadios_DeberiaRetornarTodos()
        {
            var estadio1 = CrearEstadioValido("Centenario", "Montevideo", 60000);
            var estadio2 = CrearEstadioValido("Camp Nou", "Barcelona", 99000);
            _servicio.AgregarEstadio(estadio1);
            _servicio.AgregarEstadio(estadio2);

            var resultado = _servicio.ObtenerTodos();

            Assert.AreEqual(2, resultado.Count);
        }

        [TestMethod]
        public void ModificarEstadio_ConDatosValidos_DeberiaActualizar()
        {
            var estadio = CrearEstadioValido("Centenario", "Montevideo", 60000);
            _servicio.AgregarEstadio(estadio);

            estadio.Capacidad = 65000;
            _servicio.ModificarEstadio(estadio, "Centenario");

            var resultado = _servicio.ObtenerEstadio("Centenario");
            Assert.AreEqual(65000, resultado.Capacidad);
        }

        [TestMethod]
        public void EliminarEstadio_ConNombreExistente_DeberiaEliminarlo()
        {
            var estadio = CrearEstadioValido("Centenario", "Montevideo", 60000);
            _servicio.AgregarEstadio(estadio);

            _servicio.EliminarEstadio("Centenario");
            var resultado = _servicio.ObtenerTodos();

            Assert.AreEqual(0, resultado.Count);
        }

        [TestMethod]
        [ExpectedException(typeof(Exception))]
        public void EliminarEstadio_ConNombreInexistente_DeberiaLanzarExcepcion()
        {
            _servicio.EliminarEstadio("NoExiste");
        }
        [TestMethod]
        [ExpectedException(typeof(Exception))]
        public void ModificarEstadio_ConNombreDuplicadoDeOtroEstadio_LanzaExcepcion()
        {
            var estadio1 = CrearEstadioValido("Centenario", "Montevideo", 60000);
            var estadio2 = CrearEstadioValido("Maracana", "Rio de Janeiro", 78000);
            _servicio.AgregarEstadio(estadio1);
            _servicio.AgregarEstadio(estadio2);

            estadio2.Nombre = "Centenario";
            _servicio.ModificarEstadio(estadio2, "Maracana");
        }
        [TestMethod]
        [ExpectedException(typeof(Exception))]
        public void ModificarEstadio_EstadioInexistente_LanzaExcepcion()
        {
            var estadio = new Estadio();
            estadio.Nombre = "NoExiste";
            estadio.Ciudad = "Montevideo";
            estadio.Capacidad = 60000;

            _servicio.ModificarEstadio(estadio, "NoExiste");
        }
        
        [TestMethod]
        public void AgregarEstadio_ConDatosValidos_RegistraLogDeAuditoria()
        {
            var estadio = CrearEstadioValido("Centenario", "Montevideo", 60000);
            _servicio.AgregarEstadio(estadio);
            Assert.AreEqual(1, _auditoriaRepositorio.ObtenerTodos().Count);
        }
        
        [TestMethod]
        public void ModificarEstadio_ConDatosValidos_RegistraLogDeAuditoria()
        {
            var estadio = CrearEstadioValido("Centenario", "Montevideo", 60000);
            _servicio.AgregarEstadio(estadio);
            estadio.Capacidad = 65000;
            _servicio.ModificarEstadio(estadio, "Centenario");
            Assert.AreEqual(2, _auditoriaRepositorio.ObtenerTodos().Count);
        }
        
        [TestMethod]
        public void EliminarEstadio_ConDatosValidos_RegistraLogDeAuditoria()
        {
            var estadio = CrearEstadioValido("Centenario", "Montevideo", 60000);
            _servicio.AgregarEstadio(estadio);
            _servicio.EliminarEstadio("Centenario");
            Assert.AreEqual(2, _auditoriaRepositorio.ObtenerTodos().Count);
        }
        
        [TestMethod]
        [ExpectedException(typeof(Exception))]
        public void AgregarEstadio_SinRolAdministrador_DeberiaLanzarExcepcion()
        {
            var usuario = new Usuario();
            usuario.Nombre = "Juan";
            usuario.Apellido = "Perez";
            usuario.Email = "juan@ejemplo.com";
            usuario.FechaNacimiento = new DateTime(1990, 5, 15);
            usuario.Contrasena = "Abcdef1@";
            usuario.Roles.Add(Rol.Editor);
            _sesionServicio.IniciarSesion(usuario);

            var estadio = new Estadio();
            estadio.Nombre = "Centenario";
            estadio.Ciudad = "Montevideo";
            estadio.Capacidad = 60000;

            _servicio.AgregarEstadio(estadio);
        }
        
        [TestMethod]
        [ExpectedException(typeof(Exception))]
        public void ModificarEstadio_SinRolAdministrador_DeberiaLanzarExcepcion()
        {
            var estadio = CrearEstadioValido("Centenario", "Montevideo", 60000);
            _servicio.AgregarEstadio(estadio);

            var usuarioEditor = new Usuario();
            usuarioEditor.Nombre = "Juan";
            usuarioEditor.Apellido = "Perez";
            usuarioEditor.Email = "juan@ejemplo.com";
            usuarioEditor.FechaNacimiento = new DateTime(1990, 5, 15);
            usuarioEditor.Contrasena = "Abcdef1@";
            usuarioEditor.Roles.Add(Rol.Editor);
            _sesionServicio.IniciarSesion(usuarioEditor);

            estadio.Capacidad = 65000;
            _servicio.ModificarEstadio(estadio, "Centenario");
        }

        [TestMethod]
        [ExpectedException(typeof(Exception))]
        public void EliminarEstadio_SinRolAdministrador_DeberiaLanzarExcepcion()
        {
            var estadio = CrearEstadioValido("Centenario", "Montevideo", 60000);
            _servicio.AgregarEstadio(estadio);

            var usuarioEditor = new Usuario();
            usuarioEditor.Nombre = "Juan";
            usuarioEditor.Apellido = "Perez";
            usuarioEditor.Email = "juan@ejemplo.com";
            usuarioEditor.FechaNacimiento = new DateTime(1990, 5, 15);
            usuarioEditor.Contrasena = "Abcdef1@";
            usuarioEditor.Roles.Add(Rol.Editor);
            _sesionServicio.IniciarSesion(usuarioEditor);

            _servicio.EliminarEstadio("Centenario");
        }
        
        [TestMethod]
        [ExpectedException(typeof(Exception))]
        public void EliminarEstadio_EstadioInexistente_DeberiaLanzarExcepcion()
        {
            _servicio.EliminarEstadio("EstadioQueNoExiste");
        }
    }
}