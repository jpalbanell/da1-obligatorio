# WorldCupPlanner 2026

Sistema de planificación y simulación del Mundial de Fútbol 2026. Gestiona la fase de grupos (12 grupos, 48 equipos), genera el fixture de forma determinística mediante semillas, simula resultados y organiza los cruces eliminatorios hasta la final.

> **Primer Obligatorio — Diseño de Aplicaciones 1 (DA1) · ORT Uruguay · Mayo 2026**

---

## Características

- **Gestión completa** de usuarios (con roles Administrador / Editor), equipos y estadios
- **Generación de fixture** determinística usando algoritmo Fisher–Yates con semilla configurable
- **Fase de grupos** — 12 grupos A–L, 6 partidos por grupo, calendario automático con rotación de estadios
- **Sorteo de cruces** para segunda fase con clasificación por puntos, diferencia de goles y goles a favor
- **Fases eliminatorias** — octavos, cuartos, semifinal, tercer puesto y final
- **Simulación de resultados** reproducible basada en RankingFIFA y semilla
- **Importación de equipos** desde CSV
- **Auditoría completa** — cada acción significativa queda registrada con timestamp ISO-8601 y usuario
- **Contraseñas cifradas** con SHA-256; validaciones de complejidad en el dominio

---

## Stack tecnológico

| Capa | Tecnología |
|------|-----------|
| Lenguaje | C# (.NET 8) |
| UI | Blazor Server App |
| Almacenamiento | En memoria (sin base de datos) |
| Tests | MSTest |
| CI | GitHub Actions |

---

## Arquitectura

El proyecto sigue una arquitectura en capas con separación estricta entre lógica de negocio e interfaz:

```
oblDa1/
├── Dominio/          # Entidades y reglas de negocio puras
│   └── Entidades/    # Equipo, Estadio, Usuario, Partido, Grupo, Fixture, …
├── Repositorios/     # Interfaces + implementaciones en memoria
├── Servicios/        # Lógica de aplicación (FixtureServicio, CruceServicio, …)
├── Web/              # Blazor Server — páginas, layouts y DTOs
│   ├── Components/Pages/
│   └── DTOs/
└── Tests/            # Suite MSTest organizada por capa
    ├── TestsDominio/
    ├── TestsRepositorios/
    └── TestsServicios/
```

Cada capa depende únicamente de la capa inferior a través de interfaces, lo que permite evolucionar cada componente de forma independiente.

---

## Requisitos previos

- [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)

---

## Ejecución

```bash
# Desde la raíz del repositorio
dotnet run --project Web
```

La aplicación queda disponible en `http://localhost:5153`.

**Credenciales del administrador inicial:**

| Campo | Valor |
|-------|-------|
| Email | `admin@worldcup.com` |
| Contraseña | `Admin@123` |

---

## Tests

```bash
dotnet test
```

La suite contiene **337 tests** distribuidos entre dominio, repositorios y servicios, todos en estado *passed*.

> El pipeline de GitHub Actions ejecuta los tests automáticamente en cada pull request hacia `main`.

---

## Parámetros configurables

| Parámetro | Descripción | Default |
|-----------|-------------|---------|
| `SemillaFixture` | Fisher-Yates para fixture, bombos y empates de ranking | — |
| `SemillaCompletar` | Generación automática de equipos ficticios | — |
| `SemillaCrucesFase` | Fisher-Yates para cruces de segunda fase | — |
| `SemillaSimulation` | Resultados de partidos simulados reproducibles | — |
| `FechaInicioTorneo` | Fecha base para el calendario | 2026-06-01 |
| `MaxPartidosPorDia` | Tope de partidos por jornada | 3 |
| `SeparacionEntreFechas` | Intervalo en días entre jornadas de un grupo | 3 |

Los parámetros se configuran desde la UI al generar el fixture o los cruces.

---

## Pantallas

| Pantalla | Acceso |
|----------|--------|
| Login | Todos |
| Usuarios (ABM) | Administrador |
| Equipos | Administrador |
| Estadios | Administrador |
| Fixture y cruces | Editor |
| Listado de partidos | Editor |
| Edición de partido | Editor |
| Importar equipos | Editor |
| Log de auditoría | Administrador |

---

## Equipo

| Nombre | Número de estudiante |
|--------|---------------------|
| Juan Pedro Albanell | 311570 |
| Santiago Jorcin | 325511 |
| Leandro Chiurchiu Iglesias | 286847 |
