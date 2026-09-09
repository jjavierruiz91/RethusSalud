using FluentValidation;
using RethusSalud.Application.Common;
using RethusSalud.Application.Dtos;
using RethusSalud.Application.Interfaces;
using RethusSalud.Domain.Entities;
using RethusSalud.Domain.Enums;
using DomainException = RethusSalud.Domain.Exceptions.DomainException;

namespace RethusSalud.Application.Services;

public class SolicitudService
{
    private readonly ISolicitudRepository _solicitudes;
    private readonly ICatalogoRepository _catalogos;
    private readonly IConsecutivoGenerator _consecutivos;
    private readonly IEmailSender _emailSender;
    private readonly IValidator<DatosAcademicosDto> _datosAcademicosValidator;

    public SolicitudService(
        ISolicitudRepository solicitudes,
        ICatalogoRepository catalogos,
        IConsecutivoGenerator consecutivos,
        IEmailSender emailSender,
        IValidator<DatosAcademicosDto> datosAcademicosValidator)
    {
        _solicitudes = solicitudes;
        _catalogos = catalogos;
        _consecutivos = consecutivos;
        _emailSender = emailSender;
        _datosAcademicosValidator = datosAcademicosValidator;
    }

    public async Task<Solicitud> IniciarOContinuarBorradorAsync(Solicitante solicitante, int profesionId)
    {
        var existente = await _solicitudes.GetBorradorActivoAsync(solicitante.Id);
        if (existente is not null)
        {
            return existente;
        }

        var profesion = await _catalogos.GetProfesionByIdAsync(profesionId)
            ?? throw new AppValidationException(new[] { "La profesion seleccionada no existe." });

        var solicitud = Solicitud.IniciarBorrador(solicitante, profesion);
        await _solicitudes.AddAsync(solicitud);
        return solicitud;
    }

    public Task<Solicitud?> ObtenerBorradorActivoAsync(int solicitanteId) =>
        _solicitudes.GetBorradorActivoAsync(solicitanteId);

    public Task<Solicitud?> ObtenerUltimaAsync(int solicitanteId) =>
        _solicitudes.GetUltimaBySolicitanteIdAsync(solicitanteId);

    public Task<Solicitud?> ObtenerPorIdAsync(int id) =>
        _solicitudes.GetByIdAsync(id);

    public Task<List<Solicitud>> ObtenerBandejaAsync(EtapaSolicitud etapa, BandejaFiltroDto filtro) =>
        _solicitudes.GetPorEtapaAsync(etapa, filtro);

    public Task<List<Solicitud>> ObtenerSeguimientoAsync(BandejaFiltroDto filtro) =>
        _solicitudes.GetTodasAsync(filtro);

    public async Task AprobarAsync(int solicitudId, string usuarioId, string? mensaje = null)
    {
        var solicitud = await _solicitudes.GetByIdAsync(solicitudId)
            ?? throw new InvalidOperationException("Solicitud no encontrada.");

        try
        {
            solicitud.Aprobar(usuarioId, mensaje);
        }
        catch (DomainException ex)
        {
            throw new AppValidationException(new[] { ex.Message });
        }

        await _solicitudes.SaveChangesAsync();

        var notaFuncionario = string.IsNullOrWhiteSpace(mensaje) ? "" : $"<p><em>{mensaje}</em></p>";
        var correo = solicitud.Solicitante.CorreoElectronico;
        if (solicitud.Estado == EstadoSolicitud.Aprobado)
        {
            await _emailSender.EnviarAsync(correo, "Tu certificado esta listo",
                $"<p>Hola {solicitud.Solicitante.Nombres},</p><p>Tu solicitud fue aprobada y tu certificado con folio <strong>{solicitud.Consecutivo?.Numero}</strong> ya esta disponible para descargar en el portal Rethus.</p>{notaFuncionario}");
        }
        else
        {
            await _emailSender.EnviarAsync(correo, "Tu solicitud avanzo de etapa",
                $"<p>Hola {solicitud.Solicitante.Nombres},</p><p>Tu solicitud fue aprobada en su etapa actual y ahora se encuentra en <strong>{solicitud.EtapaActual}</strong>.</p>{notaFuncionario}");
        }
    }

