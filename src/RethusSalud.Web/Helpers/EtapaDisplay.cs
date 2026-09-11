using RethusSalud.Domain.Enums;

namespace RethusSalud.Web.Helpers;

public static class EtapaDisplay
{
    public static string Texto(EtapaSolicitud etapa) => etapa switch
    {
        EtapaSolicitud.Etapa1 => "Etapa 1",
        EtapaSolicitud.Etapa2 => "Etapa 2",
        EtapaSolicitud.Etapa3 => "Etapa 3",
        EtapaSolicitud.Inventario => "Inventario",
        _ => etapa.ToString()
    };

    public static int Numero(EtapaSolicitud etapa) => (int)etapa;
}
