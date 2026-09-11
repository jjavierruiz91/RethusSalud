using System.ComponentModel.DataAnnotations;

namespace RethusSalud.Domain.Enums;

public enum GrupoEtnico
{
    [Display(Name = "Ninguna de las anteriores")]
    NingunaDeLasAnteriores = 0,

    [Display(Name = "Indígena")]
    Indigena = 1,

    [Display(Name = "Palenquero")]
    Palenquero = 2,

    [Display(Name = "Rom")]
    Rom = 3,

    [Display(Name = "Afrodescendiente")]
    AfroDescendiente = 4,

    [Display(Name = "Raizal")]
    Raizal = 5
}
