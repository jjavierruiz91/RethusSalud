using FluentValidation;
using RethusSalud.Application.Dtos;

namespace RethusSalud.Application.Validators;

public class DatosPersonalesValidator : AbstractValidator<DatosPersonalesDto>
{
    public DatosPersonalesValidator()
    {
        RuleFor(x => x.NumeroIdentificacion).NotEmpty().MaximumLength(30);
        RuleFor(x => x.LugarExpedicion).NotEmpty().MaximumLength(150);
        RuleFor(x => x.Nombres).NotEmpty().MaximumLength(150);
        RuleFor(x => x.Apellidos).NotEmpty().MaximumLength(150);
        RuleFor(x => x.PaisNacimientoId).GreaterThan(0);
        RuleFor(x => x.PaisResidenciaId).GreaterThan(0);
        RuleFor(x => x.FechaNacimiento)
            .LessThan(_ => DateOnly.FromDateTime(DateTime.UtcNow))
            .WithMessage("La fecha de nacimiento debe ser anterior a hoy.");
        RuleFor(x => x.DireccionDomicilio).NotEmpty().MaximumLength(250);
        RuleFor(x => x.Celular).NotEmpty().MaximumLength(20);
        RuleFor(x => x.CorreoElectronico).NotEmpty().EmailAddress();
    }
}
