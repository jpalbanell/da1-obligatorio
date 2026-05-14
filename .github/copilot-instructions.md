# Instrucciones para el agente de IA — WorldCupPlanner 2026

Este archivo define las convenciones y prácticas del proyecto que deben seguirse al generar o modificar código. Aplica a cualquier agente de IA utilizado durante el desarrollo (GitHub Copilot, Claude Code, etc.).

## Contexto del proyecto

WorldCupPlanner 2026 es una aplicación web para la planificación y simulación del Mundial 2026, desarrollada como obligatorio de la materia Diseño de Aplicaciones 1 (ORT Uruguay).

- **Stack:** C# .NET 8, Blazor Server, MSTest
- **Arquitectura:** capas — Dominio, Repositorios, Servicios, Web
- **Persistencia:** repositorios en memoria (Singleton)
- **Inyección de dependencias:** Microsoft.Extensions.DependencyInjection (Servicios Scoped, Repositorios Singleton)

## Idioma

- Todo el código en **español**: nombres de clases, métodos, variables, namespaces, mensajes de excepción y commits.
- Excepción única: las palabras `Test` y `Tests` se mantienen en inglés por convención de MSTest.
- Sin tildes ni caracteres especiales en commits.

## Naming

- Clases y métodos públicos: PascalCase (`AgregarEquipo`, `ObtenerTodos`).
- Variables locales y parámetros: camelCase (`equipoNuevo`, `nombreOriginal`).
- Campos privados: `_camelCase` con guion bajo (`_repositorio`, `_sesionServicio`).
- Constantes: PascalCase (`ContrasenaDefault`).
- Métodos de obtención: `Obtener...` (no `Get...`).
- Métodos de validación privados: `Validar...` (`ValidarNombreUnico`, `ValidarUsuarioExiste`).
- Tests: `MetodoBajoPrueba_Escenario_ResultadoEsperado` (`Login_ConCredencialesValidas_DeberiaRetornarUsuario`).

## Arquitectura por capas

Cada capa tiene una responsabilidad estricta. **No saltear capas.**

- **Dominio:** entidades con validaciones en setters. Lanzan `ArgumentException` cuando los datos son inválidos.
- **Repositorios:** solo operaciones de almacenamiento (Agregar, Obtener, Actualizar, Eliminar). Sin lógica de negocio.
- **Servicios:** lógica de negocio. Validan rol, ejecutan reglas y registran auditoría. Lanzan `Exception` para errores de negocio.
- **Web (Blazor):** solo consume **interfaces de Servicios**. Nunca instancia repositorios ni accede directo al dominio para escrituras.

## DTOs

- Los DTOs viven en `Web/DTOs/`.
- Cada DTO tiene métodos estáticos `FromEntity(entidad)` y `ToEntity()`.
- Los métodos de mapeo van **en el DTO**, no en la entidad.

## Servicios — estructura estándar de un método público

El orden de las operaciones en un método de servicio es:

1. Validar rol con `_sesionServicio.ValidarRol(Rol.X)`.
2. Validar reglas de negocio (existencia, unicidad, cupo, etc.) mediante métodos privados `Validar...`.
3. Ejecutar la operación en el repositorio.
4. Registrar la acción en auditoría con `_auditoriaServicio.Registrar(mensaje, _sesionServicio.ObtenerUsuarioActual())`.

Ejemplo:

```csharp
public void AgregarUsuario(Usuario usuario)
{
    _sesionServicio.ValidarRol(Rol.Administrador);
    ValidarEmailUnico(usuario.Email);
    usuario.Id = _proximoId++;
    _repositorio.Agregar(usuario);
    _auditoriaServicio.Registrar($"Alta de usuario: {usuario.Nombre}", _sesionServicio.ObtenerUsuarioActual());
}
```

## TDD

- Test que falle primero, después código de producción mínimo, después refactor.
- Todo el ciclo se commitea en un **único commit** (no se separan RED/GREEN/REFACTOR).
- Estructura AAA (Arrange-Act-Assert) en cada test.
- Un único concepto por test.
- Cubrir camino feliz y camino triste.
- Aspirar a cobertura cercana al 100%.

## Tests

- Cada clase de servicio tiene su clase de test `<NombreServicio>Tests`.
- `[TestInitialize]` inicializa repositorios, servicios y un usuario Administrador logueado.
- Método helper `CrearXValido(...)` para generar entidades de prueba sin duplicar setup.
- Excepciones de negocio se verifican con `[ExpectedException(typeof(Exception))]`.
- No llamar `Setup()` manualmente dentro de un test (viola Independent de F.I.R.S.T).

## Manejo de excepciones

- **Dominio:** `ArgumentException` con mensaje descriptivo en validaciones de setters.
- **Servicios:** `Exception` con mensaje claro en validaciones de negocio.
- No usar `catch (Exception) {}` (exception swallowing).
- No devolver null en colecciones — devolver lista vacía.

## Auditoría

- Toda operación de escritura (alta, modificación, eliminación, login, generación de fixture, etc.) se registra mediante `IAuditoriaServicio.Registrar`.
- El mensaje debe ser descriptivo e incluir el identificador relevante de la entidad.
- Cuando hay semilla involucrada (fixture, cruce, simulación), incluir el valor de la semilla en el log.

## Convenciones de Git

- Una **rama por feature o fix**, nombrada `feature/descripcion-corta` o `fix/descripcion-corta`.
- Todos los PRs apuntan a la rama `dev`. La rama `main` recibe solo el merge final de release.
- Un **PR por tarea**. No mezclar features distintas.
- Mensajes de commit: imperativo, en español, sin tildes, prefijados según Conventional Commits (`feat:`, `fix:`, `refactor:`, `test:`, `chore:`).
- `git add .` (no `-A`).
- Ejecutar `dotnet test` antes de pushear. Los tests deben pasar en verde.

## Convenciones de Clean Code

- Funciones pequeñas que hacen una sola cosa.
- Nombres que revelan intención. Evitar variables de una letra excepto índices `i`/`j` en loops cortos.
- Sin magic numbers ni magic strings — usar constantes con nombre.
- Sin código comentado — eso para eso está Git.
- Sin código muerto (campos no usados, métodos privados nunca llamados).
- DRY: evitar duplicación de lógica entre servicios. Si dos servicios necesitan la misma utilidad, extraerla a una clase compartida.

## Skills externas en uso

Este proyecto adopta las siguientes skills del repositorio `github/awesome-copilot`:

- **create-readme:** generación del README del repositorio siguiendo un formato estándar.

Las skills se aplican en la fase final del proyecto para consolidar consistencia en los commits y documentación del repositorio.
