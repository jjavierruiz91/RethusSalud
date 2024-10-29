using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using rethus_backend.Models;
using rethus_backend.Utilities.Constants.PaginatioConstants;
using rethus_backend.Models.Dto.Comments;
using rethus_backend.Models.Dto.Pagination;

namespace rethus_backend.Controllers;

[ApiController]
[Route("[controller]")]
[Authorize]
public class CommentsController : ApiBaseController
{
    public CommentsController(IServiceProvider provider)
        : base(provider) { }

    [HttpGet]
    [Authorize(Roles = Policies.FuncionarioEtapa1 + "," + Policies.User)]
    public PaginationResultDto<CommentsResponseDto> GetAllPaginado(
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 10,
        [FromQuery] string formId = "",
        [FromQuery] string userId = "",
        [FromQuery] string status = ""
    )
    {
        var request = new PaginationRequestDto<CommonQueryParametersDto>
        {
            Page = page,
            PageSize = pageSize,
            QueryParameters = new CommonQueryParametersDto { UserId = userId }
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
     [Authorize(
        Roles = Policies.FuncionarioEtapa1
            + ","
            + Policies.FuncionarioEtapa2
            + ","
            + Policies.FuncionarioEtapa3
            + ","
            + Policies.Inventory
            + ","
            + Policies.User
    )]
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
     [Authorize(
        Roles = Policies.FuncionarioEtapa1
            + ","
            + Policies.FuncionarioEtapa2
            + ","
            + Policies.FuncionarioEtapa3
            + ","
            + Policies.Inventory
            + ","
            + Policies.User
    )]
    public ActionResult<ApiResponse> ApprovedComments(string commentId)
    {
        ApiResponse resolverComments = _unitOfWork.Comments.UpdateStatusApprovedComments(commentId);
        return resolverComments;
    }

    [HttpPatch("reject/{commentId}")]
     [Authorize(
        Roles = Policies.FuncionarioEtapa1
            + ","
            + Policies.FuncionarioEtapa2
            + ","
            + Policies.FuncionarioEtapa3
            + ","
            + Policies.Inventory
            + ","
            + Policies.User
    )]
    public ActionResult<ApiResponse> RejectComments(string commentId)
    {
        ApiResponse resolverComments = _unitOfWork.Comments.UpdateStatusRejectedComments(commentId);
        return resolverComments;
    }

    [HttpGet("pagination")]
    [Authorize(
        Roles = Policies.FuncionarioEtapa1
            + ","
            + Policies.FuncionarioEtapa2
            + ","
            + Policies.FuncionarioEtapa3
            + ","
            + Policies.Inventory
            + ","
            + Policies.User
    )]
    public async Task<ActionResult<PaginationResultDto<UserFormResponseDto>>> GetPaginatedUserForms(
        [FromQuery] FilterQueryParametersDto filters,
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 10
    )
    {
        // Crea los parámetros de la query con los filtros recibidos
        var queryParameters = new FilterQueryParametersDto
        {
            UserFormId = filters.UserFormId,
            Status = filters.Status,
            Type = filters.Type
        };

        // Construye la solicitud de paginación
        var paginationRequest = new PaginationRequestDto<FilterQueryParametersDto>
        {
            Page = page,
            PageSize = pageSize,
            QueryParameters = queryParameters
        };

        // Ejecuta el método GetPagedData pasando el selector para el mapeo a UserFormResponseDto
        var result = await _unitOfWork.Comments.GetPagedData(
            paginationRequest,
            user =>
                new CommentsResponseDto
                {
                    UserFormId = user.UserFormId,
                    CommentId = user.CommentId,
                    Description = user.Description,
                    status = user.Status,
                    type = user.Type,
                    CreatedAt = user.CreatedAt,
                }
        );

        // Retorna el resultado con el formato esperado
        return Ok(result);
    } 
}
