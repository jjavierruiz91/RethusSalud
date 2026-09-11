using FluentValidation;
using RethusSalud.Application.Dtos;
using RethusSalud.Domain.Enums;

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
        RuleFor(x => x.NumeroConvalidacion)
            .NotEmpty()
            .WithMessage("El numero de convalidacion es obligatorio cuando el origen del titulo es extranjero.")
            .When(x => x.OrigenTitulo == OrigenTitulo.Extranjero);
    }
}
