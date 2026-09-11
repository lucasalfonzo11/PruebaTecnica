# API Prueba Técnica

API REST desarrollada con Minimal APIs de .NET para la gestión de usuarios, direcciones, monedas y conversión de divisas. Utiliza Entity Framework Core con SQLite, FluentValidation para validar solicitudes, autenticación por API Key y Swagger/OpenAPI en el entorno de desarrollo.

## Inicio rápido

### Requisitos previos

- .NET SDK 10.0 (el proyecto utiliza `net10.0`).
- La herramienta local `dotnet-ef` está declarada en `dotnet-tools.json`.

### Ejecutar la API

Desde la raíz del repositorio:

```bash
dotnet tool restore
dotnet ef database update
dotnet run
```

El perfil de Development configurado expone HTTP en `http://localhost:5183`. Swagger UI está disponible en:

```text
http://localhost:5183/swagger
```

Swagger se registra únicamente cuando `ASPNETCORE_ENVIRONMENT` tiene el valor `Development`.

## Base de datos y migraciones

La aplicación utiliza SQLite con la cadena de conexión `Data Source=prueba-tecnica.db`. La base se crea al aplicar las migraciones existentes; la aplicación no aplica migraciones automáticamente al iniciar.

Aplicar la migración existente:

```bash
dotnet tool restore
dotnet ef database update
```

Crear y aplicar una migración después de modificar el modelo:

```bash
dotnet tool restore
dotnet ef migrations add <NombreMigracion>
dotnet ef database update
```

Los archivos de base de datos SQLite son artefactos locales de desarrollo y Git los ignora (`*.db`, `*.db-shm`, `*.db-wal` y `*.db-journal`).

## Autenticación

Todos los endpoints, incluidas las solicitudes realizadas desde Swagger, requieren el header `X-API-KEY`. La clave de prueba configurada explícitamente en `appsettings.json` es:

```text
local-test-key
```

Ejemplo de header:

```http
X-API-KEY: local-test-key
```

Ejemplo con `curl`:

```bash
curl -H "X-API-KEY: local-test-key" http://localhost:5183/users
```

Una clave ausente, malformada o inválida devuelve `401 Unauthorized`.

## Endpoints

Todas las rutas siguientes requieren el header de API Key.

| Área | Método | Ruta | Propósito |
| --- | --- | --- | --- |
| Usuarios | `POST` | `/users` | Crear un usuario. |
| Usuarios | `GET` | `/users` | Listar usuarios; acepta el parámetro opcional `isActive`. |
| Usuarios | `GET` | `/users/{id}` | Obtener un usuario por ID. |
| Usuarios | `PUT` | `/users/{id}` | Modificar un usuario. |
| Usuarios | `DELETE` | `/users/{id}` | Eliminar físicamente un usuario. |
| Direcciones | `POST` | `/users/{userId}/addresses` | Crear una dirección para un usuario existente. |
| Direcciones | `GET` | `/users/{userId}/addresses` | Listar las direcciones de un usuario. |
| Direcciones | `PUT` | `/addresses/{id}` | Modificar una dirección. |
| Direcciones | `DELETE` | `/addresses/{id}` | Eliminar una dirección. |
| Monedas | `POST` | `/currencies` | Crear una moneda. |
| Monedas | `GET` | `/currencies` | Listar monedas y sus tasas. |
| Conversión | `POST` | `/currency/convert` | Convertir un monto entre monedas registradas. |

La documentación interactiva de OpenAPI está disponible en `/swagger` durante Development.

## Ejemplos de solicitudes

Crear un usuario:

```json
{
  "name": "Usuario de ejemplo",
  "email": "usuario.ejemplo@example.com",
  "ci": "1234567",
  "password": "Clave123"
}
```

Crear una dirección:

```json
{
  "street": "Calle de ejemplo 123",
  "city": "Asunción",
  "country": "Paraguay",
  "zipCode": "0000"
}
```

Crear una moneda:

```json
{
  "code": "PYG",
  "name": "Guaraní paraguayo",
  "rateToBase": 1.0
}
```

Convertir moneda:

```json
{
  "fromCurrencyCode": "USD",
  "toCurrencyCode": "PYG",
  "amount": 100
}
```

## Convenciones del proyecto

- El proyecto sigue una estructura orientada a CQRS: los commands y queries se organizan por funcionalidad dentro de `Application`.
- Las solicitudes de usuarios, direcciones, monedas y conversiones se validan con FluentValidation.
- Email y CI de usuarios son únicos a nivel de persistencia; los códigos de moneda también son únicos.
- La eliminación de usuarios es física. La relación entre `Users` y `Addresses` está configurada con borrado en cascada: eliminar un usuario elimina sus direcciones.
- La conversión usa los valores `RateToBase` almacenados: `amount * from.RateToBase / to.RateToBase`.

## Estado de implementación

Implementado:

- CRUD de usuarios, incluido el filtro opcional `isActive` en el listado.
- CRUD de direcciones asociadas a usuarios.
- Creación y listado de monedas.
- Conversión entre monedas registradas.
- Persistencia SQLite, migración inicial de Entity Framework Core, FluentValidation, middleware de API Key y Swagger/OpenAPI en Development.
