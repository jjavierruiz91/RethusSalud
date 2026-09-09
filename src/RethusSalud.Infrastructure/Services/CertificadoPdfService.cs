using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using QRCoder;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using RethusSalud.Application.Dtos;
using RethusSalud.Application.Interfaces;
using RethusSalud.Domain.Entities;
using RethusSalud.Domain.Enums;

namespace RethusSalud.Infrastructure.Services;

public class CertificadoPdfService : ICertificadoPdfService
{
    private static readonly string[] Meses =
    {
        "enero", "febrero", "marzo", "abril", "mayo", "junio",
        "julio", "agosto", "septiembre", "octubre", "noviembre", "diciembre"
    };

    private readonly IConfiguration _configuration;
    private readonly IWebHostEnvironment _environment;

    public CertificadoPdfService(IConfiguration configuration, IWebHostEnvironment environment)
    {
        _configuration = configuration;
        _environment = environment;
    }

    public byte[] Generar(Solicitud solicitud, FirmantesDocumentoDto firmantes)
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

        var solicitante = solicitud.Solicitante;
        var academicos = solicitud.DatosAcademicos;
        var nombreCompleto = $"{solicitante.Nombres} {solicitante.Apellidos}".ToUpperInvariant();
        var identificacionTexto = DescribirTipoIdentificacion(solicitante.TipoIdentificacion);
        var profesionTexto = solicitud.Profesion.Nombre.ToUpperInvariant();
        var fechaResolucion = FechaLarga(solicitud.Consecutivo.Fecha);
        var rutaEscudo = Path.Combine(_environment.WebRootPath, "img", "institucional", "escudo_cesar_2024.png");
        var escudo = File.Exists(rutaEscudo) ? File.ReadAllBytes(rutaEscudo) : null;

