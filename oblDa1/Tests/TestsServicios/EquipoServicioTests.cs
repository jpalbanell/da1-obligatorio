using Dominio.Entidades;
using Servicios;
using Repositorios;

namespace Tests.TestsServicios
{
    [TestClass]
    public class EquipoServicioTests
    {
        private IEquipoServicio _equipoServicio;
        private IEquipoRepositorio _equipoRepositorio;
        private IAuditoriaServicio _auditoriaServicio;
        private IAuditoriaRepositorio _auditoriaRepositorio;
        private ISesionServicio _sesionServicio;

        [TestInitialize]
        public void Setup()
        {
            _equipoRepositorio = new EquipoRepositorio();
            _auditoriaRepositorio = new AuditoriaRepositorio();
            _auditoriaServicio = new AuditoriaServicio(_auditoriaRepositorio);
            _sesionServicio = new SesionServicio();
            _equipoServicio = new EquipoServicio(_equipoRepositorio, _auditoriaServicio, _sesionServicio);

            var usuario = new Usuario();
            usuario.Nombre = "Santiago";
            usuario.Apellido = "Garcia";
            usuario.Email = "santiago@ejemplo.com";
            usuario.FechaNacimiento = new DateTime(1990, 5, 15);
            usuario.Contrasena = "Abcdef1@";
            usuario.Roles.Add(Rol.Administrador);
            _sesionServicio.IniciarSesion(usuario);
        }
        
        [TestMethod]
        public void AgregarEquipo_ConDatosValidos_AgregaCorrectamente()
        {
            var equipo = new Equipo();
            equipo.Nombre = "Uruguay";
            equipo.Confederacion = Confederacion.CONMEBOL;
            equipo.RankingFifa = 1500;

            _equipoServicio.AgregarEquipo(equipo);

            Assert.AreEqual(1, _equipoRepositorio.ObtenerTodos().Count);
        }
        
        [TestMethod]
        [ExpectedException(typeof(Exception))]
        public void AgregarEquipo_ConNombreDuplicado_LanzaExcepcion()
        {
            var equipo1 = new Equipo();
            equipo1.Nombre = "Uruguay";
            equipo1.Confederacion = Confederacion.CONMEBOL;
            equipo1.RankingFifa = 1500;

            var equipo2 = new Equipo();
            equipo2.Nombre = "Uruguay";
            equipo2.Confederacion = Confederacion.CONMEBOL;
            equipo2.RankingFifa = 1200;

            _equipoServicio.AgregarEquipo(equipo1);
            _equipoServicio.AgregarEquipo(equipo2);
        }
        
        [TestMethod]
        [ExpectedException(typeof(Exception))]
        public void AgregarEquipo_ConCupoUEFACompleto_LanzaExcepcion()
        {
            for (int i = 1; i <= 16; i++)
            {
                var equipo = new Equipo();
                equipo.Nombre = $"UEFA_{i}";
                equipo.Confederacion = Confederacion.UEFA;
                equipo.RankingFifa = 1500;
                _equipoServicio.AgregarEquipo(equipo);
            }

            var equipoExtra = new Equipo();
            equipoExtra.Nombre = "UEFA_17";
            equipoExtra.Confederacion = Confederacion.UEFA;
            equipoExtra.RankingFifa = 1500;
            _equipoServicio.AgregarEquipo(equipoExtra);
        }
        
        [TestMethod]
        [ExpectedException(typeof(Exception))]
        public void AgregarEquipo_ConCupoCONMEBOLCompleto_LanzaExcepcion()
        {
            for (int i = 1; i <= 7; i++)
            {
                var equipo = new Equipo();
                equipo.Nombre = $"CONMEBOL_{i}";
                equipo.Confederacion = Confederacion.CONMEBOL;
                equipo.RankingFifa = 1500;
                _equipoServicio.AgregarEquipo(equipo);
            }

            var equipoExtra = new Equipo();
            equipoExtra.Nombre = "CONMEBOL_8";
            equipoExtra.Confederacion = Confederacion.CONMEBOL;
            equipoExtra.RankingFifa = 1500;
            _equipoServicio.AgregarEquipo(equipoExtra);
        }
        
