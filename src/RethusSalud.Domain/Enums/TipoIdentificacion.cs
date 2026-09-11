using System.ComponentModel.DataAnnotations;

namespace RethusSalud.Domain.Enums;

public enum TipoIdentificacion
{
    [Display(Name = "Cédula de ciudadanía")]
    CedulaCiudadania = 1,

    [Display(Name = "Cédula de extranjería")]
    CedulaExtranjeria = 2,

    [Display(Name = "Pasaporte extranjero")]
    PasaporteExtranjero = 3,

    [Display(Name = "Permiso de protección temporal")]
    PermisoProteccionTemporal = 4
}
