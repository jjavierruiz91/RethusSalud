# RethusSalud — Guia de despliegue y operacion

## Arquitectura de la solucion

Clean Architecture en 4 proyectos (`src/`) + 3 proyectos de pruebas (`tests/`):

```
RethusSalud.Domain          -- entidades, maquina de estados de Solicitud, sin dependencias externas
RethusSalud.Application     -- casos de uso, DTOs, validadores (FluentValidation), interfaces de puertos
RethusSalud.Infrastructure  -- EF Core + Identity, repositorios, PDF (QuestPDF), correo (MailKit), Excel (ClosedXML)
RethusSalud.Web             -- ASP.NET Core MVC + Razor Views (composition root en Program.cs)
```

## Ambientes

El comportamiento se controla con la variable `ASPNETCORE_ENVIRONMENT`:

- **Development**: usa `dotnet user-secrets` para la cadena de conexion y credenciales. Siembra 4 cuentas de funcionarios de prueba (`etapa1/2/3/inventario@rethus.local`, clave `Demo123!`) ademas del SuperAdmin — **esto solo ocurre si `ASPNETCORE_ENVIRONMENT=Development`**.
- **Staging/Production**: toda la configuracion sensible debe venir de variables de entorno (nunca de `appsettings.json`, que se versiona en el repo con valores vacios). Las cuentas demo de funcionarios NO se siembran.

### Variables de entorno requeridas en Staging/Production

| Variable | Descripcion |
|---|---|
| `ConnectionStrings__DefaultConnection` | Cadena de conexion a SQL Server |
| `SeedAdmin__Email` / `SeedAdmin__Password` | Credenciales del SuperAdmin inicial (solo se usa si ese usuario no existe aun) |
| `Smtp__Host`, `Smtp__Port`, `Smtp__Usuario`, `Smtp__Password`, `Smtp__Remitente` | Envio de notificaciones por correo. Si `Smtp__Host` queda vacio, el sistema sigue funcionando pero no envia correos (solo lo registra en el log) |
| `DomainWebUrl` | URL publica de la aplicacion; se usa para construir el enlace del codigo QR de verificacion de certificados |
| `Storage__BasePath` | Ruta absoluta donde se guardan los documentos cargados por los ciudadanos (fuera de `wwwroot`, con backup periodico) |

Tras rotar cualquiera de estos valores, reiniciar la aplicacion.

## Migraciones de base de datos

Las migraciones se aplican automaticamente al iniciar la aplicacion (`dbContext.Database.MigrateAsync()` en `Program.cs`). Para generarlas o aplicarlas manualmente:

```bash
dotnet ef migrations add NombreMigracion --project src/RethusSalud.Infrastructure --startup-project src/RethusSalud.Web -o Persistence/Migrations
dotnet ef database update --project src/RethusSalud.Infrastructure --startup-project src/RethusSalud.Web
```

## Respaldo de base de datos

Recomendado en SQL Server: backup completo diario + backups de log de transacciones cada 15-30 minutos (recovery model FULL), retenidos al menos 30 dias. Probar la restauracion periodicamente, no solo la ejecucion del backup.

## Checklist de seguridad ya implementado

- Autorizacion por rol en cada controlador (`[Authorize(Roles = ...)]`), incluyendo verificacion de que la etapa de la solicitud coincide con la etapa del funcionario antes de aprobar/rechazar.
- Validacion de archivos subidos por tipo de contenido declarado **y** por firma binaria real (magic bytes), no solo por el `Content-Type` que envia el navegador.
- Limite de tamano de archivo (10 MB).
- Rate limiting (10 solicitudes/minuto) en los endpoints publicos sin autenticacion: consulta de estado, verificacion de folio, y login.
- Secretos fuera del control de versiones (`dotnet user-secrets` en dev, variables de entorno en produccion).
- Cuentas de funcionarios desactivables (bloquea el login de inmediato via `LockoutEnd`).
- Logging estructurado con Serilog (consola + archivo rotativo diario en `logs/`, 30 dias de retencion).

## Pruebas

```bash
dotnet test
```

23 pruebas: maquina de estados de `Solicitud` (Domain), validadores y reglas de negocio (Application, con Moq), generacion de PDF (Infrastructure).

## Ejecutar localmente

```bash
dotnet user-secrets set "ConnectionStrings:DefaultConnection" "..." --project src/RethusSalud.Web
dotnet user-secrets set "SeedAdmin:Email" "admin@ejemplo.com" --project src/RethusSalud.Web
dotnet user-secrets set "SeedAdmin:Password" "..." --project src/RethusSalud.Web
dotnet run --project src/RethusSalud.Web
```
