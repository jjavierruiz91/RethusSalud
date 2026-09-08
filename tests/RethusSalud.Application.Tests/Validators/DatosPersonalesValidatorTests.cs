using RethusSalud.Application.Dtos;
using RethusSalud.Application.Validators;
using RethusSalud.Domain.Enums;
using Xunit;

namespace RethusSalud.Application.Tests.Validators;

public class DatosPersonalesValidatorTests
{
    private readonly DatosPersonalesValidator _validator = new();

    private static DatosPersonalesDto CrearDtoValido() => new()
    {
        TipoIdentificacion = TipoIdentificacion.CedulaCiudadania,
        NumeroIdentificacion = "123456789",
        LugarExpedicion = "Valledupar",
        Genero = Genero.Masculino,
        Nombres = "Franco",
        Apellidos = "Ovalle",
        PaisNacimientoId = 1,
        FechaNacimiento = DateOnly.FromDateTime(DateTime.UtcNow.AddYears(-30)),
        PaisResidenciaId = 1,
        DireccionDomicilio = "Calle 56",
        Celular = "3001234567",
        CorreoElectronico = "franco@example.com"
    };

    [Fact]
    public void Dto_valido_pasa_la_validacion()
    {
        var resultado = _validator.Validate(CrearDtoValido());
        Assert.True(resultado.IsValid);
    }

    [Fact]
    public void Correo_invalido_falla()
    {
        var dto = CrearDtoValido();
        dto.CorreoElectronico = "no-es-un-correo";

        var resultado = _validator.Validate(dto);
        Assert.False(resultado.IsValid);
    }

    [Fact]
    public void Fecha_nacimiento_futura_falla()
    {
        var dto = CrearDtoValido();
        dto.FechaNacimiento = DateOnly.FromDateTime(DateTime.UtcNow.AddDays(1));

        var resultado = _validator.Validate(dto);
        Assert.False(resultado.IsValid);
    }

    [Fact]
    public void Numero_identificacion_vacio_falla()
    {
        var dto = CrearDtoValido();
        dto.NumeroIdentificacion = "";

        var resultado = _validator.Validate(dto);
        Assert.False(resultado.IsValid);
    }
}
