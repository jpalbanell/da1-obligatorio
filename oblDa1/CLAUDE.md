# CLAUDE.md — WorldCupPlanner 2026

## Descripción del Proyecto

WorldCupPlanner 2026 es una aplicación para gestionar la fase de grupos del Mundial de Fútbol 2026 (12 grupos de 4 equipos) y preparar los cruces mediante sorteos determinísticos (semillas), asegurando trazabilidad y reproducibilidad.

---

## 1. Requerimientos No Funcionales

### 1.1 Separación de Componentes

- La lógica de negocio y la interfaz de usuario deben estar **claramente separadas**.
- Cada capa debe poder evolucionar de forma independiente.

### 1.2 Tecnología

- **Lenguaje:** C# en .NET Core 8.
- **Framework UI:** Blazor Server App.
- **Control de versiones:** GitHub con metodología **Gitflow**.
- **Nombre del repositorio:** `NumeroEstudiante1_NumeroEstudiante2_NumeroEstudiante3`.

### 1.3 Desarrollo Guiado por Pruebas (TDD)

- Todas las clases (excepto UI) deben diseñarse y construirse con **TDD** usando **MSTest**.
- Todas las reglas de negocio deben estar completamente implementadas y probadas.
- Configurar un **pipeline de GitHub Actions** que ejecute los tests en cada pull request hacia `main`.
- No es obligatorio aprobar los tests para hacer merge, pero el pipeline debe ejecutarse siempre y reportar resultados visibles.

### 1.4 Almacenamiento

- Para esta primera entrega, toda la información se guarda **en memoria** (sin base de datos).

---

## 2. Requerimientos Funcionales

### 2.1 Gestión de Usuarios (ABM)

**Roles (no excluyentes):**

- **Administrador del Sistema:** Gestión de usuarios, equipos, estadios; generación de equipos; visualizar logs.
- **Editor:** Generar y consultar fixture; editar partidos (fecha, estadio, resultado); simular resultados; realizar sorteos de cruces (segunda fase); importación.

> Un usuario puede tener ambos roles simultáneamente.

**Información del Usuario:**

- Nombre
- Apellido
- Email
- Fecha de nacimiento (formato MM-DD-AAAA)
- Contraseña

**Requisitos de Contraseña:**

