using Microsoft.AspNetCore.Mvc;
using rethus_backend.Models;
using rethus_backend.Models.Dto.Comments;
using rethus_backend.Models.Dto.UserForm;
using rethus_backend.Models.Dto.UserFormFiles;
using rethus_backend.Utilities.Constants.PaginatioConstants;
using rethus_backend.Utilities.Templates.dto;

namespace rethus_backend.Repository.IRepository
{
    public interface IUserFormRepository : IRepository<UserForm>
    {
        bool IsUniqueUser(string userFormId);

        bool IsExistUser(string userFormId);

        UserForm GetById(string id);
        string GetUserByUserFormId(string userFormId);

        DetailsProcessUserDto GetDetailProcess(UserFormProcessDto payload);

        UserForm GetFormUserId(string userFormId);
        UserForm IsDownloadCertificate(string userId);

        DetailsProccessPersonalDto GetPersonalInformation(string userFormId);
        DetailsProccessAcademicDto GetProccessAcademic(string userFormId);
        PaginationResult<UserForm> GetAll(int? page);
        Task<ApiResponse> post(UserFormCreateDto createRequestDto);
        ApiResponse ApprovedForm(string userFormId);
        ApiResponse RejectForm(string userFormId);

        ApiResponse RegisterUserFormFile(string userFormId, UserFormFilesCreateDto payload);

        PaginationResultDto<UserFormResponseDto> GetPagination(
            PaginationRequestDto<CommonQueryParametersDto> request,
            Func<UserForm, UserFormResponseDto> mapper
        );

        Task<string> DownloadCertificateRethus(TemplateRethusDto rethusDto);

        Task<string> DownloadCertificateSso(TemplateSSODto sSODto);

        ApiResponse AddConsecutive(string userFormId, string consecutive);

        Task<string> GetFileInventory(string userFormFileId);

        void ValidateCertificateUserForm(string userFormId);

        Task<string> GetConsecutive(string userFormId);

        bool ValidateExistFileForDownload(string userId);

        ValidateFileUserForm IsValidUserForm(string userFormId);

        Task<string?> DonwloadCertificateFileByUserId(string userId);
    }
}
