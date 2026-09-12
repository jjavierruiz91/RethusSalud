using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using RethusSalud.Application.Common;
using RethusSalud.Application.Dtos;
using PagedResult = RethusSalud.Application.Common.PagedResult<RethusSalud.Domain.Entities.Solicitud>;
using RethusSalud.Application.Interfaces;
using RethusSalud.Domain.Constants;
using RethusSalud.Domain.Entities;
using RethusSalud.Domain.Enums;
using RethusSalud.Infrastructure.Persistence;

namespace RethusSalud.Infrastructure.Repositories;

public class SolicitudRepository : ISolicitudRepository
{
    private readonly ApplicationDbContext _context;

    public SolicitudRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    private IQueryable<Solicitud> ConCargasCompletas() =>
        _context.Solicitudes
            .Include(s => s.Solicitante).ThenInclude(s => s.PaisNacimiento)
            .Include(s => s.Solicitante).ThenInclude(s => s.DepartamentoNacimiento)
            .Include(s => s.Solicitante).ThenInclude(s => s.MunicipioNacimiento)
            .Include(s => s.Solicitante).ThenInclude(s => s.PaisResidencia)
            .Include(s => s.Solicitante).ThenInclude(s => s.DepartamentoResidencia)
            .Include(s => s.Solicitante).ThenInclude(s => s.MunicipioResidencia)
            .Include(s => s.Profesion)
            .Include(s => s.DatosAcademicos).ThenInclude(d => d!.PaisInstitucion)
            .Include(s => s.DatosAcademicos).ThenInclude(d => d!.DepartamentoInstitucion)
            .Include(s => s.DatosAcademicos).ThenInclude(d => d!.MunicipioInstitucion)
            .Include(s => s.Consecutivo)
            .Include(s => s.Archivos)
            .Include(s => s.Comentarios)
            .Include(s => s.Historial);

    public async Task AddAsync(Solicitud solicitud)
    {
        _context.Solicitudes.Add(solicitud);
        await _context.SaveChangesAsync();
    }

    public Task<Solicitud?> GetByIdAsync(int id) =>
        ConCargasCompletas().FirstOrDefaultAsync(s => s.Id == id);

    public Task<Solicitud?> GetBorradorActivoAsync(int solicitanteId) =>
        ConCargasCompletas()
            .Where(s => s.SolicitanteId == solicitanteId && s.Estado == EstadoSolicitud.Borrador)
            .OrderByDescending(s => s.FechaCreacion)
            .FirstOrDefaultAsync();

    public Task<Solicitud?> GetUltimaBySolicitanteIdAsync(int solicitanteId) =>
        ConCargasCompletas()
            .Where(s => s.SolicitanteId == solicitanteId)
            .OrderByDescending(s => s.FechaCreacion)
            .FirstOrDefaultAsync();

    public async Task<List<Solicitud>> GetPorEtapaAsync(EtapaSolicitud etapa, BandejaFiltroDto filtro)
    {
        var query = ConCargasCompletas().Where(s => s.EtapaActual == etapa);

        query = filtro.Estado.HasValue
            ? query.Where(s => s.Estado == filtro.Estado.Value)
            : query.Where(s => s.Estado == EstadoSolicitud.EnProceso);

        query = AplicarFiltroTexto(query, filtro.NumeroIdentificacion);

        if (filtro.TipoTramite.HasValue)
        {
            query = query.Where(s => s.TipoTramite == filtro.TipoTramite.Value);
        }

        if (filtro.Desde.HasValue)
        {
            query = query.Where(s => s.FechaCreacion >= filtro.Desde.Value);
        }

        if (filtro.Hasta.HasValue)
        {
            query = query.Where(s => s.FechaCreacion <= filtro.Hasta.Value);
        }

        return await query.OrderBy(s => s.FechaCreacion).ToListAsync();
    }

