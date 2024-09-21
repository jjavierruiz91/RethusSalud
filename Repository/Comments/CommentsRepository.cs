using System.Linq.Expressions;
using System.Net;
using System.Net.Http.Headers;
using rethus_backend.Data;
using rethus_backend.Models;
using rethus_backend.Models.Dto.Comments;
using rethus_backend.Repository.IRepository;
using rethus_backend.Utilities.Constants.PaginatioConstants;
using rethus_backend.Utilities.Constants.User.CommentsConstants;

namespace rethus_backend.Repository
{
    public class CommentsRepository : Repository<User>, ICommentsRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly IUserRepository _userRepository;
        private readonly IUserFormRepository _userFormRepository;

        // Pagination
        private readonly PaginationService<
            Comments,
            CommonQueryParametersDto,
            CommentsResponseDto
        > _paginationService;

        public CommentsRepository(
            ApplicationDbContext db,
            IUserRepository _users,
            IUserFormRepository _userForm,
            PaginationService<
                Comments,
                CommonQueryParametersDto,
                CommentsResponseDto
            > paginationService
        )
            : base(db)
        {
            _context = db;
            _userRepository = _users;
            _userFormRepository = _userForm;
            _paginationService = paginationService;
        }

        public Task CreateAsync(Comments entity)
        {
            throw new NotImplementedException();
        }

        public async Task<ApiResponse> CreateComments(CommentsCreateDto createRequestDto)
        {
            var response = new ApiResponse();
            var user = _userRepository.IsExistUserId(createRequestDto.UserFuncionarioId);
            if (user == null)
            {
                response.AddError("El usuario no existe", HttpStatusCode.NotFound, false);
                return response;
            }

            var userForm = _userFormRepository.GetUserByUserFormId(createRequestDto.UserFormId);
            if (userForm == null)
            {
                response.AddError("El formulario no existe", HttpStatusCode.NotFound, false);
                return response;
            }

            Comments newComments =
                new()
                {
                    Description = createRequestDto.Description,
                    UserId = userForm,
                    UserFormId = createRequestDto.UserFormId,
                    Status = CommentsStatus.pending,
                    Type = createRequestDto.Type,
                    CreatedAt = DateTime.Now,
                    UpdateAt = DateTime.Now
                };

            _context.Comments.Add(newComments);
            _context.SaveChanges();

            response.Result = "Ok";
            return response;
        }

        public Task<List<Comments>> GetAllAsync(
            Expression<Func<Comments, bool>>? filter = null,
            string? includeProperties = null
        )
        {
            throw new NotImplementedException();
        }

        public Task<Comments> GetAsync(
            Expression<Func<Comments, bool>> filter = null,
            bool tracked = true,
            string? includeProperties = null
        )
        {
            throw new NotImplementedException();
        }

        public PaginationResultDto<CommentsResponseDto> GetPagination(
            PaginationRequestDto<CommonQueryParametersDto> request,
            Func<Comments, CommentsResponseDto> mapper
        )
        {
            var result = _paginationService.GetPaginatedEntities(request, mapper);

            return result;
        }

        public Task RemoveAsync(Comments entity)
        {
            throw new NotImplementedException();
        }

        public bool isExistsComment(string commentId)
        {
            return _context.Comments.Any(c => c.CommentId == commentId);
        }

        public Comments GetByCommentsId(string commentId)
        {
            return _context.Comments.FirstOrDefault(c => c.CommentId == commentId);
        }

        public ApiResponse UpdateStatusApprovedComments(string commentId)
        {
            var response = new ApiResponse();

            Comments comment = GetByCommentsId(commentId);

            if (comment == null)
            {
                response.AddError("El commentario no existe", HttpStatusCode.NotFound, false);
                return response;
            }

            comment.Status = CommentsStatus.approved;

            _context.SaveChanges();
            response.Messages.Add("El comentario ha sido resuelto");
            response.StatusCode = HttpStatusCode.OK;
            return response;
        }

        public ApiResponse UpdateStatusRejectedComments(string commentId)
        {
            var response = new ApiResponse();

            Comments comment = GetByCommentsId(commentId);

            if (comment == null)
            {
                response.AddError("El commentario no existe", HttpStatusCode.NotFound, false);
                return response;
            }

            comment.Status = CommentsStatus.rejected;

            _context.SaveChanges();
            response.Messages.Add("El comentario ha sido rechazado");
            response.StatusCode = HttpStatusCode.OK;
            return response;
        }
    }
}
