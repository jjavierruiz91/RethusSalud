using rethus_backend.Data;
using rethus_backend.Models;
using rethus_backend.Models.Dto.UserForm;
using rethus_backend.Models.Dto.UserFormFiles;
using rethus_backend.Repository.IRepository;
using System.Collections.Generic;
using System.Net;
namespace rethus_backend.Repository
{
  public class UserFormRepository : Repository<UserForm>, IUserFormRepository
  {
    private readonly ApplicationDbContext _context;

    public UserFormRepository(ApplicationDbContext db) : base(db)
    {
      _context = db;
    }

    public IEnumerable<UserForm> GetAll()
    {
      return _context.UserForm;
    }

    public UserForm GetById(string id)
    {
      return _context.UserForm.Find(id);
    }

    public bool IsExistUser(string userFormId)
    {
      throw new NotImplementedException();
    }

    public bool IsUniqueUser(string userId)
    {
      UserForm user = _context.UserForm.FirstOrDefault(x => x.PersonalEmail == userId);
      if (user == null) return false;
      return true;
    }

    public Task<ApiResponse> post(UserFormCreateDto createRequestDto)
    {

      if (createRequestDto == null) return null;

      var form = new UserForm
      {
        PersonalType_identification = createRequestDto.PersonalType_identification,
        PersonalGender = createRequestDto.PersonalGender,
        PersonalIdentification = createRequestDto.PersonalIdentification,
        PersonalFirstName = createRequestDto.PersonalFirstName,
        PersonalLastName = createRequestDto.PersonalLastName,
        PersonalCountryBirth = createRequestDto.PersonalCountryBirth,
        PersonalDepartmentBirth = createRequestDto.PersonalDepartmentBirth,
        PersonalMunicipalityBirth = createRequestDto.PersonalMunicipalityBirth,
        DateBirth = createRequestDto.DateBirth,
        PersonalPlaceResidence = createRequestDto.PersonalPlaceResidence,
        PersonalDepartmentResidence = createRequestDto.PersonalDepartmentResidence,
        PersonalMunicipalityResidence = createRequestDto.PersonalMunicipalityResidence,
        PersonalAddress = createRequestDto.PersonalAddress,
        PersonalTelephone = createRequestDto.PersonalTelephone,
        PersonalPhone = createRequestDto.PersonalPhone,
        PersonalEmail = createRequestDto.PersonalEmail,
        PersonalEthnicGroup = createRequestDto.PersonalEthnicGroup,
        AcademicsOriginTitle = createRequestDto.AcademicsOriginTitle,
        AcademicsTypeInstitution = createRequestDto.AcademicsTypeInstitution,
        AcademicsProgramType = createRequestDto.AcademicsProgramType,
        AcademicsDepartmentInstitution = createRequestDto.AcademicsDepartmentInstitution,
        AcademicsMunicipalityInstitution = createRequestDto.AcademicsMunicipalityInstitution,
        AcademicsNameInstitution = createRequestDto.AcademicsNameInstitution,
        AcademicsProgramName = createRequestDto.AcademicsProgramName,
        AcademicsDateInstitution = createRequestDto.AcademicsDateInstitution,
        AcademicsGradeDate = createRequestDto.AcademicsGradeDate,
        AcademicsNumberConvalidation = createRequestDto.AcademicsNumberConvalidation,
        AcademicsDateConvalidation = createRequestDto.AcademicsDateConvalidation,
        AcademicsTitle = createRequestDto.AcademicsTitle,
        AcademicsNumberAdministrative = createRequestDto.AcademicsNumberAdministrative,
        AcademicsDateAdministrative = createRequestDto.AcademicsDateAdministrative
      };

      form.UserId = createRequestDto.userId;
      form.CreatedAt = DateTime.Now;
      form.UpdatedAt = DateTime.Now;

      var createUserForm = _context.UserForm.Add(form);
      _context.SaveChanges();

      var response = new ApiResponse();
      return Task.FromResult(response);
    }

    public ApiResponse RegisterUserFormFile(string userFormId, UserFormFilesCreateDto payload)
    {
      var response = new ApiResponse();
      var userForm = this.GetById(userFormId);

      if (userForm == null)
      {
        response.AddError("EL usuario no tiene formulario activo", HttpStatusCode.BadRequest, false);
        return response;
      };

      return response;
    }
  }
}