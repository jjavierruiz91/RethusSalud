using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using rethus_backend.Models;
using rethus_backend.Utilities.Constants.PaginatioConstants;
using rethus_backend.Models.Dto.Comments;

namespace rethus_backend.Controllers;

[ApiController]
[Route("[controller]")]
[Authorize]
public class CommentsController : ApiBaseController
{
    public CommentsController(IServiceProvider provider)
        : base(provider) { }

    [HttpGet]
    [Authorize(Roles = Policies.FuncionarioEtapa1)]
    public PaginationResultDto<CommentsResponseDto> GetAllPaginado(
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 10,
        [FromQuery] string formId = "",
        [FromQuery] string status = ""
    )
    {
        var request = new PaginationRequestDto<CommonQueryParametersDto>
        {
            Page = page,
            PageSize = pageSize,
            // QueryParameters = new CommonQueryParametersDto { UserFormId = formId, Status = status }
        };

        Func<Comments, CommentsResponseDto> mapper = user =>
            new CommentsResponseDto
            {
                CommentId = user.CommentId,
                UserFormId = user.UserFormId,
                CreatedAt = user.CreatedAt,
                Description = user.Description,
                status = user.Status
            };
        var result = _unitOfWork.Comments.GetPagination(request, mapper);

        return result;
    }

    [HttpPost]
    [Authorize(Roles = Policies.FuncionarioEtapa1)]
    public async Task<ActionResult<ApiResponse>> create([FromBody] CommentsCreateDto comments)
    {
        ApiResponse createComents = await _unitOfWork.Comments.CreateComments(comments);

        if (createComents.IsSuccess == false)
        {
            return createComents;
        }

        return Ok(createComents);
    }

    [HttpPatch("approved/{commentId}")]
    [Authorize(Roles = Policies.FuncionarioEtapa1)]
    public ActionResult<ApiResponse> ApprovedComments(string commentId)
    {
        ApiResponse resolverComments = _unitOfWork.Comments.UpdateStatusApprovedComments(commentId);
        return resolverComments;
    }

    [HttpPatch("reject/{commentId}")]
    [Authorize(Roles = Policies.FuncionarioEtapa1)]
    public ActionResult<ApiResponse> RejectComments(string commentId)
    {
        ApiResponse resolverComments = _unitOfWork.Comments.UpdateStatusRejectedComments(commentId);
        return resolverComments;
    }
}
