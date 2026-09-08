using Microsoft.Extensions.Configuration;
using QRCoder;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using RethusSalud.Application.Interfaces;
using RethusSalud.Domain.Entities;
using RethusSalud.Domain.Enums;

namespace RethusSalud.Infrastructure.Services;

public class CertificadoPdfService : ICertificadoPdfService
{
    private readonly IConfiguration _configuration;

    public CertificadoPdfService(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public byte[] Generar(Solicitud solicitud)
    {
        if (solicitud.Consecutivo is null)
        {
            throw new InvalidOperationException("La solicitud no tiene consecutivo asignado.");
        }

        var dominio = _configuration["DomainWebUrl"] ?? "https://localhost:5299";
        var urlVerificacion = $"{dominio.TrimEnd('/')}/Consulta/VerificarFolio?numero={Uri.EscapeDataString(solicitud.Consecutivo.Numero)}";

        using var qrGenerator = new QRCodeGenerator();
        using var qrData = qrGenerator.CreateQrCode(urlVerificacion, QRCodeGenerator.ECCLevel.Q);
        var qrPng = new PngByteQRCode(qrData).GetGraphic(10);

        var tituloTramite = solicitud.TipoTramite == TipoTramite.SSO
            ? "Certificado de servicio social obligatorio"
            : "Registro unico nacional de talento humano en salud";

        return Document.Create(container =>
        {
            container.Page(page =>
            {
                page.Size(PageSizes.Letter);
                page.Margin(40);
                page.DefaultTextStyle(x => x.FontSize(11));

                page.Header().Column(col =>
                {
                    col.Item().AlignCenter().Text("Secretaria de salud departamental del cesar").FontSize(16).Bold();
                    col.Item().AlignCenter().Text(tituloTramite).FontSize(12);
                });

                page.Content().PaddingVertical(30).Column(col =>
                {
                    col.Spacing(10);
                    col.Item().Text(text =>
                    {
                        text.Span("Se certifica que ");
                        text.Span($"{solicitud.Solicitante.Nombres} {solicitud.Solicitante.Apellidos}").Bold();
                        text.Span($", identificado(a) con {DescribirTipoIdentificacion(solicitud.Solicitante.TipoIdentificacion)} numero {solicitud.Solicitante.NumeroIdentificacion}, se encuentra registrado(a) como ");
                        text.Span(solicitud.Profesion.Nombre).Bold();
                        text.Span(" bajo el folio ");
                        text.Span(solicitud.Consecutivo.Numero).Bold();
                        text.Span(".");
                    });

                    if (solicitud.DatosAcademicos is not null)
                    {
                        col.Item().Text(text =>
                        {
                            text.Span("Titulo obtenido en ");
                            text.Span(solicitud.DatosAcademicos.NombreInstitucion).Bold();
                            text.Span(" el ");
                            text.Span(solicitud.DatosAcademicos.FechaGrado.ToString("dd/MM/yyyy"));
                            text.Span(".");
                        });
                    }
                });

                page.Footer().PaddingTop(30).Row(row =>
                {
                    row.RelativeItem().Column(c =>
                    {
                        c.Item().AlignCenter().PaddingBottom(4).Text("_____________________________");
                        c.Item().AlignCenter().Text("Firma funcionario autorizado").FontSize(9);
                    });
                    row.RelativeItem().Column(c =>
                    {
                        c.Item().AlignCenter().Width(60).Height(60).Border(2)
                            .BorderColor(Colors.Blue.Medium).Padding(4)
                            .AlignMiddle().AlignCenter().Text("SELLO\nOFICIAL").FontSize(8).AlignCenter();
                    });
                    row.RelativeItem().Column(c =>
                    {
                        c.Item().AlignCenter().Width(70).Height(70).Image(qrPng);
                        c.Item().AlignCenter().Text("Verificar folio").FontSize(8);
                    });
                });
            });
        }).GeneratePdf();
    }

    private static string DescribirTipoIdentificacion(TipoIdentificacion tipo) => tipo switch
    {
        TipoIdentificacion.CedulaCiudadania => "cedula de ciudadania",
        TipoIdentificacion.CedulaExtranjeria => "cedula de extranjeria",
        TipoIdentificacion.PasaporteExtranjero => "pasaporte",
        TipoIdentificacion.PermisoProteccionTemporal => "permiso de proteccion temporal",
        _ => "identificacion"
    };
}