    public async Task<List<Solicitud>> GetTodasAsync(BandejaFiltroDto filtro)
    {
        var query = ConCargasCompletas().Where(s => s.Estado != EstadoSolicitud.Borrador);

        if (filtro.Estado.HasValue)
        {
            query = query.Where(s => s.Estado == filtro.Estado.Value);
        }

        if (filtro.Etapa.HasValue)
        {
            query = query.Where(s => s.EtapaActual == filtro.Etapa.Value);
        }

        query = AplicarFiltroTexto(query, filtro.NumeroIdentificacion);

        if (filtro.TipoTramite.HasValue)
        {
            query = query.Where(s => s.TipoTramite == filtro.TipoTramite.Value);
        }

        if (filtro.Desde.HasValue)
        {
            query = query.Where(s => s.FechaCreacion >= filtro.Desde.Value);
        }

        if (filtro.Hasta.HasValue)
        {
            query = query.Where(s => s.FechaCreacion <= filtro.Hasta.Value);
        }

        return await query.OrderBy(s => s.FechaCreacion).ToListAsync();
    }

    private IQueryable<Solicitud> ParaListado() =>
        _context.Solicitudes
            .Include(s => s.Solicitante)
            .Include(s => s.Consecutivo);

    // Busqueda mixta: coincide por numero de identificacion, nombres, apellidos o el nombre completo.
    // Se recorta (Trim) cada valor porque algunos registros tienen espacios sobrantes guardados,
    // lo que rompia la coincidencia al concatenar "Nombres + Apellidos".
    private static IQueryable<Solicitud> AplicarFiltroTexto(IQueryable<Solicitud> query, string? texto)
    {
        if (string.IsNullOrWhiteSpace(texto))
        {
            return query;
        }

        var textoBuscado = texto.Trim();

        return query.Where(s =>
            s.Solicitante.NumeroIdentificacion.Contains(textoBuscado) ||
            s.Solicitante.Nombres.Contains(textoBuscado) ||
            s.Solicitante.Apellidos.Contains(textoBuscado) ||
            (s.Solicitante.Nombres.Trim() + " " + s.Solicitante.Apellidos.Trim()).Contains(textoBuscado));
    }

    private static IQueryable<Solicitud> AplicarFiltrosComunes(IQueryable<Solicitud> query, BandejaFiltroDto filtro)
    {
        query = AplicarFiltroTexto(query, filtro.NumeroIdentificacion);

        if (filtro.TipoTramite.HasValue)
        {
            query = query.Where(s => s.TipoTramite == filtro.TipoTramite.Value);
        }

        if (filtro.Desde.HasValue)
        {
            query = query.Where(s => s.FechaCreacion >= filtro.Desde.Value);
        }

        if (filtro.Hasta.HasValue)
        {
            query = query.Where(s => s.FechaCreacion <= filtro.Hasta.Value);
        }

        return query;
    }

    public async Task<BandejaPagedResult> GetPorEtapaPagedAsync(EtapaSolicitud etapa, BandejaFiltroDto filtro, int pageNumber, int pageSize)
    {
        var query = ParaListado().Where(s => s.EtapaActual == etapa);
        query = filtro.Estado.HasValue
            ? query.Where(s => s.Estado == filtro.Estado.Value)
            : query.Where(s => s.Estado == EstadoSolicitud.EnProceso);
        query = AplicarFiltrosComunes(query, filtro);

        var totalCount = await query.CountAsync();
        var totalEnProceso = await query.CountAsync(s => s.Estado == EstadoSolicitud.EnProceso);
        var totalAprobadas = await query.CountAsync(s => s.Estado == EstadoSolicitud.Aprobado);
        var limiteEspera = DateTime.UtcNow.AddDays(-2);
        var totalEsperandoLargo = await query.CountAsync(s => s.Estado == EstadoSolicitud.EnProceso && s.FechaCreacion <= limiteEspera);

        var items = await query
            .OrderBy(s => s.FechaCreacion)
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        return new BandejaPagedResult
        {
            Pagina = new PagedResult { Items = items, PageNumber = pageNumber, PageSize = pageSize, TotalCount = totalCount },
            TotalEnProceso = totalEnProceso,
            TotalAprobadas = totalAprobadas,
            TotalEsperandoLargo = totalEsperandoLargo
        };
    }

    private IQueryable<Solicitud> ConstruirQueryTodas(BandejaFiltroDto filtro)
    {
        var query = ParaListado().Where(s => s.Estado != EstadoSolicitud.Borrador);

        if (filtro.Estado.HasValue)
        {
            query = query.Where(s => s.Estado == filtro.Estado.Value);
        }

        if (filtro.Etapa.HasValue)
        {
            query = query.Where(s => s.EtapaActual == filtro.Etapa.Value);
        }

        return AplicarFiltrosComunes(query, filtro);
    }

