using Microsoft.EntityFrameworkCore.ChangeTracking;
using rethus_backend.Models;
using rethus_backend.Models.Dto.Comments;
using rethus_backend.Models.Dto.Pagination;
using rethus_backend.Models.Dto.User;
using rethus_backend.Utilities.Constants.PaginatioConstants;

namespace rethus_backend.Repository.IRepository
{
    public interface ICommentsRepository : IRepository<Comments>
    {
        Task<ApiResponse> CreateComments(CommentsCreateDto createRequestDto);
        public ApiResponse UpdateStatusApprovedComments(string commentId);

        public ApiResponse UpdateStatusRejectedComments(string commentId);

        ApiResponse UpdateStatusUpdatedComments(string commentId);
        ApiResponse UpdateStatusProcesssComments(string commentId);
        bool isExistsComment(string commentId);

        Comments GetByCommentsId(string commentId);
        public PaginationResultDto<CommentsResponseDto> GetPagination(
            PaginationRequestDto<CommonQueryParametersDto> request,
            Func<Comments, CommentsResponseDto> mapper
        );

        Task<PaginationResultDto<TResult>> GetPagedData<TResult>(
            PaginationRequestDto<FilterQueryParametersDto> request,
            Func<Comments, TResult> selector
        );
        bool CountPendingCommentsExternalForm(string formId);
    }
}