    public async Task RechazarAsync(int solicitudId, string usuarioId, string motivo)
    {
        var solicitud = await _solicitudes.GetByIdAsync(solicitudId)
            ?? throw new InvalidOperationException("Solicitud no encontrada.");

        try
        {
            solicitud.Rechazar(usuarioId, motivo);
        }
        catch (DomainException ex)
        {
            throw new AppValidationException(new[] { ex.Message });
        }

        await _solicitudes.SaveChangesAsync();

        await _emailSender.EnviarAsync(solicitud.Solicitante.CorreoElectronico, "Tu solicitud fue rechazada",
            $"<p>Hola {solicitud.Solicitante.Nombres},</p><p>Tu solicitud fue rechazada por el siguiente motivo:</p><p><em>{motivo}</em></p>");
    }

    public async Task CorregirAsync(int solicitudId, string usuarioId)
    {
        var solicitud = await _solicitudes.GetByIdAsync(solicitudId)
            ?? throw new InvalidOperationException("Solicitud no encontrada.");

        try
        {
            solicitud.Corregir(usuarioId);
        }
        catch (DomainException ex)
        {
            throw new AppValidationException(new[] { ex.Message });
        }

        await _solicitudes.SaveChangesAsync();
    }

    public async Task AsignarConsecutivoAutomaticoAsync(int solicitudId)
    {
        var solicitud = await _solicitudes.GetByIdAsync(solicitudId)
            ?? throw new InvalidOperationException("Solicitud no encontrada.");

        var numero = await _consecutivos.GenerarSiguienteAsync(solicitud.TipoTramite);

        try
        {
            solicitud.AsignarConsecutivo(numero, ModoConsecutivo.Automatico);
        }
        catch (DomainException ex)
        {
            throw new AppValidationException(new[] { ex.Message });
        }

        await _solicitudes.SaveChangesAsync();
    }

    public async Task AsignarConsecutivoManualAsync(int solicitudId, string numero)
    {
        var solicitud = await _solicitudes.GetByIdAsync(solicitudId)
            ?? throw new InvalidOperationException("Solicitud no encontrada.");

        try
        {
            solicitud.AsignarConsecutivo(numero, ModoConsecutivo.Manual);
        }
        catch (DomainException ex)
        {
            throw new AppValidationException(new[] { ex.Message });
        }

        await _solicitudes.SaveChangesAsync();
    }

    public Task<Solicitud?> ObtenerPorConsecutivoAsync(string numero) =>
        _solicitudes.GetByConsecutivoAsync(numero);

    public async Task AgregarComentarioAsync(int solicitudId, string usuarioId, string texto)
    {
        if (string.IsNullOrWhiteSpace(texto))
        {
            throw new AppValidationException(new[] { "El comentario no puede estar vacio." });
        }

        var solicitud = await _solicitudes.GetByIdAsync(solicitudId)
            ?? throw new InvalidOperationException("Solicitud no encontrada.");

        solicitud.AgregarComentario(new Comentario
        {
            AutorUserId = usuarioId,
            Texto = texto,
            FechaCreacion = DateTime.UtcNow
        });

        await _solicitudes.SaveChangesAsync();
    }

    public async Task GuardarDatosAcademicosAsync(int solicitudId, DatosAcademicosDto dto)
    {
        var validacion = await _datosAcademicosValidator.ValidateAsync(dto);
        if (!validacion.IsValid)
        {
            throw new AppValidationException(validacion.Errors.Select(e => e.ErrorMessage));
        }

        var solicitud = await _solicitudes.GetByIdAsync(solicitudId)
            ?? throw new InvalidOperationException("Solicitud no encontrada.");

        solicitud.DatosAcademicos = new DatosAcademicos
        {
            SolicitudId = solicitud.Id,
            OrigenTitulo = dto.OrigenTitulo,
            TipoInstitucion = dto.TipoInstitucion,
            TipoPrograma = dto.TipoPrograma,
            PaisInstitucionId = dto.PaisInstitucionId,
            DepartamentoInstitucionId = dto.DepartamentoInstitucionId,
            MunicipioInstitucionId = dto.MunicipioInstitucionId,
            NombreInstitucion = dto.NombreInstitucion,
            NombrePrograma = dto.NombrePrograma,
            FechaGrado = dto.FechaGrado,
            NumeroConvalidacion = dto.NumeroConvalidacion,
            FechaConvalidacion = dto.FechaConvalidacion,
            TituloEquivalente = dto.TituloEquivalente
        };

        await _solicitudes.SaveChangesAsync();
    }
}
