# Especificación de despliegue — ASP.NET Core + IIS + SQL Server (Windows on-premise)

Guía reutilizable, basada en un despliegue real. Aplica a cualquier proyecto ASP.NET Core
(MVC o Razor Pages) que se publique en IIS sobre Windows Server, con SQL Server como motor
de base de datos. Reemplaza los valores entre `< >` por los del proyecto específico.

## 1. Prerrequisitos del servidor

- [ ] **.NET Hosting Bundle** instalado (no solo el runtime) — instala `AspNetCoreModuleV2`,
      que conecta IIS con el proceso .NET. Sin esto el sitio no arranca. Reiniciar IIS después
      de instalarlo (`net stop was /y && net start w3svc`).
- [ ] **SQL Server** accesible desde el servidor de IIS. Más simple si está en la misma
      máquina (evita configurar TCP/IP remoto, SQL Browser, firewall).
- [ ] **Application Pool** dedicado, en modo **".NET CLR version: No Managed Code"**.

## 2. Publicar y copiar

```bash
dotnet publish <Proyecto.Web.csproj> -c Release -o <carpeta_publicacion>
```

Copiar esa carpeta a `C:\inetpub\wwwroot\<NombreDelSitio>\`.

**No todo cambio requiere publicar de nuevo:**
- Cambio de configuración (`appsettings.*.json`) → copiar solo ese archivo de texto, reciclar el Application Pool.
- Cambio de código en un solo archivo `.cs` → recompilar y reemplazar solo el `.dll` afectado, no toda la carpeta.
- Cambio de assets estáticos (imágenes, PDFs de referencia, etc.) → copiar solo esos archivos.

## 3. Plantilla de `appsettings.Production.json`

```json
{
  "UseHttpsRedirection": true,
  "ConnectionStrings": {
    "DefaultConnection": "Server=<SERVIDOR>\\<INSTANCIA>;Database=<NombreBD>;User Id=<usuario>;Password=<password>;TrustServerCertificate=True;"
  },
  "SeedAdmin": {
    "Email": "",
    "Password": ""
  },
  "DomainWebUrl": "https://<dominio-o-ip-publica>",
  "Smtp": {
    "Host": "",
    "Port": "587",
    "Usuario": "",
    "Password": "",
    "Remitente": ""
  },
  "Storage": {
    "BasePath": ""
  }
}
```

Puntos clave de cada campo:
- **`ConnectionStrings:DefaultConnection`** — el error más común es **olvidar el prefijo `Server=`** antes del nombre de instancia. Sin eso, .NET no puede interpretar la cadena y la app truena al arrancar con un `500.30` genérico.
- **`DomainWebUrl`** (o el equivalente en tu proyecto) — si el sistema genera enlaces públicos (QR, correos, links de verificación), **debe ser la IP/dominio público**, nunca uno privado (`192.168.x.x`) — de lo contrario esos enlaces no funcionan para nadie fuera de la red interna.
- **`UseHttpsRedirection`** — si el sitio todavía no tiene certificado SSL, ponlo en `false` temporalmente (ver sección 5). Si el código no tiene este flag como configurable, agrégalo — es un cambio de una línea y evita que el sitio quede inutilizable mientras se consigue el certificado.
- **`Storage:BasePath`** (o donde sea que el proyecto guarde archivos subidos por usuarios) — **debe apuntar fuera de la carpeta publicada**, para no perder esos archivos en el próximo despliegue.

## 4. Errores típicos y su causa real

| Síntoma | Causa | Cómo confirmarlo |
|---|---|---|
| `500.30 - app failed to start` | Cadena de conexión mal formada (falta `Server=`), o la app no puede conectar a la BD al arrancar | Probar la cadena de conexión por separado con `sqlcmd`/SSMS antes de asumir que es un bug de la app |
| Timeouts intermitentes de SQL | Base de datos en modo `RESTRICTED_USER` (típico tras un backup/restore) | `SELECT name, user_access_desc FROM sys.databases` |
| El sitio nunca carga / redirige en bucle | `UseHttpsRedirection()` forzado sin binding HTTPS en IIS | Revisar bindings del sitio en IIS Manager |
| Error genérico sin detalle | Sin logging habilitado | Activar `stdoutLogEnabled="true"` en `web.config` temporalmente, o revisar Visor de eventos → Aplicación → origen `IIS AspNetCore Module V2` |
| Archivos/fotos desaparecen tras cada despliegue | Carpetas que la app crea en tiempo de ejecución (uploads, logs, imágenes de perfil) viven dentro de la carpeta publicada, y un `robocopy /MIR` las borra | Identificar TODAS las carpetas de escritura en tiempo de ejecución antes del primer despliegue |
| Un archivo/documento falla con error crudo | El código no maneja el caso de archivo faltante (`FileNotFoundException` sin capturar) | Envolver la lectura de archivos en try/catch y devolver un 404 amigable en vez de dejar que la excepción llegue cruda |

## 5. Checklist de copia (robocopy)

Antes del primer despliegue, identifica todas las carpetas que la aplicación escribe en
tiempo de ejecución (uploads de usuarios, logs, imágenes generadas, etc.) y excluye siempre
esas carpetas de cualquier sincronización posterior:

```bash
robocopy <origen> <destino> /MIR /XD "<carpeta_uploads>" "logs" "App_Data" "<otras_carpetas_runtime>"
```

`/MIR` espeja el contenido (borra en destino lo que no está en origen) — sin `/XD`, cualquier
carpeta creada en tiempo de ejecución que no exista en tu build local se borra en cada
despliegue.

## 6. Permisos NTFS

El Application Pool necesita permiso de lectura (y escritura, si la app guarda archivos ahí)
en la carpeta del sitio:

1. Clic derecho en la carpeta → Propiedades → pestaña **Seguridad**.
2. Verificar que `IIS_IUSRS` (o el identity específico `IIS AppPool\<NombreDelPool>`) tenga
   al menos "Lectura y ejecución"; "Escritura" en las carpetas donde la app escribe.
3. **Ojo:** el permiso de "Compartir" (pestaña Compartir) es para acceso por red — no es lo
   mismo que el permiso NTFS que usa el proceso de IIS corriendo localmente en el servidor.

## 7. Checklist final antes de dar el sitio por listo

- [ ] Cadena de conexión probada de forma independiente
- [ ] Estado de la base de datos confirmado (`MULTI_USER`)
- [ ] Decisión tomada sobre HTTPS (certificado real, o `UseHttpsRedirection: false` temporal)
- [ ] Carpetas de escritura en tiempo de ejecución identificadas y protegidas con `/XD`
- [ ] Permisos NTFS verificados para el Application Pool
- [ ] `DomainWebUrl` (o equivalente) apuntando a la URL pública, no privada
- [ ] Manejo de errores agregado para archivos/recursos que puedan faltar
- [ ] Logging detallado desactivado (solo se activa temporalmente para diagnosticar)

---
*Documento generado a partir de un despliegue real (RethusSalud, sep. 2026). Adaptar nombres
de carpetas y campos de configuración al proyecto específico.*
