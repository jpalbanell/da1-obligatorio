---
name: reporte-contexto
description: >
  Genera un reporte estructurado del estado del proyecto, pensado para
  compartir con otra instancia de Claude (chat) o con un compañero de equipo
  y ponerlo al día rápido. Usar cuando el usuario pide un "reporte de
  contexto", "reporte del proyecto", "poné al día a Claude", "resumime cómo
  está el proyecto" o algo similar. Acepta un foco opcional: si el usuario
  menciona un área concreta (p. ej. "enfocate en la capa de servicios"),
  el reporte profundiza en eso además de las secciones base.
---

# Reporte de contexto del proyecto

El objetivo es producir un documento en Markdown que otra instancia de Claude
(o un compañero) pueda leer y entender en qué estado está el proyecto sin tener
acceso al repo. Priorizá ser fiel a lo que hay en el código y en git: no
inventes ni asumas; si un dato no se puede obtener, decilo explícitamente.

## Procedimiento

Antes de redactar, recopilá información real del repositorio:

- `git branch -a` y `git status` para ramas y estado actual.
- `git log --oneline -15` en la rama actual.
- `git log --oneline origin/dev..HEAD` y `git diff --stat origin/dev...HEAD`
  para ver qué cambió respecto de `dev`.
- Revisá el `.sln` y los `.csproj` para listar los proyectos/capas.
- Si es viable y rápido, `dotnet build` para reportar si compila.

## Secciones que SIEMPRE se incluyen (base fija)

### 1. Resumen de arquitectura
- Proyectos del `.sln` y la responsabilidad de cada capa.
- Patrón general (repositorios, servicios, interfaces, inyección, etc.).
- Stack: lenguaje, framework, base de datos, librerías de testing.
- Si la arquitectura cambió hace poco, aclarar el "antes → después".

### 2. Estado de Git
- Rama actual.
- Ramas activas (locales y remotas) y para qué es cada una.
- Qué está mergeado a `dev` y qué sigue en progreso.
- Últimos commits de la rama actual.

### 3. Últimos cambios
- A partir del diff contra `dev`, listar los archivos tocados agrupados por capa.
- Resumir QUÉ cambió y POR QUÉ, no solo los nombres de archivo.
- Destacar especialmente: cambios de arquitectura, firmas de constructores,
  interfaces, contratos públicos y cualquier cosa que afecte a otras capas.

### 4. Estado de los tests
- Cantidad aproximada y ubicación de los tests.
- Estrategia: unitarios, uso de mocks (Moq u otro), EF Core In-Memory, etc.
- ¿Compila la solución? ¿Pasan los tests? (reportar el resultado real;
  si no se corrieron, decirlo).

### 5. Tarea / feature en curso
- En qué se está trabajando en la rama actual.
- Qué quedó hecho y qué falta.

### 6. Conflictos / riesgos potenciales
- Solapamientos entre ramas (dos personas tocando lo mismo).
- Cambios que pueden romper otras partes al mergear.
- Decisiones pendientes de coordinar con el equipo.

## Sección variable (foco solicitado)

Si el usuario indicó un foco al invocar la skill, agregá al final una sección
**"Foco solicitado: <tema>"** que profundice en ese tema: archivos relevantes,
fragmentos de código clave, decisiones de diseño, y cómo se relaciona con el
resto. Si no se indicó foco, omití esta sección.

## Formato de salida

- Markdown con encabezados claros, en español.
- Conciso y factual: resumí, no vuelques archivos enteros. Incluí solo
  fragmentos de código cuando sean realmente esclarecedores.
- Pensado para que otra instancia de Claude se ponga en contexto leyéndolo una vez.
- Cerrá con una lista breve de "Preguntas abiertas / a decidir" si las hay.
