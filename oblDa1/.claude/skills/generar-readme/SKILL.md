---
name: generar-readme
description: >
  Genera o actualiza el archivo README.md del proyecto a partir del estado real
  del repositorio. El README incluye: una breve descripción de la solución, las
  tecnologías utilizadas, la estructura de la solución y las instrucciones para
  ejecutarla. Usar cuando el usuario pide "generá el README", "actualizá el
  readme", "escribí el README.md del proyecto" o similar.
---

# Generar el README.md del proyecto

El objetivo es producir un `README.md` en la raíz del repositorio que sea fiel
al estado actual del código. No inventes datos: leé el repositorio para obtener
la información real. Si algún dato no se puede determinar, dejá un marcador
`<!-- TODO: completar -->` en vez de suponer.

## Recopilación previa

Antes de escribir, inspeccioná el repo:
- El archivo `.sln` y los `.csproj` para listar los proyectos y sus dependencias.
- Los paquetes NuGet usados (EF Core, proveedor de base de datos, librerías de
  testing, etc.).
- El `docker-compose.yml` si existe (motor de base de datos, puertos, credenciales).
- Los `appsettings*.json` para la cadena de conexión y configuración de arranque.
- Cómo se ejecuta la app (proyecto de arranque, comandos `dotnet`).

## Estructura del README.md a generar

Generá el archivo con estas cuatro secciones (las que pide la consigna), más un
encabezado con el nombre del proyecto:

### 1. Descripción de la solución
Un párrafo breve que explique qué hace la aplicación y para qué sirve.

### 2. Tecnologías utilizadas
Lista de lenguaje, framework, motor de base de datos, ORM, librerías de testing
y herramientas relevantes, con sus versiones cuando se puedan determinar.

### 3. Estructura de la solución
Listado de los proyectos del `.sln` y la responsabilidad de cada uno
(capa de dominio, repositorios, servicios, interfaces, web, tests). Explicá
brevemente cómo se relacionan.

### 4. Instrucciones para su ejecución
Pasos concretos y ordenados para levantar el proyecto desde cero:
- Requisitos previos (SDK, Docker, etc.).
- Cómo levantar la base de datos (p. ej. `docker-compose up -d`).
- Cadena de conexión y configuración necesaria.
- Cómo correr la aplicación y cómo correr los tests.

## Pautas de salida
- Escribí el archivo en `README.md` en la raíz del repositorio.
- Markdown claro, con encabezados y bloques de código para los comandos.
- En español.
- Si ya existe un `README.md`, actualizá su contenido respetando esta estructura
  en vez de duplicar secciones.
