using System.Globalization;
using Microsoft.EntityFrameworkCore;
using rethus_backend.Data;
using rethus_backend.Models;
using rethus_backend.Models.Dto.Comments;
using rethus_backend.Models.Dto.Pagination;
using rethus_backend.RepositoryV2;
using rethus_backend.Utilities.Constants.PaginatioConstants;
using rethus_backend.Utilities.Constants.User.UserFormConstants;

public class RepositoryPaginationV2<T> : IPaginationRepositoryV2<T>
    where T : class
{
    private readonly ApplicationDbContext _context;

    public RepositoryPaginationV2(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<PaginationResultDto<TResult>> GetPagedAsync<TResult>(
        PaginationRequestDto<FilterQueryParametersDto> request,
        Func<T, TResult> selector
    )
    {
        IQueryable<T> query = _context.Set<T>();

        // Aplicar los filtros de acuerdo a los parámetros en request.QueryParameters
        if (!string.IsNullOrEmpty(request.QueryParameters.UserFormId))
        {
            query = query.Where(
                x => EF.Property<string>(x, "UserFormId") == request.QueryParameters.UserFormId
            );
        }

        if (request.QueryParameters.Status.HasValue)
        {
            query = query.Where(
                x => EF.Property<UserFormStatus>(x, "Status") == request.QueryParameters.Status
            );
        }

        if (!string.IsNullOrEmpty(request.QueryParameters.PersonalIdentification))
        {
            query = query.Where(
                x =>
                    EF.Property<string>(x, "PersonalIdentification")
                    == request.QueryParameters.PersonalIdentification
            );
        }

        if (request.QueryParameters.TypeProcedure.HasValue)
        {
            query = query.Where(
                x =>
                    EF.Property<ConfigurationTypeProcedure>(x, "TypeProcedure")
                    == request.QueryParameters.TypeProcedure
            );
        }

        if (request.QueryParameters.Step.HasValue)
        {
            query = query.Where(
                x => EF.Property<ReviewStepForm>(x, "StepForm") == request.QueryParameters.Step
            );
        }

        if (
            !string.IsNullOrEmpty(request.QueryParameters.startDate)
            && !string.IsNullOrEmpty(request.QueryParameters.endDate)
        )
        {
            string startDateString = request.QueryParameters.startDate?.Trim(); // Eliminar espacios
            string endDateString = request.QueryParameters.endDate?.Trim(); // Eliminar espacios

            DateTime startDate = DateTime.ParseExact(
                startDateString,
                "yyyy-MM-dd",
                CultureInfo.InvariantCulture
            );
            DateTime endDate = DateTime
                .ParseExact(endDateString, "yyyy-MM-dd", CultureInfo.InvariantCulture)
                .AddDays(1)
                .AddTicks(-1);

            query = query.Where(
                x =>
                    EF.Property<DateTime>(x, "CreatedAt") >= startDate
                    && EF.Property<DateTime>(x, "CreatedAt") <= endDate
            );
        }
        // Aplicar ordenación por una columna (por ejemplo, CreatedAt)
        query = query.OrderByDescending(x => EF.Property<DateTime>(x, "CreatedAt"));

        // Contar total de ítems
        int totalItems = await query.CountAsync();

        // Aplicar paginación
        query = query.Skip((request.Page - 1) * request.PageSize).Take(request.PageSize);

        // Ejecutar la consulta y obtener los datos
        var data = query.Select(selector).ToList();

        // Retornar el resultado paginado
        return new PaginationResultDto<TResult>
        {
            Data = data,
            PageSize = request.PageSize,
            CurrentPage = request.Page,
            TotalItems = totalItems,
            TotalPages = (int)Math.Ceiling((double)totalItems / request.PageSize)
        };
    }
}
