using Microsoft.AspNetCore.Http;
using rethus_backend.Data;
using rethus_backend.Repository.IRepository;
using System.IdentityModel.Tokens.Jwt;
using System.Threading.Tasks;
using System.IO;

public class JwtExpirationValidationMiddleware
{
    private readonly RequestDelegate _next;

    private readonly IServiceScopeFactory _serviceScopeFactory;

    public JwtExpirationValidationMiddleware(
        RequestDelegate next,
        IServiceScopeFactory serviceScopeFactory
    )
    {
        _next = next;
        _serviceScopeFactory = serviceScopeFactory;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        // Obtener el token del header Authorization
        var authorizationHeader = context.Request.Headers["Authorization"].FirstOrDefault();

        if (
            !string.IsNullOrEmpty(authorizationHeader)
            && authorizationHeader.StartsWith("Bearer ", StringComparison.OrdinalIgnoreCase)
        )
        {
            var token = authorizationHeader.Substring("Bearer ".Length).Trim();

            using (var scope = _serviceScopeFactory.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

                try
                {
                    // Validar que el token no haya expirado
                    var handler = new JwtSecurityTokenHandler();
                    var jwtToken = handler.ReadJwtToken(token);

                    var userId = jwtToken.Payload["userId"]?.ToString(); // Extraemos el userId

                    if (string.IsNullOrEmpty(userId))
                    {
                        context.Response.StatusCode = StatusCodes.Status400BadRequest;
                        await context.Response.WriteAsync("Token no contiene el userId.");
                        return;
                    }

                    var userInfo = dbContext.Users.FirstOrDefault(user => user.UserId == userId);

                    // Obtener la fecha de expiración del token
                    var expirationDate = jwtToken.ValidTo;

                    if (expirationDate < DateTime.UtcNow)
                    {
                        context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                        await context.Response.WriteAsync(
                            "El token ha expirado. Debes iniciar sesión nuevamente."
                        );
                        return;
                    }

                    // Almacenar la información del usuario en HttpContext
                    context.Items["User"] = userInfo;

                    // Registrar la actividad del usuario con información adicional
                    var clientIp = context.Connection.RemoteIpAddress?.ToString();
                    var userAgent = context.Request.Headers["User-Agent"].FirstOrDefault();
                    var startTime = DateTime.UtcNow;

                    // Procesar la solicitud
                    await _next(context);

                    var duration = DateTime.UtcNow - startTime;
                    var responseStatusCode = context.Response.StatusCode;

                    var logEntry =
                        $"{TimeZoneInfo.ConvertTimeBySystemTimeZoneId(DateTime.UtcNow, "America/Bogota"):yyyy-MM-dd HH:mm:ss} | UserId: {userId} | Role: {userInfo?.roles} | IP: {clientIp} | User-Agent: {userAgent} | Request: {context.Request.Method} {context.Request.Path} | Status: {responseStatusCode} | Duration: {duration.TotalMilliseconds}ms";

                    // Crear la carpeta de logs agrupada por año
                    var logsDirectory = Path.Combine(
                        Directory.GetCurrentDirectory(),
                        "logs",
                        DateTime.UtcNow.Year.ToString()
                    );
                    Directory.CreateDirectory(logsDirectory);

                    // Ruta del archivo de log
                    var logFilePath = Path.Combine(logsDirectory, "UserActivityLog.txt");

                    // Guardar el log en el archivo
                    await File.AppendAllTextAsync(logFilePath, logEntry + Environment.NewLine);
                }
                catch (Exception ex)
                {
                    context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                    await context.Response.WriteAsync("Token inválido o mal formado.");
                    return;
                }
            }
        }
        else
        {
            // Si no está autenticado, pasar al siguiente middleware
            await _next(context);
        }
    }
}
