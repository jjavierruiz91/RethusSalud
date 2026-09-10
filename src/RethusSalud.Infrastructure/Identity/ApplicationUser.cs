using Microsoft.AspNetCore.Identity;

namespace RethusSalud.Infrastructure.Identity;

public class ApplicationUser : IdentityUser
{
    public string NombreCompleto { get; set; } = string.Empty;
    public bool Activo { get; set; } = true;
    public string? FotoUrl { get; set; }
    public string? Cargo { get; set; }
    public string? FirmaUrl { get; set; }
    public string? Telefono { get; set; }
    public string? NumeroIdentificacion { get; set; }
}
