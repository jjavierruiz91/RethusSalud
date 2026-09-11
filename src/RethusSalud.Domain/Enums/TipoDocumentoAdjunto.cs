using System.ComponentModel.DataAnnotations;

namespace RethusSalud.Domain.Enums;

public enum TipoDocumentoAdjunto
{
    [Display(Name = "Cédula ampliada")]
    CedulaAmpliada = 1,

    [Display(Name = "Diploma de grado")]
    DiplomaGrado = 2,

    [Display(Name = "Acta de grado")]
    ActaGrado = 3,

    [Display(Name = "Tarjeta profesional")]
    TarjetaProfesional = 4
}
