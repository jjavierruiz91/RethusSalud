using RethusSalud.Domain.Enums;

namespace RethusSalud.Web.Helpers;

public static class EstadoDisplay
{
    public static string Texto(EstadoSolicitud estado) => estado switch
    {
        EstadoSolicitud.Borrador => "Borrador",
        EstadoSolicitud.EnProceso => "En proceso",
        EstadoSolicitud.Aprobado => "Aprobado",
        EstadoSolicitud.Rechazado => "Rechazado",
        _ => estado.ToString()
    };

    public static readonly EstadoSolicitud[] FiltrablesPorFuncionario =
    {
        EstadoSolicitud.EnProceso, EstadoSolicitud.Aprobado, EstadoSolicitud.Rechazado
    };
}