    public async Task<PagedResult> GetTodasPagedAsync(BandejaFiltroDto filtro, int pageNumber, int pageSize)
    {
        var query = ConstruirQueryTodas(filtro);

        var totalCount = await query.CountAsync();
        var items = await query
            .OrderBy(s => s.FechaCreacion)
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        return new PagedResult { Items = items, PageNumber = pageNumber, PageSize = pageSize, TotalCount = totalCount };
    }

    public async Task<SeguimientoPagedResult> GetSeguimientoPagedAsync(BandejaFiltroDto filtro, int pageNumber, int pageSize)
    {
        var query = ConstruirQueryTodas(filtro);

        var totalCount = await query.CountAsync();
        var totalEnProceso = await query.CountAsync(s => s.Estado == EstadoSolicitud.EnProceso);
        var totalAprobadas = await query.CountAsync(s => s.Estado == EstadoSolicitud.Aprobado);
        var totalRechazadas = await query.CountAsync(s => s.Estado == EstadoSolicitud.Rechazado);

        var items = await query
            .OrderBy(s => s.FechaCreacion)
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        return new SeguimientoPagedResult
        {
            Pagina = new PagedResult { Items = items, PageNumber = pageNumber, PageSize = pageSize, TotalCount = totalCount },
            TotalEnProceso = totalEnProceso,
            TotalAprobadas = totalAprobadas,
            TotalRechazadas = totalRechazadas
        };
    }

    public Task<ArchivoAdjunto?> GetArchivoByIdAsync(int archivoId) =>
        _context.ArchivosAdjuntos.FirstOrDefaultAsync(a => a.Id == archivoId);

    private static IQueryable<Solicitud> AplicarFiltrosDashboard(IQueryable<Solicitud> query, DashboardFiltroDto filtro)
    {
        if (filtro.Etapa.HasValue)
        {
            query = query.Where(s => s.EtapaActual == filtro.Etapa.Value);
        }

        if (filtro.PaisId.HasValue)
        {
            query = query.Where(s => s.Solicitante.PaisResidenciaId == filtro.PaisId.Value);
        }

        if (filtro.DepartamentoId.HasValue)
        {
            query = query.Where(s => s.Solicitante.DepartamentoResidenciaId == filtro.DepartamentoId.Value);
        }

        if (filtro.MunicipioId.HasValue)
        {
            query = query.Where(s => s.Solicitante.MunicipioResidenciaId == filtro.MunicipioId.Value);
        }

        if (filtro.Genero.HasValue)
        {
            query = query.Where(s => s.Solicitante.Genero == filtro.Genero.Value);
        }

        if (filtro.GrupoEtnico.HasValue)
        {
            query = query.Where(s => s.Solicitante.GrupoEtnico == filtro.GrupoEtnico.Value);
        }

        if (filtro.OrigenTitulo.HasValue)
        {
            query = query.Where(s => s.DatosAcademicos != null && s.DatosAcademicos.OrigenTitulo == filtro.OrigenTitulo.Value);
        }

        if (filtro.TipoInstitucion.HasValue)
        {
            query = query.Where(s => s.DatosAcademicos != null && s.DatosAcademicos.TipoInstitucion == filtro.TipoInstitucion.Value);
        }

        return query;
    }

