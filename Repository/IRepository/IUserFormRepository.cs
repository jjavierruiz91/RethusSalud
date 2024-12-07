using rethus_backend.Data;
using rethus_backend.Models;
using rethus_backend.Models.Dto.Comments;
using rethus_backend.Models.Dto.Pagination;
using rethus_backend.Models.Dto.UserForm;
using rethus_backend.Models.Dto.UserFormFiles;
using rethus_backend.Utilities.Constants.PaginatioConstants;
using rethus_backend.Utilities.Constants.User.UserFormConstants;
using rethus_backend.Utilities.Templates.dto;

namespace rethus_backend.Repository.IRepository
{
    public interface IUserFormRepository : IRepository<UserForm>
    {
        bool IsUniqueUser(string userFormId);

        bool IsExistUser(string userFormId);

        UserForm GetById(string id);
        string GetUserByUserFormId(string userFormId);
        string GetUserFormIdByUserId(string userId);
        DetailsProcessUserDto GetDetailProcess(UserFormProcessDto payload);

        bool ExistFormWithConsecutive(string Consecutive);

        UserForm GetFormUserId(string userFormId);
        UserForm IsDownloadCertificate(string userId);
        int GetTotalRecords();
        int GetTotalRecordsByDateRange(string startDate, string endDate);
        DetailsProccessPersonalDto GetPersonalInformation(string userFormId);
        DetailsEditProccessPersonalDto GetEditPersonalInformation(string userFormId);
        DetailsEditProccessAcademicDto GetEditInformationAcademic(string userFormId);
        DetailsProccessAcademicDto GetProccessAcademic(string userFormId);
        PaginationResult<UserForm> GetAll(int? page);
        Task<ApiResponse> post(UserFormCreateDto createRequestDto);
        Task<ApiResponse> updateFormInformation(string formId, UserFormUpdateDto payload);
        ApiResponse ApprovedForm(string userFormId, ReviewStepForm stepForm);
        ApiResponse RejectForm(string userFormId);

        ApiResponse RegisterUserFormFile(string userFormId, UserFormFilesCreateDto payload);

        Task<string> DownloadCertificateRethus(TemplateRethusDto rethusDto);

        Task<string> DownloadCertificateSso(TemplateSSODto sSODto);

        Task<ApiResponse> AddConsecutive(string userFormId, UserFormConsecutiveDto payload);
        Task<ApiResponse> UpdateConsecutive(string userFormId, UserFormConsecutiveDto payload);

        Task<string> GetFileInventory(string userFormFileId);

        Task ValidateCertificateUserForm(string userFormId);

        Task<UserFormConsecutiveResponseDto> GetInformationConsecutive(string userFormId);

        bool ValidateExistFileForDownload(string userId);

        ValidateFileUserForm IsValidUserForm(string userFormId);

        Task<string?> DonwloadCertificateFileByUserId(string userId);
        ValidateFormUserDto ValidateFormUserId(string UserId);
        Task GenerateExcelWithBatches(
            string filePath,
            int totalRecords,
            int batchSize,
            ApplicationDbContext dbContext,
            string startDate,
            string endDate
        );

        Task<PaginationResultDto<TResult>> GetPagedData<TResult>(
            PaginationRequestDto<FilterQueryParametersDto> request,
            Func<UserForm, TResult> selector
        );
        Task<List<UserForm>> GetUserFormsByBatch(
            int batchSize,
            int pageNumber,
            ApplicationDbContext dbContext
        );
        Task ProcessUserFormCertificatesByBatch(
            int batchSize,
            int maxDegreeOfParallelism,
            int consecutiveStart,
            int consecutiveEnd,
            string consecutiveDate,
            ApplicationDbContext dbContext
        );
        public bool IsTypeProgramIsPsicologia(string FormId);

        Task ProcessGenerateZipPdf(
            int batchSize,
            DateTime startDate,
            DateTime endDate,
            ApplicationDbContext dbContext
        );

        Task<int> GetCountFormReadyForGenerate(DateTime startDate, DateTime endDate);
    }
}
