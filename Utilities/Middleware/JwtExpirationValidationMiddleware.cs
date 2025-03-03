using Microsoft.AspNetCore.Http;
using rethus_backend.Data;
using rethus_backend.Repository.IRepository;
using System.IdentityModel.Tokens.Jwt;
using System.Threading.Tasks;

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
                        // Si no se encuentra el userId en el token, retornar un error 400
                        context.Response.StatusCode = StatusCodes.Status400BadRequest;
                        await context.Response.WriteAsync("Token no contiene el userId.");
                        return;
                    }

                    var userInfo = dbContext.Users.FirstOrDefault(user => user.UserId == userId);

                    // Obtener la fecha de expiración del token
                    var expirationDate = jwtToken.ValidTo;

                    if (expirationDate < DateTime.UtcNow)
                    {
                        // Si el token ha expirado, retornar un error 401
                        context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                        await context.Response.WriteAsync(
                            "El token ha expirado. Debes iniciar sesión nuevamente."
                        );
                        return;
                    }

                    // Almacenar la información del usuario en HttpContext
                    context.Items["User"] = userInfo;
                }
                catch (Exception ex)
                {
                    // Si el token no es válido, también retornar un error 401
                    context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                    await context.Response.WriteAsync("Token inválido o mal formado.");
                    return;
                }
            }
        }

        // Si está autenticado, pasar al siguiente middleware
        await _next(context);
    }
}
