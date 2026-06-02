---
name: revision-tests
description: >
  Analiza los cambios de código (diff) para detectar huecos de testing antes
  de commitear, mergear o pasar a producción. Identifica funciones o
  comportamientos nuevos o modificados que podrían no tener test, y tests que
  podrían haber quedado obsoletos o redundantes tras un refactor. Usar cuando
  el usuario pide "revisar tests", "ver si faltan tests", "chequear cobertura
  de los cambios", "qué tests me faltan", "ver si sobran tests", o antes de
  finalizar una entrega. Acepta opcionalmente el rango de diff a analizar.
---

# Revisión de tests sobre los cambios

Esta skill hace un análisis **heurístico** de cobertura: compara el código de
producción que cambió contra los tests existentes y señala posibles huecos.
NO es una herramienta de cobertura real (como coverlet); trabaja leyendo el
código, así que sus hallazgos son CANDIDATOS para revisar, no verdades
absolutas. Puede haber falsos positivos y negativos: la decisión final siempre
es del desarrollador.

## Definir el alcance del diff

Por defecto, analizá los cambios todavía no consolidados:
- `git diff HEAD` (cambios en el working tree) y `git diff --staged`.
- `git show HEAD` (qué cambió el último commit).

Si el usuario indica otro rango, usalo. Rangos típicos:
- Rama completa contra dev: `git diff origin/dev...HEAD`
- Lo que iría a producción: `git diff main...dev` (o la rama de release).

Indicá SIEMPRE al principio del reporte qué rango se analizó.

## Procedimiento

1. Obtené el diff del rango elegido.
2. Separá los archivos de **producción** (Dominio, Servicios, Repositorios, etc.)
   de los archivos de **test** (proyecto Tests).
3. En el código de producción, identificá:
   - Métodos o propiedades públicas nuevas o con firma cambiada.
   - Clases nuevas.
   - Nuevas ramas de lógica (if/switch), validaciones y excepciones que se lanzan.
   - Métodos eliminados o renombrados (por refactor).
4. En el proyecto de tests, buscá referencias a esos métodos/clases (por nombre)
   para ver qué está cubierto y qué no.

## Qué reportar

### 1. Tests que podrían FALTAR
Por cada método público o comportamiento nuevo/cambiado que no tenga un test
que lo ejercite, listalo e indicá QUÉ escenario debería cubrirse (camino feliz,
validación, excepción, caso borde). Respetá los dos niveles de testing del curso:
- Métodos de **repositorio** → test con EF Core In-Memory.
- Lógica de **servicio** → test que mockea el repositorio (con Moq).

No escribas el test completo salvo que se pida; describí el caso a cubrir.

### 2. Tests que podrían SOBRAR o estar obsoletos
- Tests que referencian métodos/clases que ya no existen o se renombraron
  (candidatos a romper la compilación o a borrarse).
- Tests duplicados tras un refactor que unificó comportamiento.
- Tests de una clase o servicio que fue absorbido o eliminado.

### 3. Riesgos de cobertura
- Nuevas validaciones o excepciones sin un test que las verifique.
- Cambios de comportamiento que invalidan lo que un test daba por sentado.

## Formato de salida
- Markdown en español, conciso.
- Agrupá por las tres categorías de arriba.
- Para cada ítem: archivo + método/clase, y una línea de por qué.
- Cerrá con un resumen tipo "X posibles tests faltantes, Y posibles tests
  obsoletos" y una recomendación de prioridad.
- Recordá explícitamente que es un análisis heurístico para revisión humana.