        [TestMethod]
        [ExpectedException(typeof(Exception))]
        public void AgregarEquipo_ConCupoCONCАCAFCompleto_LanzaExcepcion()
        {
            for (int i = 1; i <= 7; i++)
            {
                var equipo = new Equipo();
                equipo.Nombre = $"CONCACAF_{i}";
                equipo.Confederacion = Confederacion.CONCACAF;
                equipo.RankingFifa = 1500;
                _equipoServicio.AgregarEquipo(equipo);
            }

            var equipoExtra = new Equipo();
            equipoExtra.Nombre = "CONCACAF_8";
            equipoExtra.Confederacion = Confederacion.CONCACAF;
            equipoExtra.RankingFifa = 1500;
            _equipoServicio.AgregarEquipo(equipoExtra);
        }
        
        [TestMethod]
        [ExpectedException(typeof(Exception))]
        public void AgregarEquipo_ConCupoCAFCompleto_LanzaExcepcion()
        {
            for (int i = 1; i <= 9; i++)
            {
                var equipo = new Equipo();
                equipo.Nombre = $"CAF_{i}";
                equipo.Confederacion = Confederacion.CAF;
                equipo.RankingFifa = 1500;
                _equipoServicio.AgregarEquipo(equipo);
            }

            var equipoExtra = new Equipo();
            equipoExtra.Nombre = "CAF_10";
            equipoExtra.Confederacion = Confederacion.CAF;
            equipoExtra.RankingFifa = 1500;
            _equipoServicio.AgregarEquipo(equipoExtra);
        }
        
        [TestMethod]
        [ExpectedException(typeof(Exception))]
        public void AgregarEquipo_ConCupoAFCCompleto_LanzaExcepcion()
        {
            for (int i = 1; i <= 8; i++)
            {
                var equipo = new Equipo();
                equipo.Nombre = $"AFC_{i}";
                equipo.Confederacion = Confederacion.AFC;
                equipo.RankingFifa = 1500;
                _equipoServicio.AgregarEquipo(equipo);
            }

            var equipoExtra = new Equipo();
            equipoExtra.Nombre = "AFC_9";
            equipoExtra.Confederacion = Confederacion.AFC;
            equipoExtra.RankingFifa = 1500;
            _equipoServicio.AgregarEquipo(equipoExtra);
        }
        
        [TestMethod]
        [ExpectedException(typeof(Exception))]
        public void AgregarEquipo_ConCupoOFCCompleto_LanzaExcepcion()
        {
            var equipo = new Equipo();
            equipo.Nombre = "OFC_1";
            equipo.Confederacion = Confederacion.OFC;
            equipo.RankingFifa = 1500;
            _equipoServicio.AgregarEquipo(equipo);

            var equipoExtra = new Equipo();
            equipoExtra.Nombre = "OFC_2";
            equipoExtra.Confederacion = Confederacion.OFC;
            equipoExtra.RankingFifa = 1500;
            _equipoServicio.AgregarEquipo(equipoExtra);
        }
        
        [TestMethod]
        public void EditarEquipo_ConDatosValidos_EditaCorrectamente()
        {
            var equipo = new Equipo();
            equipo.Nombre = "Uruguay";
            equipo.Confederacion = Confederacion.CONMEBOL;
            equipo.RankingFifa = 1500;
            _equipoServicio.AgregarEquipo(equipo);

            equipo.RankingFifa = 1800;
            _equipoServicio.EditarEquipo(equipo);

            Assert.AreEqual(1800, _equipoRepositorio.ObtenerPorNombre("Uruguay").RankingFifa);
        }
        
