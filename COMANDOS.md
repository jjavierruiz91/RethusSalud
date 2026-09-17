# Comandos más usados — RethusSalud

Referencia rápida de comandos .NET/EF Core para este proyecto. Todos se corren desde la
raíz del repo (`D:\PROYECTOS VISUAL SOFTWARE\RethusSalud`), salvo que se indique lo contrario.

## Los 3 del día a día

```bash
# Publicar (genera la carpeta lista para copiar al servidor)
dotnet publish "src/RethusSalud.Web/RethusSalud.Web.csproj" -c Release -o "C:\Users\Lenovo\Desktop\RethusPublish"

# Compilar y verificar que no hay errores (sin publicar)
dotnet build

# Correr el sitio localmente para probar
dotnet run --project src/RethusSalud.Web
```

## Ejecución y desarrollo

```bash
# Como "dotnet run", pero recompila y reinicia solo cuando guardas un cambio
dotnet watch run --project src/RethusSalud.Web

# Restaurar los paquetes NuGet (normalmente automático, útil si algo falla raro)
dotnet restore

# Limpiar archivos de compilación (bin/obj) — sirve si algo se ve "pegado" o corrupto
dotnet clean

# Ver la versión del SDK de .NET instalada
dotnet --version
```

## Pruebas

```bash
# Correr todas las pruebas automatizadas del proyecto
dotnet test

# Correr solo las pruebas de un proyecto específico
dotnet test tests/RethusSalud.Application.Tests
```

## Migraciones de base de datos (Entity Framework Core)

```bash
# Crear una nueva migración (después de cambiar una entidad/modelo)
dotnet ef migrations add NombreDeLaMigracion --project src/RethusSalud.Infrastructure --startup-project src/RethusSalud.Web

# Aplicar las migraciones pendientes a la base de datos
dotnet ef database update --project src/RethusSalud.Infrastructure --startup-project src/RethusSalud.Web

# Ver el listado de migraciones y cuáles ya se aplicaron
dotnet ef migrations list --project src/RethusSalud.Infrastructure --startup-project src/RethusSalud.Web

# Deshacer la última migración (solo si AÚN NO se aplicó a la base de datos)
dotnet ef migrations remove --project src/RethusSalud.Infrastructure --startup-project src/RethusSalud.Web

# Generar el script SQL de todas las migraciones (útil para aplicarlo manualmente en el servidor)
dotnet ef migrations script --project src/RethusSalud.Infrastructure --startup-project src/RethusSalud.Web --output migraciones.sql
```

⚠️ `dotnet ef database drop` **borra la base de datos completa** — nunca correrlo sin confirmar antes con quien lo pida, y nunca contra el servidor de producción.

## Consultar la base de datos desde la terminal

```bash
sqlcmd -S "DESKTOP-F77QBT7\SQLEXPRESS" -U fuetheRuiz -P 123456 -C -d RethusSaludDB -Q "TU CONSULTA AQUI"
```

## Calidad de código

```bash
# Revisar/corregir el formato del código según las reglas de .editorconfig
dotnet format
```

---
*Referencia creada en sep. 2026. Actualízala si cambian las rutas de los proyectos o las
credenciales de la base de datos local.*
