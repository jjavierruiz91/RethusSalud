using rethus_backend.Data;
using rethus_backend.Models;
using rethus_backend.Models.Dto.UserFormFiles;
using rethus_backend.Repository.IRepository;
using rethus_backend.Utilities.Constants.UserConstants;
using rethus_backend.Utilities.FileHelper;
namespace rethus_backend.Repository
{
  public class UserFormFilesRepository : Repository<UserFormFiles>, IUserFormFilesRepository
  {

    private readonly ApplicationDbContext _context;
    private readonly IConfiguration _config;
    private readonly IUserFormRepository _useForm;

    public UserFormFilesRepository(ApplicationDbContext db, IConfiguration config, IUserFormRepository userForm) : base(db)
    {
      _context = db;
      _config = config;
      _useForm = userForm;
    }

    public IEnumerable<UserFormFiles> GetAll()
    {
      return _context.UserFormFiles;
    }

    public UserFormFiles GetById(string id)
    {
      return _context.UserFormFiles.Find(id);
    }

    public bool IsExist(string userFormId)
    {
      UserFormFiles user = _context.UserFormFiles.FirstOrDefault(x => x.UserFormId == userFormId);
      if (user == null) return false;
      return true;
    }

    public bool IsUnique(string userFormId)
    {
      UserFormFiles user = _context.UserFormFiles.FirstOrDefault(x => x.UserFormId == userFormId);
      return user == null;
    }

    public ApiResponse RegisterUserFormFile(string userId, UserFormFilesCreateDto payload)
    {
      var response = new ApiResponse();
      var userForm = _context.UserForm.FirstOrDefault(x => x.UserId == userId && x.status == "active");

      if (userForm == null)
      {
        response.IsSuccess = false;
        response.AddError("EL usuario no tiene formulario activo");
        return response;
      };

      if (payload.files.Count == 0)
      {
        response.IsSuccess = false;
        response.AddError("La lista de archivo no puede estar vacia");
      }

      // var amount = GetAmountFilesByTypeProcedure(userForm.typeProcedure);

      // if (amount == 0 || payload.files.Count != (int)amount)
      // {
      //   response.IsSuccess = false;
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