        [TestMethod]
        [ExpectedException(typeof(Exception))]
        public void EditarEquipo_ConNombreDuplicadoDeOtroEquipo_LanzaExcepcion()
        {
            var equipo1 = new Equipo();
            equipo1.Nombre = "Uruguay";
            equipo1.Confederacion = Confederacion.CONMEBOL;
            equipo1.RankingFifa = 1500;
            _equipoServicio.AgregarEquipo(equipo1);

            var equipo2 = new Equipo();
            equipo2.Nombre = "Argentina";
            equipo2.Confederacion = Confederacion.CONMEBOL;
            equipo2.RankingFifa = 1200;
            _equipoServicio.AgregarEquipo(equipo2);

            equipo2.Nombre = "Uruguay";
            _equipoServicio.EditarEquipo(equipo2);
        }
        
        [TestMethod]
        public void EditarEquipo_ConMismoNombre_NoLanzaExcepcion()
        {
            var equipo = new Equipo();
            equipo.Nombre = "Uruguay";
            equipo.Confederacion = Confederacion.CONMEBOL;
            equipo.RankingFifa = 1500;
            _equipoServicio.AgregarEquipo(equipo);

            equipo.RankingFifa = 1800;
            _equipoServicio.EditarEquipo(equipo);

            Assert.AreEqual(1800, _equipoRepositorio.ObtenerPorNombre("Uruguay").RankingFifa);
        }
        
        [TestMethod]
        public void EliminarEquipo_EquipoExistente_EliminaCorrectamente()
        {
            var equipo = new Equipo();
            equipo.Nombre = "Uruguay";
            equipo.Confederacion = Confederacion.CONMEBOL;
            equipo.RankingFifa = 1500;
            _equipoServicio.AgregarEquipo(equipo);

            _equipoServicio.EliminarEquipo("Uruguay");

            Assert.AreEqual(0, _equipoRepositorio.ObtenerTodos().Count);
        }
        
        [TestMethod]
        [ExpectedException(typeof(Exception))]
        public void EliminarEquipo_EquipoInexistente_LanzaExcepcion()
        {
            _equipoServicio.EliminarEquipo("Uruguay");
        }
        
        [TestMethod]
        public void ObtenerTodos_ListaVacia_RetornaListaVacia()
        {
            var resultado = _equipoServicio.ObtenerTodos();

            Assert.AreEqual(0, resultado.Count);
        }
        
        [TestMethod]
        public void ObtenerTodos_ConEquipos_RetornaListaCorrecta()
        {
            var equipo = new Equipo();
            equipo.Nombre = "Uruguay";
            equipo.Confederacion = Confederacion.CONMEBOL;
            equipo.RankingFifa = 1500;
            _equipoServicio.AgregarEquipo(equipo);

            var resultado = _equipoServicio.ObtenerTodos();

            Assert.AreEqual(1, resultado.Count);
        }
        
        [TestMethod]
        public void ObtenerPorNombre_NombreExistente_RetornaEquipoCorrecto()
        {
            var equipo = new Equipo();
            equipo.Nombre = "Uruguay";
            equipo.Confederacion = Confederacion.CONMEBOL;
            equipo.RankingFifa = 1500;
            _equipoServicio.AgregarEquipo(equipo);

            var resultado = _equipoServicio.ObtenerPorNombre("Uruguay");

            Assert.AreEqual(equipo, resultado);
        }
        
        [TestMethod]
        public void ObtenerPorNombre_NombreInexistente_RetornaNull()
        {
            var resultado = _equipoServicio.ObtenerPorNombre("Uruguay");

            Assert.IsNull(resultado);
        }
        
        [TestMethod]
        public void AgregarEquipo_ConDatosValidos_RegistraLogDeAuditoria()
        {
            var equipo = new Equipo();
            equipo.Nombre = "Uruguay";
            equipo.Confederacion = Confederacion.CONMEBOL;
            equipo.RankingFifa = 1500;

            _equipoServicio.AgregarEquipo(equipo);

            Assert.AreEqual(1, _auditoriaRepositorio.ObtenerTodos().Count);
        }
        
