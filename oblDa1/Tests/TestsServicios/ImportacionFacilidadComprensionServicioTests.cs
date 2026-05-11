using Dominio.Entidades;
using Repositorios;
using Servicios;

namespace Tests.TestsServicios
{
    [TestClass]
    public class ImportacionFacilidadComprensionServicioTests
    {
        private IEquipoRepositorio _equipoRepositorio = null!;
        private IEquipoServicio _equipoServicio = null!;
        private IImportacionServicio _servicio = null!;
        private IAuditoriaRepositorio _auditoriaRepositorio = null!;
        private IAuditoriaServicio _auditoriaServicio = null!;
        private ISesionServicio _sesionServicio = null!;

        [TestInitialize]
        public void Setup()
        {
            _equipoRepositorio = new EquipoRepositorio();
            _auditoriaRepositorio = new AuditoriaRepositorio();
            _auditoriaServicio = new AuditoriaServicio(_auditoriaRepositorio);
            _sesionServicio = new SesionServicio();
            _equipoServicio = new EquipoServicio(_equipoRepositorio, _auditoriaServicio, _sesionServicio);
            _servicio = new ImportacionFacilidadComprensionServicio(_equipoRepositorio, _equipoServicio, _auditoriaServicio, _sesionServicio);
            var usuario = new Usuario();
            usuario.Nombre = "Juan";
            usuario.Apellido = "Perez";
            usuario.Email = "juan@ejemplo.com";
            usuario.FechaNacimiento = new DateTime(1990, 5, 15);
            usuario.Contrasena = "Abcdef1@";
            usuario.Roles.Add(Rol.Editor);
            _sesionServicio.IniciarSesion(usuario);
        }

        [TestMethod]
        public void ImportarEquipos_ConUnEquipoValido_DeberiaImportarlo()
        {
            var csv = "Nombre,Confederación,RankingFIFA\nUruguay,CONMEBOL,1500";

            var resultado = _servicio.ImportarEquipos(csv);

            Assert.AreEqual(1, resultado.EquiposImportados);
            Assert.AreEqual(0, resultado.Errores.Count);
        }
        
        [TestMethod]
        public void ImportarEquipos_ConVariosEquiposValidos_DeberiaImportarlos()
        {
            var csv = "Nombre,Confederación,RankingFIFA\nUruguay,CONMEBOL,1500\nArgentina,CONMEBOL,2000";

            var resultado = _servicio.ImportarEquipos(csv);

            Assert.AreEqual(2, resultado.EquiposImportados);
            Assert.AreEqual(0, resultado.Errores.Count);
        }
        
        [TestMethod]
        public void ImportarEquipos_ConConfederacionInvalida_DeberiaRegistrarError()
        {
            var csv = "Nombre,Confederación,RankingFIFA\nUruguay,INVALIDA,1500";

            var resultado = _servicio.ImportarEquipos(csv);

            Assert.AreEqual(0, resultado.EquiposImportados);
            Assert.AreEqual(1, resultado.Errores.Count);
        }
        
        [TestMethod]
        public void ImportarEquipos_ConRankingFueraDeRango_DeberiaRegistrarError()
        {
            var csv = "Nombre,Confederación,RankingFIFA\nUruguay,CONMEBOL,9999";

            var resultado = _servicio.ImportarEquipos(csv);

            Assert.AreEqual(0, resultado.EquiposImportados);
            Assert.AreEqual(1, resultado.Errores.Count);
        }
        
        [TestMethod]
        public void ImportarEquipos_ConNombreDuplicado_DeberiaRegistrarError()
        {
            var csv = "Nombre,Confederación,RankingFIFA\nUruguay,CONMEBOL,1500\nUruguay,CONMEBOL,1600";

            var resultado = _servicio.ImportarEquipos(csv);

            Assert.AreEqual(1, resultado.EquiposImportados);
            Assert.AreEqual(1, resultado.Errores.Count);
        }
        
        [TestMethod]
        public void ImportarEquipos_ConFilaInvalidaEntreValidas_DeberiaImportarLasValidas()
        {
            var csv = "Nombre,Confederación,RankingFIFA\nUruguay,CONMEBOL,1500\nMalo,INVALIDA,1500\nArgentina,CONMEBOL,2000";

            var resultado = _servicio.ImportarEquipos(csv);

            Assert.AreEqual(2, resultado.EquiposImportados);
            Assert.AreEqual(1, resultado.Errores.Count);
        }
        
        [TestMethod]
        public void ImportarEquipos_ConSoloEncabezado_DeberiaRetornarCeroImportados()
        {
            var csv = "Nombre,Confederación,RankingFIFA";

            var resultado = _servicio.ImportarEquipos(csv);

            Assert.AreEqual(0, resultado.EquiposImportados);
            Assert.AreEqual(0, resultado.Errores.Count);
        }
        
        [TestMethod]
        [ExpectedException(typeof(Exception))]
        public void ImportarEquipos_SinRolEditor_DeberiaLanzarExcepcion()
        {
            var usuarioSinRol = new Usuario();
            usuarioSinRol.Nombre = "Juan";
            usuarioSinRol.Apellido = "Perez";
            usuarioSinRol.Email = "juan@ejemplo.com";
            usuarioSinRol.FechaNacimiento = new DateTime(1990, 5, 15);
            usuarioSinRol.Contrasena = "Abcdef1@";
            _sesionServicio.IniciarSesion(usuarioSinRol);

            var csv = "Nombre,Confederación,RankingFIFA\nUruguay,CONMEBOL,1500";
            _servicio.ImportarEquipos(csv);
        }

        [TestMethod]
        public void ImportarEquipos_DeberiaRegistrarAuditoria()
        {
            var csv = "Nombre,Confederación,RankingFIFA\nUruguay,CONMEBOL,1500";

            _servicio.ImportarEquipos(csv);

            var logs = _auditoriaRepositorio.ObtenerTodos();
            Assert.AreEqual(1, logs.Count);
            Assert.IsTrue(logs[0].Accion.Contains("Importación"));
        }

        [TestMethod]
        public void ImportarEquipos_ConErrores_DeberiaRegistrarAuditoriaIncluyendoErrores()
        {
            var csv = "Nombre,Confederación,RankingFIFA\nUruguay,CONMEBOL,1500\nMalo,INVALIDA,1500";

            _servicio.ImportarEquipos(csv);

            var logs = _auditoriaRepositorio.ObtenerTodos();
            Assert.AreEqual(1, logs.Count);
            Assert.IsTrue(logs[0].Accion.Contains("1 errores"));
        }
        
        [TestMethod]
        public void ImportarEquipos_ConRankingFueraDeRangoInferior_DeberiaRegistrarError()
        {
            var csv = "Nombre,Confederación,RankingFIFA\nUruguay,CONMEBOL,100";

            var resultado = _servicio.ImportarEquipos(csv);

            Assert.AreEqual(0, resultado.EquiposImportados);
            Assert.AreEqual(1, resultado.Errores.Count);
        }
        
        [TestMethod]
        public void ImportarEquipos_ExcediendoCupoConfederacion_DeberiaRegistrarError()
        {
            var csv = "Nombre,Confederación,RankingFIFA\n" +
                      "OFC_A,OFC,1500\n" +
                      "OFC_B,OFC,1600";

            var resultado = _servicio.ImportarEquipos(csv);

            Assert.AreEqual(1, resultado.EquiposImportados);
            Assert.AreEqual(1, resultado.Errores.Count);
        }
    }
}