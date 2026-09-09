namespace RethusSalud.Web.Models.Shared;

public class PaginacionViewModel
{
    public int PaginaActual { get; set; }
    public int TotalPaginas { get; set; }
    public int TotalRegistros { get; set; }
    public string Accion { get; set; } = "";
    public Dictionary<string, string?> RouteValues { get; set; } = new();
}
