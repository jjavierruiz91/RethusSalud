using Microsoft.AspNetCore.Http;

namespace RethusSalud.Web.Models.Account;

public class ActualizarPerfilViewModel
{
    public string? Cargo { get; set; }
    public IFormFile? Firma { get; set; }
}