        [TestMethod]
        public void EditarEquipo_ConDatosValidos_RegistraLogDeAuditoria()
        {
            var equipo = new Equipo();
            equipo.Nombre = "Uruguay";
            equipo.Confederacion = Confederacion.CONMEBOL;
            equipo.RankingFifa = 1500;
            _equipoServicio.AgregarEquipo(equipo);

            equipo.RankingFifa = 1800;
            _equipoServicio.EditarEquipo(equipo);

            Assert.AreEqual(2, _auditoriaRepositorio.ObtenerTodos().Count);
        }
        [TestMethod]
        public void EliminarEquipo_EquipoExistente_RegistraLogDeAuditoria()
        {
            var equipo = new Equipo();
            equipo.Nombre = "Uruguay";
            equipo.Confederacion = Confederacion.CONMEBOL;
            equipo.RankingFifa = 1500;
            _equipoServicio.AgregarEquipo(equipo);

            _equipoServicio.EliminarEquipo("Uruguay");

            Assert.AreEqual(2, _auditoriaRepositorio.ObtenerTodos().Count);
        }
        [TestMethod]
        public void CompletarEquiposAutomaticamente_SinEquipos_Genera48Equipos()
        {
            _equipoServicio.CompletarEquiposAutomaticamente(42);
            Assert.AreEqual(48, _equipoRepositorio.ObtenerTodos().Count);
        }

        [TestMethod]
        public void CompletarEquiposAutomaticamente_ConAlgunosEquipos_CompletaHasta48()
        {
            var equipo = new Equipo();
            equipo.Nombre = "Uruguay";
            equipo.Confederacion = Confederacion.CONMEBOL;
            equipo.RankingFifa = 1500;
            _equipoServicio.AgregarEquipo(equipo);

            _equipoServicio.CompletarEquiposAutomaticamente(42);

            Assert.AreEqual(48, _equipoRepositorio.ObtenerTodos().Count);
        }

        [TestMethod]
        public void CompletarEquiposAutomaticamente_Con48Equipos_NoAgregaNinguno()
        {
            _equipoServicio.CompletarEquiposAutomaticamente(42);
            _equipoServicio.CompletarEquiposAutomaticamente(42);
            Assert.AreEqual(48, _equipoRepositorio.ObtenerTodos().Count);
        }

        [TestMethod]
        public void CompletarEquiposAutomaticamente_RespetaCuposPorConfederacion()
        {
            _equipoServicio.CompletarEquiposAutomaticamente(42);
            Assert.IsTrue(_equipoRepositorio.ObtenerTodos().Count(e => e.Confederacion == Confederacion.UEFA) <= 16);
            Assert.IsTrue(_equipoRepositorio.ObtenerTodos().Count(e => e.Confederacion == Confederacion.CONMEBOL) <= 7);
            Assert.IsTrue(_equipoRepositorio.ObtenerTodos().Count(e => e.Confederacion == Confederacion.CONCACAF) <= 7);
            Assert.IsTrue(_equipoRepositorio.ObtenerTodos().Count(e => e.Confederacion == Confederacion.CAF) <= 9);
            Assert.IsTrue(_equipoRepositorio.ObtenerTodos().Count(e => e.Confederacion == Confederacion.AFC) <= 8);
            Assert.IsTrue(_equipoRepositorio.ObtenerTodos().Count(e => e.Confederacion == Confederacion.OFC) <= 1);
        }

        [TestMethod]
        public void CompletarEquiposAutomaticamente_GeneraNombresDeterministicos()
        {
            _equipoServicio.CompletarEquiposAutomaticamente(42);
            var equipos = _equipoRepositorio.ObtenerTodos();
            Assert.IsTrue(equipos.Any(e => e.Nombre.StartsWith("AFC_")));
            Assert.IsTrue(equipos.Any(e => e.Nombre.StartsWith("CAF_")));
            Assert.IsTrue(equipos.Any(e => e.Nombre.StartsWith("UEFA_")));
        }

