using rethus_backend.Models;
using rethus_backend.Models.Dto.Pagination;
using rethus_backend.Utilities.Constants.PaginatioConstants;

namespace rethus_backend.RepositoryV2
{
    public interface IPaginationRepositoryV2<T>
        where T : class
    {
        Task<PaginationResultDto<TResult>> GetPagedAsync<TResult>(
            PaginationRequestDto<FilterQueryParametersDto> request,
            Func<T, TResult> selector
        );
    }
}
