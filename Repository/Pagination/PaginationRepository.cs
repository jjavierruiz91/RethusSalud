using System.Linq.Expressions;
using rethus_backend.Data;
using rethus_backend.Models.Dto.Comments;
using rethus_backend.Utilities.Constants.PaginatioConstants;

namespace rethus_backend.Repository
{
    public class PaginationService<T, TQueryParameters, TResponseDto>
        where T : class
    {
        private readonly ApplicationDbContext _context;

        public PaginationService(ApplicationDbContext context)
        {
            _context = context;
        }

        public PaginationResultDto<TResponseDto> GetPaginatedEntities(
            PaginationRequestDto<TQueryParameters> request,
            Func<T, TResponseDto> mapper
        )
        {
            IQueryable<T> query = _context.Set<T>();
            if (
                request.QueryParameters != null
                && request.QueryParameters is CommonQueryParametersDto testParams
            )
            {
                query = QueryHelper.ApplyFilters(query, testParams);
            }

            // Contar el total de elementos
            int totalItems = query.Count();

            // Calcular el número total de páginas
            int totalPages = (int)Math.Ceiling((double)totalItems / request.PageSize);

            // Aplicar paginación
            query = query.Skip((request.Page - 1) * request.PageSize).Take(request.PageSize);

            // Mapear entidades a DTO de respuesta
            var entitiesDto = query.Select(mapper).ToList();

            // Construir el objeto de resultado
            var result = new PaginationResultDto<TResponseDto>
            {
                Data = entitiesDto,
                TotalItems = totalItems,
                TotalPages = totalPages,
                PageSize = request.PageSize,
                CurrentPage = request.Page
            };

            return result;
        }
    }
}
