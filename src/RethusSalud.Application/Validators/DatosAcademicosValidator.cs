using FluentValidation;
using RethusSalud.Application.Dtos;

namespace RethusSalud.Application.Validators;

public class DatosAcademicosValidator : AbstractValidator<DatosAcademicosDto>
{
    public DatosAcademicosValidator()
    {
        RuleFor(x => x.TipoPrograma).NotEmpty().MaximumLength(150);
        RuleFor(x => x.PaisInstitucionId).GreaterThan(0);
        RuleFor(x => x.NombreInstitucion).NotEmpty().MaximumLength(250);
        RuleFor(x => x.NombrePrograma).NotEmpty().MaximumLength(150);
        RuleFor(x => x.FechaGrado)
            .LessThanOrEqualTo(_ => DateOnly.FromDateTime(DateTime.UtcNow))
            .WithMessage("La fecha de grado no puede ser futura.");
    }
}
