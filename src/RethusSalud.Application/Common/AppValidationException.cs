namespace RethusSalud.Application.Common;

public class AppValidationException : Exception
{
    public IReadOnlyList<string> Errors { get; }

    public AppValidationException(IEnumerable<string> errors) : base("Se encontraron errores de validacion.")
    {
        Errors = errors.ToList();
    }
}
