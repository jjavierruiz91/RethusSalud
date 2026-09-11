namespace RethusSalud.Web.Models.Dashboard;

public class DonutSegmentoViewModel
{
    public string Color { get; set; } = string.Empty;
    public double DashA { get; set; }
    public double DashB { get; set; }
    public double Offset { get; set; }
    public string Label { get; set; } = string.Empty;
    public int Cantidad { get; set; }
}

public class BarraItemViewModel
{
    public string Label { get; set; } = string.Empty;
    public int Cantidad { get; set; }
}
