using System.ComponentModel.DataAnnotations;

namespace RethusSalud.Domain.Enums;

public enum TipoInstitucion
{
    [Display(Name = "Educación superior")]
    EducacionSuperior = 1,

    [Display(Name = "Educación para el trabajo y desarrollo humano")]
    EducacionParaElTrabajo = 2
}
