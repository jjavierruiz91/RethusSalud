namespace RethusSalud.Web.Models.Admin;

public class CiudadanoViewModel
{
    public string Id { get; set; } = string.Empty;
    public string Nombre { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string? NumeroIdentificacion { get; set; }
    public bool Activo { get; set; }
}
