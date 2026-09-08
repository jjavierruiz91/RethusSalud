using RethusSalud.Application.Dtos;
using RethusSalud.Application.Validators;
using RethusSalud.Domain.Enums;
using Xunit;

namespace RethusSalud.Application.Tests.Validators;

public class DatosAcademicosValidatorTests
{
    private readonly DatosAcademicosValidator _validator = new();

    private static DatosAcademicosDto CrearDtoValido() => new()
    {
        OrigenTitulo = OrigenTitulo.Local,
        TipoInstitucion = TipoInstitucion.EducacionSuperior,
        TipoPrograma = "Universitario",
        PaisInstitucionId = 1,
        NombreInstitucion = "Universidad Popular del Cesar",
        NombrePrograma = "Psicologia",
        FechaGrado = DateOnly.FromDateTime(DateTime.UtcNow.AddYears(-1))
    };

    [Fact]
    public void Dto_valido_pasa_la_validacion()
    {
        var resultado = _validator.Validate(CrearDtoValido());
        Assert.True(resultado.IsValid);
    }

    [Fact]
    public void Fecha_grado_futura_falla()
    {
        var dto = CrearDtoValido();
        dto.FechaGrado = DateOnly.FromDateTime(DateTime.UtcNow.AddDays(1));

        var resultado = _validator.Validate(dto);
        Assert.False(resultado.IsValid);
    }

    [Fact]
    public void Nombre_institucion_vacio_falla()
    {
        var dto = CrearDtoValido();
        dto.NombreInstitucion = "";

        var resultado = _validator.Validate(dto);
        Assert.False(resultado.IsValid);
    }
}