    public async Task<DashboardResultDto> GetDashboardAsync(DashboardFiltroDto filtro)
    {
        var baseQuery = AplicarFiltrosDashboard(_context.Solicitudes.Where(s => s.Estado != EstadoSolicitud.Borrador), filtro);

        var query = baseQuery;
        if (filtro.Desde.HasValue)
        {
            query = query.Where(s => s.FechaCreacion >= filtro.Desde.Value);
        }

        if (filtro.Hasta.HasValue)
        {
            query = query.Where(s => s.FechaCreacion <= filtro.Hasta.Value);
        }

        var total = await query.CountAsync();
        var enProceso = await query.CountAsync(s => s.Estado == EstadoSolicitud.EnProceso);
        var aprobadas = await query.CountAsync(s => s.Estado == EstadoSolicitud.Aprobado);
        var rechazadas = await query.CountAsync(s => s.Estado == EstadoSolicitud.Rechazado);
        var limiteEspera = DateTime.UtcNow.AddDays(-2);
        var esperandoLargo = await query.CountAsync(s => s.Estado == EstadoSolicitud.EnProceso && s.FechaCreacion <= limiteEspera);

        var aprobadasQuery = query.Where(s => s.Estado == EstadoSolicitud.Aprobado);
        double? tiempoPromedio = await aprobadasQuery.AnyAsync()
            ? await aprobadasQuery.AverageAsync(s => EF.Functions.DateDiffHour(s.FechaCreacion, s.FechaActualizacion) / 24.0)
            : null;

        var porEtapa = await query
            .Where(s => s.EtapaActual != null)
            .GroupBy(s => s.EtapaActual!.Value)
            .Select(g => new EtapaConteoDto(g.Key, g.Count()))
            .ToListAsync();

        var porPaisCrudo = await query
            .GroupBy(s => new { s.Solicitante.PaisResidenciaId, Nombre = s.Solicitante.PaisResidencia.Nombre })
            .Select(g => new { g.Key.PaisResidenciaId, g.Key.Nombre, Cantidad = g.Count() })
            .OrderByDescending(g => g.Cantidad)
            .ToListAsync();

        var porPais = porPaisCrudo
            .Select(g => new PaisConteoDto(g.PaisResidenciaId, g.Nombre, g.Cantidad))
            .ToList();

        var porDepartamentoCrudo = await query
            .GroupBy(s => new
            {
                s.Solicitante.DepartamentoResidenciaId,
                Nombre = s.Solicitante.DepartamentoResidencia != null ? s.Solicitante.DepartamentoResidencia.Nombre : "Sin departamento"
            })
            .Select(g => new { g.Key.DepartamentoResidenciaId, g.Key.Nombre, Cantidad = g.Count() })
            .OrderByDescending(g => g.Cantidad)
            .Take(8)
            .ToListAsync();

        var porDepartamento = porDepartamentoCrudo
            .Select(g => new DepartamentoConteoDto(g.DepartamentoResidenciaId, g.Nombre, g.Cantidad))
            .ToList();

        var porMunicipioCrudo = await query
            .GroupBy(s => new
            {
                s.Solicitante.MunicipioResidenciaId,
                Nombre = s.Solicitante.MunicipioResidencia != null ? s.Solicitante.MunicipioResidencia.Nombre : "Sin municipio"
            })
            .Select(g => new { g.Key.MunicipioResidenciaId, g.Key.Nombre, Cantidad = g.Count() })
            .OrderByDescending(g => g.Cantidad)
            .Take(8)
            .ToListAsync();

        var porMunicipio = porMunicipioCrudo
            .Select(g => new MunicipioConteoDto(g.MunicipioResidenciaId, g.Nombre, g.Cantidad))
            .ToList();

        var porGenero = await query
            .GroupBy(s => s.Solicitante.Genero)
            .Select(g => new GeneroConteoDto(g.Key, g.Count()))
            .ToListAsync();

        var porGrupoEtnico = await query
            .GroupBy(s => s.Solicitante.GrupoEtnico)
            .Select(g => new GrupoEtnicoConteoDto(g.Key, g.Count()))
            .ToListAsync();

        var porOrigenTituloCrudo = await query
            .Where(s => s.DatosAcademicos != null)
            .GroupBy(s => s.DatosAcademicos!.OrigenTitulo)
            .Select(g => new { Origen = g.Key, Cantidad = g.Count() })
            .ToListAsync();
        var porOrigenTitulo = porOrigenTituloCrudo.Select(g => new OrigenTituloConteoDto(g.Origen, g.Cantidad)).ToList();

        var porTipoInstitucionCrudo = await query
            .Where(s => s.DatosAcademicos != null)
            .GroupBy(s => s.DatosAcademicos!.TipoInstitucion)
            .Select(g => new { Tipo = g.Key, Cantidad = g.Count() })
            .ToListAsync();
        var porTipoInstitucion = porTipoInstitucionCrudo.Select(g => new TipoInstitucionConteoDto(g.Tipo, g.Cantidad)).ToList();

        var topProfesionesCrudo = await query
            .GroupBy(s => new { s.ProfesionId, s.Profesion.Nombre })
            .Select(g => new { g.Key.ProfesionId, g.Key.Nombre, Cantidad = g.Count() })
            .OrderByDescending(g => g.Cantidad)
            .Take(8)
            .ToListAsync();
        var topProfesiones = topProfesionesCrudo.Select(g => new ProfesionConteoDto(g.ProfesionId, g.Nombre, g.Cantidad)).ToList();

        var idsFiltrados = await query.Select(s => s.Id).ToListAsync();
        var porTipoDocumentoCrudo = await _context.ArchivosAdjuntos
            .Where(a => idsFiltrados.Contains(a.SolicitudId))
            .GroupBy(a => a.TipoDocumento)
            .Select(g => new { Tipo = g.Key, Cantidad = g.Count() })
            .ToListAsync();
        var porTipoDocumento = porTipoDocumentoCrudo.Select(g => new TipoDocumentoConteoDto(g.Tipo, g.Cantidad)).ToList();

        var fechasNacimiento = await query.Select(s => s.Solicitante.FechaNacimiento).ToListAsync();
        var hoy = DateOnly.FromDateTime(DateTime.UtcNow);
        var porRangoEdad = fechasNacimiento
            .Select(f =>
            {
                var edad = hoy.Year - f.Year - (hoy < f.AddYears(hoy.Year - f.Year) ? 1 : 0);
                return edad switch
                {
                    < 26 => (Rango: "18-25", Orden: 1),
                    < 36 => (Rango: "26-35", Orden: 2),
                    < 46 => (Rango: "36-45", Orden: 3),
                    < 56 => (Rango: "46-55", Orden: 4),
                    _ => (Rango: "56+", Orden: 5)
                };
            })
            .GroupBy(r => r)
            .Select(g => new RangoEdadConteoDto(g.Key.Rango, g.Key.Orden, g.Count()))
            .OrderBy(r => r.Orden)
            .ToList();

        var solicitudesRecientesCrudo = await query
            .OrderByDescending(s => s.FechaCreacion)
            .Take(10)
            .Select(s => new
            {
                s.Id,
                s.Solicitante.NumeroIdentificacion,
                Nombre = s.Solicitante.Nombres + " " + s.Solicitante.Apellidos,
                Profesion = s.Profesion.Nombre,
                s.Estado,
                s.EtapaActual,
                s.FechaCreacion
            })
            .ToListAsync();
        var solicitudesRecientes = solicitudesRecientesCrudo
            .Select(s => new SolicitudResumenDto(s.Id, s.NumeroIdentificacion, s.Nombre, s.Profesion, s.Estado, s.EtapaActual, s.FechaCreacion))
            .ToList();

        var desdeTendenciaMes = DateTime.UtcNow.AddMonths(-5);
        var desdeTendencia = new DateTime(desdeTendenciaMes.Year, desdeTendenciaMes.Month, 1);
        var tendenciaMensual = await baseQuery
            .Where(s => s.FechaCreacion >= desdeTendencia)
            .GroupBy(s => new { s.FechaCreacion.Year, s.FechaCreacion.Month })
            .Select(g => new TendenciaMesDto(g.Key.Year, g.Key.Month, g.Count()))
            .ToListAsync();

        var desempeno = await ObtenerDesempenoFuncionariosAsync();

        return new DashboardResultDto
        {
            Total = total,
            EnProceso = enProceso,
            Aprobadas = aprobadas,
            Rechazadas = rechazadas,
            EsperandoLargo = esperandoLargo,
            TiempoPromedioAprobacionDias = tiempoPromedio,
            PorEtapa = porEtapa,
            PorPais = porPais,
            PorDepartamento = porDepartamento,
            PorMunicipio = porMunicipio,
            PorGenero = porGenero,
            PorGrupoEtnico = porGrupoEtnico,
            PorOrigenTitulo = porOrigenTitulo,
            PorTipoInstitucion = porTipoInstitucion,
            PorTipoDocumento = porTipoDocumento,
            TopProfesiones = topProfesiones,
            PorRangoEdad = porRangoEdad,
            TendenciaMensual = tendenciaMensual.OrderBy(t => t.Anio).ThenBy(t => t.Mes).ToList(),
            DesempenoFuncionarios = desempeno,
            SolicitudesRecientes = solicitudesRecientes
        };
    }

