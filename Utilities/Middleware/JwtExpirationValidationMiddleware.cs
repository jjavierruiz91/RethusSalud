using Microsoft.AspNetCore.Http;
using System.IdentityModel.Tokens.Jwt;
using System.Threading.Tasks;

public class JwtExpirationValidationMiddleware
{
    private readonly RequestDelegate _next;

    public JwtExpirationValidationMiddleware(RequestDelegate next)
    {
        _next = next;
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

            try
            {
                // Validar que el token no haya expirado
                var handler = new JwtSecurityTokenHandler();
                var jwtToken = handler.ReadJwtToken(token);

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
            }
            catch (Exception ex)
            {
                // Si el token no es válido, también retornar un error 401
                context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                await context.Response.WriteAsync("Token inválido o mal formado.");
                return;
            }
        }

        // Si está autenticado, pasar al siguiente middleware
        await _next(context);
    }
}
