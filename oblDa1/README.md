# WorldCupPlanner 2026

## Descripción de la solución

WorldCupPlanner 2026 es una aplicación web para gestionar el Mundial de Fútbol 2026. Permite administrar equipos (48, distribuidos en 6 confederaciones), estadios, usuarios con roles diferenciados (Administrador y Editor), y la generación del fixture de la fase de grupos (12 grupos de 4 equipos). Incluye sorteos determinísticos mediante semillas para la generación de fixture y cruces de segunda fase, simulación de resultados, importación de equipos desde CSV, exportación de datos, y un log de auditoría completo de todas las operaciones.

---

## Tecnologías utilizadas

| Tecnología | Versión |
|---|---|
| Lenguaje | C# |
| Runtime | .NET 8 |
| Framework UI | Blazor Server App |
| ORM | Entity Framework Core 8 |
| Motor de base de datos | Azure SQL Edge (SQL Server) |
| Proveedor EF Core | Microsoft.EntityFrameworkCore.SqlServer 8.x |
| Lazy Loading | Microsoft.EntityFrameworkCore.Proxies 8.x |
| Framework de tests | MSTest 3.1.1 |
| Mocking | Moq 4.20.72 |
| EF InMemory (tests) | Microsoft.EntityFrameworkCore.InMemory 8.x |
| Test SDK | Microsoft.NET.Test.Sdk 17.8.0 |
| Exportación XLSX | ClosedXML 0.105.0 |
| Contenedores | Docker / Docker Compose |

---

## Estructura de la solución

La solución `oblDa1.sln` sigue una arquitectura en capas con separación clara entre dominio, contratos, implementaciones y UI:

```
oblDa1.sln
├── Dominio          → Entidades del negocio y lógica de dominio pura
├── IRepositorios    → Contratos (interfaces) de acceso a datos
├── IServicios       → Contratos (interfaces) de la capa de servicios
├── Repositorios     → Implementación EF Core de los repositorios (SQL Server)
├── Servicios        → Implementación de la lógica de negocio y casos de uso
├── Web              → Aplicación Blazor Server (UI, páginas, layouts, DTOs)
└── Tests            → Suite de tests unitarios e integración (MSTest)
```

### Detalle de cada proyecto

**Dominio**
Contiene las entidades (`Equipo`, `Estadio`, `Usuario`, `Partido`, `Fixture`, `Grupo`, `Confederacion`, `LogAuditoria`, `Notificacion`, `Incidencia`, etc.) y la lógica estrictamente del dominio, incluyendo `BarajadorDeterministico` para los sorteos Fisher–Yates con semilla.

**IRepositorios**
Define los contratos de persistencia: `IEquipoRepositorio`, `IEstadioRepositorio`, `IUsuarioRepositorio`, `IFixtureRepositorio`, `IGrupoRepositorio`, `IPartidoRepositorio`, `IAuditoriaRepositorio`, `INotificacionRepositorio`.

**IServicios**
Define los contratos de negocio: `IUsuarioServicio`, `ITorneoServicio`, `ISesionServicio`, `IAuditoriaServicio`, `IImportacionServicio`, `IExportacionServicio`, `INotificacionServicio`, `IMotorSimulacion`, `IMotorSimulacionSelector`.

**Repositorios**
Implementa los repositorios usando Entity Framework Core contra SQL Server. Incluye `SqlContext` (DbContext), configuraciones de entidades y migraciones.

**Servicios**
Implementa la lógica de negocio: gestión de usuarios y sesión, generación de fixture y cruces, simulación de resultados (con motores aleatorio y probabilístico), importación CSV, exportación CSV/XLSX, auditoría y notificaciones.

**Web**
Aplicación Blazor Server con las siguientes páginas:

| Página | Descripción |
|---|---|
| `Login` | Autenticación de usuarios |
| `Usuarios` | ABM de usuarios (solo Administrador) |
| `Equipos` | CRUD de equipos + generador automático |
| `Estadios` | CRUD de estadios (solo Administrador) |
| `Fixture` | Generación y visualización del fixture y cruces |
| `Partidos` | Listado de partidos con filtros |
| `EditarPartido` | Modificación de fecha, estadio y resultado |
| `Importar` | Importación de equipos desde CSV |
| `Exportar` | Exportación de datos en CSV o XLSX |
| `LogAuditoria` | Visualización del log de auditoría |
| `CambiarContrasena` | Cambio de contraseña del usuario logueado |
| `NotificacionesPeriodista` | Vista de notificaciones |

**Tests**
519 tests distribuidos en tres carpetas:
- `TestsDominio`: tests de entidades y lógica de dominio
- `TestsRepositorios`: tests de repositorios con EF Core InMemory
- `TestsServicios`: tests de servicios con mocks (Moq)

---

## Instrucciones para su ejecución

### Requisitos previos

- [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
- [Docker Desktop](https://www.docker.com/products/docker-desktop/)

### 1. Levantar la base de datos

Desde la raíz del repositorio, ejecutar:

```bash
docker-compose up -d
```

Esto levanta un contenedor de Azure SQL Edge (compatible con SQL Server) en el puerto `1433` con:
- Usuario: `sa`
- Contraseña: `P4ssword`
- Base de datos: `DA1Db`

### 2. Aplicar las migraciones

```bash
dotnet ef database update --project Repositorios --startup-project Web
```

### 3. Ejecutar la aplicación

```bash
dotnet run --project Web
```

La aplicación queda disponible en `https://localhost:5001` (o el puerto que indique la consola).

### 4. Ejecutar los tests

```bash
dotnet test Tests/Tests.csproj
```

Los tests no requieren base de datos: los de repositorios usan EF Core InMemory y los de servicios usan Moq.