        [TestMethod]
        public void CompletarEquiposAutomaticamente_ConMismaSemilla_ProduceMismoRankingFifa()
        {
            _equipoServicio.CompletarEquiposAutomaticamente(42);
            var rankings1 = _equipoRepositorio.ObtenerTodos().Select(e => e.RankingFifa).ToList();

            var equipoRepositorio2 = new EquipoRepositorio();
            var equipoServicio2 = new EquipoServicio(equipoRepositorio2, _auditoriaServicio, _sesionServicio);
            equipoServicio2.CompletarEquiposAutomaticamente(42);
            var rankings2 = equipoRepositorio2.ObtenerTodos().Select(e => e.RankingFifa).ToList();

            CollectionAssert.AreEqual(rankings1, rankings2);
        }
        [TestMethod]
        public void CompletarEquiposAutomaticamente_RegistraLogDeAuditoria()
        {
            _equipoServicio.CompletarEquiposAutomaticamente(42);

            Assert.IsTrue(_auditoriaRepositorio.ObtenerTodos().Count > 0);
        }
        
        [TestMethod]
        public void CompletarEquiposAutomaticamente_RegistraLogConDetallePorConfederacion()
        {
            _equipoServicio.CompletarEquiposAutomaticamente(42);

            var log = _auditoriaRepositorio.ObtenerTodos().First();
            Assert.IsTrue(log.Accion.Contains("UEFA"));
            Assert.IsTrue(log.Accion.Contains("CONMEBOL"));
            Assert.IsTrue(log.Accion.Contains("42"));
        }
        
        [TestMethod]
        [ExpectedException(typeof(Exception))]
        public void AgregarEquipo_SinRolAdministrador_DeberiaLanzarExcepcion()
        {
            var usuarioEditor = new Usuario();
            usuarioEditor.Nombre = "Juan";
            usuarioEditor.Apellido = "Perez";
            usuarioEditor.Email = "juan@ejemplo.com";
            usuarioEditor.FechaNacimiento = new DateTime(1990, 5, 15);
            usuarioEditor.Contrasena = "Abcdef1@";
            usuarioEditor.Roles.Add(Rol.Editor);
            _sesionServicio.IniciarSesion(usuarioEditor);

            var equipo = new Equipo();
            equipo.Nombre = "Uruguay";
            equipo.Confederacion = Confederacion.CONMEBOL;
            equipo.RankingFifa = 1500;
            _equipoServicio.AgregarEquipo(equipo);
        }
        
        [TestMethod]
        public void CompletarEquiposAutomaticamente_NombreGenerado_DebeEmpezarDesdeUnosPorConfederacion()
        {
            _equipoServicio.CompletarEquiposAutomaticamente(42);

            var equipos = _equipoServicio.ObtenerTodos();
    
            Assert.IsTrue(equipos.Any(e => e.Nombre == "UEFA_01"));
            Assert.IsTrue(equipos.Any(e => e.Nombre == "CAF_01"));
            Assert.IsTrue(equipos.Any(e => e.Nombre == "AFC_01"));
        }
        
        [TestMethod]
        [ExpectedException(typeof(Exception))]
        public void CompletarEquiposAutomaticamente_SinRolAdministrador_DeberiaLanzarExcepcion()
        {
            var usuarioEditor = new Usuario();
            usuarioEditor.Nombre = "Juan";
            usuarioEditor.Apellido = "Perez";
            usuarioEditor.Email = "juan@ejemplo.com";
            usuarioEditor.FechaNacimiento = new DateTime(1990, 5, 15);
            usuarioEditor.Contrasena = "Abcdef1@";
            usuarioEditor.Roles.Add(Rol.Editor);
            _sesionServicio.IniciarSesion(usuarioEditor);

            _equipoServicio.CompletarEquiposAutomaticamente(42);
        }
        
    }
}