using rethus_backend.Data;
using rethus_backend.Models;
using rethus_backend.Models.Dto.UserForm;
using rethus_backend.Models.Dto.UserFormFiles;
using rethus_backend.Repository.IRepository;
using rethus_backend.Utilities.Constants.UserConstants;
using rethus_backend.Utilities.FileHelper;
using System.Collections.Generic;
using System.Net;
namespace rethus_backend.Repository
{
  public class UserFormRepository : Repository<UserForm>, IUserFormRepository
  {
    private readonly ApplicationDbContext _context;
    private readonly IConfiguration _config;

    public UserFormRepository(ApplicationDbContext db, IConfiguration config) : base(db)
    {
      _context = db;
      _config = config;
    }

    public PaginationResult<UserForm> GetAll(int? page)
    {
      int _page = page ?? 1;
      int pageSize = 10;

      int totalRecords = _context.UserForm.Count();
      int total_pages = (int)Math.Ceiling((decimal)totalRecords / pageSize);

      var pensiones = _context.UserForm
          .Skip((_page - 1) * pageSize)
          .Take(pageSize)
          .ToList();

      var paginationResult = new PaginationResult<UserForm>
      {
        TotalPages = total_pages,
        TotalRecords = totalRecords,
        CurrentPage = _page,
        Records = pensiones
      };

      return paginationResult;
    }

    public UserForm GetById(string id)
    {
      return _context.UserForm.Find(id);
    }

    public DetailsProcessUserDto GetDetailProcessByUserId(string userId)
    {
      var userForm = _context.UserForm
            .Where(user => user.UserId == userId)
            .Select(columns => new DetailsProcessUserDto
            {
              PersonalType_identification = columns.PersonalType_identification,
              PersonalIdentification = columns.PersonalIdentification ?? 0,
              PersonalFirstName = columns.PersonalFirstName,
              PersonalLastName = columns.PersonalLastName,
              PersonalEmail = columns.PersonalEmail
            }).FirstOrDefault();

      return userForm;
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
      var response = new ApiResponse();
      if (createRequestDto == null) return null;

      var user = _context.Configurations.FirstOrDefault(x => x.UserId == createRequestDto.userId);
      if (user == null)
      {
        response.AddError("El usuario no existe", HttpStatusCode.BadRequest, false);
        return Task.FromResult(response);
      }

      var userForm = _context.UserForm.FirstOrDefault(x => x.UserId == createRequestDto.userId);
      if (userForm != null && userForm.status == "active")
      {
        response.AddError("El usuario tiene un tramite en proceso", HttpStatusCode.BadRequest, false);
        return Task.FromResult(response);
      }

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
        AcademicsDateAdministrative = createRequestDto.AcademicsDateAdministrative,
        typeProcedure = user.type_procedure
      };

      form.UserId = createRequestDto.userId;
      form.status = "active";
      form.stepForm = "etapa_1";
      form.CreatedAt = DateTime.Now;
      form.UpdatedAt = DateTime.Now;

      var createUserForm = _context.UserForm.Add(form);
      _context.SaveChanges();

      response.IsSuccess = true;
      response.StatusCode = HttpStatusCode.OK;
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

      if (payload.files.Count == 0)
      {
        response.AddError("La lista de archivo no puede estar vacia");
      }

      // var amount = GetAmountFilesByTypeProcedure(userForm.typeProcedure);

      // if (amount == 0 || payload.files.Count != (int)amount)
      // {
      //   response.AddError("El tipo de tramite no coincide con la cantidad de archivo requerida");
      // }

      var ruta = _config.GetSection("routeFileProcedures").Value + userForm.PersonalIdentification;

      FileHelper.CreateFolder(ruta);

      foreach (var item in payload.files)
      {
        var baseUrlFile = FileHelper.AddAsync(item, ruta);

        var form = new UserFormFiles
        {
          size = item.Length,
          filename = item.FileName,
          type = item.ContentType,
          url = baseUrlFile,
          UserFormId = userForm.UserFormId
        };

        var createRegister = _context.UserFormFiles.Add(form);
        _context.SaveChanges();

      }
      return response;
    }

    public EnumMaximumAmountFiles GetAmountFilesByTypeProcedure(int typeProcedure)
    {
      var procedure = (EnumProcedure)typeProcedure;

      if (procedure == EnumProcedure.RGNTHST) return EnumMaximumAmountFiles.RGNTHST;

      if (procedure == EnumProcedure.TCSSO) return EnumMaximumAmountFiles.TCSSO;

      return EnumMaximumAmountFiles.DF;
    }
  }
}