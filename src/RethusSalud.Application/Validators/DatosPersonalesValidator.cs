using FluentValidation;
using RethusSalud.Application.Dtos;
using RethusSalud.Domain.Constants;

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

        RuleFor(x => x.DepartamentoNacimientoId)
            .NotNull().WithMessage("El departamento de nacimiento es obligatorio.")
            .When(x => x.PaisNacimientoId == Catalogos.PaisColombiaId);
        RuleFor(x => x.MunicipioNacimientoId)
            .NotNull().WithMessage("El municipio de nacimiento es obligatorio.")
            .When(x => x.PaisNacimientoId == Catalogos.PaisColombiaId);
        RuleFor(x => x.DepartamentoNacimientoTexto)
            .NotEmpty().WithMessage("El departamento o provincia de nacimiento es obligatorio.").MaximumLength(150)
            .When(x => x.PaisNacimientoId != Catalogos.PaisColombiaId);
        RuleFor(x => x.MunicipioNacimientoTexto)
            .NotEmpty().WithMessage("El municipio o ciudad de nacimiento es obligatorio.").MaximumLength(150)
            .When(x => x.PaisNacimientoId != Catalogos.PaisColombiaId);

        RuleFor(x => x.DepartamentoResidenciaId).NotNull().WithMessage("El departamento de residencia es obligatorio.");
        RuleFor(x => x.MunicipioResidenciaId).NotNull().WithMessage("El municipio de residencia es obligatorio.");

        RuleFor(x => x.FechaNacimiento)
            .LessThan(_ => DateOnly.FromDateTime(DateTime.UtcNow))
            .WithMessage("La fecha de nacimiento debe ser anterior a hoy.");
        RuleFor(x => x.DireccionDomicilio).NotEmpty().MaximumLength(250);
        RuleFor(x => x.Celular).NotEmpty().MaximumLength(20);
        RuleFor(x => x.CorreoElectronico).NotEmpty().EmailAddress();
    }
}
