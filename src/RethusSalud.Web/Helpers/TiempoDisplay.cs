namespace RethusSalud.Web.Helpers;

public static class TiempoDisplay
{
    public static string Transcurrido(DateTime desde)
    {
        var span = DateTime.UtcNow - desde;
        var dias = span.Days;
        var horas = span.Hours;
        var minutos = span.Minutes;

        if (dias > 0)
        {
            return $"{dias} d {horas} h {minutos} min";
        }

        if (horas > 0)
        {
            return $"{horas} h {minutos} min";
        }

        return $"{minutos} min";
    }
}