        return Document.Create(container =>
        {
            container.Page(page =>
            {
                page.Size(216, 330, Unit.Millimetre); // Tamaño Oficio (216 x 330 mm)
                page.Margin(28);
                page.DefaultTextStyle(x => x.FontFamily("Arial Narrow").FontSize(11f).LineHeight(1.22f));

                page.Header().Table(table =>
                {
                    table.ColumnsDefinition(columns =>
                    {
                        columns.ConstantColumn(90);
                        columns.RelativeColumn();
                        columns.RelativeColumn();
                    });

                    table.Cell().RowSpan(2).Border(0.75f).BorderColor(Colors.Black).Padding(6)
                        .AlignMiddle().AlignCenter().Column(logo =>
                    {
                        logo.Spacing(3);
                        if (escudo is not null)
                        {
                            logo.Item().AlignCenter().Height(32).Image(escudo).FitArea();
                        }

                        logo.Item().AlignCenter().Text("GOBERNACIÓN DEL CESAR").FontSize(8).Bold();
                    });

                    table.Cell().ColumnSpan(2).Border(0.75f).BorderColor(Colors.Black).Padding(6)
                        .AlignMiddle().AlignCenter()
                        .Text("POR LA CUAL SE CONCEDE UNA AUTORIZACIÓN PARA EL EJERCICIO DE LA PROFESIÓN U OCUPACIÓN DEL ÁREA DE LA SALUD.")
                        .Bold().FontSize(11f);

                    table.Cell().Border(0.75f).BorderColor(Colors.Black).Padding(6)
                        .Text($"RESOLUCIÓN No {solicitud.Consecutivo.Numero}").FontSize(10f);

                    table.Cell().Border(0.75f).BorderColor(Colors.Black).Padding(6)
                        .Text($"FECHA: {fechaResolucion}").FontSize(10f);
                });

                page.Content().PaddingVertical(5).Column(col =>
                {
                    col.Spacing(6);

                    col.Item().Text(
                        "LA SECRETARÍA DE SALUD DEL DEPARTAMENTO DEL CESAR, en uso de sus atribuciones legales y en especial " +
                        "las conferidas en el Decreto 1352 de 2000 y,").Justify();

                    col.Item().AlignCenter().Text("CONSIDERANDO").Bold();

                    col.Item().Text(
                        "Que el artículo 1 del Decreto 1352 de 2000, modificó el artículo 1 del Decreto 1875 de 1994, estableciendo:").Justify();

                    col.Item().PaddingLeft(20).Text(
                        "\"ARTICULO 1o. Modifíquese el artículo 1o. del Decreto 1875 de 1994, el cual quedará así: Competencia " +
                        "para la autorización del ejercicio profesional. Las Direcciones Departamentales de Salud y la Secretaría " +
                        "Distrital de Salud de Santa Fe Bogotá, expedirán el acto administrativo mediante el cual se autorice el " +
                        "ejercicio de las profesiones del área de la salud en todo el territorio nacional\".").Italic().Justify();

                    col.Item().Text(text =>
                    {
                        text.Justify();
                        text.Span("Que ");
                        text.Span(nombreCompleto).Bold();
                        text.Span($", identificado(a) con {identificacionTexto} No. {solicitante.NumeroIdentificacion} expedida en {solicitante.LugarExpedicion}, solicitó ante esta Secretaría de Salud del Departamento del Cesar, autorización del ejercicio profesional de ");
                        text.Span(profesionTexto).Bold();
                        text.Span(" que le otorgó ");
                        text.Span(academicos?.NombreInstitucion ?? "-").Bold();
                        text.Span($", el {academicos?.FechaGrado.ToString("dd/MM/yyyy")}.");
                    });

                    if (academicos is not null && academicos.OrigenTitulo == OrigenTitulo.Extranjero
                        && !string.IsNullOrWhiteSpace(academicos.NumeroConvalidacion) && academicos.FechaConvalidacion.HasValue)
                    {
                        col.Item().Text(text =>
                        {
                            text.Justify();
                            text.Span("Que ");
                            text.Span(nombreCompleto).Bold();
                            text.Span(", convalidó en Colombia el título de ");
                            text.Span(profesionTexto).Bold();
                            text.Span($", otorgado el {academicos.FechaGrado:dd/MM/yyyy}, por la ");
                            text.Span(academicos.NombreInstitucion).Bold();
                            text.Span($", convalidado mediante la Resolución número {academicos.NumeroConvalidacion} del {academicos.FechaConvalidacion:dd 'de' MMMM 'del' yyyy}, proferido por el MINISTERIO DE EDUCACIÓN NACIONAL DE LA REPÚBLICA DE COLOMBIA.");
                        });
                    }

                    col.Item().Text(
                        "Que estudiada la documentación presentada por el solicitante, esta cumple con los requisitos establecidos en las normas legales vigentes.").Justify();

                    col.Item().Text("Que en mérito de lo expuesto,");

                    col.Item().AlignCenter().Text("RESUELVE").Bold();

                    col.Item().Text(text =>
                    {
                        text.Justify();
                        text.Span("ARTÍCULO PRIMERO. ").Bold();
                        text.Span("Autorizar a ");
                        text.Span(nombreCompleto).Bold();
                        text.Span($", identificado(a) con {identificacionTexto} No. {solicitante.NumeroIdentificacion} expedida en {solicitante.LugarExpedicion}, para el ejercicio profesional en todo el Territorio Nacional de ");
                        text.Span(profesionTexto).Bold();
                        text.Span(".");
                    });

                    col.Item().Text(text =>
                    {
                        text.Justify();
                        text.Span("ARTÍCULO SEGUNDO. ").Bold();
                        text.Span("Notificar a ");
                        text.Span(nombreCompleto).Bold();
                        text.Span($", identificado(a) con {identificacionTexto} No. {solicitante.NumeroIdentificacion} expedida en {solicitante.LugarExpedicion}, del contenido de la presente Resolución, en los términos del artículo 67 de la Ley 1437 de 2011, Código de Procedimiento Administrativo y de lo Contencioso Administrativo.");
                    });

                    col.Item().Text(text =>
                    {
                        text.Justify();
                        text.Span("ARTÍCULO TERCERO. ").Bold();
                        text.Span("Esta Resolución será inscrita en el Registro Único Nacional del Talento Humano en Salud – RETHUS, para ejercer la profesión en todo el Territorio Nacional.");
                    });

                    col.Item().PaddingTop(2).AlignCenter().Text("COMUNÍQUESE, NOTIFÍQUESE Y CÚMPLASE").Bold();

                    col.Item().PaddingTop(4).Text($"Dado en Valledupar a los {fechaResolucion}");

                    col.Item().PaddingTop(5).AlignCenter().Column(jefe =>
                    {
                        jefe.Spacing(1);
                        var imagenJefe = CargarImagenFirma(firmantes.Jefe?.FirmaUrl);
                        if (imagenJefe is not null)
                        {
                            jefe.Item().AlignCenter().Height(55).Image(imagenJefe).FitArea();
                        }

                        jefe.Item().AlignCenter().Text(firmantes.Jefe?.Nombre ?? "").Bold().FontSize(10f);
                        jefe.Item().AlignCenter().Text(firmantes.Jefe?.Cargo ?? "Secretaria de Salud Departamento del Cesar").FontSize(9f);
                    });

                    col.Item().PaddingTop(5).Table(table =>
                    {
                        table.ColumnsDefinition(columns =>
                        {
                            columns.ConstantColumn(105);
                            columns.RelativeColumn();
                            columns.ConstantColumn(55);
                        });

                        void Fila(string etiqueta, FirmanteDto? firmante)
                        {
                            table.Cell().Border(0.75f).BorderColor(Colors.Grey.Lighten1).Padding(4)
                                .Text(etiqueta).Bold();

                            table.Cell().Border(0.75f).BorderColor(Colors.Grey.Lighten1).Padding(4)
                                .Text(firmante is not null
                                    ? $"{firmante.Nombre}{(string.IsNullOrWhiteSpace(firmante.Cargo) ? "" : $", {firmante.Cargo}")}."
                                    : "Sin asignar.");

                            table.Cell().Border(0.75f).BorderColor(Colors.Grey.Lighten1).Padding(2)
                                .AlignMiddle().AlignCenter().Element(celda =>
                            {
                                var imagen = CargarImagenFirma(firmante?.FirmaUrl);
                                if (imagen is not null)
                                {
                                    celda.Height(16).Image(imagen).FitArea();
                                }
                            });
                        }

                        Fila("Proyectó:", firmantes.Proyecto);
                        Fila("Aprobó:", firmantes.Aprobo);
                        Fila("Revisó:", firmantes.Reviso);
                    });

                    col.Item().PaddingTop(3).Text(
                        "Los arriba firmantes declaramos que hemos revisado el documento, cuyo contenido se encuentra ajustado a " +
                        "las disposiciones legales vigentes, bajo nuestra responsabilidad lo presentamos para firma.").FontSize(8f).Italic().Justify();
                });

                page.Footer().PaddingTop(10).Column(col =>
                {
                    col.Item().Row(row =>
                    {
                        row.RelativeItem().Column(c =>
                        {
                            c.Item().Text("COORDINACIÓN DE PRESTACIÓN Y DESARROLLO DE SERVICIOS DE SALUD").FontSize(8);
                            c.Item().Text("Transversal 18 No 19-65 Valledupar – Rethus@saludcesar.gov.co").FontSize(8);
                        });
                        row.ConstantItem(55).Column(c =>
                        {
                            c.Item().AlignRight().Width(48).Height(48).Image(qrPng);
                            c.Item().AlignRight().Text("Verificar folio").FontSize(7.5f);
                        });
                    });
                });
            });
        }).GeneratePdf();
    }

    internal byte[]? CargarImagenFirma(string? firmaUrl)
    {
        if (string.IsNullOrWhiteSpace(firmaUrl))
        {
            return null;
        }

        var rutaRelativa = firmaUrl.Split('?')[0].TrimStart('/').Replace('/', Path.DirectorySeparatorChar);
        var rutaCompleta = Path.Combine(_environment.WebRootPath, rutaRelativa);
        return File.Exists(rutaCompleta) ? File.ReadAllBytes(rutaCompleta) : null;
    }

    private static string FechaLarga(DateOnly fecha) =>
        $"{fecha.Day} de {Meses[fecha.Month - 1]} del {fecha.Year}";

    private static string DescribirTipoIdentificacion(TipoIdentificacion tipo) => tipo switch
    {
        TipoIdentificacion.CedulaCiudadania => "cédula de ciudadanía",
        TipoIdentificacion.CedulaExtranjeria => "cédula de extranjería",
        TipoIdentificacion.PasaporteExtranjero => "pasaporte",
        TipoIdentificacion.PermisoProteccionTemporal => "permiso por protección temporal",
        _ => "identificación"
    };
}