- Mínimo 8 caracteres.
- Al menos una letra mayúscula (A-Z).
- Al menos una letra minúscula (a-z).
- Al menos un número (0-9).
- Al menos un carácter especial (@, #, $, .).
- Persistida con algún tipo de cifrado, **nunca en texto plano**.

**Gestión de contraseñas:**

- Los administradores pueden generar una contraseña por defecto o reiniciar la de un usuario existente.
- Los usuarios pueden definir su propia contraseña solo estando logueados.

> **IMPORTANTE:** Este requerimiento debe ser desarrollado utilizando IA generativa (ver sección 3.1).

### 2.2 Gestión de Equipos (CRUD)

**Entidad Equipo:**

| Campo | Reglas |
|-------|--------|
| Nombre | Obligatorio, 1–60 chars, **único en el sistema** |
| Confederación | Obligatorio; valores: AFC, CAF, CONCACAF, CONMEBOL, OFC, UEFA |
| Ranking FIFA | Obligatorio; entero entre 300–2500 |

**Cupos por confederación:**

| Confederación | Cupo |
|---------------|------|
| UEFA | 16 |
| CONMEBOL | 7 |
| CONCACAF | 7 |
| CAF | 9 |
| AFC | 8 |
| OFC | 1 |
| **Total** | **48** |

**Reglas adicionales:**

- Empates de RankingFIFA se resuelven por sorteo determinístico (Fisher–Yates) usando `SemillaFixture`, auditando el orden resultante.
- Los equipos deben ordenarse del 1 al 48 para el sorteo del fixture.

### 2.3 Gestión de Estadios (CRUD)

**Entidad Estadio:**

| Campo | Reglas |
|-------|--------|
| Nombre | Obligatorio, 1–80 chars, **único en el sistema** |
| Ciudad | Obligatorio, 1–60 chars |
| Descripción | Opcional, 1–400 chars |
| Capacidad locativa | Obligatorio, **≥ 20,000 espectadores** |

### 2.4 Generación Automática de Equipos

- Si hay menos de 48 equipos, permitir completar automáticamente hasta el total respetando cupos.
- Nombres determinísticos: `UEFA_01`, `CONMEBOL_02`, etc.
- RankingFIFA reproducible usando `SemillaCompletar`.
- Auditar: cantidad completada por confederación, semilla usada y rangos asignados.

### 2.5 Edición de Partido

**Entidad Partido:**

| Campo | Reglas |
|-------|--------|
| Id | Numérico incremental autogenerado |
| Fecha | Obligatorio, formato ISO 8601, fecha válida |
| Estadio | Obligatorio |
| Equipo local | Obligatorio |
| Equipo visitante | Obligatorio, distinto de local |
| Grupo | Obligatorio, etiqueta A–L |
| Goles local | — |
| Goles visitante | — |
| Vencedor | — |

**Restricción:** Una vez generados los cruces para la segunda fase (ver 2.8), **no se pueden editar** los partidos de la fase anterior.

### 2.6 Simulación de Resultados

- Simular resultados de partidos de fase de grupos y todas las fases restantes.
- Basarse en `RankingActual` de los equipos.
- Usar `SemillaSimulation` para reproducibilidad.

### 2.7 Generación de Fixture (Primera Fase)

**Condición previa:** Debe haber **48 equipos cargados** y **al menos 4 estadios**.

**Estructura:** 12 grupos (A–L) de 4 equipos, 6 partidos por grupo.

**Ordenamiento en bombos:**

- 4 bombos de 12 equipos.
- Orden principal: RankingFIFA DESC.
- Empates: sorteo determinístico (Fisher–Yates) con `SemillaFixture` (obligatoria).

**Distribución en grupos (round-robin + confederaciones):**

- Asignación A→L en round-robin sobre el orden anterior.
- UEFA puede repetir hasta 2 equipos en un grupo; el resto de confederaciones, no.

**Calendario por grupo:**

- 6 partidos (ida única):
    - Jornada 1: E1 vs E4, E2 vs E3
    - Jornada 2: E1 vs E3, E2 vs E4
    - Jornada 3: E1 vs E2, E3 vs E4
- Fechas desde `FechaInicioTorneo` (default: 2026-06-01), separación de 3 días (D, D+3, D+6).
- Máximo 3 partidos por día con separación de 4 horas entre cada uno.
- Hora de inicio de cada fecha: **14:00hs**.
- La última fecha de cada grupo se juega el mismo día a la misma hora.

**Rotación de estadios:**

- Ordenar estadios por nombre normalizado ASC.
- Asignación circular: `Partido #n → Estadio[(n-1) mod CantEstadios]`.

### 2.8 Cruces por Sorteo (Segunda Fase)

**Criterios de clasificación (en orden):**

1. Puntos (Pts)
2. Diferencia de goles (DG)
3. Goles a favor (GF)
4. Desempate final por sorteo determinístico con `SemillaCrucesFase`

**Ranking resultante:**

- 12 primeros y 12 segundos de cada grupo + 8 mejores terceros = 32 equipos.

**Emparejamientos:**

- **A1–A8:** Los 8 primeros con mayor puntaje vs los 8 terceros.
- **B1–B4:** Los 4 primeros restantes vs los 4 segundos con menor puntaje.
- **B5–B8:** Los 8 segundos restantes se cruzan entre sí.

**Regla de emparejamiento:** Se barajan ambas listas con Fisher–Yates usando `SemillaCrucesFase`. Cada equipo se empareja con el primer disponible que **no pertenezca al mismo grupo**. Si no es posible, se pasa al siguiente.

**Fases eliminatorias:**

- **Octavos:** C1 = Ganador A1 vs Ganador B1 ... C8 = Ganador A8 vs Ganador B8
- **Cuartos:** D1 = Ganador C1 vs C2 | D2 = C3 vs C4 | D3 = C5 vs C6 | D4 = C7 vs C8
- **Semifinal:** S1 = Ganador D1 vs D2 | S2 = Ganador D3 vs D4
- **Tercer puesto:** Perdedor S1 vs Perdedor S2
- **Final:** Ganador S1 vs Ganador S2

### 2.9 Importación

- Importar CSV con 48 equipos.
- Encabezados exactos: `Nombre`, `Confederación`, `RankingFIFA`.
- Validar: nombre único, confederación válida, rango válido.

### 2.10 Auditoría (Log)

Registrar con **timestamp ISO-8601** y **usuario**:

- Altas/ediciones de usuarios.
- Altas, ediciones y eliminaciones de estadios.
- Altas, ediciones y eliminaciones de equipos.
- Generación automática de equipos.
- Generación de fixture.
- Modificación de partidos.
- Realización de sorteo para cruces.
- Importación de equipos (éxitos y errores).

### 2.11 Pantallas Mínimas (UI)

1. **Login**
2. **Usuarios (ABM)** — solo Administrador
3. **Equipos** — con generador automático para completar los 48
4. **Estadios** — solo Administrador
5. **Fixture y cruces** — generar fixture, mostrar 12 grupos con participantes; al acceder al grupo, mostrar equipos, estadio y fecha; generar cruces de segunda fase una vez completada la primera; mostrar ganadores y cruces progresivamente
6. **Listado de partidos** — organizados por fecha, filtrar por fecha, estadio, grupo y fase
7. **Edición de partido** — modificar fecha, estadio o cargar resultado
8. **Visualización de log**
9. **Importar**

---

## 3. Requerimientos de IA Generativa

### 3.1 Implementación de Gestión de Usuarios con IA

- El ABM de Usuarios debe ser desarrollado usando IA generativa (total o parcialmente).
- Incluir tests unitarios.
- Cumplir Clean Code.
- Seguir la estructura y convenciones del resto del proyecto.

### 3.2 Skills e Instrucciones

Usar al menos una de las siguientes skills de GitHub Copilot:

- **Add Educational Comments**
- **Create-readme**
- **csharp-mstest** (solo para código generado por IA)
- **git-commit**

Opcionalmente, se puede crear una nueva skill.

### 3.3 Alternativas de Implementación (Importación)

Generar 3 implementaciones distintas del algoritmo de importación, cada una priorizando:

1. **Extensibilidad:** facilitar adaptación a cambios (ej. agregar nuevos campos).
2. **Bajo acoplamiento y alta cohesión:** mínimo acoplamiento con el resto de clases.
3. **Facilidad de comprensión:** Clean Code en nombres, parámetros, constantes, métodos que hacen una sola cosa.

- Cada implementación en una **rama dedicada** del repositorio.
- Seleccionar una de forma justificada para la solución final.

---

## 4. Definiciones Importantes

### Normalización

Minúsculas → sin tildes → no alfanuméricos se convierten a espacio → colapsar espacios → trim.

### Determinismo con Semillas

- Sorteos con `System.Random(Semilla)`.
- Ante misma entrada, resultado idéntico.
- Cada uso de semilla debe quedar en el log de auditoría (timestamp + usuario).

### Algoritmo Fisher-Yates

- Barajado determinístico obligatorio para:
    - Resolver empates de RankingFIFA.
    - Ordenar equipos en bombos.
    - Barajar listas de clasificados en cruces de eliminación.

### Round-Robin de Grupos

- Distribución A→L sobre la lista de equipos previamente ordenada.

### Rotación de Estadios

- Asignación circular siguiendo nombres normalizados ASC.

---

## 5. Parámetros Configurables (por UI o archivo de configuración)

| Parámetro | Descripción | Default |
|-----------|-------------|---------|
| `SemillaFixture` | Fisher-Yates para fixture, orden de bombos y empates de ranking | — |
| `SemillaCompletar` | Generación automática de equipos ficticios | — |
| `SemillaCrucesFase` | Fisher-Yates para cruces de segunda fase | — |
| `SemillaSimulation` | Resultados de partidos simulados reproducibles | — |
| `FechaInicioTorneo` | Fecha base para el calendario | 2026-06-01 |
| `MaxPartidosPorDia` | Tope máximo de partidos por jornada | 3 |
| `SeparacionEntreFechas` | Intervalo en días entre jornadas dentro de un grupo | 3 |

---

## 6. Convenciones de Código

- Seguir convenciones de C#: https://docs.microsoft.com/en-us/dotnet/csharp/fundamentals/coding-style/coding-conventions
- Cumplir Clean Code (capítulos 1 al 10 y 12).
- Framework de tests: **MSTest**.
- Cobertura mínima esperada: **90% de líneas** (justificar si no se alcanza).
- Reporte de cobertura con **JetBrains dotCover**.

---

## 7. Estructura del Repositorio (Gitflow)

- Rama principal: `main`
- Ramas de feature, release y hotfix según Gitflow.
- Un commit por cada test para "Generación de fixture" indicando paso del ciclo TDD (RED, GREEN, REFACTOR).
- Para el resto de funcionalidades, commits cuando los tests estén en GREEN.
- Release y Tag en `main` con nombre adecuado.
- La carpeta de aplicación compilada debe incluirse en el repo.
- La documentación **NO** va en el repositorio (se entrega por Gestión).

---

## 8. Pipeline CI (GitHub Actions)

```yaml
# El pipeline debe:
# - Ejecutar los tests MSTest en cada pull request hacia main
# - Reportar resultados visibles
# - No es obligatorio aprobar para mergear
```

---

## 9. Reglas de Negocio Clave (Resumen)

- Nombre de equipo: único en el sistema.
- Nombre de estadio: único en el sistema.
- Capacidad de estadio: ≥ 20,000.
- RankingFIFA: entre 300 y 2500.
- Cupos por confederación deben respetarse.
- UEFA puede repetir hasta 2 equipos por grupo; el resto de confederaciones no.
- No se puede generar fixture sin 48 equipos y al menos 4 estadios.
- Partidos de primera fase se bloquean al generar cruces de segunda fase.
- Contraseña: 8+ chars, mayúscula, minúscula, número, carácter especial, cifrada.
- Fecha de nacimiento en formato MM-DD-AAAA.
- Todos los sorteos deben ser determinísticos y auditados.

---

## 10. Entrega

- **Fecha límite:** 14/05/2026 hasta las 21:00 en gestion.ort.edu.uy.
- **Formato:** zip, rar o pdf (máx. 40MB).
- **Defensa:** entre 15/05/2026 y 04/06/2026 (presencial).
- **Documentación:** máximo 15 páginas (sin contar anexos), en PDF, entregada solo por Gestión.
- **Repositorio GitHub:** en la organización IngSoft-DA1, merge a main y Release/Tag antes de las 21:00 del día de entrega.
