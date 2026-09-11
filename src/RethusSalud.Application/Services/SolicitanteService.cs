using FluentValidation;
using RethusSalud.Application.Common;
using RethusSalud.Application.Dtos;
using RethusSalud.Application.Interfaces;
using RethusSalud.Domain.Constants;
using RethusSalud.Domain.Entities;
using RethusSalud.Domain.Enums;

namespace RethusSalud.Application.Services;

public class SolicitanteService
{
    private readonly ISolicitanteRepository _solicitantes;
    private readonly IValidator<DatosPersonalesDto> _datosPersonalesValidator;

    public SolicitanteService(ISolicitanteRepository solicitantes, IValidator<DatosPersonalesDto> datosPersonalesValidator)
    {
        _solicitantes = solicitantes;
        _datosPersonalesValidator = datosPersonalesValidator;
    }

    public async Task<Solicitante> RegistrarAsync(string applicationUserId, string nombres, string correoElectronico, string numeroIdentificacion)
    {
        if (await _solicitantes.ExisteNumeroIdentificacionAsync(numeroIdentificacion))
        {
            throw new AppValidationException(new[] { "Ya existe un solicitante registrado con este numero de identificacion." });
        }

        var solicitante = new Solicitante
        {
            ApplicationUserId = applicationUserId,
            Nombres = nombres,
            Apellidos = string.Empty,
            NumeroIdentificacion = numeroIdentificacion,
            CorreoElectronico = correoElectronico,
            Celular = string.Empty,
            DireccionDomicilio = string.Empty,
            LugarExpedicion = string.Empty,
            TipoIdentificacion = TipoIdentificacion.CedulaCiudadania,
            PaisNacimientoId = Catalogos.PaisColombiaId,
            PaisResidenciaId = Catalogos.PaisColombiaId
        };

        await _solicitantes.AddAsync(solicitante);
        return solicitante;
    }

    public async Task<Solicitante> ObtenerPorUsuarioAsync(string applicationUserId)
    {
        return await _solicitantes.GetByApplicationUserIdAsync(applicationUserId)
            ?? throw new InvalidOperationException("No existe un solicitante asociado a este usuario.");
    }

    public async Task AceptarTerminosAsync(string applicationUserId)
    {
        var solicitante = await ObtenerPorUsuarioAsync(applicationUserId);
        solicitante.AceptarTerminos();
        await _solicitantes.UpdateAsync(solicitante);
    }

    public async Task ActualizarDatosPersonalesAsync(string applicationUserId, DatosPersonalesDto dto)
    {
        // La residencia siempre debe ser en Colombia; se ignora cualquier otro valor recibido.
        dto.PaisResidenciaId = Catalogos.PaisColombiaId;

        var validacion = await _datosPersonalesValidator.ValidateAsync(dto);
        if (!validacion.IsValid)
        {
            throw new AppValidationException(validacion.Errors.Select(e => e.ErrorMessage));
        }

        var solicitante = await ObtenerPorUsuarioAsync(applicationUserId);

        if (await _solicitantes.ExisteNumeroIdentificacionAsync(dto.NumeroIdentificacion, solicitante.Id))
        {
            throw new AppValidationException(new[] { "Ya existe otro solicitante registrado con este numero de identificacion." });
        }

        var esColombiaNacimiento = dto.PaisNacimientoId == Catalogos.PaisColombiaId;

        solicitante.TipoIdentificacion = dto.TipoIdentificacion;
        solicitante.NumeroIdentificacion = dto.NumeroIdentificacion;
        solicitante.LugarExpedicion = dto.LugarExpedicion;
        solicitante.Genero = dto.Genero;
        solicitante.Nombres = dto.Nombres;
        solicitante.Apellidos = dto.Apellidos;
        solicitante.PaisNacimientoId = dto.PaisNacimientoId;
        solicitante.DepartamentoNacimientoId = esColombiaNacimiento ? dto.DepartamentoNacimientoId : null;
        solicitante.MunicipioNacimientoId = esColombiaNacimiento ? dto.MunicipioNacimientoId : null;
        solicitante.DepartamentoNacimientoTexto = esColombiaNacimiento ? null : dto.DepartamentoNacimientoTexto;
        solicitante.MunicipioNacimientoTexto = esColombiaNacimiento ? null : dto.MunicipioNacimientoTexto;
        solicitante.FechaNacimiento = dto.FechaNacimiento;
        solicitante.PaisResidenciaId = dto.PaisResidenciaId;
        solicitante.DepartamentoResidenciaId = dto.DepartamentoResidenciaId;
        solicitante.MunicipioResidenciaId = dto.MunicipioResidenciaId;
        solicitante.DireccionDomicilio = dto.DireccionDomicilio;
        solicitante.TelefonoFijo = dto.TelefonoFijo;
        solicitante.Celular = dto.Celular;
        solicitante.CorreoElectronico = dto.CorreoElectronico;
        solicitante.GrupoEtnico = dto.GrupoEtnico;

        await _solicitantes.UpdateAsync(solicitante);
    }
}
