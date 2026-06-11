# WorldCupPlanner 2026

Aplicación web para gestionar la fase de grupos del Mundial de Fútbol 2026 (12 grupos de 4 equipos) y preparar los cruces de segunda fase mediante sorteos determinísticos con semillas auditadas. Permite administrar usuarios, equipos y estadios; generar y editar el fixture; simular resultados; importar equipos desde CSV; y exportar datos a XLSX.

## Tecnologías utilizadas

| Categoría | Tecnología | Versión |
|-----------|-----------|---------|
| Lenguaje | C# | .NET 8 |
| Framework UI | Blazor Server App | .NET 8 |
| ORM | Entity Framework Core (Code First) | 8.x |
| Proveedor BD | Microsoft.EntityFrameworkCore.SqlServer | 8.x |
| Lazy Loading | Microsoft.EntityFrameworkCore.Proxies | 8.x |
| Base de datos | Azure SQL Edge (contenedor Docker) | latest |
| Testing | MSTest | 3.1.1 |
| Mocking | Moq | 4.20.72 |
| Test SDK | Microsoft.NET.Test.Sdk | 17.8.0 |
| BD en memoria (tests) | Microsoft.EntityFrameworkCore.InMemory | 8.x |
| Exportación XLSX | ClosedXML | 0.105.0 |
| CI | GitHub Actions | — |

## Estructura de la solución

La solución `oblDa1.sln` sigue una arquitectura en capas con separación estricta de responsabilidades:

```
oblDa1.sln
├── Dominio/          — Entidades del dominio (Equipo, Estadio, Partido, Usuario, etc.)
│                       Sin dependencias externas.
├── IRepositorios/    — Contratos (interfaces) de acceso a datos.
│                       Depende de: Dominio.
├── IServicios/       — Contratos (interfaces) de lógica de negocio.
│                       Depende de: Dominio.
├── Repositorios/     — Implementación EF Core de los repositorios (DbContext, migraciones).
│                       Depende de: Dominio, IRepositorios.
├── Servicios/        — Lógica de negocio: fixture, sorteos, importación CSV, exportación XLSX.
│                       Depende de: Dominio, IServicios, Repositorios.
├── Web/              — Interfaz Blazor Server (páginas, componentes, DI).
│                       Depende de: Servicios, Repositorios.
└── Tests/            — Suite de tests MSTest con Moq (489 tests).
                        Depende de: Dominio, IServicios, Servicios, Repositorios.
```

El flujo de dependencias es unidireccional: `Web → Servicios → Repositorios → Dominio`. Las capas de interfaces (`IRepositorios`, `IServicios`) permiten invertir dependencias para facilitar el testing con mocks.

## Instrucciones para su ejecución

### Requisitos previos

- [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
- [Docker Desktop](https://www.docker.com/products/docker-desktop/)

### 1. Levantar la base de datos

```bash
docker-compose up -d
```

Esto inicia un contenedor `sqlserver` (Azure SQL Edge) en el puerto `1433` con las credenciales:

| Parámetro | Valor |
|-----------|-------|
| Host | `localhost,1433` |
| Usuario | `sa` |
| Contraseña | `P4ssword` |

La cadena de conexión ya está configurada en `oblDa1/Web/appsettings.json`:

```json
"ConnectionStrings": {
  "DefaultConnection": "Server=localhost,1433;Database=DA1Db;User Id=sa;Password=P4ssword;TrustServerCertificate=true"
}
```

### 2. Ejecutar la aplicación

```bash
cd oblDa1/Web
dotnet run
```

Al arrancar, `Program.cs` aplica las migraciones de EF Core automáticamente (`Database.Migrate()`): crea la base de datos `DA1Db` y todas las tablas si no existen. No es necesario ejecutar ningún script SQL para el arranque normal.

La aplicación queda disponible en:

- **HTTP:** `http://localhost:5153`
- **HTTPS:** `https://localhost:7041`

### 3. Cargar datos de prueba

La carpeta `oblDa1/Scripts/` contiene dos respaldos de base de datos solicitados en la entrega:

| Script | Contenido |
|--------|-----------|
| `Scripts/bd_vacia.sql` | Estructura de tablas vacía (respaldo de base sin datos) |
| `Scripts/bd_datos_prueba.sql` | Base con usuarios, equipos, estadios y partidos de ejemplo |

> **Importante:** estos scripts son respaldos de la base, no pasos del arranque normal. La app ya crea las tablas vía migraciones; ejecutar `bd_vacia.sql` sobre una base existente causaría errores de "tabla ya existe".

Para iniciar con datos de ejemplo, levantá la app una vez (para que cree las tablas) y luego cargá el script de datos usando un cliente SQL (Azure Data Studio, DBeaver, SSMS o `sqlcmd`):

```bash
sqlcmd -S localhost,1433 -U sa -P P4ssword -d DA1Db -i oblDa1/Scripts/bd_datos_prueba.sql
```

### 4. Ejecutar los tests

```bash
dotnet test oblDa1/oblDa1.sln
```

Corre los 489 tests MSTest.
