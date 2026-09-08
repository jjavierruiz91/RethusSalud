using Microsoft.AspNetCore.Identity;

namespace RethusSalud.Infrastructure.Identity;

public class ApplicationUser : IdentityUser
{
    public string NombreCompleto { get; set; } = string.Empty;
    public bool Activo { get; set; } = true;
}
