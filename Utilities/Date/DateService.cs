using System.Globalization;

namespace rethus_backend.Utilities.Email.DateService
{
    public class DateService
    {
        public static string FormatLongDate(DateTime fecha)
        {
            return fecha.ToString("dd 'de' MMMM 'del' yyyy", new CultureInfo("es-ES"));
        }
    }
}