    private async Task<List<FuncionarioDesempenoDto>> ObtenerDesempenoFuncionariosAsync()
    {
        // Un avance de etapa (Etapa1->Etapa2, Etapa2->Etapa3, etc.) deja el Estado en EnProceso, no en Aprobado
        // (solo la aprobacion final en Inventario pone Estado=Aprobado). Por eso contamos EnProceso tambien como
        // "aprobada" aqui: si Inventario aprobo 15 solicitudes, esas mismas 15 tuvieron que pasar por Etapa1/2/3
        // antes, y cada uno de esos funcionarios debe recibir credito por su propio paso.
        var conteos = await _context.HistorialEstados
            .Where(h => h.EstadoResultante == EstadoSolicitud.EnProceso || h.EstadoResultante == EstadoSolicitud.Aprobado || h.EstadoResultante == EstadoSolicitud.Rechazado)
            .GroupBy(h => h.UsuarioId)
            .Select(g => new
            {
                UsuarioId = g.Key,
                Aprobadas = g.Count(h => h.EstadoResultante == EstadoSolicitud.EnProceso || h.EstadoResultante == EstadoSolicitud.Aprobado),
                Rechazadas = g.Count(h => h.EstadoResultante == EstadoSolicitud.Rechazado)
            })
            .ToListAsync();

        if (conteos.Count == 0)
        {
            return new List<FuncionarioDesempenoDto>();
        }

        var usuarioIds = conteos.Select(c => c.UsuarioId).ToList();

        var usuarios = await _context.Users
            .Where(u => usuarioIds.Contains(u.Id))
            .Select(u => new { u.Id, u.NombreCompleto })
            .ToListAsync();

        var rolesPorUsuario = await (
            from ur in _context.UserRoles
            join r in _context.Roles on ur.RoleId equals r.Id
            where usuarioIds.Contains(ur.UserId)
            select new { ur.UserId, r.Name }
        ).ToListAsync();

        EtapaSolicitud? EtapaDeRoles(string usuarioId)
        {
            var roles = rolesPorUsuario.Where(r => r.UserId == usuarioId).Select(r => r.Name).ToList();
            if (roles.Contains(Roles.FuncionarioEtapa1)) return EtapaSolicitud.Etapa1;
            if (roles.Contains(Roles.FuncionarioEtapa2)) return EtapaSolicitud.Etapa2;
            if (roles.Contains(Roles.FuncionarioEtapa3)) return EtapaSolicitud.Etapa3;
            if (roles.Contains(Roles.Inventario)) return EtapaSolicitud.Inventario;
            return null;
        }

        return conteos
            .Select(c =>
            {
                var nombre = usuarios.FirstOrDefault(u => u.Id == c.UsuarioId)?.NombreCompleto;
                var etapa = EtapaDeRoles(c.UsuarioId);
                return new FuncionarioDesempenoDto(
                    c.UsuarioId,
                    string.IsNullOrWhiteSpace(nombre) ? "Funcionario" : nombre,
                    etapa,
                    c.Aprobadas + c.Rechazadas,
                    c.Aprobadas,
                    c.Rechazadas);
            })
            .Where(d => d.Etapa.HasValue) // excluye al ciudadano que radico (su registro de "Solicitud radicada" tambien queda en EnProceso, pero no es un funcionario)
            .OrderByDescending(d => d.Gestionadas)
            .Take(10)
            .ToList();
    }

    public Task<Solicitud?> GetByConsecutivoAsync(string numero) =>
        ConCargasCompletas().FirstOrDefaultAsync(s => s.Consecutivo != null && s.Consecutivo.Numero == numero);

    public async Task SaveChangesAsync()
    {
        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateException ex) when (EsViolacionDeUnicidad(ex))
        {
            throw new AppValidationException(new[] { "Ese numero de consecutivo ya existe. Elige otro." });
        }
    }

    private static bool EsViolacionDeUnicidad(DbUpdateException ex) =>
        ex.InnerException is SqlException sqlEx && (sqlEx.Number == 2601 || sqlEx.Number == 2627);
}
