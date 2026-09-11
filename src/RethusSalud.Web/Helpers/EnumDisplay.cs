using System.ComponentModel.DataAnnotations;

namespace RethusSalud.Web.Helpers;

public static class EnumDisplay
{
    public static string Texto(Enum valor)
    {
        var campo = valor.GetType().GetField(valor.ToString());
        var atributo = campo?.GetCustomAttributes(typeof(DisplayAttribute), false).FirstOrDefault() as DisplayAttribute;
        return atributo?.Name ?? valor.ToString();
    }
}
